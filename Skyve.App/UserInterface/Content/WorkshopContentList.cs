using Skyve.App.UserInterface.Panels;

using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Content;
public class WorkshopContentList : ContentList
{
	private ulong _id;

	public WorkshopContentList(SkyvePage page, bool loaded, GetAllItems getItems, Func<LocaleHelper.Translation> getItemText) : base(page, loaded, getItems, getItemText)
	{
	}

	protected override async void OnSearch()
	{
		var id = ++_id;

		ListControl.Clear();
		ListControl.Loading = true;

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