using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class PlaysetSortingDropDown : SlickSelectionDropDown<ProfileSorting>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(ProfileSorting)).Cast<ProfileSorting>().Where(x => x != ProfileSorting.Downloads).ToArray();

			SelectedItem = (ProfileSorting)ServiceCenter.Get<ISettings>().UserSettings.PageSettings.GetOrAdd(SkyvePage.Profiles).Sorting;
		}
	}

	protected override bool SearchMatch(string searchText, ProfileSorting item)
	{
		return searchText.SearchCheck(LocaleHelper.GetGlobalText($"Sorting_{item}"));
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, ProfileSorting item)
	{
		var text = LocaleHelper.GetGlobalText($"Sorting_{item}");

		using var icon = IconManager.GetIcon("Sort", rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
