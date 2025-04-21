using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class PackageActionDropDown : SlickSelectionDropDown<StatusAction>
{
	public bool IsFlipped { get; set; }

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(175);
	}

	protected override bool SearchMatch(string searchText, StatusAction item)
	{
		var text = LocaleCR.Get(!IsFlipped || item is StatusAction.NoAction ? item.ToString() : $"Flipped{item}");

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, StatusAction item)
	{
		var text = LocaleCR.Get(!IsFlipped || item is StatusAction.NoAction ? item.ToString() : $"Flipped{item}");
		var notification = CRNAttribute.GetNotification(item);
		var color = notification.GetColor();

		using var icon = notification.GetIcon(false).Get(rectangle.Height - 2).Color(color);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
