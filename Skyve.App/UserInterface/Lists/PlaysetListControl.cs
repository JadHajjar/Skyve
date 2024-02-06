using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Panels;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class PlaysetListControl : SlickStackedListControl<IPlayset, PlaysetListControl.Rectangles>
{
	private ProfileSorting sorting;
	private static IPlayset? downloading;
	private static IPlayset? opening;
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
		GridItemSize = new Size(190, 285);

		sorting = (ProfileSorting)_settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Profiles).Sorting;

		_notifier.PlaysetUpdated += ProfileManager_ProfileUpdated;
		_notifier.PlaysetChanged += ProfileManager_ProfileUpdated;

		imagePrompt = new IOSelectionDialog()
		{
			ValidExtensions = IO.ImageExtensions
		};
	}

	public void SetSorting(ProfileSorting selectedItem)
	{
		if (sorting == selectedItem)
		{
			return;
		}

		sorting = selectedItem;
		ResetScroll();

		SortingChanged();

		if (selectedItem != ProfileSorting.Downloads)
		{
			var settings = _settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Profiles);
			settings.Sorting = (int)selectedItem;
			_settings.SessionSettings.Save();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notifier.PlaysetUpdated -= ProfileManager_ProfileUpdated;
			_notifier.PlaysetChanged -= ProfileManager_ProfileUpdated;
		}

		base.Dispose(disposing);
	}

	private void ProfileManager_ProfileUpdated()
	{
		if (Loading)
		{
			Loading = false;
		}

		if (!ReadOnly)
		{
			SetItems(_playsetManager.Playsets);
		}
	}

	protected override void OnViewChanged()
	{
		if (GridView)
		{
			Padding = UI.Scale(new Padding(7, 10, 7, 5), UI.UIScale);
			GridPadding = UI.Scale(new Padding(4), UI.UIScale);
		}
		else
		{
			Padding = UI.Scale(new Padding(5, 2, 5, 2), UI.FontScale);
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		OnViewChanged();
	}

	protected override IEnumerable<DrawableItem<IPlayset, Rectangles>> OrderItems(IEnumerable<DrawableItem<IPlayset, Rectangles>> items)
	{
		return sorting switch
		{
			ProfileSorting.Downloads => items.OrderByDescending(x => x.Item is IOnlinePlayset op ? op.Downloads : 0),
			ProfileSorting.Color => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenBy(x => x.Item.GetCustomPlayset().Color?.GetHue() ?? float.MaxValue).ThenBy(x => x.Item.GetCustomPlayset().Color?.GetBrightness() ?? float.MaxValue).ThenBy(x => x.Item.GetCustomPlayset().Color?.GetSaturation() ?? float.MaxValue),
			ProfileSorting.Name => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenBy(x => x.Item.Name),
			ProfileSorting.DateCreated => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.GetCustomPlayset().DateCreated),
			ProfileSorting.Usage => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.GetCustomPlayset().Usage).ThenBy(x => x.Item.DateUpdated),
			ProfileSorting.LastEdit => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.DateUpdated),
			ProfileSorting.LastUsed or _ => items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite).ThenByDescending(x => x.Item.GetCustomPlayset().DateUsed),
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

		if (e.Button == MouseButtons.Middle)
		{
			throw new NotImplementedException();
			//if (item.Rectangles.Icon.Contains(e.Location) && !ReadOnly)
			//{
			//	item.Item.Color = null;
			//	_profileManager.Save(item.Item!);
			//}
			//else if (item.Rectangles.EditThumbnail.Contains(e.Location) && item.Item is IPlayset profile)
			//{
			//	profile.Banner = null;
			//	_profileManager.Save(profile);
			//}
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		if (item.Rectangles.Favorite.Contains(e.Location) && !ReadOnly)
		{
			var customPlayset = item.Item.GetCustomPlayset();
			customPlayset.IsFavorite = !customPlayset.IsFavorite;
			_playsetManager.Save(customPlayset);
		}
		else if (item.Rectangles.ActivateButton.Contains(e.Location))
		{
			if (ReadOnly)
			{
				DownloadProfile(item.Item);
			}
			else
			{
				LoadPlaysetStarted?.Invoke();

				await _playsetManager.ActivatePlayset(item.Item);
			}
		}
		else if (item.Rectangles.Icon.Contains(e.Location) && !ReadOnly)
		{
			ChangeColor(item.Item);
		}
		else if (item.Rectangles.Folder.Contains(e.Location) && !ReadOnly)
		{
			PlatformUtil.OpenFolder(_playsetManager.GetFileName(item.Item!));
		}
		else if (item.Rectangles.Author.Contains(e.Location) && item.Item.GetCustomPlayset().OnlineInfo?.Author is IUser author)
		{
			Program.MainForm.PushPanel(new PC_UserPage(author));
		}
		else if (item.Rectangles.ViewContents.Contains(e.Location))
		{
			ShowProfileContents(item.Item);
		}
		else if (item.Rectangles.EditThumbnail.Contains(e.Location) && item.Item is IPlayset profile)
		{
			throw new NotImplementedException();
			//if (imagePrompt.PromptFile(Program.MainForm) == DialogResult.OK)
			//{
			//	try
			//	{
			//		using var img = Image.FromFile(imagePrompt.SelectedPath);

			//		if (img.Width > 700 || img.Height > 700)
			//		{
			//			using var smallImg = new Bitmap(img, img.Size.GetProportionalDownscaledSize(700));
			//			profile.Banner = smallImg;
			//		}
			//		else
			//		{
			//			profile.Banner = img as Bitmap;
			//		}

			//		_profileManager.Save(profile);

			//		Invalidate(item.Item);
			//	}
			//	catch { }
			//}
		}
	}

	private async void DownloadProfile(IPlayset item)
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

	private void ChangeColor(IPlayset item)
	{
		var customPlayset = item.GetCustomPlayset();
		var colorDialog = new SlickColorPicker(customPlayset.Color ?? FormDesign.Design.ActiveColor);

		if (colorDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		customPlayset.Color = colorDialog.Color;

		_playsetManager.Save(customPlayset);
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

	private async void ShowProfileContents(IPlayset item)
	{
		try
		{
			IEnumerable<IPackage>? packages;

			if (ReadOnly)
			{
				Loading = true;
				opening = item;
				throw new NotImplementedException();
				//packages = (await ServiceCenter.Get<SkyveApiUtil>().GetUserProfileContents(item.ProfileId))?.Packages;
				opening = null;
				Loading = false;
			}
			else
			{
				//packages = item.Packages;
			}

			//Program.MainForm.PushPanel(new PC_GenericPackageList(packages ?? Enumerable.Empty<IPackage>(), true)
			//{
			//	Text = item.Name
			//});
		}
		catch (Exception ex)
		{
			Program.MainForm.TryInvoke(() => MessagePrompt.Show(ex, Locale.FailedToDownloadPlayset, form: Program.MainForm));
		}
	}

	//protected override bool IsFlowBreak(int index, DrawableItem<IPlayset, Rectangles> currentItem, DrawableItem<IPlayset, Rectangles> nextItem)
	//{
	//	return currentItem.Item.GetCustomPlayset().IsFavorite && (!nextItem?.Item.GetCustomPlayset().IsFavorite ?? false);
	//}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		if (Loading || AnyVisibleItems())
		{
			return;
		}

		e.Graphics.ResetClip();

		using var font = UI.Font(9.75F, FontStyle.Italic);
		using var brush = new SolidBrush(FormDesign.Design.LabelColor);
		using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(ItemCount == 0 ? Locale.NoPlaysetsFound : Locale.NoPlaysetsMatchFilters, font, brush, ClientRectangle, stringFormat);
	}

	protected override void OnPaintItemGrid(ItemPaintEventArgs<IPlayset, Rectangles> e)
	{
		var isPressed = e.HoverState.HasFlag(HoverState.Pressed);
		var customPlayset = e.Item.GetCustomPlayset();
		var banner = customPlayset.GetThumbnail();
		var onBannerColor = (customPlayset.Color ?? Color.Black).GetTextColor();
		using var onBannerBrush = new SolidBrush(Color.FromArgb(banner is null ? 125 : 50, onBannerColor));

		if (customPlayset.Color.HasValue)
		{
			e.BackColor = FormDesign.Design.AccentBackColor.MergeColor(customPlayset.Color.Value, 85);
		}

		base.OnPaintItemGrid(e);

		if (banner is null)
		{
			using var brush = new SolidBrush(customPlayset.Color ?? FormDesign.Design.AccentColor);

			e.Graphics.FillRoundedRectangle(brush, e.Rects.Thumbnail, (int)(5 * UI.FontScale));

			using var icon = customPlayset.Usage.GetIcon().Get(e.Rects.Thumbnail.Width * 3 / 4).Color(onBannerColor);

			e.Graphics.DrawImage(icon, e.Rects.Thumbnail.CenterR(icon.Size));
		}
		else
		{
			if (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.Thumbnail.Contains(CursorLocation))
			{
				using (var blurredBanner = banner.Blur(doNotDispose: true, preferredSize: e.Rects.Thumbnail.Size))
				{
					e.Graphics.DrawRoundedImage(blurredBanner, e.Rects.Thumbnail, (int)(5 * UI.FontScale));
				}

				using var brush = new SolidBrush(Color.FromArgb(75, onBannerColor.GetAccentColor()));
				e.Graphics.FillRoundedRectangle(brush, e.Rects.Thumbnail, (int)(5 * UI.FontScale));
			}
			else
			{
				e.Graphics.DrawRoundedImage(banner, e.Rects.Thumbnail, (int)(5 * UI.FontScale));
			}
		}


		DrawFavoriteButton(e, customPlayset, onBannerColor, onBannerBrush);

		//if (!ReadOnly && e.Rects.Icon.Contains(CursorLocation))
		//{
		//	e.Graphics.FillRoundedRectangle(e.Rects.Icon.Gradient(Color.FromArgb(40, textColor), 1.5F), e.Rects.Icon, 4);
		//}

		using var profileIcon = customPlayset.GetIcon().Get(e.Rects.Icon.Height * 3 / 4);

		//e.Graphics.DrawImage(profileIcon.Color(!ReadOnly && e.Rects.Icon.Contains(CursorLocation) ? FormDesign.Design.ActiveColor : textColor), e.Rects.Icon.CenterR(profileIcon.Size));

		using var textBrush = new SolidBrush(FormDesign.Design.ForeColor);
		using var fadedBrush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.ForeColor));
		using var textFont = UI.Font(9.5F, FontStyle.Bold).FitTo(e.Item.Name, e.Rects.Text, e.Graphics);
		using var smallTextFont = UI.Font(7.25F);
		using var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(e.Item.Name, textFont, textBrush, e.Rects.Text);

		e.Graphics.DrawString(customPlayset.Usage > 0 ? Locale.UsagePlayset.Format(LocaleHelper.GetGlobalText(customPlayset.Usage.ToString())) : Locale.GenericPlayset, smallTextFont, fadedBrush, new Point(e.Rects.Text.X, e.Rects.Text.Y + (int)e.Graphics.Measure(e.Item.Name, textFont, e.Rects.Text.Width).Height));

		//	e.Graphics.DrawString($"{Locale.ContainCount.FormatPlural(e.Item.ModCount, Locale.Mod.FormatPlural(e.Item.ModCount).ToLower())} • {e.Item.ModSize.SizeString(0)}", smallTextFont, textBrush, e.Rects.Content, centerFormat);
		e.Graphics.DrawLabel($"{Locale.ContainCount.FormatPlural(e.Item.ModCount, Locale.Mod.FormatPlural(e.Item.ModCount).ToLower())} • {e.Item.ModSize.SizeString(0)}", null, FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 50), e.Rects.Content, ContentAlignment.MiddleCenter);

		var isHovered = e.Rects.DotsRect.Contains(CursorLocation);
		using var img = IconManager.GetIcon("I_VertialMore", e.Rects.DotsRect.Height * 3 / 4).Color(isHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor);

		e.Graphics.DrawImage(img, e.Rects.DotsRect.CenterR(img.Size));

		var labelRects = e.Rects.Content;

