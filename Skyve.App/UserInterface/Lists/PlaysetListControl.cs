﻿using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Panels;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class PlaysetListControl : SlickStackedListControl<IPlayset, PlaysetListControl.Rectangles>
{
	private PlaysetSorting sorting;
	private static IPlayset? downloading;
	private bool dragActive;
	private Padding InnerPadding;
	private bool activating;
	private static readonly IPlayset? opening;
	private readonly IOSelectionDialog imagePrompt;

	public bool ReadOnly { get; set; }

	public event Action? LoadPlaysetStarted;

	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IUserService _userService;
	private readonly IPlaysetManager _playsetManager;
	private readonly ICompatibilityManager _compatibilityManager;

	public PlaysetListControl(bool readOnly)
	{
		ServiceCenter.Get(out _settings, out _notifier, out _userService, out _playsetManager, out _compatibilityManager);

		ReadOnly = readOnly;
		SeparateWithLines = true;
		AllowDrop = true;
		ItemHeight = 30;
		GridItemSize = new Size(190, 295);

		sorting = (PlaysetSorting)_settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Playsets).Sorting;

		imagePrompt = new IOSelectionDialog()
		{
			ValidExtensions = IO.ImageExtensions
		};
	}

	public void SetSorting(PlaysetSorting selectedItem)
	{
		if (sorting == selectedItem)
		{
			return;
		}

		sorting = selectedItem;
		ResetScroll();

		SortingChanged();

		if (selectedItem != PlaysetSorting.Downloads)
		{
			var settings = _settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Playsets);
			settings.Sorting = (int)selectedItem;
			_settings.SessionSettings.Save();
		}
	}

	protected override void OnViewChanged()
	{
		if (GridView)
		{
			Padding = UI.Scale(new Padding(2, 5, 2, 0));
			InnerPadding = UI.Scale(new Padding(4));
		}
		else
		{
			Padding = UI.Scale(new Padding(5, 2, 5, 2));
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		OnViewChanged();
	}

	protected override IEnumerable<IDrawableItem<IPlayset>> OrderItems(IEnumerable<IDrawableItem<IPlayset>> items)
	{
		return sorting switch
		{
			PlaysetSorting.Downloads => items.OrderByDescending(x => x.Item is IOnlinePlayset op ? op.Downloads : 0),
			PlaysetSorting.Color => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenBy(x => x.Item.GetCustomPlayset().Color?.GetHue() ?? float.MaxValue).ThenBy(x => x.Item.GetCustomPlayset().Color?.GetBrightness() ?? float.MaxValue).ThenBy(x => x.Item.GetCustomPlayset().Color?.GetSaturation() ?? float.MaxValue),
			PlaysetSorting.Name => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenBy(x => x.Item.Name),
			PlaysetSorting.DateCreated => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.GetCustomPlayset().DateCreated),
			PlaysetSorting.Usage => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.GetCustomPlayset().Usage).ThenBy(x => x.Item.DateUpdated),
			PlaysetSorting.LastEdit => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.DateUpdated),
			PlaysetSorting.LastUsed or _ => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.GetCustomPlayset().DateUsed),
		};
	}

	protected override async void OnItemMouseClick(DrawableItem<IPlayset, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		if (e.Button == MouseButtons.Right)
		{
			ShowRightClickMenu(item.Item);
			return;
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		if (item.Rectangles.DotsRect.Contains(e.Location) && !ReadOnly)
		{
			ShowRightClickMenu(item.Item);
			return;
		}

		if (item.Rectangles.Favorite.Contains(e.Location))
		{
			var customPlayset = item.Item.GetCustomPlayset();
			customPlayset.IsFavorite = !customPlayset.IsFavorite;
			_playsetManager.Save(customPlayset);
			return;
		}

		if (item.Rectangles.Thumbnail.Contains(e.Location) && !ReadOnly)
		{
			ServiceCenter.Get<IAppInterfaceService>().OpenPlaysetPage(item.Item);
			return;
		}

		if (item.Rectangles.ActivateButton.Contains(e.Location))
		{
			if (ReadOnly)
			{
				DownloadProfile(item.Item);
			}
			else if (!activating)
			{
				try
				{
					activating = true;

					item.Loading = true;

					LoadPlaysetStarted?.Invoke();

					Invalidate();

					await _playsetManager.ActivatePlayset(item.Item);
				}
				catch (Exception ex)
				{
					MessagePrompt.Show(ex, "Failed to activate playset");
				}
				finally
				{
					item.Loading = false;
					activating = false;
				}
			}

			return;
		}

		if (item.Rectangles.Author.Contains(e.Location) && item.Item.GetCustomPlayset().OnlineInfo?.Author is IUser author)
		{
			Program.MainForm.PushPanel(new PC_UserPage(author));

			return;
		}

		if (item.Rectangles.ViewContents.Contains(e.Location))
		{
			ShowPlaysetContents(item.Item);

			return;
		}

		if (item.Rectangles.EditThumbnail.Contains(e.Location))
		{
			if (imagePrompt.PromptFile(Program.MainForm) == DialogResult.OK)
			{
				try
				{
					var customPlayset = item.Item.GetCustomPlayset();

					customPlayset.SetThumbnail(Image.FromFile(imagePrompt.SelectedPath));

					_playsetManager.Save(customPlayset);

					Invalidate(item.Item);
				}
				catch { }
			}
		}
	}

	private void DownloadProfile(IPlayset _)
	{
		try
		{
			//downloading = item;
			//Invalidate();
			//await ServiceCenter.Get<IOnlinePlaysetUtil>().DownloadPlayset(item);
			//downloading = null;
			//Invalidate();
		}
		catch (Exception ex)
		{
			Program.MainForm.TryInvoke(() => MessagePrompt.Show(ex, Locale.FailedToDownloadPlayset, form: Program.MainForm));
		}
	}

	private void ShowRightClickMenu(IPlayset playset)
	{
		this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(playset, !ReadOnly)));
	}

	private async Task ShareProfile(IPlayset item)
	{
		Loading = true;
		downloading = item;
		await ServiceCenter.Get<IOnlinePlaysetUtil>().Share(item!);
		downloading = null;
		Loading = false;
	}

	private void ShowPlaysetContents(IPlayset item)
	{
		try
		{
			Program.MainForm.PushPanel(new PC_PlaysetContents(item));
		}
		catch (Exception ex)
		{
			Program.MainForm.TryInvoke(() => MessagePrompt.Show(ex, Locale.FailedToDownloadPlayset, form: Program.MainForm));
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		if (!Loading && !AnyVisibleItems())
		{
			e.Graphics.ResetClip();

			using var font = UI.Font(9.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.LabelColor);
			using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

			e.Graphics.DrawString(ItemCount == 0 ? Locale.NoPlaysetsFound : Locale.NoPlaysetsMatchFilters, font, brush, ClientRectangle, stringFormat);
		}

		if (dragActive)
		{
			var border = UI.Scale(16);
			var rectangle = ClientRectangle.Pad(border);
			using var font = UI.Font(11.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.ForeColor);
			using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			using var backBrush = new SolidBrush(Color.FromArgb(200, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor)));
			using var pen = new Pen(FormDesign.Design.ActiveColor, (float)(1.5 * UI.FontScale)) { DashStyle = DashStyle.Dash };

			e.Graphics.ResetClip();
			e.Graphics.FillRoundedRectangle(backBrush, rectangle, border);
			e.Graphics.DrawRoundedRectangle(pen, rectangle, border);
			e.Graphics.DrawString(Locale.DropImportFile.Format(Locale.Playset), font, brush, ClientRectangle, stringFormat);
		}
	}

	protected override void OnPaintItemGrid(ItemPaintEventArgs<IPlayset, Rectangles> e)
	{
		var isActive = e.Item.Equals(_playsetManager.CurrentPlayset);
		var borderRadius = UI.Scale(5);
		var isPressed = e.HoverState.HasFlag(HoverState.Pressed);
		var customPlayset = e.Item.GetCustomPlayset();
		var banner = customPlayset.GetThumbnail();
		var backColor = customPlayset.Color ?? banner?.GetThemedAverageColor() ?? FormDesign.Design.AccentBackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 7 : -6, Sat: 3);
		var onBannerColor = (banner?.GetThemedAverageColor() ?? FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 7 : -6, Sat: 3)).GetTextColor();
		var backRect = e.ClipRectangle.Pad(UI.Scale(8));
		using var onBannerBrush = new SolidBrush(Color.FromArgb(banner is null ? 125 : 50, onBannerColor));

		e.BackColor = backColor;

		using (var brush = Gradient(Color.FromArgb(FormDesign.Design.IsDarkTheme ? 100 : 70, backColor), backRect, 3.5f))
		{
			e.Graphics.FillRoundedRectangleWithShadow(backRect, borderRadius, UI.Scale(8), BackColor, Color.FromArgb(10, isActive ? FormDesign.Design.GreenColor : backColor));

			e.Graphics.FillRoundedRectangle(brush, backRect, borderRadius);
		}

		if (banner is null)
		{
			using var brush = new SolidBrush(Color.FromArgb(40, onBannerColor));

			e.Graphics.FillRoundedRectangle(brush, e.Rects.Thumbnail, borderRadius, botLeft: false, botRight: false);

			using var icon = customPlayset.Usage.GetIcon().Get(e.Rects.Thumbnail.Width * 3 / 4).Color(onBannerColor);

			e.Graphics.DrawImage(icon, e.Rects.Thumbnail.CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(banner, e.Rects.Thumbnail, borderRadius, botLeft: false, botRight: false);
		}

		if (isActive)
		{
			e.Rects.ActivateButton = e.Rects.ActivateButton.Pad(0, Padding.Vertical, 0, 0);

			using var greenBrush = new SolidBrush(FormDesign.Design.GreenColor);
			e.Graphics.FillRoundedRectangle(greenBrush, e.Rects.ActivateButton.Pad(-InnerPadding.Left - UI.Scale(2)), borderRadius, topLeft: false, topRight: false);

			var text = Locale.ActivePlayset.One.ToUpper();
			using var font = UI.Font(9F, FontStyle.Bold).FitTo(text, e.Rects.ActivateButton, e.Graphics);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
			using var textBrush2 = new SolidBrush(FormDesign.Design.GreenColor.GetTextColor());

			e.Graphics.DrawString(text, font, textBrush2, e.Rects.ActivateButton, format);
		}

		if (HoverState.HasFlag(HoverState.Hovered) && e.Rects.Thumbnail.Contains(CursorLocation))
		{
			using var brush = new SolidBrush(Color.FromArgb(40, onBannerColor));

			e.Graphics.FillRoundedRectangle(brush, e.Rects.Thumbnail, borderRadius, botLeft: false, botRight: false);
		}

		DrawFavoriteButton(e, customPlayset, onBannerColor, onBannerBrush);

		using var textBrush = new SolidBrush(backColor.MergeColor(BackColor, (FormDesign.Design.IsDarkTheme ? 100 : 70) * 100 / 255).GetTextColor());
		using var fadedBrush = new SolidBrush(Color.FromArgb(175, textBrush.Color));
		using var textFont = UI.Font(9.5F, FontStyle.Bold).FitTo(e.Item.Name, e.Rects.Text, e.Graphics);
		using var smallTextFont = UI.Font(7.25F);
		using var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(e.Item.Name, textFont, textBrush, e.Rects.Text);

		var height = (int)e.Graphics.Measure(e.Item.Name, textFont, e.Rects.Text.Width).Height;
		e.Graphics.DrawString(customPlayset.Usage > 0 ? Locale.UsagePlayset.Format(LocaleHelper.GetGlobalText(customPlayset.Usage.ToString())) : Locale.GenericPlayset, smallTextFont, fadedBrush, new Point(e.Rects.Text.X, e.Rects.Text.Y + height));

		var contentText = $"{Locale.ContainCount.FormatPlural(e.Item.ModCount, Locale.Mod.FormatPlural(e.Item.ModCount).ToLower())} • {e.Item.ModSize.SizeString(0)}";
		using (var contentBrush = Gradient(Color.FromArgb(FormDesign.Design.IsDarkTheme ? 100 : 70, backColor.Tint(Lum: backColor.IsDark() ? 6 : -6)), backRect, 3.5f))
		using (var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center })
		{
			e.Graphics.FillRectangle(contentBrush, e.Rects.Content.Pad(0, 4 * (height - UI.Scale(18)) / 5, 0, 0).CenterR(backRect.Width, smallTextFont.Height * 5 / 3));
			e.Graphics.DrawString(contentText, smallTextFont, textBrush, e.Rects.Content.Pad(0, 4 * (height - UI.Scale(18)) / 5, 0, 0), format);
		}

		using var pen = new Pen(isActive ? FormDesign.Design.GreenColor : customPlayset.IsFavorite ? FormDesign.Modern.ActiveColor : backColor, UI.Scale(2.5f)) { Alignment = PenAlignment.Center };
		e.Graphics.DrawRoundedRectangle(pen, backRect.Pad(-1), borderRadius);

		var isHovered = e.Rects.DotsRect.Contains(CursorLocation);
		using var img = IconManager.GetIcon("VertialMore", e.Rects.DotsRect.Height * 3 / 4).Color(isHovered ? FormDesign.Design.ActiveColor : textBrush.Color);

		e.Graphics.DrawImage(img, e.Rects.DotsRect.CenterR(img.Size));

