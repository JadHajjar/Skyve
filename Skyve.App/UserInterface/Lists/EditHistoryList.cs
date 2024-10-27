using Skyve.App.UserInterface.Content;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class EditHistoryList : SlickStackedListControl<PackageEdit>
{
	private readonly IUserService _userService;
	private readonly IWorkshopService _workshopService;
	
	public EditHistoryList()
    {
		ServiceCenter.Get(out _userService, out _workshopService);

		SeparateWithLines = true;
		DynamicSizing = true;
    }

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(5));
	}

	protected override IEnumerable<IDrawableItem<PackageEdit>> OrderItems(IEnumerable<IDrawableItem<PackageEdit>> items)
	{
		return items.OrderByDescending(x => x.Item.EditDate);
	}
	protected override void OnPaintItemList(ItemPaintEventArgs<PackageEdit, GenericDrawableItemRectangles<PackageEdit>> e)
	{
		base.OnPaintItemList(e);

		var author = _userService.TryGetUser(e.Item.Username);

		using var brush = new SolidBrush(FormDesign.Design.ForeColor);
		using var authorBrush = new SolidBrush(UserIcon.GetUserColor(author.Id?.ToString() ?? string.Empty, true));
		using var fontBold = UI.Font(9.75F, FontStyle.Bold);
		using var font = UI.Font(9F);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };

		DrawAuthorImage(e, author, e.ClipRectangle.Pad(Padding).Align(UI.Scale(new Size(24, 24)), ContentAlignment.TopLeft), authorBrush.Color);

		e.Graphics.DrawString(author.Name, fontBold, authorBrush, e.ClipRectangle.Pad(UI.Scale(24) + Padding.Horizontal, Padding.Top, Padding.Right, Padding.Bottom).ClipTo(UI.Scale(24)), format);

		var authorSize = e.Graphics.Measure(author.Name, fontBold);

		e.Graphics.DrawString($"edited {e.Item.EditDate.ToRelatedString().ToLower()}", font, brush, e.ClipRectangle.Pad(UI.Scale(24) + Padding.Horizontal + (int)authorSize.Width, Padding.Top, Padding.Right, Padding.Bottom).ClipTo(UI.Scale(24)), format);
	
		if (string.IsNullOrEmpty(e.Item.Note))
		{
			e.DrawableItem.CachedHeight = UI.Scale(24) + Padding.Vertical * 2;
			
			return;
		}

		var noteRect = e.ClipRectangle.Pad(UI.Scale(24) + Padding.Horizontal * 2, UI.Scale(24) + Padding.Vertical, Padding.Right, Padding.Bottom);
		e.Graphics.DrawString(e.Item.Note, base.Font, brush, noteRect);

		using var pen = new Pen(FormDesign.Design.AccentColor, UI.Scale(2.5f)) { EndCap = System.Drawing.Drawing2D.LineCap.Round, StartCap = System.Drawing.Drawing2D.LineCap.Round };

		e.Graphics.DrawLine(pen, noteRect.X - Padding.Left, noteRect.Y, noteRect.X - Padding.Left, noteRect.Bottom - Padding.Bottom);
		
		e.DrawableItem.CachedHeight = UI.Scale(24) + Padding.Vertical * 3 + (int)e.Graphics.Measure(e.Item.Note, Font, noteRect.Width).Height;
	}

	private void DrawAuthorImage(PaintEventArgs e, IUser author, Rectangle rectangle, Color color)
	{
		var image = _workshopService.GetUser(author).GetThumbnail();

		if (image != null)
		{
			e.Graphics.DrawRoundImage(image, rectangle.Pad((int)(1.5 * UI.FontScale)));
		}
		else
		{
			using var authorIcon = IconManager.GetIcon("Author", rectangle.Height);

			e.Graphics.DrawImage(authorIcon.Color(color, color.A), rectangle.CenterR(authorIcon.Size));
		}
	}
}
