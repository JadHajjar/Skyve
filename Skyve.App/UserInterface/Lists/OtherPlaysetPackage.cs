using Skyve.App.Interfaces;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class OtherPlaysetPackage : SlickStackedListControl<IPlayset, OtherPlaysetPackage.Rectangles>
{
	public IPackageIdentity Package { get; }

	private readonly IPlaysetManager _playsetManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IModLogicManager _modLogicManager;
	private readonly IModUtil _modUtil;
	private readonly IWorkshopService _workshopService;
	private readonly INotifier _notifier;
	private readonly ISubscriptionsManager _subscriptionsManager;

	public OtherPlaysetPackage(IPackageIdentity package)
	{
		ServiceCenter.Get(out _notifier, out _packageUtil, out _playsetManager, out _modLogicManager, out _modUtil, out _workshopService, out _subscriptionsManager);
		SeparateWithLines = true;
		Package = package;
		SetItems(_playsetManager.Playsets);

		_notifier.PlaysetUpdated += Notifier_PlaysetUpdated;
		_notifier.PlaysetChanged += Notifier_PlaysetUpdated;

		AltStateChanged += AltKeyStateChanged;
	}

	private void AltKeyStateChanged(object sender, EventArgs e)
	{
		Invalidate();
	}

	private void Notifier_PlaysetUpdated()
	{
		Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			AltStateChanged -= AltKeyStateChanged;

			_notifier.PlaysetUpdated -= Notifier_PlaysetUpdated;
			_notifier.PlaysetChanged -= Notifier_PlaysetUpdated;
		}

		base.Dispose(disposing);
	}

	protected override void UIChanged()
	{
		ItemHeight = 35;

		base.UIChanged();

		Padding = UI.Scale(new Padding(3, 1, 3, 1));
	}

	protected override void CanDrawItemInternal(CanDrawItemEventArgs<IPlayset> args)
	{
		base.CanDrawItemInternal(args);

		if (!args.DoNotDraw)
		{
			var cr = Package.GetPackageInfo();
			var playsetUsage = args.Item.GetCustomPlayset().Usage;

			if (cr is not null && playsetUsage > 0 && !cr.Usage.HasFlag(playsetUsage))
			{
				args.DrawableItem.Hidden = true;
			}
		}
	}

	protected override IEnumerable<IDrawableItem<IPlayset>> OrderItems(IEnumerable<IDrawableItem<IPlayset>> items)
	{
		return items.OrderByDescending(x => x.Item.GetCustomPlayset().IsFavorite)
			.ThenByDescending(x => x.Item.GetCustomPlayset().DateUsed);
	}

	protected override async void OnItemMouseClick(DrawableItem<IPlayset, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		if (e.Button == MouseButtons.Right)
		{
			this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(item.Item, true)));
			return;
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		if (item.Rectangles.IncludedRect.Contains(e.Location))
		{
			var isIncluded = _packageUtil.IsIncluded(Package, out var partialIncluded, item.Item.Id, false) && !partialIncluded;

			Loading = item.Loading = true;

			if (!isIncluded || (ModifierKeys.HasFlag(Keys.Alt) && !_modLogicManager.IsRequired(Package.GetLocalPackageIdentity(), _modUtil, item.Item.Id)))
			{
				await _packageUtil.SetIncluded(Package, !isIncluded, item.Item.Id, false);
			}
			else
			{
				var enable = !_packageUtil.IsEnabled(Package, item.Item.Id, false);

				if (enable || !_modLogicManager.IsRequired(Package.GetLocalPackageIdentity(), _modUtil, item.Item.Id))
				{
					await _packageUtil.SetEnabled(Package, enable, item.Item.Id);
				}
			}

			item.Loading = false;
			Invalidate(item.Item);
		}

		if (item.Rectangles.UpdateRect.Contains(e.Location))
		{
			Loading = item.Loading = true;

			await _packageUtil.SetIncluded(Package, true, item.Item.Id, false, false);

			item.Loading = false;
			Invalidate(item.Item);
		}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<IPlayset, Rectangles> e)
	{
		var customPlayset = e.Item.GetCustomPlayset();

		if (e.Item == _playsetManager.CurrentPlayset)
		{
			e.BackColor = FormDesign.Design.AccentBackColor.MergeColor(FormDesign.Design.GreenColor, e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 80);

			using var brush = new SolidBrush(e.BackColor);
			e.Graphics.FillRectangle(brush, e.ClipRectangle.Pad(Padding.Left, 0, Padding.Right, 0));
		}
		else if (e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.BackColor = FormDesign.Design.AccentBackColor.MergeColor(customPlayset.Color ?? FormDesign.Design.ActiveColor, 75);

			using var brush = new SolidBrush(e.BackColor);
			e.Graphics.FillRectangle(brush, e.ClipRectangle.Pad(Padding.Left, 0, Padding.Right, 0));
		}
		else if (customPlayset.Color.HasValue)
		{
			e.BackColor = BackColor.MergeColor(customPlayset.Color.Value, 95);

			using var brush = new SolidBrush(e.BackColor);
			e.Graphics.FillRectangle(brush, e.ClipRectangle.Pad(Padding.Left, 0, Padding.Right, 0));
		}
		else
		{
			e.BackColor = BackColor;
		}

		var localIdentity = Package.GetLocalPackageIdentity();
		var isIncluded = _packageUtil.IsIncluded(Package, out var partialIncluded, e.Item.Id, false) || partialIncluded;
		var isEnabled = _packageUtil.IsEnabled(Package, e.Item.Id, false);
		var versionId = isIncluded ? _packageUtil.GetSelectedVersion(Package, e.Item.Id) : null;
		var version = isIncluded ? Package.GetWorkshopInfo()?.Changelog.FirstOrDefault(x => x.VersionId == versionId)?.Version : null;

		DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out var activeColor);

		DrawIcon(e, customPlayset);

		using var font = UI.Font(9F, FontStyle.Bold);
		using var textBrush = new SolidBrush(e.BackColor.GetTextColor());
		using var stringFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(e.Item.Name, font, textBrush, e.Rects.TextRect, stringFormat);

		if (e.Item.Equals(_playsetManager.CurrentPlayset))
		{
			var rect = e.Rects.TextRect;
			var textSize = e.Graphics.Measure(e.Item.Name, font);
			using var smallFont = UI.Font(6.75F);

			rect.X += (int)textSize.Width + Padding.Left;

			SlickButton.AlignAndDraw(e.Graphics, rect, ContentAlignment.MiddleLeft, new ButtonDrawArgs
			{
				Text = Locale.ActivePlayset.One.ToUpper(),
				Padding = UI.Scale(new Padding(1)),
				Font = smallFont,
				BorderRadius = Padding.Left,
				ColorStyle = ColorStyle.Green,
				ButtonType = ButtonType.Active
			});
		}

		if (!string.IsNullOrEmpty(version))
		{
			var upToDate = version == Package.GetWorkshopInfo()?.Changelog.LastOrDefault().Version;
			var labelRect = e.Graphics.DrawLabel("v" + version, null, Color.FromArgb(80, upToDate ? FormDesign.Design.GreenColor : FormDesign.Design.OrangeColor), e.ClipRectangle.Pad(0, 0, Padding.Right * 2, 0), ContentAlignment.MiddleRight);

			if (!upToDate && !e.DrawableItem.Loading)
			{
				e.Rects.UpdateRect = SlickButton.AlignAndDraw(e.Graphics, e.ClipRectangle.Pad(0, 0, labelRect.Width + (Padding.Horizontal * 3), 0), ContentAlignment.MiddleRight, new ButtonDrawArgs
				{
					Cursor = CursorLocation,
					HoverState = e.HoverState,
					Icon = "ReDownload"
				});
			}
			else
			{
				e.Rects.UpdateRect = default;
			}
		}
	}

	private void DrawIcon(ItemPaintEventArgs<IPlayset, Rectangles> e, ICustomPlayset customPlayset)
	{
		var banner = customPlayset.GetThumbnail();
		var onBannerColor = (customPlayset.Color ?? Color.Black).GetTextColor();

		if (banner is null)
		{
			using var brush = new SolidBrush(customPlayset.Color ?? FormDesign.Design.AccentColor);

			e.Graphics.FillRoundedRectangle(brush, e.Rects.Thumbnail, UI.Scale(4));

			using var icon = customPlayset.Usage.GetIcon().Get(e.Rects.Thumbnail.Width * 3 / 4).Color(onBannerColor);

			e.Graphics.DrawImage(icon, e.Rects.Thumbnail.CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(banner, e.Rects.Thumbnail, UI.Scale(4), customPlayset.Color ?? e.BackColor);
		}
	}

	protected override IDrawableItemRectangles<IPlayset> GenerateRectangles(IPlayset item, Rectangle rectangle, IDrawableItemRectangles<IPlayset> current)
	{
		var rects = new Rectangles(item)
		{
			IncludedRect = rectangle.Pad(Padding + Padding).Align(UI.Scale(new Size(24, 24)), ContentAlignment.MiddleLeft),
		};

		rects.Thumbnail = rectangle.Pad(rects.IncludedRect.Right + (2 * Padding.Left)).Align(rects.IncludedRect.Size, ContentAlignment.MiddleLeft);

		rects.TextRect = new Rectangle(rects.Thumbnail.Right + Padding.Left, rectangle.Y, rectangle.Width - rects.Thumbnail.Right - (2 * Padding.Left), rectangle.Height);

		return rects;
	}

#if CS2
	private void DrawIncludedButton(ItemPaintEventArgs<IPlayset, Rectangles> e, bool isIncluded, bool isPartialIncluded, bool isEnabled, ILocalPackageIdentity? localIdentity, out Color activeColor)
	{
		activeColor = default;

		var required = _modLogicManager.IsRequired(localIdentity, _modUtil, e.Item.Id);
		var isHovered = e.DrawableItem.Loading || (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.IncludedRect.Contains(CursorLocation));

		if (!required && isIncluded && isHovered)
		{
			isPartialIncluded = false;
			isEnabled = !isEnabled;
		}

		if (isIncluded && !required && isHovered && ModifierKeys.HasFlag(Keys.Alt))
		{
			activeColor = FormDesign.Design.RedColor;
		}
		else if (isEnabled)
		{
			activeColor = isPartialIncluded ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
		}
		else if (required)
		{
			activeColor = Color.FromArgb(200, ForeColor.MergeColor(BackColor));
		}

		Color iconColor;

		if (required && activeColor != default)
		{
			iconColor = !FormDesign.Design.IsDarkTheme ? activeColor.MergeColor(ForeColor, 75) : activeColor;
			activeColor = activeColor.MergeColor(BackColor, !FormDesign.Design.IsDarkTheme ? 35 : 20);
		}
		else if (activeColor == default && isHovered)
		{
			iconColor = isIncluded ? isEnabled ? FormDesign.Design.GreenColor : FormDesign.Design.RedColor : FormDesign.Design.ActiveColor;
			activeColor = Color.FromArgb(40, iconColor);
		}
		else
		{
			if (activeColor == default)
			{
				activeColor = Color.FromArgb(20, ForeColor);
			}
			else if (isHovered)
			{
				activeColor = activeColor.MergeColor(ForeColor, 75);
			}

			iconColor = activeColor.GetTextColor();
		}

		using var brush = e.Rects.IncludedRect.Gradient(activeColor);
		e.Graphics.FillRoundedRectangle(brush, e.Rects.IncludedRect, UI.Scale(4));

		if (e.DrawableItem.Loading)
		{
			var rectangle = e.Rects.IncludedRect.CenterR(e.Rects.IncludedRect.Height * 3 / 5, e.Rects.IncludedRect.Height * 3 / 5);
#if CS2
			if (_subscriptionsManager.Status.ModId != Package.Id || _subscriptionsManager.Status.Progress == 0 || !_subscriptionsManager.Status.IsActive)
			{
				DrawLoader(e.Graphics, rectangle, iconColor);
				return;
			}

			var width = Math.Min(Math.Min(rectangle.Width, rectangle.Height), (int)(32 * UI.UIScale));
			var size = (float)Math.Max(2, width / (8D - (Math.Abs(100 - LoaderPercentage) / 50)));
			var drawSize = new SizeF(width - size, width - size);
			var rect = new RectangleF(new PointF(rectangle.X + ((rectangle.Width - drawSize.Width) / 2), rectangle.Y + ((rectangle.Height - drawSize.Height) / 2)), drawSize).Pad(size / 2);
			using var pen = new Pen(iconColor, size) { StartCap = LineCap.Round, EndCap = LineCap.Round };

			e.Graphics.DrawArc(pen, rect, -90, 360 * _subscriptionsManager.Status.Progress);
#else
			DrawLoader(e.Graphics, rectangle, iconColor);
#endif
			return;
		}

		var icon = new DynamicIcon((isHovered && isIncluded && ModifierKeys.HasFlag(Keys.Alt)) ? "X" : isPartialIncluded ? "Slash" : isEnabled ? "Ok" : !isIncluded ? "Add" : "Enabled");
		using var includedIcon = icon.Get(e.Rects.IncludedRect.Height * 3 / 4).Color(iconColor);

		e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
	}
#else
		private void DrawIncludedButton(ItemPaintEventArgs<T, Rectangles> e, bool isIncluded, bool partialIncluded, ILocalPackageData? package, out Color activeColor)
		{
			activeColor = default;

			if (package is null && e.Item.IsLocal())
			{
				return; // missing local item
			}

			var inclEnableRect = e.Rects.EnabledRect == Rectangle.Empty ? e.Rects.IncludedRect : Rectangle.Union(e.Rects.IncludedRect, e.Rects.EnabledRect);
			var incl = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item) ? "Wait" : partialIncluded ? "Slash" : isIncluded ? "Ok" : package is null ? "Add" : "Enabled");
			var required = package is not null && _modLogicManager.IsRequired(package, _modUtil);

			DynamicIcon? enabl = null;

			if (_settings.UserSettings.AdvancedIncludeEnable && package is not null)
			{
				enabl = new DynamicIcon(package.IsEnabled() ? "Checked" : "Checked_OFF");

				if (isIncluded)
				{
					activeColor = partialIncluded ? FormDesign.Design.YellowColor : package.IsEnabled() ? FormDesign.Design.GreenColor : FormDesign.Design.RedColor;
				}
				else if (package.IsEnabled())
				{
					activeColor = FormDesign.Design.YellowColor;
				}
			}
			else if (isIncluded)
			{
				activeColor = partialIncluded ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
			}

			Color iconColor;

			if (required && activeColor != default)
			{
				iconColor = FormDesign.Design.Type is FormDesignType.Light ? activeColor.MergeColor(ForeColor, 75) : activeColor;
				activeColor = activeColor.MergeColor(BackColor, FormDesign.Design.Type is FormDesignType.Light ? 35 : 20);
			}
			else if (activeColor == default && inclEnableRect.Contains(CursorLocation))
{
				activeColor = Color.FromArgb(40, FormDesign.Design.ActiveColor);
				iconColor = FormDesign.Design.ActiveColor;
			}
			else
			{
				if (activeColor == default)
					activeColor = Color.FromArgb(20, ForeColor);
				else if (inclEnableRect.Contains(CursorLocation))
					activeColor = activeColor.MergeColor(ForeColor, 75);

				iconColor = activeColor.GetTextColor();
			}

			using var brush = inclEnableRect.Gradient(activeColor);

			e.Graphics.FillRoundedRectangle(brush, inclEnableRect, UI.Scale(4));

			using var includedIcon = incl.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);
			using var enabledIcon = enabl?.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);

			e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
			if (enabledIcon is not null)
			{
				e.Graphics.DrawImage(enabledIcon, e.Rects.EnabledRect.CenterR(includedIcon.Size));
			}
		}