#if CS1
		labelRects.Y += e.Graphics.DrawLabel(Locale.ContainCount.FormatPlural(e.Item.AssetCount, Locale.Asset.FormatPlural(e.Item.AssetCount).ToLower()), IconManager.GetSmallIcon("Assets"), FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 75), labelRects, ContentAlignment.TopLeft).Height + InnerPadding.Top;
#endif

		if (customPlayset.OnlineInfo?.Author?.Name is not null and not "")
		{
			var name = customPlayset.OnlineInfo.Author.Name;

			using var userIcon = IconManager.GetSmallIcon("User");

			e.Rects.Author = e.Graphics.DrawLabel(name, userIcon, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor, 25), default, ContentAlignment.TopLeft);

			throw new NotImplementedException();
		}

#if CS1
		else if (e.Item.IsMissingItems)
		{
			using var icon = IconManager.GetSmallIcon("MinorIssues");
			e.Graphics.DrawLabel(Locale.IncludesItemsYouDoNotHave, icon, FormDesign.Design.RedColor.MergeColor(FormDesign.Design.BackColor, 50), e.Rects.Content, ContentAlignment.TopRight);
		}
#endif
		if (ReadOnly)
		{
			return;
		}

		if (downloading == e.Item)
		{
			using var brush = new SolidBrush(Color.FromArgb(100, FormDesign.Design.BackColor));
			e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.InvertPad(InnerPadding), borderRadius);

			DrawLoader(e.Graphics, e.ClipRectangle.CenterR(UI.Scale(new Size(24, 24))));
		}

		if (!isActive)
		{
			e.Graphics.ResetClip();

			SlickButton.Draw(e.Graphics, new ButtonDrawArgs
			{
				Text = Locale.ActivatePlayset.ToString().ToUpper(),
				Icon = "Check",
				Rectangle = e.Rects.ActivateButton,
				Padding = UI.Scale(new Padding(8, 4, 8, 4)),
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				Enabled = !activating,
				Control = e.DrawableItem.Loading ? this : null,
				BackgroundColor = backColor.MergeColor(onBannerColor, 85).MergeColor(BackColor, 40).Tint(null, -6, 6),
				ActiveColor = FormDesign.Design.GreenColor
			});
		}

		if (isActive || activating)
		{
			e.Rects.ActivateButton = default;
		}
	}

	private void DrawFavoriteButton(ItemPaintEventArgs<IPlayset, Rectangles> e, ICustomPlayset customPlayset, Color onBannerColor, SolidBrush onBannerBrush)
	{
		var borderRadius = UI.Scale(5);
		var favViewRect = ReadOnly ? e.Rects.ViewContents : e.Rects.Favorite;

		if (customPlayset.IsFavorite)
		{
			using var favBrush = new SolidBrush(FormDesign.Modern.ActiveColor);
			e.Graphics.FillRoundedRectangle(favBrush, favViewRect, borderRadius);
		}
		else if (e.HoverState.HasFlag(HoverState.Hovered) && favViewRect.Contains(CursorLocation))
		{
			e.Graphics.FillRoundedRectangle(onBannerBrush, favViewRect, borderRadius);
		}

		var fav = new DynamicIcon(ReadOnly ? "ViewFile" : customPlayset.IsFavorite != favViewRect.Contains(CursorLocation) ? "StarFilled" : "Star");
		using var favIcon = fav.Get(favViewRect.Height * 3 / 4);

		if (e.DrawableItem.Loading)
		{
			DrawLoader(e.Graphics, favViewRect.CenterR(favIcon.Size));
		}
		else if (customPlayset.IsFavorite || !GridView || (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.Thumbnail.Contains(CursorLocation)))
		{
			e.Graphics.DrawImage(favIcon.Color(customPlayset.IsFavorite ? FormDesign.Modern.ActiveForeColor : favViewRect.Contains(CursorLocation) ? FormDesign.Modern.ActiveColor : onBannerColor), favViewRect.CenterR(favIcon.Size));
		}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<IPlayset, Rectangles> e)
	{
		var borderRadius = UI.Scale(5);
		var customPlayset = e.Item.GetCustomPlayset();
		var banner = customPlayset.GetThumbnail();

		if (e.Item.Equals(_playsetManager.CurrentPlayset))
		{
			e.BackColor = FormDesign.Design.AccentBackColor.MergeColor(FormDesign.Design.GreenColor, e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 80);
		}
		else
		{
			e.BackColor = e.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.AccentBackColor.MergeColor(customPlayset.Color ?? FormDesign.Design.ActiveColor, 85) : FormDesign.Design.AccentBackColor;
		}

		var onBannerColor = e.BackColor.GetTextColor();
		using var onBannerBrush = new SolidBrush(Color.FromArgb(banner is null ? 125 : 50, onBannerColor));

		base.OnPaintItemList(e);

		DrawFavoriteButton(e, customPlayset, onBannerColor, onBannerBrush);

		if (banner is null)
		{
			using var brush = new SolidBrush(customPlayset.Color ?? FormDesign.Design.AccentColor);

			e.Graphics.FillRoundedRectangle(brush, e.Rects.Thumbnail, borderRadius);

			using var icon = customPlayset.Usage.GetIcon().Get(e.Rects.Thumbnail.Width * 3 / 4).Color(brush.Color.GetTextColor());

			e.Graphics.DrawImage(icon, e.Rects.Thumbnail.CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(banner, e.Rects.Thumbnail, borderRadius, e.BackColor);
		}

		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			Icon = "VertialMore",
			BorderRadius = borderRadius,
			Rectangle = e.Rects.DotsRect,
			Cursor = CursorLocation,
			HoverState = e.HoverState,
			BackgroundColor = e.BackColor,
			ActiveColor = e.Item.Equals(_playsetManager.CurrentPlayset) ? FormDesign.Design.GreenColor : customPlayset.Color ?? FormDesign.Design.ActiveColor
		});

		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			Icon = "Cog",
			BorderRadius = borderRadius,
			Rectangle = e.Rects.EditSettings,
			Cursor = CursorLocation,
			HoverState = e.HoverState,
			BackgroundColor = e.BackColor,
			ActiveColor = e.Item.Equals(_playsetManager.CurrentPlayset) ? FormDesign.Design.GreenColor : customPlayset.Color ?? FormDesign.Design.ActiveColor
		});

		using var pen = new Pen(FormDesign.Design.AccentColor, (int)UI.FontScale);

		e.Graphics.DrawLine(pen, e.Rects.Content.Right + Padding.Horizontal, e.Rects.Content.Y, e.Rects.Content.Right + Padding.Horizontal, e.Rects.Content.Bottom);

		if (e.Item.Equals(_playsetManager.CurrentPlayset))
		{
			using var font = UI.Font(8.25F, FontStyle.Bold);
			var rect = SlickButton.AlignAndDraw(e.Graphics, e.Rects.Content, ContentAlignment.MiddleRight, new ButtonDrawArgs
			{
				Text = Locale.ActivePlayset.One.ToUpper(),
				Padding = UI.Scale(new Padding(2)),
				Font = font,
				BorderRadius = Padding.Left,
				ColorStyle = ColorStyle.Green,
				ButtonType = ButtonType.Active
			}).Rectangle;

			e.Rects.Content = new Rectangle(e.Rects.Text.Right, e.ClipRectangle.Y, rect.X - e.Rects.Text.Right, e.ClipRectangle.Height);
		}
		else if (e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.Rects.ActivateButton = SlickButton.AlignAndDraw(e.Graphics, e.Rects.Content, ContentAlignment.MiddleRight, new ButtonDrawArgs
			{
				Text = Locale.ActivatePlayset.ToString().ToUpper(),
				Icon = "Check",
				Padding = UI.Scale(new Padding(4)),
				Cursor = CursorLocation,
				HoverState = e.HoverState,
				BackgroundColor = e.BackColor,
				Control = e.DrawableItem.Loading ? this : null,
				Enabled = !activating,
				ActiveColor = FormDesign.Design.GreenColor
			});

			e.Rects.Content = new Rectangle(e.Rects.Text.Right, e.ClipRectangle.Y, e.Rects.ActivateButton.X - e.Rects.Text.Right, e.ClipRectangle.Height);
		}

		using var textBrush = new SolidBrush(e.Rects.Thumbnail.Contains(CursorLocation) || e.Rects.Text.Contains(CursorLocation) ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor);
		using var fadedBrush = new SolidBrush(Color.FromArgb(175, FormDesign.Design.ForeColor));
		using var textFont = UI.Font(8.25F, FontStyle.Bold).FitTo(e.Item.Name, e.Rects.Text, e.Graphics);
		using var smallTextFont = UI.Font(7F);
		using var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(e.Item.Name, textFont, textBrush, e.Rects.Text);

		using var format = new StringFormat { LineAlignment = StringAlignment.Far };
		e.Graphics.DrawString(customPlayset.Usage > 0 ? Locale.UsagePlayset.Format(LocaleHelper.GetGlobalText(customPlayset.Usage.ToString())) : Locale.GenericPlayset, smallTextFont, fadedBrush, e.Rects.Text.Pad(0, 0, 0, -Padding.Vertical), format);

		e.Graphics.DrawLabel($"{Locale.ContainCount.FormatPlural(e.Item.ModCount, Locale.Mod.FormatPlural(e.Item.ModCount).ToLower())} • {e.Item.ModSize.SizeString(0)}", null, FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 50), e.Rects.Content, ContentAlignment.MiddleLeft);

		e.Rects.Thumbnail = Rectangle.Union(e.Rects.Thumbnail, e.Rects.Text);

