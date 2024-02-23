using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class OtherPlaysetPackage : SlickStackedListControl<IPlayset, OtherPlaysetPackage.Rectangles>
{
	public IPackageIdentity Package { get; }

	private readonly IPlaysetManager _playsetManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IModLogicManager _modLogicManager;
	private readonly IModUtil _modUtil;
	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly INotifier _notifier;

	public OtherPlaysetPackage(IPackageIdentity package)
	{
		ServiceCenter.Get(out _notifier, out _packageUtil, out _playsetManager, out _modLogicManager, out _modUtil, out _subscriptionsManager);
		SeparateWithLines = true;
		Package = package;
		SetItems(_playsetManager.Playsets);

		_notifier.PlaysetUpdated += Notifier_PlaysetUpdated;
		_notifier.PlaysetChanged += Notifier_PlaysetUpdated;
	}

	private void Notifier_PlaysetUpdated()
	{
		Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notifier.PlaysetUpdated -= Notifier_PlaysetUpdated;
			_notifier.PlaysetChanged -= Notifier_PlaysetUpdated;
		}

		base.Dispose(disposing);
	}

	protected override void UIChanged()
	{
		ItemHeight = 28;

		base.UIChanged();

		Padding = UI.Scale(new Padding(3, 1, 3, 1), UI.FontScale);
	}

	protected override void CanDrawItemInternal(CanDrawItemEventArgs<IPlayset> args)
	{
		base.CanDrawItemInternal(args);

		if (!args.DoNotDraw)
		{
			var cr = Package.GetPackageInfo();
			var playsetUsage = args.Item.GetCustomPlayset().Usage;

			if (cr is not null && playsetUsage > 0 && cr.Usage.HasFlag(playsetUsage))
			{
				args.DoNotDraw = true;
			}
		}
	}

	protected override IEnumerable<DrawableItem<IPlayset, Rectangles>> OrderItems(IEnumerable<DrawableItem<IPlayset, Rectangles>> items)
	{
		return items.OrderByDescending(x => x.Item.DateUpdated);
	}

	protected override bool IsItemActionHovered(DrawableItem<IPlayset, Rectangles> item, Point location)
	{
		var rects = item.Rectangles;

		if (rects.IncludedRect.Contains(location))
		{
			setTip(string.Format(Locale.IncludeExcludeOtherPlayset, Package, item.Item), rects.IncludedRect);
			return true;
		}
		else if (rects.LoadRect.Contains(location))
		{
			setTip(Locale.ActivatePlayset, rects.LoadRect);
			return true;
		}

		void setTip(string text, Rectangle rectangle) => SlickTip.SetTo(this, text, offset: new Point(rectangle.X, item.Bounds.Y));

		return false;
	}

	protected override async void OnItemMouseClick(DrawableItem<IPlayset, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		if (item.Rectangles.IncludedRect.Contains(e.Location))
		{
			var isIncluded = _packageUtil.IsIncluded(Package, out var partialIncluded, item.Item.Id) && !partialIncluded;

			Loading = item.Loading = true;

			if (!isIncluded)
			{
				await _packageUtil.SetIncluded(Package, !isIncluded, item.Item.Id);
			}
			else
			{
				var enable = !_packageUtil.IsEnabled(Package, item.Item.Id);

				if (enable || !_modLogicManager.IsRequired(Package.GetLocalPackageIdentity(), _modUtil))
				{
					await _packageUtil.SetEnabled(Package, enable, item.Item.Id);
				}
			}

			item.Loading = false;
			Invalidate(item.Item);

			Loading = SafeGetItems().Any(x => x.Loading);
		}

		if (item.Rectangles.LoadRect.Contains(e.Location))
		{
			Loading = item.Loading = true;

			await _packageUtil.SetIncluded(Package, false, item.Item.Id);

			item.Loading = false;

			Loading = SafeGetItems().Any(x => x.Loading);
		}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<IPlayset, Rectangles> e)
	{
		var customPlayset = e.Item.GetCustomPlayset();

		base.OnPaintItemList(e);

		if (e.Item == _playsetManager.CurrentPlayset)
		{
			e.BackColor = FormDesign.Design.AccentBackColor.MergeColor(FormDesign.Design.GreenColor, e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 80);

			using var brush = new SolidBrush(e.BackColor);
			e.Graphics.FillRectangle(brush, e.ClipRectangle.Pad(Padding.Left, 0, Padding.Right, 0));
		}
		else if (e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.BackColor = FormDesign.Design.AccentBackColor.MergeColor(customPlayset.Color ?? FormDesign.Design.ActiveColor, 85);

			using var brush = new SolidBrush(e.BackColor);
			e.Graphics.FillRectangle(brush, e.ClipRectangle.Pad(Padding.Left, 0, Padding.Right, 0));
		}

		var localIdentity = Package.GetLocalPackageIdentity();
		var isIncluded = _packageUtil.IsIncluded(Package, out var partialIncluded, e.Item.Id) || partialIncluded;
		var isEnabled = _packageUtil.IsEnabled(Package, e.Item.Id);

		DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out var activeColor);

		DrawIcon(e, customPlayset);

		using var font = UI.Font(9F, FontStyle.Bold);
		using var textBrush = new SolidBrush(e.BackColor.GetTextColor());
		using var stringFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(e.Item.Name, font, textBrush, e.Rects.TextRect, stringFormat);

		if (e.Item == _playsetManager.CurrentPlayset)
		{
			var rect = e.Rects.TextRect;
			var textSize = e.Graphics.Measure(e.Item.Name, font);
			using var smallFont = UI.Font(6.75F);

			rect.X += (int)textSize.Width + Padding.Left;

			SlickButton.AlignAndDraw(e.Graphics, rect, ContentAlignment.MiddleLeft, new ButtonDrawArgs
			{
				Text = Locale.ActivePlayset.One.ToUpper(),
				Padding = UI.Scale(new Padding(1), UI.FontScale),
				Font = smallFont,
				BorderRadius = Padding.Left,
				ColorStyle = ColorStyle.Green,
				ButtonType = ButtonType.Active
			});
		}

		if (isIncluded)
		{
			e.Rects.LoadRect = SlickButton.AlignAndDraw(e.Graphics, e.ClipRectangle.Pad(0, 0, Padding.Right * 2, 0), ContentAlignment.MiddleRight, new ButtonDrawArgs
			{
				Icon = "I_X",
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				Size = e.Rects.IncludedRect.Size,
				ColorStyle = ColorStyle.Red,
				ButtonType = ButtonType.Hidden
			}).Rectangle;
		}
		else
		{
			e.Rects.LoadRect = default;
		}
	}

	private void DrawIcon(ItemPaintEventArgs<IPlayset, Rectangles> e, ICustomPlayset customPlayset)
	{
		var banner = customPlayset.GetThumbnail();
		var onBannerColor = (customPlayset.Color ?? Color.Black).GetTextColor();

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
	}

	protected override Rectangles GenerateRectangles(IPlayset item, Rectangle rectangle)
	{
		var rects = new Rectangles(item)
		{
			IncludedRect = rectangle.Pad(Padding + Padding).Align(UI.Scale(new Size(24, 24), UI.FontScale), ContentAlignment.MiddleLeft),
			LoadRect = rectangle.Pad(0, 0, Padding.Right, 0).Align(new Size(ItemHeight, ItemHeight), ContentAlignment.TopRight)
		};

		rects.Thumbnail = rectangle.Pad(rects.IncludedRect.Right + (2 * Padding.Left)).Align(rects.IncludedRect.Size, ContentAlignment.MiddleLeft);

		rects.TextRect = new Rectangle(rects.Thumbnail.Right + Padding.Left, rectangle.Y, rects.LoadRect.X - rects.Thumbnail.Right - (2 * Padding.Left), rectangle.Height);

		return rects;
	}

