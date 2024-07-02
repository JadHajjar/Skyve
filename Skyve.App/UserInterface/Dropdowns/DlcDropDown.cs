using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class DlcDropDown : SlickMultiSelectionDropDown<IDlcInfo>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = ServiceCenter.Get<IDlcManager>().Dlcs.ToArray();
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(200);
	}

	protected override IEnumerable<IDlcInfo> OrderItems(IEnumerable<IDlcInfo> items)
	{
		return items.OrderByDescending(x => SelectedItems.Contains(x)).ThenByDescending(x => x.ReleaseDate);
	}

	protected override bool SearchMatch(string searchText, IDlcInfo item)
	{
		return searchText.SearchCheck(item.Name.Remove("Cities: Skylines - ").Replace("Content Creator Pack", "CCP"));
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IDlcInfo item, bool selected)
	{
		if (item is null)
		{
			return;
		}

		var text = item.Name.Remove("Cities: Skylines - ").Replace("Content Creator Pack", "CCP");
		var icon = item.GetThumbnail();

		if (icon != null)
		{
			e.Graphics.DrawRoundedImage(icon, rectangle.Align(new Size(rectangle.Height * 460 / 215, rectangle.Height), ContentAlignment.MiddleLeft), UI.Scale(4));
		}

		rectangle = rectangle.Pad((rectangle.Height * 460 / 215) + Padding.Left, 0, 0, 0);

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.AlignToFontSize(Font), new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
	}

	protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<IDlcInfo> items)
	{
		if (items.Count() == 1)
		{
			PaintItem(e, rectangle, foreColor, hoverState, items.First(), false);

			return;
		}

		if (!items.Any())
		{
			using var icon = IconManager.GetIcon("Slash", rectangle.Height - 2).Color(foreColor);

			e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

			e.Graphics.DrawString(LocaleCR.NoRequiredDlcs, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

			return;
		}

		e.Graphics.DrawString(LocaleCR.DlcsSelected.FormatPlural(items.Count()), Font, new SolidBrush(foreColor), rectangle.AlignToFontSize(Font), new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
	}
}
