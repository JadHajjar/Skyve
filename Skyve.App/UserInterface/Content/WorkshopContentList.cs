using Skyve.App.UserInterface.Panels;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class WorkshopContentList : ContentList
{
	private ulong _id;

	public WorkshopContentList(SkyvePage page, bool loaded, GetAllItems getItems, Func<LocaleHelper.Translation> getItemText) : base(page, loaded, getItems, getItemText)
	{
		DD_Sorting.WorkshopSort = true;
		I_SortOrder.Parent = null;

		TLP_Main.SetColumnSpan(DD_SearchTime, 2);
		DD_SearchTime.Visible = true;

		PaginationControl = new()
		{
			Dock = DockStyle.Bottom,
			Margin = default
		};

		TagsControl = new()
		{
			Loading = true,
			Margin = default,
			Dock = DockStyle.Fill
		};

		TagsControl.SelectedTagChanged += FilterChanged;

		TLP_Main.Controls.Add(TagsControl, TLP_Main.ColumnCount - 1, TLP_Main.RowCount - 1);

		TLP_Main.RowStyles.Add(new());
		TLP_Main.RowCount++;

		TLP_Main.Controls.Add(PaginationControl, 0, TLP_Main.RowCount - 1);
		TLP_Main.SetColumnSpan(PaginationControl, TLP_Main.ColumnCount - 1);

		TLP_Main.SetRowSpan(TagsControl, 2);
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		DD_SearchTime.Enabled = DD_Sorting.SelectedItem is PackageSorting.Best;
	}

	protected override async void OnSearch()
	{
		var id = ++_id;

		I_Refresh.Loading = true;

		if (PaginationControl is not null)
		{
			PaginationControl.Page = 0;
		}

		if (!string.IsNullOrWhiteSpace(TB_Search.Text))
		{
			await Task.Delay(250);
		}

		if (id == _id)
		{
			await RefreshItems();
		}
	}
}