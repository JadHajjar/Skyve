using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class SortingWorkshopDropDown : SlickSelectionDropDown<WorkshopQuerySorting>
{
	public SkyvePage SkyvePage { get; set; }

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(WorkshopQuerySorting)).Cast<WorkshopQuerySorting>().ToArray();

			selectedItem = WorkshopQuerySorting.Popularity;
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Middle)
		{
			SelectedItem = WorkshopQuerySorting.Popularity;
		}

		base.OnMouseClick(e);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Height = 0;
	}

	protected override bool SearchMatch(string searchText, WorkshopQuerySorting item)
	{
		return searchText.SearchCheck(LocaleHelper.GetGlobalText($"Sorting_{item}"));
	}

	public override void ResetValue()
	{

	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, WorkshopQuerySorting item)
	{
		var text = LocaleHelper.GetGlobalText($"Sorting_{item}");

		using var icon = IconManager.GetIcon(GetIcon(item), rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}

	private static string GetIcon(WorkshopQuerySorting item)
	{
		return item switch
		{
			WorkshopQuerySorting.Name => "FileName",
			WorkshopQuerySorting.DateUpdated => "UpdateTime",
			WorkshopQuerySorting.DateCreated => "Add",
			WorkshopQuerySorting.Popularity => "Star",
			WorkshopQuerySorting.Best => "People",
			WorkshopQuerySorting.Rating => "Vote",
			_ => "Check",
		};
	}
}