#if CS1
		labelRects.Y += e.Graphics.DrawLabel(Locale.ContainCount.FormatPlural(e.Item.AssetCount, Locale.Asset.FormatPlural(e.Item.AssetCount).ToLower()), IconManager.GetSmallIcon("I_Assets"), FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 75), labelRects, ContentAlignment.TopLeft).Height + GridPadding.Top;
#endif

		if (customPlayset.OnlineInfo?.Author?.Name is not null and not "")
		{
			var name = customPlayset.OnlineInfo.Author.Name;

			using var userIcon = IconManager.GetSmallIcon("I_User");

			e.Rects.Author = e.Graphics.DrawLabel(name, userIcon, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor, 25), labelRects, ContentAlignment.TopLeft);
		}

		//if (e.Item == _profileManager.CurrentPlayset)
		//{
		//	e.Graphics.DrawLabel(Locale.ActivePlayset.One.ToUpper(), null, FormDesign.Design.GreenColor, e.Rects.Content, ContentAlignment.TopRight);
		//}
#if CS1
		else if (e.Item.IsMissingItems)
		{
			using var icon = IconManager.GetSmallIcon("I_MinorIssues");
			e.Graphics.DrawLabel(Locale.IncludesItemsYouDoNotHave, icon, FormDesign.Design.RedColor.MergeColor(FormDesign.Design.BackColor, 50), e.Rects.Content, ContentAlignment.TopRight);
		}
