using Skyve.App.Utilities;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class DlcListControl : SlickStackedListControl<IDlcInfo, DlcListControl.Rectangles>
{
	private readonly ISettings _settings;
	private readonly IDlcManager _dlcManager;

	public DlcListControl()
	{
		GridView = true;
		DynamicSizing = true;
		GridItemSize = new Size(128 * 460 / 215, 128);

		ServiceCenter.Get(out _settings, out _dlcManager);
	}

	protected override void OnViewChanged()
	{
		if (GridView)
		{
			Padding = UI.Scale(new Padding(2, 4, 2, 0), UI.UIScale);
			GridPadding = UI.Scale(new Padding(8), UI.UIScale);
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

	public override void SetItems(IEnumerable<IDlcInfo> items)
	{
		base.SetItems(items);

		Loading = false;
	}

	protected override IEnumerable<IDrawableItem<IDlcInfo>> OrderItems(IEnumerable<IDrawableItem<IDlcInfo>> items)
	{
		return items.OrderByDescending(x => x.Item.ReleaseDate);
	}

	protected override void OnItemMouseClick(DrawableItem<IDlcInfo, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		var rects = item.Rectangles;

		if (e.Button == MouseButtons.Left)
		{
			if (rects.IncludedRect.Contains(e.Location) && _dlcManager.IsAvailable(item.Item))
			{
				_dlcManager.SetIncluded(item.Item, !_dlcManager.IsIncluded(item.Item));
			}
			else if (rects.CenterRect.Contains(e.Location))
			{
				PlatformUtil.OpenUrl($"https://store.steampowered.com/app/{item.Item.Id}");
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!Loading && !AnyVisibleItems())
		{
			e.Graphics.ResetClip();

			using var font = UI.Font(9.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.LabelColor);
			using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

			e.Graphics.DrawString(ItemCount == 0 ? Locale.NoDlcsNoInternet : Locale.NoDlcsOpenGame, font, brush, ClientRectangle, stringFormat);
		}

		base.OnPaint(e);
	}

	protected override void OnPaintItemGrid(ItemPaintEventArgs<IDlcInfo, Rectangles> e)
	{
		var isPressed = e.HoverState.HasFlag(HoverState.Pressed);
		var isIncluded = _dlcManager.IsIncluded(e.Item);

		e.BackColor = BackColor;

		if (e.IsSelected)
		{
			e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
		}

		if (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.CenterRect.Contains(CursorLocation))
		{
			e.Graphics.FillRoundedRectangleWithShadow(e.ClipRectangle, UI.Scale(5), UI.Scale(6), FormDesign.Design.AccentBackColor, Color.FromArgb(20, FormDesign.Design.AccentBackColor.MergeColor(FormDesign.Design.InfoColor)), true);
		}

		DrawThumbnail(e);

		var height = DrawTitleAndTagsAndVersion(e);

		if (string.IsNullOrEmpty(e.Item.Price) && (e.Item.ReleaseDate == DateTime.MinValue || e.Item.ReleaseDate > DateTime.UtcNow.Date))
		{
			e.Graphics.DrawLabel("TBD", null, FormDesign.Design.InfoColor, e.Rects.TextRect.ClipTo(height - e.Rects.TextRect.Y), ContentAlignment.BottomRight);
		}
		else if (!_dlcManager.IsAvailable(e.Item))
		{
			e.Graphics.DrawLabel(e.Item.Price.IfEmpty(Locale.Free), null, FormDesign.Design.GreenColor, e.Rects.TextRect.ClipTo(height - e.Rects.TextRect.Y), ContentAlignment.BottomRight);
		}
		else
		{
			e.Graphics.DrawLabel(Locale.Owned, null, FormDesign.Design.ActiveColor, e.Rects.TextRect.ClipTo(height - e.Rects.TextRect.Y), ContentAlignment.BottomRight);
		}

		var description = System.Net.WebUtility.HtmlDecode(e.Item.Description);
		using var font = UI.Font(7F);
		var brush = new SolidBrush(Color.FromArgb(180, FormDesign.Design.ForeColor));
		e.Graphics.DrawString(description, font, brush, new Rectangle(e.Rects.IconRect.X, height, e.Rects.IconRect.Width, e.ClipRectangle.Height));

		e.DrawableItem.CachedHeight = height - e.ClipRectangle.Y + Padding.Vertical + GridPadding.Vertical + UI.Scale(3) + (int)e.Graphics.Measure(description, font, e.Rects.IconRect.Width).Height;

		//if (isIncluded)
		//{
		//	var outerColor = Color.FromArgb(FormDesign.Design.IsDarkTheme ? 65 : 100, activeColor);

		//	using var pen = new Pen(outerColor, (float)(1.5 * UI.FontScale));

		//	e.Graphics.DrawRoundedRectangle(pen, e.ClipRectangle.InvertPad(GridPadding - new Padding((int)pen.Width)), UI.Scale(5));
		//}
		//else if (!_dlcManager.IsAvailable(e.Item.Id))
		//{
		//	using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
		//	e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
		//}
	}

	private void DrawThumbnail(ItemPaintEventArgs<IDlcInfo, Rectangles> e)
	{
		var thumbnail = e.Item is IThumbnailObject thumbnailObject ? thumbnailObject.GetThumbnail() : null;

		//if (thumbnail is null)
		//{
		//	using var generic = IconManager.GetIcon("Dlc", e.Rects.IconRect.Height).Color(BackColor);
		//	using var brush = new SolidBrush(FormDesign.Design.IconColor);

		//	e.Graphics.FillRoundedRectangle(brush, e.Rects.IconRect, UI.Scale(5));
		//	e.Graphics.DrawImage(generic, e.Rects.IconRect.CenterR(generic.Size));
		//}
		//else
		//{
		drawThumbnail(thumbnail ?? Properties.Resources.Cities2Dlc);
		//}

		void drawThumbnail(Bitmap image)
		{
			using var pen = new Pen(e.BackColor.MergeColor(FormDesign.Design.IsDarkTheme ? Color.FromArgb(30, 30, 30) : Color.Gray), 1.5f) { Alignment = PenAlignment.Center };

			e.Graphics.DrawRoundedImage(image, e.Rects.IconRect, UI.Scale(5), e.BackColor);
			e.Graphics.DrawRoundedRectangle(pen, e.Rects.IconRect, UI.Scale(5));
		}
	}

	private int DrawTitleAndTagsAndVersion(ItemPaintEventArgs<IDlcInfo, Rectangles> e)
	{
		var text = e.Item.Name.RegexRemove("^.+?- ").RegexRemove("(Content )?Creator Pack: ");
		using var font = UI.Font(11.25F, FontStyle.Bold).FitToWidth(text, e.Rects.TextRect, e.Graphics);
		using var brush = new SolidBrush(/*e.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor :*/ ForeColor);

		var nameHeight = e.Graphics.DrawHighResText(text, font, brush, e.Rects.TextRect, 1.5f);
		//e.Graphics.DrawString(text, font, brush, e.Rects.TextRect.ClipTo(e.Rects.TextRect.Height + UI.Scale(4)));

		using var smallFont = UI.Font(8.25F);

		var y = e.Rects.TextRect.Y + nameHeight;

		if (e.Item.Creators?.Any() ?? false)
		{
			using var smallBrush = new SolidBrush(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.ActiveColor));
			var subText = Locale.CreatorPackBy.Format(e.Item.Creators.ListStrings(", "));

			e.Graphics.DrawString(subText, smallFont, smallBrush, new Rectangle(e.Rects.TextRect.X, y, e.Rects.TextRect.Width - UI.Scale(50), e.ClipRectangle.Height));

			y += (int)e.Graphics.Measure(subText, smallFont, e.Rects.TextRect.Width - UI.Scale(50)).Height;
		}

		if (e.Item.ReleaseDate.Year > 1)
		{
			using var smallBrush = new SolidBrush(FormDesign.Design.LabelColor);
			var subText = _settings.UserSettings.ShowDatesRelatively
				? e.Item.ReleaseDate.ToLocalTime().ToRelatedString(true, false)
				: e.Item.ReleaseDate.ToLocalTime().ToString("D");

			e.Graphics.DrawString(subText, smallFont, smallBrush, new Rectangle(e.Rects.TextRect.X, y, e.Rects.TextRect.Width - UI.Scale(50), e.ClipRectangle.Height));

			y += (int)e.Graphics.Measure(subText, smallFont, e.Rects.TextRect.Width - UI.Scale(50)).Height;
		}

		return y + (GridPadding.Bottom / 2);
	}

	private void DrawIncludedButton(ItemPaintEventArgs<IDlcInfo, Rectangles> e, bool isIncluded, out Color activeColor)
	{
		activeColor = default;

		var incl = new DynamicIcon(!_dlcManager.IsAvailable(e.Item) ? "Slash" : isIncluded ? "Ok" : "Enabled");

		if (isIncluded)
		{
			activeColor = !_dlcManager.IsAvailable(e.Item) ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
		}

		Color iconColor;

		if (activeColor == default && e.Rects.IncludedRect.Contains(CursorLocation))
		{
			activeColor = Color.FromArgb(20, ForeColor);
			iconColor = FormDesign.Design.ForeColor;
		}
		else
		{
			iconColor = activeColor.GetTextColor();
		}

		using var brush = e.Rects.IncludedRect.Gradient(activeColor);

		e.Graphics.FillRoundedRectangle(brush, e.Rects.IncludedRect, UI.Scale(4));

		using var includedIcon = incl.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);

		e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
	}

	protected override IDrawableItemRectangles<IDlcInfo> GenerateRectangles(IDlcInfo item, Rectangle rectangle)
	{
		rectangle = rectangle.Pad(GridPadding.Left / 2);

		var rects = new Rectangles(item)
		{
			CenterRect = rectangle,
			IconRect = rectangle.Align(new Size(rectangle.Width, rectangle.Width * 215 / 460), ContentAlignment.TopCenter)
		};

		using var font = UI.Font(11.25F, FontStyle.Bold);

		rects.TextRect = new Rectangle(rects.IconRect.X, rects.IconRect.Bottom+UI.Scale(4), rects.IconRect.Width, font.Height + UI.Scale(2));

		//rects.IncludedRect = rects.TextRect.Align(UI.Scale(new Size(28, 28)), ContentAlignment.TopRight);

		return rects;
	}

	public class Rectangles : IDrawableItemRectangles<IDlcInfo>
	{
		public Rectangle IncludedRect;
		public Rectangle IconRect;
		public Rectangle TextRect;
		public Rectangle CenterRect;

		public IDlcInfo Item { get; set; }

		public Rectangles(IDlcInfo item)
		{
			Item = item;
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			if (IncludedRect.Contains(location) && ServiceCenter.Get<IDlcManager>().IsAvailable(Item))
			{
				if (ServiceCenter.Get<IDlcManager>().IsIncluded(Item))
				{
					text = Locale.ExcludeItem.Format(Item.Name);
				}
				else
				{
					text = Locale.IncludeItem.Format(Item.Name);
				}

				point = IncludedRect.Location;
				return true;
			}

			if (CenterRect.Contains(location))
			{
				text = Locale.ViewXOnSteam.Format(Item.Name.RegexRemove("^.+?- "));
				point = new(IconRect.Location.X - UI.Scale(4), IconRect.Y - UI.Scale(4));
				return true;
			}

			text = string.Empty;
			point = default;
			return false;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return CenterRect.Contains(location);
		}
	}
}
