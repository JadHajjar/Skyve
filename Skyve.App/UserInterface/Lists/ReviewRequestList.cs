using Skyve.Systems.Compatibility.Domain.Api;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class ReviewRequestList : SlickStackedListControl<ReviewRequest, ReviewRequestList.Rectangles>
{
	private readonly IWorkshopService _workshopService;
	public ReviewRequestList()
	{
		_workshopService = ServiceCenter.Get<IWorkshopService>();
		HighlightOnHover = true;
		SeparateWithLines = true;
		ItemHeight = 64;
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

	protected override bool IsItemActionHovered(DrawableItem<ReviewRequest, Rectangles> item, Point location)
	{
		return true;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!Items.Any())
		{
			e.Graphics.DrawString(Locale.SelectPackage, UI.Font(9F, FontStyle.Italic), new SolidBrush(ForeColor), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		base.OnPaint(e);
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<ReviewRequest, Rectangles> e)
	{
		base.OnPaintItemList(e);

		var User = _workshopService.GetUser(e.Item.UserId);
		var imageRect = e.ClipRectangle.Pad(Padding);
		imageRect.Width = imageRect.Height /= 2;
		var image = User?.GetUserAvatar();

		if (image is not null)
		{
			e.Graphics.DrawRoundedImage(image, imageRect, (int)(3 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}
		else
		{
			using var generic = Properties.Resources.I_GenericUser.Color(FormDesign.Design.IconColor);

			e.Graphics.DrawRoundedImage(generic, imageRect, (int)(3 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}

		var textRect = e.ClipRectangle.Pad(imageRect.Right + Padding.Left, 0, 0, 0);
		using var brush = new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : ForeColor);
		e.Graphics.DrawString(User?.Name ?? Locale.UnknownUser, Font, brush, textRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

		using var typeIcon = IconManager.GetSmallIcon(e.Item.IsInteraction ? "I_Switch" : e.Item.IsStatus ? "I_Statuses" : "I_Content");
		using var dateIcon = IconManager.GetSmallIcon("I_UpdateTime");
		var r = e.Graphics.DrawLabel(e.Item.Timestamp.ToLocalTime().ToString("g"), dateIcon, FormDesign.Design.AccentColor, e.ClipRectangle, ContentAlignment.BottomLeft, true);
		e.Graphics.DrawLabel(e.Item.IsInteraction ? "Interaction" : e.Item.IsStatus ? "Status" : "Other", typeIcon, FormDesign.Design.AccentColor, e.ClipRectangle.Pad(0, 0, 0, r.Height + Padding.Top), ContentAlignment.BottomLeft, true);


		e.Graphics.DrawString(e.Item.PackageNote, UI.Font(7.5F), brush, textRect.Pad((int)(125 * UI.FontScale), 0, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far });
	}

	public class Rectangles : IDrawableItemRectangles<ReviewRequest>
	{
		public ReviewRequest Item { get; set; }

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
			return true;
		}
	}
}