#endif

		var loadText = ReadOnly ? _playsetManager.Playsets.Any(x => x.Name!.Equals(e.Item.Name, StringComparison.InvariantCultureIgnoreCase)) ? Locale.UpdatePlayset : Locale.DownloadPlayset : Locale.ActivatePlayset;
		var loadIcon = new DynamicIcon(downloading == e.Item && ReadOnly ? "I_Wait" : ReadOnly && _playsetManager.Playsets.Any(x => x.Name!.Equals(e.Item.Name, StringComparison.InvariantCultureIgnoreCase)) ? "I_Refresh" : "I_Install");
		using var importIcon = ReadOnly ? loadIcon.Default : loadIcon.Get(e.Rects.Folder.Height * 3 / 4);
		var loadSize = SlickButton.GetSize(e.Graphics, importIcon, loadText, Font, null);

		if (!ReadOnly)
		{
			loadSize.Height = e.Rects.Folder.Height;
		}

		//e.Rects.ActivateButton = e.Rects.Content.Align(loadSize, ContentAlignment.BottomRight);

		//SlickButton.DrawButton(e, e.Rects.ActivateButton, loadText, Font, importIcon, null, e.Rects.ActivateButton.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal);

		if (ReadOnly)
		{
			return;
		}

		//using var folderIcon = IconManager.GetIcon("I_Folder", e.Rects.Folder.Height * 3 / 4);

		e.Rects.Folder.X = e.Rects.ActivateButton.X - GridPadding.Left - e.Rects.Folder.Width;

		if (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.Thumbnail.Contains(CursorLocation))
		{
			if (e.Rects.EditThumbnail.Contains(CursorLocation))
			{
				e.Graphics.FillRoundedRectangle(onBannerBrush, e.Rects.EditThumbnail, GridPadding.Left);
			}

			using var editIcon = IconManager.GetIcon("I_EditImage", e.Rects.Favorite.Height * 3 / 4);

			e.Graphics.DrawImage(editIcon.Color(e.Rects.EditThumbnail.Contains(CursorLocation) && banner is not null ? FormDesign.Design.ActiveColor : onBannerColor), e.Rects.EditThumbnail.CenterR(editIcon.Size));


			if (e.Rects.EditSettings.Contains(CursorLocation))
			{
				e.Graphics.FillRoundedRectangle(onBannerBrush, e.Rects.EditSettings, GridPadding.Left);
			}

			using var settingsIcon = IconManager.GetIcon("I_Cog", e.Rects.Favorite.Height * 3 / 4);

			e.Graphics.DrawImage(settingsIcon.Color(e.Rects.EditSettings.Contains(CursorLocation) && banner is not null ? FormDesign.Design.ActiveColor : onBannerColor), e.Rects.EditSettings.CenterR(editIcon.Size));
		}

		//SlickButton.DrawButton(e, e.Rects.Folder, string.Empty, Font, folderIcon, null, e.Rects.Folder.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal);

		//if (banner is null)
		//{
		//	e.Rects.Merge.X = e.Rects.ActivateButton.X - e.Rects.Merge.Width - GridPadding.Left;
		//	e.Rects.Exclude.X = e.Rects.Merge.X - e.Rects.Exclude.Width - GridPadding.Left;

		//	using var i_Merge = IconManager.GetIcon("I_Merge", e.Rects.Folder.Height * 3 / 4);
		//	using var i_Exclude = IconManager.GetIcon("I_Exclude", e.Rects.Folder.Height * 3 / 4);
		//	SlickButton.DrawButton(e, e.Rects.Merge, string.Empty, Font, i_Merge, null, e.Rects.Merge.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal);
		//	SlickButton.DrawButton(e, e.Rects.Exclude, string.Empty, Font, i_Exclude, null, e.Rects.Exclude.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal);
		//}

		if (downloading == e.Item)
		{
			using var brush = new SolidBrush(Color.FromArgb(100, FormDesign.Design.BackColor));
			e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.InvertPad(GridPadding), (int)(5 * UI.FontScale));

			DrawLoader(e.Graphics, e.ClipRectangle.CenterR(UI.Scale(new Size(24, 24), UI.FontScale)));
		}

		if (e.Item == _playsetManager.CurrentPlayset)
		{
			using var pen = new Pen(FormDesign.Design.GreenColor, (float)(1.5 * UI.FontScale));
			using var brush = new SolidBrush(FormDesign.Design.GreenColor);

			e.Rects.ActivateButton = e.Rects.ActivateButton.Pad(0, GridPadding.Horizontal, 0, 0);

			e.Graphics.ResetClip();
			e.Graphics.FillRoundedRectangle(brush, e.Rects.ActivateButton.InvertPad(GridPadding), (int)(5 * UI.FontScale), false, false);
			e.Graphics.DrawRoundedRectangle(pen, e.ClipRectangle.InvertPad(GridPadding - new Padding((int)pen.Width - 1)), (int)(5 * UI.FontScale));

			var text = Locale.ActivePlayset.One.ToUpper();
			using var font = UI.Font(9F, FontStyle.Bold).FitTo(text, e.Rects.ActivateButton, e.Graphics);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
			using var textBrush2 = new SolidBrush(brush.Color.GetTextColor());

			e.Graphics.DrawString(text, font, textBrush2, e.Rects.ActivateButton, format);
		}
		else
		{
			e.Graphics.ResetClip();

			SlickButton.Draw(e.Graphics, new ButtonDrawArgs
			{
				Text = Locale.ActivatePlayset.ToString().ToUpper(),
				Icon = "I_Check",
				Rectangle = e.Rects.ActivateButton,
				Padding = UI.Scale(new Padding(8, 4, 8, 4), UI.FontScale),
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				BackgroundColor = e.BackColor,
				ActiveColor = FormDesign.Design.GreenColor
			});
		}
	}

	private void DrawFavoriteButton(ItemPaintEventArgs<IPlayset, Rectangles> e, ICustomPlayset customPlayset, Color onBannerColor, SolidBrush onBannerBrush)
	{
		var favViewRect = ReadOnly ? e.Rects.ViewContents : e.Rects.Favorite;

		if (e.HoverState.HasFlag(HoverState.Hovered) && favViewRect.Contains(CursorLocation))
		{
			e.Graphics.FillRoundedRectangle(onBannerBrush, favViewRect, (int)(5 * UI.FontScale));
		}
		else if (customPlayset.IsFavorite)
		{
			using var favBrush = new SolidBrush(Color.FromArgb(100, FormDesign.Design.ActiveColor));
			e.Graphics.FillRoundedRectangle(favBrush, favViewRect, (int)(5 * UI.FontScale));
		}

		var fav = new DynamicIcon(ReadOnly ? "I_ViewFile" : customPlayset.IsFavorite != favViewRect.Contains(CursorLocation) ? "I_StarFilled" : "I_Star");
		using var favIcon = fav.Get(favViewRect.Height * 3 / 4);

		if (opening == e.Item)
		{
			DrawLoader(e.Graphics, favViewRect.CenterR(favIcon.Size));
		}
		else if (customPlayset.IsFavorite || !GridView || (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.Thumbnail.Contains(CursorLocation)))
		{
			e.Graphics.DrawImage(favIcon.Color(favViewRect.Contains(CursorLocation) != customPlayset.IsFavorite ? FormDesign.Design.ActiveColor : onBannerColor), favViewRect.CenterR(favIcon.Size));
		}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<IPlayset, Rectangles> e)
	{
		var customPlayset = e.Item.GetCustomPlayset();
		var banner = customPlayset.GetThumbnail();
		var onBannerColor = (customPlayset.Color ?? Color.Black).GetTextColor();
		using var onBannerBrush = new SolidBrush(Color.FromArgb(banner is null ? 125 : 50, onBannerColor));

		if (e.Item == _playsetManager.CurrentPlayset)
		{
			e.BackColor = FormDesign.Design.AccentBackColor.MergeColor(FormDesign.Design.GreenColor, e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 80);
		}
		else
		{
			e.BackColor = e.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.AccentBackColor.MergeColor(customPlayset.Color ?? FormDesign.Design.ActiveColor, 85) : FormDesign.Design.AccentBackColor;
		}

		base.OnPaintItemList(e);

		DrawFavoriteButton(e, customPlayset, onBannerColor, onBannerBrush);

		if (banner is null)
		{
			using var brush = new SolidBrush(customPlayset.Color ?? FormDesign.Design.AccentColor);

			e.Graphics.FillRoundedRectangle(brush, e.Rects.Thumbnail, (int)(5 * UI.FontScale));

			using var icon = customPlayset.Usage.GetIcon().Get(e.Rects.Thumbnail.Width * 3 / 4).Color(onBannerColor);

			e.Graphics.DrawImage(icon, e.Rects.Thumbnail.CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(banner, e.Rects.Thumbnail, (int)(5 * UI.FontScale));
		}

		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			Icon = "I_VertialMore",
			BorderRadius = (int)(5 * UI.FontScale),
			Rectangle = e.Rects.DotsRect,
			Cursor = CursorLocation,
			HoverState = e.HoverState,
			BackgroundColor = e.BackColor,
			ActiveColor = e.Item == _playsetManager.CurrentPlayset ? FormDesign.Design.GreenColor : customPlayset.Color ?? FormDesign.Design.ActiveColor
		});

		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			Icon = "I_Cog",
			BorderRadius = (int)(5 * UI.FontScale),
			Rectangle = e.Rects.EditSettings,
			Cursor = CursorLocation,
			HoverState = e.HoverState,
			BackgroundColor = e.BackColor,
			ActiveColor = e.Item == _playsetManager.CurrentPlayset ? FormDesign.Design.GreenColor : customPlayset.Color ?? FormDesign.Design.ActiveColor
		});

		using var pen = new Pen(FormDesign.Design.AccentColor, (int)(UI.FontScale));

		e.Graphics.DrawLine(pen, e.Rects.Content.Right + Padding.Horizontal, e.Rects.Content.Y, e.Rects.Content.Right + Padding.Horizontal, e.Rects.Content.Bottom);

		if (e.Item == _playsetManager.CurrentPlayset)
		{
			using var font = UI.Font(8.25F, FontStyle.Bold);
			var rect = SlickButton.AlignAndDraw(e.Graphics, e.Rects.Content, ContentAlignment.MiddleRight, new ButtonDrawArgs
			{
				Text = Locale.ActivePlayset.One.ToUpper(),
				Padding = UI.Scale(new Padding(2), UI.FontScale),
				Font = font,
				BorderRadius = Padding.Left,
				ColorStyle = ColorStyle.Green,
				ButtonType = ButtonType.Active
			}).Rectangle;

			e.Rects.Content = new Rectangle(e.Rects.Text.Right, e.ClipRectangle.Y, rect.X - e.Rects.Text.Right, e.ClipRectangle.Height);
		}
		else
		{
			e.Rects.ActivateButton = SlickButton.AlignAndDraw(e.Graphics, e.Rects.Content, ContentAlignment.MiddleRight, new ButtonDrawArgs
			{
				Text = Locale.ActivatePlayset.ToString().ToUpper(),
				Icon = "I_Check",
				Padding = UI.Scale(new Padding(4), UI.FontScale),
				Cursor = CursorLocation,
				HoverState = e.HoverState,
				BackgroundColor = e.BackColor,
				ActiveColor = FormDesign.Design.GreenColor
			});

			e.Rects.Content = new Rectangle(e.Rects.Text.Right, e.ClipRectangle.Y, e.Rects.ActivateButton.X - e.Rects.Text.Right, e.ClipRectangle.Height);
		}

		using var textBrush = new SolidBrush(FormDesign.Design.ForeColor);
		using var fadedBrush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.ForeColor));
		using var textFont = UI.Font(8.25F, FontStyle.Bold).FitTo(e.Item.Name, e.Rects.Text, e.Graphics);
		using var smallTextFont = UI.Font(7F);
		using var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(e.Item.Name, textFont, textBrush, e.Rects.Text);

		e.Graphics.DrawString(customPlayset.Usage > 0 ? Locale.UsagePlayset.Format(LocaleHelper.GetGlobalText(customPlayset.Usage.ToString())) : Locale.GenericPlayset, smallTextFont, fadedBrush, new Point(e.Rects.Text.X, e.Rects.Text.Y + (int)e.Graphics.Measure(e.Item.Name, textFont, e.Rects.Text.Width).Height));

		//	e.Graphics.DrawString($"{Locale.ContainCount.FormatPlural(e.Item.ModCount, Locale.Mod.FormatPlural(e.Item.ModCount).ToLower())} • {e.Item.ModSize.SizeString(0)}", smallTextFont, textBrush, e.Rects.Content, centerFormat);
		e.Graphics.DrawLabel($"{Locale.ContainCount.FormatPlural(e.Item.ModCount, Locale.Mod.FormatPlural(e.Item.ModCount).ToLower())} • {e.Item.ModSize.SizeString(0)}", null, FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 50), e.Rects.Content, ContentAlignment.MiddleLeft);


