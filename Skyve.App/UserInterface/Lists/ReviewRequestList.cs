using Skyve.App.Utilities;

using SkyveApi.Domain.Generic;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class ReviewRequestList : SlickStackedListControl<ReviewRequest, ReviewRequestList.Rectangles>
{
	private readonly IWorkshopService _workshopService;
	private readonly INotifier _notifier;

	public ReviewRequestList()
	{
		ServiceCenter.Get(out _workshopService, out _notifier);

		SeparateWithLines = true;
		DynamicSizing = true;
		ItemHeight = 64;

		_notifier.WorkshopUsersInfoLoaded += _notifier_WorkshopUsersInfoLoaded;
	}

	protected override void Dispose(bool disposing)
	{
		_notifier.WorkshopUsersInfoLoaded -= _notifier_WorkshopUsersInfoLoaded;

		base.Dispose(disposing);
	}

	private void _notifier_WorkshopUsersInfoLoaded()
	{
		Invalidate();
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);
		Font = UI.Font(8.25F, FontStyle.Bold);
	}

	protected override IEnumerable<DrawableItem<ReviewRequest, Rectangles>> OrderItems(IEnumerable<DrawableItem<ReviewRequest, Rectangles>> items)
	{
		return items.OrderBy(x => x.Item.Timestamp);
	}

	protected override Rectangles GenerateRectangles(ReviewRequest item, Rectangle rectangle)
	{
		return new Rectangles(item);
	}

	protected override void OnItemMouseClick(DrawableItem<ReviewRequest, Rectangles> item, MouseEventArgs e)
	{
		if (item.Rectangles.TextRectangle.Contains(e.Location))
		{
			Clipboard.SetText(item.Item.PackageNote);
		}
		else if (item.Rectangles.UserRectangle.Contains(e.Location))
		{
			var user = _workshopService.GetUser(item.Item.UserId);

			if (user != null)
			{
				PlatformUtil.OpenUrl(user.ProfileUrl);
			}
		}
		else if (item.Rectangles.ViewRectangle.Contains(e.Location))
		{
			base.OnItemMouseClick(item, e);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (ItemCount == 0)
		{
			e.Graphics.DrawString(Locale.SelectPackage, UI.Font(9F, FontStyle.Italic), new SolidBrush(ForeColor), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		base.OnPaint(e);
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<ReviewRequest, Rectangles> e)
	{
		base.OnPaintItemList(e);

		var user = _workshopService.GetUser(e.Item.UserId);
		var imageRect = e.ClipRectangle.Pad(Padding).Align(new Size(ItemHeight / 2, ItemHeight / 2), ContentAlignment.TopLeft);
		var image = user?.GetUserAvatar();

		e.Rects.ViewRectangle = SlickButton.AlignAndDraw(e.Graphics, e.ClipRectangle.Pad(Padding), ContentAlignment.TopRight, new ButtonDrawArgs
		{
			HoverState = e.HoverState,
			Cursor = CursorLocation,
			Text = LocaleCR.ViewRequest,
			Icon = "I_Link"
		}).Rectangle;

		if (image is not null)
		{
			e.Graphics.DrawRoundedImage(image, imageRect, (int)(3 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}
		else
		{
			using var generic = IconManager.GetIcon("I_User", imageRect.Height).Color(e.BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, imageRect, (int)(5 * UI.FontScale));
			e.Graphics.DrawImage(generic, imageRect.CenterR(generic.Size));
		}

		var textRect = e.ClipRectangle.Pad(imageRect.Right + Padding.Left, 0, e.Rects.ViewRectangle.Width + Padding.Horizontal, 0);
		using var textBrush = new SolidBrush(ForeColor);
		e.Graphics.DrawString(user?.Name ?? Locale.UnknownUser, Font, textBrush, textRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

		e.Rects.UserRectangle = Rectangle.Union(imageRect, new Rectangle(textRect.Location, e.Graphics.Measure(user?.Name ?? Locale.UnknownUser, Font, textRect.Width).ToSize()));

		if (e.Rects.UserRectangle.Contains(CursorLocation))
		{
			using var brush = new SolidBrush(Color.FromArgb(40, FormDesign.Design.ActiveColor));
			e.Graphics.FillRoundedRectangle(brush, e.Rects.UserRectangle, Padding.Left);
		}

		using var typeIcon = IconManager.GetSmallIcon(e.Item.IsInteraction ? "I_Switch" : e.Item.IsStatus ? "I_Statuses" : "I_Content");
		using var dateIcon = IconManager.GetSmallIcon("I_UpdateTime");
		var r = e.Graphics.DrawLabel(e.Item.Timestamp.ToLocalTime().ToString("g"), dateIcon, FormDesign.Design.AccentColor, e.ClipRectangle, ContentAlignment.BottomLeft, true);
		e.Graphics.DrawLabel(LocaleHelper.GetGlobalText(e.Item.IsInteraction ? "Interaction" : e.Item.IsStatus ? "Status" : "Other"), typeIcon, FormDesign.Design.AccentColor, e.ClipRectangle.Pad(0, 0, 0, r.Height + Padding.Top), ContentAlignment.BottomLeft, true);

		e.Rects.TextRectangle = SlickButton.AlignAndDraw(e.Graphics, e.ClipRectangle.Pad(0, e.Rects.ViewRectangle.Height + Padding.Vertical, Padding.Right, 0), ContentAlignment.MiddleRight, new ButtonDrawArgs
		{
			HoverState = e.HoverState,
			Cursor = CursorLocation,
			Icon = "I_Copy"
		}).Rectangle;

		using var smallfont = UI.Font(8.25F);
		var noteRect = e.ClipRectangle.Pad((int)(125 * UI.FontScale), e.Rects.ViewRectangle.Height + Padding.Vertical, e.Rects.TextRectangle.Width + Padding.Horizontal, 0);
		e.Graphics.DrawString(e.Item.PackageNote, smallfont, textBrush, noteRect, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far });

		e.DrawableItem.CachedHeight = Math.Max(ItemHeight, noteRect.Top + Padding.Bottom + (int)e.Graphics.Measure(e.Item.PackageNote, smallfont, noteRect.Width).Height);
	}

	public class Rectangles : IDrawableItemRectangles<ReviewRequest>
	{
		public ReviewRequest Item { get; set; }

		public Rectangle UserRectangle;
		public Rectangle TextRectangle;
		public Rectangle ViewRectangle;

		public Rectangles(ReviewRequest item)
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
			return UserRectangle.Contains(location) || TextRectangle.Contains(location) || ViewRectangle.Contains(location);
		}
	}
}