#if CS1
		if (e.Item.IsMissingItems)
		{
			e.Rects.Text.X += e.Graphics.DrawLabel(Locale.IncludesItemsYouDoNotHave, IconManager.GetSmallIcon("MinorIssues"), FormDesign.Design.RedColor.MergeColor(FormDesign.Design.BackColor, 50), e.Rects.Text, large ? ContentAlignment.TopLeft : ContentAlignment.BottomLeft).Width + Padding.Left;
		}
#endif
	}

	protected override IDrawableItemRectangles<IPlayset> GenerateRectangles(IPlayset item, Rectangle rectangle, IDrawableItemRectangles<IPlayset> current)
	{
		var rects = new Rectangles(item);

		if (GridView)
		{
			rectangle = rectangle.Pad(InnerPadding.Left + UI.Scale(8));

			var size = UI.Scale(new Size(32, 32));

			rects.Thumbnail = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Width - InnerPadding.Top).InvertPad(InnerPadding);

			rectangle = rectangle.Pad(UI.Scale(2));

			rects.ActivateButton = new Rectangle(rectangle.X, rectangle.Bottom - UI.Scale(24), rectangle.Width, UI.Scale(24));
			rects.Content = new Rectangle(rectangle.X, rectangle.Y + rects.Thumbnail.Height + InnerPadding.Top, rectangle.Width, rectangle.Height - rects.Thumbnail.Height - InnerPadding.Top);

			rects.Favorite = rects.Thumbnail.Pad(InnerPadding).Pad(0, 0, 0, 0).Align(size, ContentAlignment.TopLeft);

			rects.Text = new Rectangle(rects.Thumbnail.X + InnerPadding.Left, rects.Thumbnail.Bottom + (InnerPadding.Top * 2), rects.Thumbnail.Width, size.Height);
			rects.Content = new Rectangle(rectangle.X, rects.Text.Bottom + InnerPadding.Top, rectangle.Width, rects.ActivateButton.Y - rects.Text.Bottom - (InnerPadding.Top * 2));

			rects.EditThumbnail = rects.Thumbnail.Pad(InnerPadding).Align(size, ContentAlignment.TopRight);
			rects.EditSettings = rects.Thumbnail.Pad(InnerPadding).Align(size, ContentAlignment.BottomRight);

			rects.DotsRect = rects.Text.Align(UI.Scale(new Size(24, 24)), ContentAlignment.TopRight);
			rects.Text.Width -= rects.DotsRect.Width;
		}
		else
		{
			rectangle = rectangle.Pad(Padding);
			var size = new Size(rectangle.Height, rectangle.Height);

			rects.Favorite = rects.Thumbnail = rectangle.Align(size, ContentAlignment.MiddleLeft);
			rects.Thumbnail.X += rects.Thumbnail.Width + Padding.Left;

			rects.Text = new Rectangle(rects.Thumbnail.Right + Padding.Left, rectangle.Y - Padding.Top, rectangle.Width / 3, rectangle.Height);

			rects.DotsRect = rects.EditSettings = rectangle.Align(size, ContentAlignment.TopRight);
			rects.EditSettings.X -= rects.EditSettings.Width + Padding.Left;

			rects.Content = Rectangle.FromLTRB(rects.Text.Right, rectangle.Y, rects.EditSettings.X - (Padding.Horizontal * 2) - (int)UI.FontScale, rectangle.Bottom);
		}

		if (ReadOnly)
		{
			rects.ViewContents = rects.Favorite;
			rects.Favorite = rects.EditThumbnail = default;
		}

		return rects;
	}

	protected override void OnDragEnter(DragEventArgs drgevent)
	{
		base.OnDragEnter(drgevent);


		if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
		{
			var file = ((string[])drgevent.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();

			if (Path.GetExtension(file).ToLower() is ".zip" or ".json")
			{
				drgevent.Effect = DragDropEffects.Copy;
				dragActive = true;
				Invalidate();
			}

			return;
		}

		dragActive = false;
		drgevent.Effect = DragDropEffects.None;
		Invalidate();
	}

	protected override void OnDragLeave(EventArgs e)
	{
		base.OnDragLeave(e);

		dragActive = false;
		Invalidate();
	}

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		base.OnDragDrop(drgevent);

		var file = ((string[])drgevent.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();

		if (file != null)
		{
			if (CrossIO.CurrentPlatform is not Platform.Windows)
			{
				var realPath = ServiceCenter.Get<IIOUtil>().ToRealPath(file);

				if (CrossIO.FileExists(realPath))
				{
					file = realPath!;
				}
			}

			(PanelContent.GetParentPanel(this) as PC_PlaysetList)?.Import(file);

			SortingChanged(false);
		}

		dragActive = false;
		Invalidate();
	}

	public class Rectangles : IDrawableItemRectangles<IPlayset>
	{
		public IPlayset Item { get; set; }

		public Rectangles(IPlayset item)
		{
			Item = item;
		}

		public Rectangle Thumbnail;
		public Rectangle Favorite;
		//public Rectangle Icon;
		//public Rectangle Folder;
		public Rectangle Text;
		public Rectangle Content;
		public Rectangle ActivateButton;
		//public Rectangle Exclude;
		//public Rectangle Merge;
		public Rectangle EditThumbnail;
		public Rectangle Author;
		public Rectangle ViewContents;
		public Rectangle DotsRect;
		public Rectangle EditSettings;

		public bool IsHovered(Control instance, Point location)
		{
			return
				Favorite.Contains(location) ||
				ActivateButton.Contains(location) ||
				Thumbnail.Contains(location) ||
				Author.Contains(location) ||
				EditThumbnail.Contains(location) ||
				ViewContents.Contains(location) ||
				DotsRect.Contains(location) ||
				EditSettings.Contains(location);
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			if (Favorite.Contains(location))
			{
				text = Item.GetCustomPlayset().IsFavorite ? Locale.UnFavoriteThisPlayset : Locale.FavoriteThisPlayset;
				point = Favorite.Location;
				return true;
			}

			if (ActivateButton.Contains(location))
			{
				text = (instance as PlaysetListControl)!.ReadOnly ? ServiceCenter.Get<IPlaysetManager>().Playsets.Any(x => x.Name!.Equals(Item.Name, StringComparison.InvariantCultureIgnoreCase)) ? Locale.UpdatePlaysetTip : Locale.DownloadPlaysetTip : Locale.ActivatePlayset;
				point = ActivateButton.Location;
				return true;
			}

			if (Thumbnail.Contains(location))
			{
				text = Locale.OpenPlaysetPage;
				point = Thumbnail.Location;
				return true;
			}

			if (Author.Contains(location))
			{
				text = Locale.OpenAuthorPage;
				point = Author.Location;
				return true;
			}

			if (EditThumbnail.Contains(location))
			{
				text = Locale.EditPlaysetThumbnail;
				point = EditThumbnail.Location;
				return true;
			}

			if (ViewContents.Contains(location))
			{
				text = Locale.ViewThisPlaysetsPackages;
				point = ViewContents.Location;
				return true;
			}

			if (EditSettings.Contains(location))
			{
				text = Locale.ChangePlaysetSettings;
				point = EditSettings.Location;
				return true;
			}

			text = string.Empty;
			point = default;

			return false;
		}
	}
}
