using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;


namespace Skyve.App.UserInterface.Dropdowns;

public class PackageTypeDropDown : SlickSelectionDropDown<PackageType>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(PackageType)).Cast<PackageType>().ToArray();
		}
	}

	protected override IEnumerable<PackageType> OrderItems(IEnumerable<PackageType> items)
	{
		return items.OrderBy(x => LocaleCR.Get($"{x}").One);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(200);
	}

	protected override bool SearchMatch(string searchText, PackageType item)
	{
		var text = LocaleCR.Get($"{item}");

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, PackageType item)
	{
		var text = LocaleCR.Get($"{item}");

		using var icon = IconManager.GetIcon("Cog", rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
