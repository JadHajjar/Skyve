using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;


namespace Skyve.App.UserInterface.Dropdowns;

public class PackageTypeDropDown : SlickMultiSelectionDropDown<PackageType>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(PackageType)).Cast<PackageType>().ToArray();
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

	protected override IEnumerable<PackageType> OrderItems(IEnumerable<PackageType> items)
	{
		return items.OrderBy(x => LocaleCR.Get(x.ToString()).One);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(200);
	}

	protected override bool SearchMatch(string searchText, PackageType item)
	{
		var text = LocaleCR.Get(item.ToString());

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, PackageType item, bool selected)
	{
		var text = LocaleCR.Get(item.ToString());

		using var icon = item.GetIcon().Get(rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}

	protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<PackageType> items)
	{
		var text = !items.Any() ? Locale.Invalid : items.Count() == Items.Length ? Locale.AllUsages : items.ListStrings(x => LocaleCR.Get(x.ToString()), ", ");
		var iconName = !items.Any() ? "X" : items.Count() == Items.Length ? "Slash" : items.First().GetIcon();

		using var icon = iconName.Get(rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var format = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, base.Font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(base.Font, ContentAlignment.MiddleLeft, e.Graphics), format);
	}
}
