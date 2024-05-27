using Skyve.App.Utilities;

using System.Drawing;
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

		_settings = ServiceCenter.Get<ISettings>();
		_dlcManager = ServiceCenter.Get<IDlcManager>();
	}

	protected override void UIChanged()
	{
		GridItemSize = new Size(450, 120);
		Padding = UI.Scale(new Padding(6), UI.UIScale);
		GridPadding = UI.Scale(new Padding(4), UI.UIScale);

		base.UIChanged();
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (Live)
		{
			Loading = _dlcManager.Dlcs.Count() == 0;
		}
	}

	protected override IEnumerable<DrawableItem<IDlcInfo, Rectangles>> OrderItems(IEnumerable<DrawableItem<IDlcInfo, Rectangles>> items)
	{
		return items.OrderByDescending(x => x.Item.ReleaseDate);
	}

	protected override void OnItemMouseClick(DrawableItem<IDlcInfo, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		var rects = item.Rectangles;

		if (e.Button == MouseButtons.Left)
		{
			if (rects.IncludedRect.Contains(e.Location) && _dlcManager.IsAvailable(item.Item.Id))
			{
				_dlcManager.SetIncluded(item.Item, !_dlcManager.IsIncluded(item.Item));
			}
			else
			{
				PlatformUtil.OpenUrl($"https://store.steampowered.com/app/{item.Item.Id}");
			}
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

	protected override void OnPaintItemGrid(ItemPaintEventArgs<IDlcInfo, Rectangles> e)
	{
		var isPressed = false;
		var isIncluded = _dlcManager.IsIncluded(e.Item);

		if (e.IsSelected)
		{
			e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
		}

		if (e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.BackColor = (e.IsSelected ? e.BackColor : FormDesign.Design.AccentBackColor).MergeColor(FormDesign.Design.ActiveColor, !e.Rects.IncludedRect.Contains(CursorLocation) && e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);

			isPressed = e.HoverState.HasFlag(HoverState.Pressed) && !e.Rects.IncludedRect.Contains(CursorLocation);
		}

		base.OnPaintItemGrid(e);

		DrawThumbnail(e);
		DrawTitleAndTagsAndVersion(e, isPressed);
		DrawIncludedButton(e, isIncluded, out var activeColor);

		var height = (e.Rects.IconRect.Bottom - e.Rects.TextRect.Bottom - GridPadding.Vertical) / 2;
		var dateRect = e.Graphics.DrawLargeLabel(new Point(e.Rects.TextRect.X, e.Rects.TextRect.Bottom + GridPadding.Top), _settings.UserSettings.ShowDatesRelatively ? e.Item.ReleaseDate.ToLocalTime().ToRelatedString(true, false) : e.Item.ReleaseDate.ToLocalTime().ToString("D"), "UpdateTime", height: height, smaller: true);

		e.Graphics.DrawLargeLabel(new Point(e.Rects.TextRect.X, dateRect.Bottom + GridPadding.Top), e.Item.Price.IfEmpty(Locale.Free), null, FormDesign.Design.GreenColor, height: height, smaller: true);

		var description = System.Net.WebUtility.HtmlDecode(e.Item.Description);
		using var font = UI.Font(7F);
		e.Graphics.DrawString(description, font, new SolidBrush(Color.FromArgb(180, isPressed ? FormDesign.Design.ActiveForeColor : ForeColor)), new Rectangle(e.Rects.IconRect.X, e.Rects.IconRect.Bottom + GridPadding.Vertical, e.ClipRectangle.Width, 9999));

		e.DrawableItem.CachedHeight = -e.ClipRectangle.Top + e.Rects.IconRect.Bottom + (GridPadding.Vertical * 4) + (int)e.Graphics.Measure(description, font, e.ClipRectangle.Width).Height;

		if (isIncluded)
		{
			var outerColor = Color.FromArgb(FormDesign.Design.IsDarkTheme ? 65 : 100, activeColor);

			using var pen = new Pen(outerColor, (float)(1.5 * UI.FontScale));

			e.Graphics.DrawRoundedRectangle(pen, e.ClipRectangle.InvertPad(GridPadding - new Padding((int)pen.Width)), (int)(5 * UI.FontScale));
		}
		else if (!_dlcManager.IsAvailable(e.Item.Id))
		{
			using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
			e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
		}
	}

	private void DrawThumbnail(ItemPaintEventArgs<IDlcInfo, Rectangles> e)
	{
		var thumbnail = e.Item.GetThumbnail();

		if (thumbnail is null)
		{
			using var generic = IconManager.GetIcon("Dlc", e.Rects.IconRect.Height).Color(BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, e.Rects.IconRect, (int)(5 * UI.FontScale));
			e.Graphics.DrawImage(generic, e.Rects.IconRect.CenterR(generic.Size));
		}
		else
		{
			drawThumbnail(thumbnail);
		}

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, e.Rects.IconRect, (int)(5 * UI.FontScale), FormDesign.Design.BackColor);
	}

	private void DrawTitleAndTagsAndVersion(ItemPaintEventArgs<IDlcInfo, Rectangles> e, bool isPressed)
	{
		var text = e.Item.Name.Remove("Cities: Skylines - ");
		using var font = UI.Font(10.5F, FontStyle.Bold);
		using var brush = new SolidBrush(isPressed ? FormDesign.Design.ActiveForeColor : (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)) && e.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : ForeColor);

		e.Graphics.DrawString(text, font, brush, e.Rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Near });
	}

	private void DrawIncludedButton(ItemPaintEventArgs<IDlcInfo, Rectangles> e, bool isIncluded, out Color activeColor)
	{
		activeColor = default;

		var incl = new DynamicIcon(!_dlcManager.IsAvailable(e.Item.Id) ? "Slash" : isIncluded ? "Ok" : "Enabled");

		if (isIncluded)
		{
			activeColor = !_dlcManager.IsAvailable(e.Item.Id) ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
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

		e.Graphics.FillRoundedRectangle(brush, e.Rects.IncludedRect, (int)(4 * UI.FontScale));

		using var includedIcon = incl.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);

		e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
	}

	protected override Rectangles GenerateRectangles(IDlcInfo item, Rectangle rectangle)
	{
		var rects = new Rectangles(item)
		{
			IconRect = rectangle.Align(UI.Scale(new Size(64 * 460 / 215, 64), UI.UIScale), ContentAlignment.TopLeft)
		};

		rects.TextRect = rectangle.Pad(rects.IconRect.Width + GridPadding.Left, 0, 0, rectangle.Height).AlignToFontSize(UI.Font(10.5F, FontStyle.Bold), ContentAlignment.TopLeft);

		rects.IncludedRect = rects.TextRect.Align(UI.Scale(new Size(28, 28), UI.FontScale), ContentAlignment.TopRight);

		rects.TextRect.Width = rects.IncludedRect.X - rects.TextRect.X;

		rects.CenterRect = rects.TextRect.Pad(-GridPadding.Horizontal, 0, 0, 0);

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
			if (IncludedRect.Contains(location) && ServiceCenter.Get<IDlcManager>().IsAvailable(Item.Id))
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

			text = Locale.ViewXOnSteam.Format(Item.Name.Remove("Cities: Skylines - "));
			point = IconRect.Location;
			return true;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return true;
		}
	}
}
