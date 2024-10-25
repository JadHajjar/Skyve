using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
internal class WorkshopSearchTimeDropDown : SlickSelectionDropDown<WorkshopSearchTime>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(WorkshopSearchTime)).Cast<WorkshopSearchTime>().OrderBy(x => x is WorkshopSearchTime.AllTime).ToArray();

			selectedItem = WorkshopSearchTime.Month;
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Middle)
		{
			SelectedItem = WorkshopSearchTime.Month;
		}

		base.OnMouseClick(e);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Height = 0;
	}

	protected override bool SearchMatch(string searchText, WorkshopSearchTime item)
	{
		return searchText.SearchCheck(LocaleHelper.GetGlobalText($"SearchTime_{item}"));
	}

	public override void ResetValue()
	{
		SelectedItem = WorkshopSearchTime.Month;
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, WorkshopSearchTime item)
	{
		var text = LocaleHelper.GetGlobalText($"SearchTime_{item}");

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle, e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle, format);
	}
}