#if CS2
	private void DrawIncludedButton(ItemPaintEventArgs<IPlayset, Rectangles> e, bool isIncluded, bool isPartialIncluded, bool isEnabled, ILocalPackageIdentity? localIdentity, out Color activeColor)
	{
		activeColor = default;

		if (localIdentity is null && Package.IsLocal())
		{
			return; // missing local item
		}

		var required = _modLogicManager.IsRequired(localIdentity, _modUtil);
		var isHovered = e.DrawableItem.Loading || (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.IncludedRect.Contains(CursorLocation));

		if (!required && isIncluded && isHovered)
		{
			isPartialIncluded = false;
			isEnabled = !isEnabled;
		}

		if (isEnabled)
		{
			activeColor = isPartialIncluded ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
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
		e.Graphics.FillRoundedRectangle(brush, e.Rects.IncludedRect, (int)(4 * UI.FontScale));

		if (e.DrawableItem.Loading)
		{
			DrawLoader(e.Graphics, e.Rects.IncludedRect.CenterR(e.Rects.IncludedRect.Height / 2, e.Rects.IncludedRect.Height / 2), iconColor);
			return;
		}

		var icon = new DynamicIcon(isPartialIncluded ? "I_Slash" : isEnabled ? "I_Ok" : !isIncluded ? "I_Add" : "I_Enabled");
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
			var incl = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item) ? "I_Wait" : partialIncluded ? "I_Slash" : isIncluded ? "I_Ok" : package is null ? "I_Add" : "I_Enabled");
			var required = package is not null && _modLogicManager.IsRequired(package, _modUtil);

			DynamicIcon? enabl = null;

			if (_settings.UserSettings.AdvancedIncludeEnable && package is not null)
			{
				enabl = new DynamicIcon(package.IsEnabled() ? "I_Checked" : "I_Checked_OFF");

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

			e.Graphics.FillRoundedRectangle(brush, inclEnableRect, (int)(4 * UI.FontScale));

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
		public Rectangle LoadRect;
		public Rectangle TextRect;

		public IPlayset Item { get; set; }

		public Rectangles(IPlayset item)
		{
			Item = item;
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			text = string.Empty;
			point = default;
			return false;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return
				IncludedRect.Contains(location) ||
				LoadRect.Contains(location);
		}
	}
}
