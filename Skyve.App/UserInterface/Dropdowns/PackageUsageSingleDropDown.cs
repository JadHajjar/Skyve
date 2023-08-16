using System.Drawing;
using System.Windows.Forms;


namespace Skyve.App.UserInterface.Dropdowns;

internal class PackageUsageSingleDropDown : SlickSelectionDropDown<PackageUsage>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = new[] { (PackageUsage)(-1) }.Concat(Enum.GetValues(typeof(PackageUsage)).Cast<PackageUsage>()).ToArray();
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = (int)(200 * UI.FontScale);
	}

	protected override bool SearchMatch(string searchText, PackageUsage item)
	{
		var text = (int)item == -1 ? Locale.AnyUsage : LocaleCR.Get(item.ToString());

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, PackageUsage item)
	{
		var text = (int)item == -1 ? Locale.AnyUsage : LocaleCR.Get(item.ToString());

		using var icon = item.GetIcon().Get(rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}
}
