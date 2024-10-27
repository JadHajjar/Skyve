using Skyve.App.UserInterface.Content;
using Skyve.App.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class ReviewRequestList : SlickStackedListControl<ReviewRequest, ReviewRequestList.Rectangles>
{
	private readonly IUserService _userService;
	private readonly IWorkshopService _workshopService;
	private readonly INotifier _notifier;

	public ReviewRequestList()
	{
		ServiceCenter.Get(out _userService, out _notifier, out _workshopService);

		SeparateWithLines = true;
		DynamicSizing = true;
		GridItemSize = new Size(300, 200);
		GridView = true;

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

		GridPadding = UI.Scale(new Padding(5));
		Padding = UI.Scale(new Padding(10));
		Font = UI.Font(8.25F, FontStyle.Bold);
	}

	protected override IEnumerable<IDrawableItem<ReviewRequest>> OrderItems(IEnumerable<IDrawableItem<ReviewRequest>> items)
	{
		return items.OrderBy(x => x.Item.Timestamp);
	}

	protected override IDrawableItemRectangles<ReviewRequest> GenerateRectangles(ReviewRequest item, Rectangle rectangle)
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
			var user = _userService.TryGetUser(item.Item.UserId);

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

	protected override void OnPaintItemGrid(ItemPaintEventArgs<ReviewRequest, Rectangles> e)
	{
		base.OnPaintItemGrid(e);

		var user = _userService.TryGetUser(e.Item.UserId);
		var avatar = _workshopService.GetUser(user).GetThumbnail();

		using var font = UI.Font(10F);
		using var brush = new SolidBrush(UserIcon.GetUserColor(user.Id?.ToString() ?? string.Empty, true));
		using var icon = IconManager.GetIcon("User", font.Height * 5 / 4).Color(brush.Color);

		var nameSize = e.Graphics.Measure(user.Name, font);
		var nameHeight = Math.Max(icon.Height, (int)nameSize.Height);
		e.Rects.UserRectangle = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, nameHeight);

		if (avatar is null)
		{
			e.Graphics.DrawImage(icon, e.Rects.UserRectangle.Align(new Size(nameHeight, nameHeight), ContentAlignment.MiddleLeft).CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundImage(avatar, e.Rects.UserRectangle.Align(new Size(nameHeight, nameHeight), ContentAlignment.MiddleLeft).CenterR(icon.Size));
		}

		e.Graphics.DrawString(user.Name, font, brush, e.Rects.UserRectangle.Pad(nameHeight + GridPadding.Left, 0, 0, 0));

		using var smallFont = UI.Font(7.5F);
		using var timeBrush = new SolidBrush(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.ActiveColor));
		e.Graphics.DrawString(e.Item.Timestamp.ToLocalTime().ToRelatedString(true, false), smallFont, timeBrush, e.Rects.UserRectangle, new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });

		e.Rects.UserRectangle.Width = nameHeight + GridPadding.Horizontal + (int)nameSize.Width;

		if (e.Rects.UserRectangle.Contains(CursorLocation))
		{
			using var hoverBrush = new SolidBrush(Color.FromArgb(40, FormDesign.Design.ActiveColor));
			e.Graphics.FillRoundedRectangle(hoverBrush, e.Rects.UserRectangle.InvertPad(GridPadding), GridPadding.Left);
		}

		e.Rects.TextRectangle = SlickButton.AlignAndDraw(e.Graphics, e.ClipRectangle.Pad(0, e.Rects.UserRectangle.Height + Padding.Top, 0, 0), ContentAlignment.TopRight, new ButtonDrawArgs
		{
			BackgroundColor = e.BackColor,
			HoverState = e.HoverState,
			Cursor = CursorLocation,
			Icon = "Copy"
		}).Rectangle;

		using var textFont = UI.Font(8.25F);
		var noteRect = Rectangle.FromLTRB(e.ClipRectangle.X, e.Rects.TextRectangle.Y, e.Rects.TextRectangle.X, e.Rects.TextRectangle.Y);
		var noteSize = e.Graphics.Measure(e.Item.PackageNote, textFont, noteRect.Width);

		noteRect.Height = (int)noteSize.Height;

		using var fadedBrush = new SolidBrush(Color.FromArgb(200, FormDesign.Design.ForeColor));
		e.Graphics.DrawString(e.Item.PackageNote, textFont, fadedBrush, noteRect);

		e.Rects.ViewRectangle = new Rectangle(e.ClipRectangle.X, noteRect.Bottom + Padding.Vertical, e.ClipRectangle.Width, UI.Scale(28));

		using var buttonFont = UI.Font(9.5F);
		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			BackgroundColor = e.BackColor,
			Font = buttonFont,
			HoverState = e.HoverState,
			Rectangle = e.Rects.ViewRectangle,
			Cursor = CursorLocation,
			Text = LocaleCR.ViewRequest,
			Icon = "Link"
		});

		e.DrawableItem.CachedHeight = e.Rects.ViewRectangle.Bottom - e.ClipRectangle.Y + Padding.Vertical + GridPadding.Vertical;
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
