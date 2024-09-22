using Skyve.App.UserInterface.Panels;

using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Content;
public class WorkshopContentList : ContentList
{
	private ulong _id;

	public WorkshopContentList(SkyvePage page, bool loaded, GetAllItems getItems, Func<LocaleHelper.Translation> getItemText) : base(page, loaded, getItems, getItemText)
	{
		DD_Sorting.WorkshopSort = true;
		I_SortOrder.Visible = false;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		DD_SearchTime.Visible = DD_Sorting.SelectedItem is PackageSorting.Best;
	}

	protected override async void OnSearch()
	{
		var id = ++_id;

		I_Refresh.Loading = true;

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