#endif

	public class Rectangles : IDrawableItemRectangles<IPlayset>
	{
		public Rectangle IncludedRect;
		public Rectangle Thumbnail;
		public Rectangle TextRect;
		public Rectangle UpdateRect;

		public IPlayset Item { get; set; }

		public Rectangles(IPlayset item)
		{
			Item = item;
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			if (IncludedRect.Contains(location))
			{
				var listControl = (instance as OtherPlaysetPackage)!;

				var isIncluded = listControl._packageUtil.IsIncluded(listControl.Package, out var partialIncluded, Item.Id, false) && !partialIncluded;

				if (!isIncluded || ModifierKeys.HasFlag(Keys.Alt))
				{
					if (!isIncluded)
					{
						text = Locale.IncludeItem;
					}
					else if (!listControl._modLogicManager.IsRequired(listControl.Package.GetLocalPackageIdentity(), listControl._modUtil, Item.Id))
					{
						text = Locale.ExcludeItem;
					}
					else
					{
						text = Locale.ThisModIsRequiredYouCantDisableIt;
					}
				}
				else
				{
					var isEnabled = listControl._packageUtil.IsEnabled(listControl.Package, Item.Id, false);

					if (!isEnabled)
					{
						text = Locale.EnableItem + "\r\n\r\n" + Locale.AltClickTo.Format(Locale.ExcludeItem.ToLower());
					}
					else if (!listControl._modLogicManager.IsRequired(listControl.Package.GetLocalPackageIdentity(), listControl._modUtil, Item.Id))
					{
						text = Locale.DisableItem + "\r\n\r\n" + Locale.AltClickTo.Format(Locale.ExcludeItem.ToLower());
					}
					else
					{
						text = Locale.ThisModIsRequiredYouCantDisableIt;
					}
				}

				point = IncludedRect.Location;
				return true;
			}

			if (UpdateRect.Contains(location))
			{
				point = UpdateRect.Location;
				text = Locale.UpdateMod;
				return true;
			}

			text = string.Empty;
			point = default;
			return false;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return IncludedRect.Contains(location) || UpdateRect.Contains(location);
		}
	}
}
