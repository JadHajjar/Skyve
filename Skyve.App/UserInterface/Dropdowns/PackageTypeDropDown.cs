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

		Width = (int)(200 * UI.FontScale);
	}

	protected override bool SearchMatch(string searchText, PackageType item)
	{
		var text = LocaleCR.Get($"{item}");

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, PackageType item)
	{
		var text = LocaleCR.Get($"{item}");

		using var icon = IconManager.GetIcon("I_Cog", rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}
}