#if CS1
		if (e.Item.IsMissingItems)
		{
			e.Rects.Text.X += e.Graphics.DrawLabel(Locale.IncludesItemsYouDoNotHave, IconManager.GetSmallIcon("I_MinorIssues"), FormDesign.Design.RedColor.MergeColor(FormDesign.Design.BackColor, 50), e.Rects.Text, large ? ContentAlignment.TopLeft : ContentAlignment.BottomLeft).Width + Padding.Left;
		}
#endif
	}

	protected override Rectangles GenerateRectangles(IPlayset item, Rectangle rectangle)
	{
		var rects = new Rectangles(item);

		if (GridView)
		{
			var size = UI.Scale(new Size(32, 32), UI.FontScale);

			rects.ActivateButton = new Rectangle(rectangle.X, rectangle.Bottom - (int)(24 * UI.FontScale), rectangle.Width, (int)(24 * UI.FontScale));
			rects.Thumbnail = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Width - GridPadding.Top);
			rects.Content = new Rectangle(rectangle.X, rectangle.Y + rects.Thumbnail.Height + GridPadding.Top, rectangle.Width, rectangle.Height - rects.Thumbnail.Height - GridPadding.Top);

			rects.Icon = rects.Thumbnail.Pad(GridPadding).Pad(0, 0, 0, 0).Align(size, ContentAlignment.BottomLeft);

			rects.Favorite = rects.Thumbnail.Pad(GridPadding).Pad(0, 0, 0, 0).Align(size, ContentAlignment.TopLeft);

			rects.Text = new Rectangle(rects.Thumbnail.X + GridPadding.Left, rects.Thumbnail.Bottom + (GridPadding.Top * 2), rects.Thumbnail.Width, size.Height);
			rects.Content = new Rectangle(rectangle.X, rects.Text.Bottom + GridPadding.Top, rectangle.Width, rects.ActivateButton.Y - rects.Text.Bottom - (GridPadding.Top * 2));

			rects.EditThumbnail = rects.Thumbnail.Pad(GridPadding).Align(size, ContentAlignment.TopRight);
			rects.EditSettings = rects.Thumbnail.Pad(GridPadding).Align(size, ContentAlignment.BottomRight);

			rects.DotsRect = rects.Text.Align(UI.Scale(new Size(24, 24), UI.FontScale), ContentAlignment.TopRight);
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

			rects.Content = Rectangle.FromLTRB(rects.Text.Right, rectangle.Y, rects.EditSettings.X - Padding.Horizontal * 2 - (int)UI.FontScale, rectangle.Bottom);
		}

		if (ReadOnly)
		{
			rects.ViewContents = rects.Favorite;
			rects.Folder = rects.Favorite = rects.Exclude = rects.Merge = rects.EditThumbnail = default;
		}

		return rects;
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
		public Rectangle Icon;
		public Rectangle Folder;
		public Rectangle Text;
		public Rectangle Content;
		public Rectangle ActivateButton;
		public Rectangle Exclude;
		public Rectangle Merge;
		public Rectangle EditThumbnail;
		public Rectangle Author;
		public Rectangle ViewContents;
		public Rectangle DotsRect;
		public Rectangle EditSettings;

		public bool IsHovered(Control instance, Point location)
		{
			return
				Favorite.Contains(location) ||
				(Icon.Contains(location) && !(instance as PlaysetListControl)!.ReadOnly) ||
				ActivateButton.Contains(location) ||
				Merge.Contains(location) ||
				Author.Contains(location) ||
				EditThumbnail.Contains(location) ||
				Exclude.Contains(location) ||
				ViewContents.Contains(location) ||
				DotsRect.Contains(location) ||
				EditSettings.Contains(location) ||
				Folder.Contains(location);
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			if (Favorite.Contains(location))
			{
				text = Item.GetCustomPlayset().IsFavorite ? Locale.UnFavoriteThisPlayset : Locale.FavoriteThisPlayset;
				point = Favorite.Location;
				return true;
			}

			if (Icon.Contains(location) && !(instance as PlaysetListControl)!.ReadOnly)
			{
				text = Locale.ChangePlaysetColor;
				point = Icon.Location;
				return true;
			}

			if (ActivateButton.Contains(location))
			{
				text = (instance as PlaysetListControl)!.ReadOnly ? ServiceCenter.Get<IPlaysetManager>().Playsets.Any(x => x.Name!.Equals(Item.Name, StringComparison.InvariantCultureIgnoreCase)) ? Locale.UpdatePlaysetTip : Locale.DownloadPlaysetTip : Locale.ActivatePlayset;
				point = ActivateButton.Location;
				return true;
			}

			if (Folder.Contains(location))
			{
				text = Locale.OpenPlaysetFolder;
				point = Folder.Location;
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

			text = string.Empty;
			point = default;

			return false;
		}
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
				Invalidate();
			}

			return;
		}

		drgevent.Effect = DragDropEffects.None;
		Invalidate();
	}

	protected override void OnDragLeave(EventArgs e)
	{
		base.OnDragLeave(e);

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

		Invalidate();
	}
}
