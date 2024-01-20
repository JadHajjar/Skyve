using Extensions;

using Skyve.App.UserInterface.Generic;
using Skyve.App.UserInterface.Panels;
using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Extensions.TicketBooth;

namespace Skyve.App.UserInterface.Content;
public class WorkshopContentList : ContentList
{
	private ulong _id;
	public WorkshopContentList(SkyvePage page, bool loaded, GetAllItems getItems, ApplyAll setIncluded, ApplyAll setEnabled, Func<LocaleHelper.Translation> getItemText, Func<string> getCountText) : base(page, loaded, getItems, setIncluded, setEnabled, getItemText, getCountText)
	{
	}

	protected override async void OnSearch()
	{
		var id = ++_id;

		ListControl.Clear();
		ListControl.Loading = true;

		if (!string.IsNullOrWhiteSpace(TB_Search.Text))
		await Task.Delay(250);

		if (id == _id)
		await RefreshItems();
	}
}