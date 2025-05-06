using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class PlaysetSortingDropDown : SlickSelectionDropDown<PlaysetSorting>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(PlaysetSorting)).Cast<PlaysetSorting>().Where(x => x != PlaysetSorting.Downloads).ToArray();

			SelectedItem = (PlaysetSorting)ServiceCenter.Get<ISettings>().UserSettings.PageSettings.GetOrAdd(SkyvePage.Playsets).Sorting;
		}
	}

	protected override bool SearchMatch(string searchText, PlaysetSorting item)
	{
		return searchText.SearchCheck(LocaleHelper.GetGlobalText($"Sorting_{item}"));
	}

	private string GetIcon(PlaysetSorting item)
	{
		return item switch
		{
			PlaysetSorting.Name => "FileName",
			PlaysetSorting.LastUsed => "UpdateTime",
			PlaysetSorting.LastEdit => "Edit",
			PlaysetSorting.DateCreated => "Add",
			PlaysetSorting.Usage => "Star",
			PlaysetSorting.Color => "Paint",
			_ => "Sort"
		};
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, PlaysetSorting item)
	{
		var text = LocaleHelper.GetGlobalText($"Sorting_{item}");

		using var icon = IconManager.GetIcon(GetIcon(item), rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
