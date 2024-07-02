using System.Drawing;
using System.Windows.Forms;


namespace Skyve.App.UserInterface.Dropdowns;

public class PackageUsageDropDown : SlickMultiSelectionDropDown<PackageUsage>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(PackageUsage)).Cast<PackageUsage>().ToArray();
			SelectedItems = Items;
		}
	}

	public override void ResetValue()
	{
		SelectedItems = Items;

		listDropDown?.Invalidate();
		OnSelectedItemChanged();
		Invalidate();
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		//Width = UI.Scale(200);
	}

	protected override bool SearchMatch(string searchText, PackageUsage item)
	{
		var text = LocaleCR.Get($"{item}");

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, PackageUsage item, bool selected)
	{
		var text = LocaleCR.Get($"{item}");

		using var icon = item.GetIcon().Get(rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);

	}

	protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<PackageUsage> items)
	{
		var text = !items.Any() ? Locale.Invalid : items.Count() == Items.Length ? Locale.AllUsages : items.ListStrings(x => LocaleCR.Get($"{x}"), ", ");
		var iconName = !items.Any() ? "X" : items.Count() == Items.Length ? "Slash" : items.First().GetIcon();

		using var icon = iconName.Get(rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}
}
