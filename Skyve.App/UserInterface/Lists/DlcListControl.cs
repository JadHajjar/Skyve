using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class DlcListControl : SlickStackedListControl<IDlcInfo, DlcListControl.Rectangles>
{
	public IEnumerable<IDlcInfo> FilteredItems => SafeGetItems().Select(x => x.Item);

	private readonly ISettings _settings;
	private readonly IDlcManager _dlcManager;

	public DlcListControl()
	{
		HighlightOnHover = true;
		SeparateWithLines = true;

		_settings = ServiceCenter.Get<ISettings>();
		_dlcManager = ServiceCenter.Get<IDlcManager>();

		Loading = _dlcManager.Dlcs.Count() == 0;
	}

	protected override void UIChanged()
	{
		ItemHeight = 46;

		base.UIChanged();

		Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);
	}

	protected override IEnumerable<DrawableItem<IDlcInfo, Rectangles>> OrderItems(IEnumerable<DrawableItem<IDlcInfo, Rectangles>> items)
	{
		return items.OrderByDescending(x => x.Item.ReleaseDate);
	}

	protected override void OnItemMouseClick(DrawableItem<IDlcInfo, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		var rects = item.Rectangles;

		if (rects.IncludedRect.Contains(e.Location) && _dlcManager.IsAvailable(item.Item.Id))
		{
			_dlcManager.SetIncluded(item.Item, !_dlcManager.IsIncluded(item.Item));
		}

		if (rects.SteamRect.Contains(e.Location))
		{
			PlatformUtil.OpenUrl($"https://store.steampowered.com/app/{item.Item.Id}");
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

	protected override void OnPaintItemList(ItemPaintEventArgs<IDlcInfo, Rectangles> e)
	{
		var large = false;
		var rects = e.Rects;
		var isPressed = e.HoverState.HasFlag(HoverState.Pressed);

		e.HoverState &= ~HoverState.Pressed;

		base.OnPaintItemList(e);

		var owned = _dlcManager.IsAvailable(e.Item.Id);
		var isIncluded = owned && _dlcManager.IsIncluded(e.Item);

		if (owned)
		{
			if (isIncluded)
			{
				e.Graphics.FillRoundedRectangle(rects.IncludedRect.Gradient(Color.FromArgb(rects.IncludedRect.Contains(CursorLocation) ? 150 : 255, FormDesign.Design.GreenColor), 1.5F), rects.IncludedRect.Pad(0, Padding.Vertical, 0, Padding.Vertical), 4);
			}
			else if (rects.IncludedRect.Contains(CursorLocation))
			{
				e.Graphics.FillRoundedRectangle(rects.IncludedRect.Gradient(Color.FromArgb(20, ForeColor), 1.5F), rects.IncludedRect.Pad(0, Padding.Vertical, 0, Padding.Vertical), 4);
			}
		}

		var dIcon = new DynamicIcon(!owned ? "I_Slash" : isIncluded ? "I_Ok" : "I_Enabled");
		using (var icon = large ? dIcon.Large : dIcon.Get(rects.IncludedRect.Height / 2))
		{
			e.Graphics.DrawImage(icon.Color(owned && rects.IncludedRect.Contains(CursorLocation) ? FormDesign.Design.ActiveColor : isIncluded ? FormDesign.Design.ActiveForeColor : ForeColor), rects.IncludedRect.CenterR(icon.Size));
		}

		var iconRectangle = rects.IconRect;
		var textRect = rects.TextRect;

		var iconImg = e.Item.GetThumbnail();

		if (iconImg is null)
		{
			using var authorIcon = Properties.Resources.I_DlcIcon.Color(FormDesign.Design.IconColor);

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.IconColor), iconRectangle, (int)(4 * UI.FontScale));
			e.Graphics.FillRectangle(new SolidBrush(BackColor), iconRectangle.CenterR(iconRectangle.Height - 4, iconRectangle.Height - 4));
			e.Graphics.DrawImage(authorIcon, iconRectangle.CenterR(iconRectangle.Height - 2, iconRectangle.Height - 2));
		}
		else
		{
			e.Graphics.DrawRoundedImage(iconImg, iconRectangle, (int)(4 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}

		e.Graphics.DrawString(e.Item.Name.Remove("Cities: Skylines - "), UI.Font(large ? 11.25F : 9F, FontStyle.Bold), new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : ForeColor), textRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
		e.Graphics.DrawString(System.Net.WebUtility.HtmlDecode(e.Item.Description), UI.Font(large ? 9.75F : 7F), new SolidBrush(Color.FromArgb(150, e.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : ForeColor)), textRect.Pad(0, (int)e.Graphics.Measure(e.Item.Name, UI.Font(large ? 11.25F : 9F, FontStyle.Bold)).Height, 0, 0), new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Far });

		if (e.Item.ReleaseDate != DateTime.MinValue)
		{
			DrawLabel(e, _settings.UserSettings.ShowDatesRelatively ? e.Item.ReleaseDate.ToLocalTime().ToRelatedString(true, false) : e.Item.ReleaseDate.ToString("D"), IconManager.GetSmallIcon("I_UpdateTime"), FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 50), rects.TextRect, ContentAlignment.TopRight);
		}

		using (var steamIcon = IconManager.GetIcon("I_Steam", rects.SteamRect.Height / 2))
		{
			SlickButton.DrawButton(e, rects.SteamRect, string.Empty, Font, steamIcon, null, rects.SteamRect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal);
		}

		if (!isIncluded)
		{
			var filledRect = e.ClipRectangle.Pad(0, -Padding.Top, 0, -Padding.Bottom);
			e.Graphics.SetClip(filledRect);
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 30 : 85, BackColor)), filledRect);
		}
	}

	private Rectangle DrawLabel(ItemPaintEventArgs<IDlcInfo, Rectangles> e, string? text, Bitmap? icon, Color color, Rectangle rectangle, ContentAlignment alignment)
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

	protected override Rectangles GenerateRectangles(IDlcInfo item, Rectangle rectangle)
	{
		var includeItemHeight = ItemHeight;
		var iconSize = rectangle.Height - Padding.Vertical;
		var rects = new Rectangles(item)
		{
			IncludedRect = rectangle.Pad(1 * Padding.Left, 0, 0, 0).Align(new Size(includeItemHeight - 2, rectangle.Height - 2), ContentAlignment.MiddleLeft),
			SteamRect = rectangle.Pad(0, 0, Padding.Right, 0).Align(new Size(includeItemHeight, ItemHeight), ContentAlignment.TopRight)
		};

		rects.IconRect = rectangle.Pad(rects.IncludedRect.Right + 2 * Padding.Left).Align(new Size(iconSize * 460 / 215, iconSize), ContentAlignment.MiddleLeft);

		rects.CenterRect = new Rectangle(rects.IconRect.X, rectangle.Y, rects.SteamRect.X - rects.IconRect.X, rectangle.Height);

		rects.TextRect = rectangle.Pad(rects.IconRect.X + rects.IconRect.Width + Padding.Left, 0, rectangle.Width - rects.CenterRect.Right, 0);

		return rects;
	}

	public class Rectangles : IDrawableItemRectangles<IDlcInfo>
	{
		public Rectangle IncludedRect;
		public Rectangle IconRect;
		public Rectangle TextRect;
		public Rectangle SteamRect;
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
				text = Locale.ExcludeInclude;
				point = IncludedRect.Location;
				return true;
			}
			else if (SteamRect.Contains(location))
			{
				text = Locale.ViewOnSteam;
				point = SteamRect.Location;
				return true;
			}
			else
			{
				text = Locale.ViewOnSteam;
				point = default;
				return true;
			}
		}

		public bool IsHovered(Control instance, Point location)
		{
			return
				IncludedRect.Contains(location) ||
				SteamRect.Contains(location);
		}
	}
}
