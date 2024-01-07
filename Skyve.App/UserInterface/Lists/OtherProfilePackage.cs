using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class OtherProfilePackage : SlickStackedListControl<ICustomPlayset, OtherProfilePackage.Rectangles>
{
	public IPackageIdentity Package { get; }

	private readonly IPlaysetManager  _playsetManager;
	private readonly IPackageUtil _packageUtil;
	private readonly INotifier _notifier;

	public OtherProfilePackage(IPackageIdentity package)
	{
		ServiceCenter.Get(out _notifier, out _packageUtil, out _playsetManager);
		HighlightOnHover = true;
		SeparateWithLines = true;
		Package = package;
		SetItems(_playsetManager.Playsets);

		_notifier.PlaysetUpdated += ProfileManager_ProfileUpdated;
		_notifier.PlaysetChanged += ProfileManager_ProfileUpdated;
	}

	private void ProfileManager_ProfileUpdated()
	{
		Invalidate();
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
			setTip(Locale.LoadPlayset, rects.LoadRect);
			return true;
		}

		void setTip(string text, Rectangle rectangle) => SlickTip.SetTo(this, text, offset: new Point(rectangle.X, item.Bounds.Y));

		return false;
	}

	protected override void OnItemMouseClick(DrawableItem<ICustomPlayset, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		var rects = item.Rectangles;

		if (rects.IncludedRect.Contains(e.Location))
		{
			var isIncluded = _packageUtil.IsIncluded(Package, item.Item.Id);
			_packageUtil.SetIncluded(Package, !isIncluded, item.Item.Id);
		}

		if (rects.LoadRect.Contains(e.Location))
		{
			_playsetManager.SetCurrentPlayset(item.Item);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (Loading)
		{
			base.OnPaint(e);
		}
		else if (!Items.Any())
		{
			e.Graphics.DrawString(Locale.NoDlcsNoInternet, UI.Font(9.75F, FontStyle.Italic), new SolidBrush(ForeColor), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
		else if (!SafeGetItems().Any())
		{
			e.Graphics.DrawString(Locale.NoDlcsOpenGame, UI.Font(9.75F, FontStyle.Italic), new SolidBrush(ForeColor), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
		else
		{
			base.OnPaint(e);
		}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<ICustomPlayset, Rectangles> e)
	{
		var large = false;
		var rects = e.Rects;
		var isPressed = e.HoverState.HasFlag(HoverState.Pressed);

		e.HoverState &= ~HoverState.Pressed;

		base.OnPaintItemList(e);

		var isIncluded = _packageUtil.IsIncluded(Package, e.Item.Id);

		if (isIncluded)
		{
			e.Graphics.FillRoundedRectangle(rects.IncludedRect.Gradient(Color.FromArgb(rects.IncludedRect.Contains(CursorLocation) ? 150 : 255, FormDesign.Design.GreenColor), 1.5F), rects.IncludedRect.Pad(0, Padding.Vertical, 0, Padding.Vertical), 4);
		}
		else if (rects.IncludedRect.Contains(CursorLocation))
		{
			e.Graphics.FillRoundedRectangle(rects.IncludedRect.Gradient(Color.FromArgb(20, ForeColor), 1.5F), rects.IncludedRect.Pad(0, Padding.Vertical, 0, Padding.Vertical), 4);
		}

		var incl = new DynamicIcon(isIncluded ? "I_Ok" : "I_Enabled");
		using var icon = large ? incl.Large : incl.Get(rects.IncludedRect.Height / 2);

		e.Graphics.DrawImage(icon.Color(rects.IncludedRect.Contains(CursorLocation) ? FormDesign.Design.ActiveColor : isIncluded ? FormDesign.Design.ActiveForeColor : ForeColor), rects.IncludedRect.CenterR(icon.Size));

		var iconColor = FormDesign.Design.IconColor;

		if (e.Item.Color != null)
		{
			iconColor = e.Item.Color.Value.GetTextColor();

			e.Graphics.FillRoundedRectangle(rects.IconRect.Gradient(e.Item.Color.Value, 1.5F), rects.IconRect.Pad(0, Padding.Vertical, 0, Padding.Vertical), 4);
		}

		using var profileIcon = e.Item.GetIcon().Default;

		e.Graphics.DrawImage(profileIcon.Color(iconColor), rects.IconRect.CenterR(profileIcon.Size));

		e.Graphics.DrawString(e.Item.Name, UI.Font(large ? 11.25F : 9F, FontStyle.Bold), new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : ForeColor), rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center });

		var rect = DrawLabel(e, Locale.IncludedCount.FormatPlural(e.Item.AssetCount, Locale.Asset.FormatPlural(e.Item.AssetCount).ToLower()), IconManager.GetSmallIcon("I_Assets"), FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 50), rects.TextRect, ContentAlignment.MiddleRight);
		rect = DrawLabel(e, Locale.IncludedCount.FormatPlural(e.Item.ModCount, Locale.Mod.FormatPlural(e.Item.ModCount).ToLower()), IconManager.GetSmallIcon("I_Mods"), FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 50), rects.TextRect.Pad(0, 0, rect.Width + Padding.Left, 0), ContentAlignment.MiddleRight);

		if (e.Item == _playsetManager.CurrentPlayset)
		{
			DrawLabel(e, Locale.CurrentPlayset, IconManager.GetSmallIcon("I_Ok"), FormDesign.Design.ActiveColor, rects.TextRect.Pad(0, 0, rects.TextRect.Right - rect.X + Padding.Left, 0), ContentAlignment.MiddleRight);
		}

		SlickButton.DrawButton(e, rects.LoadRect, string.Empty, Font, IconManager.GetIcon("I_Import"), null, rects.LoadRect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal);

		if (!isIncluded)
		{
			var filledRect = e.ClipRectangle.Pad(0, -Padding.Top, 0, -Padding.Bottom);
			e.Graphics.SetClip(filledRect);
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 30 : 85, BackColor)), filledRect);
		}
	}

	private Rectangle DrawLabel(ItemPaintEventArgs<ICustomPlayset, Rectangles> e, string? text, Bitmap? icon, Color color, Rectangle rectangle, ContentAlignment alignment)
	{
		if (text == null)
		{
			return Rectangle.Empty;
		}

		var large = false;
		var size = e.Graphics.Measure(text, UI.Font(large ? 9F : 7.5F)).ToSize();

		if (icon is not null)
		{
			size.Width += icon.Width + Padding.Left;
		}

		size.Width += Padding.Left;

		rectangle = rectangle.Pad(Padding).Align(size, alignment);

		using var backBrush = rectangle.Gradient(color);
		using var foreBrush = new SolidBrush(color.GetTextColor());

		e.Graphics.FillRoundedRectangle(backBrush, rectangle, (int)(3 * UI.FontScale));
		e.Graphics.DrawString(text, UI.Font(large ? 9F : 7.5F), foreBrush, icon is null ? rectangle : rectangle.Pad(icon.Width + Padding.Left * 2 - 2, 0, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

		if (icon is not null)
		{
			e.Graphics.DrawImage(icon.Color(color.GetTextColor()), rectangle.Pad(Padding.Left, 0, 0, 0).Align(icon.Size, ContentAlignment.MiddleLeft));
		}

		return rectangle;
	}

	protected override Rectangles GenerateRectangles(ICustomPlayset item, Rectangle rectangle)
	{
		var rects = new Rectangles(item)
		{
			IncludedRect = rectangle.Pad(1 * Padding.Left, 0, 0, 0).Align(new Size(rectangle.Height - 2, rectangle.Height - 2), ContentAlignment.MiddleLeft),
			LoadRect = rectangle.Pad(0, 0, Padding.Right, 0).Align(new Size(ItemHeight, ItemHeight), ContentAlignment.TopRight)
		};

		rects.IconRect = rectangle.Pad(rects.IncludedRect.Right + 2 * Padding.Left).Align(rects.IncludedRect.Size, ContentAlignment.MiddleLeft);

		rects.TextRect = new Rectangle(rects.IconRect.Right + Padding.Left, rectangle.Y, rects.LoadRect.X - rects.IconRect.Right - 2 * Padding.Left, rectangle.Height);

		return rects;
	}

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
