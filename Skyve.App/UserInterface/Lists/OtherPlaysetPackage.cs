using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class OtherPlaysetPackage : SlickStackedListControl<ICustomPlayset, OtherPlaysetPackage.Rectangles>
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

	protected override IEnumerable<DrawableItem<ICustomPlayset, Rectangles>> OrderItems(IEnumerable<DrawableItem<ICustomPlayset, Rectangles>> items)
	{
		return items.OrderByDescending(x => x.Item.DateUpdated);
	}

	protected override bool IsItemActionHovered(DrawableItem<ICustomPlayset, Rectangles> item, Point location)
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

	protected override async void OnItemMouseClick(DrawableItem<ICustomPlayset, Rectangles> item, MouseEventArgs e)
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
				await _packageUtil.SetEnabled(Package, !_packageUtil.IsEnabled(Package, item.Item.Id), item.Item.Id);
			}

			item.Loading = false;

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

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		//if (Loading)
		//{
		//	base.OnPaint(e);
		//}
		//else if (!Items.Any())
		//{
		//	e.Graphics.DrawString(Locale.NoDlcsNoInternet, UI.Font(9.75F, FontStyle.Italic), new SolidBrush(ForeColor), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		//}
		//else if (!SafeGetItems().Any())
		//{
		//	e.Graphics.DrawString(Locale.NoDlcsOpenGame, UI.Font(9.75F, FontStyle.Italic), new SolidBrush(ForeColor), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		//}
		//else
		//{
		//	base.OnPaint(e);
		//}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<ICustomPlayset, Rectangles> e)
	{
		if (e.Item == _playsetManager.CurrentPlayset)
		{
			e.BackColor = BackColor.MergeColor(FormDesign.Design.GreenColor, e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 75);
		}
		else
		{
			e.BackColor = e.HoverState.HasFlag(HoverState.Hovered) ? BackColor.MergeColor(FormDesign.Design.ActiveColor, 95) : BackColor;
		}

		base.OnPaintItemList(e);

		var localIdentity = Package.GetLocalPackageIdentity();
		var isIncluded = _packageUtil.IsIncluded(Package, out var partialIncluded, e.Item.Id) || partialIncluded;
		var isEnabled = _packageUtil.IsEnabled(Package, e.Item.Id);

		DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out var activeColor);

		DrawIcon(e);

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
	}

	private void DrawIcon(ItemPaintEventArgs<ICustomPlayset, Rectangles> e)
	{
		var iconColor = FormDesign.Design.IconColor;

		if (e.Item.Color != null)
		{
			iconColor = e.Item.Color.Value.GetTextColor();

			using var gradientBrush = e.Rects.IconRect.Gradient(e.Item.Color.Value, 1.5F);
			e.Graphics.FillRoundedRectangle(gradientBrush, e.Rects.IconRect.Pad(0, Padding.Vertical, 0, Padding.Vertical), 4);
		}

		using var playsetIcon = e.Item.GetIcon().Get(e.Rects.IncludedRect.Height * 3 / 4);

		e.Graphics.DrawImage(playsetIcon.Color(iconColor), e.Rects.IconRect.CenterR(playsetIcon.Size));
	}

	protected override Rectangles GenerateRectangles(ICustomPlayset item, Rectangle rectangle)
	{
		var rects = new Rectangles(item)
		{
			IncludedRect = rectangle.Pad(Padding + Padding).Align(UI.Scale(new Size(24, 24), UI.FontScale), ContentAlignment.MiddleLeft),
			LoadRect = rectangle.Pad(0, 0, Padding.Right, 0).Align(new Size(ItemHeight, ItemHeight), ContentAlignment.TopRight)
		};

		rects.IconRect = rectangle.Pad(rects.IncludedRect.Right + (2 * Padding.Left)).Align(rects.IncludedRect.Size, ContentAlignment.MiddleLeft);

		rects.TextRect = new Rectangle(rects.IconRect.Right + Padding.Left, rectangle.Y, rects.LoadRect.X - rects.IconRect.Right - (2 * Padding.Left), rectangle.Height);

		return rects;
	}

#if CS2
	private void DrawIncludedButton(ItemPaintEventArgs<ICustomPlayset, Rectangles> e, bool isIncluded, bool isPartialIncluded, bool isEnabled, ILocalPackageIdentity? localIdentity, out Color activeColor)
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

		var icon = new DynamicIcon(_subscriptionsManager.IsSubscribing(Package) ? "I_Wait" : isPartialIncluded ? "I_Slash" : isEnabled ? "I_Ok" : !isIncluded ? "I_Add" : "I_Enabled");
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

	public class Rectangles : IDrawableItemRectangles<ICustomPlayset>
	{
		public Rectangle IncludedRect;
		public Rectangle IconRect;
		public Rectangle LoadRect;
		public Rectangle TextRect;

		public ICustomPlayset Item { get; set; }

		public Rectangles(ICustomPlayset item)
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
