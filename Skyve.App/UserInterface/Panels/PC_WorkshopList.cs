﻿using Skyve.App.UserInterface.Content;

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public class PC_WorkshopList : PanelContent
{
	private readonly IWorkshopService _workshopService;
	protected internal readonly WorkshopContentList LC_Items;
	private int currentPage;
	private bool listLoading;
	private bool endOfPagesReached;

	public PC_WorkshopList() : base(false)
	{
		ServiceCenter.Get(out _workshopService);

		Padding = new Padding(0, 30, 0, 0);

		LC_Items = new(SkyvePage.Workshop, false, GetItems, GetItemText)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

		LC_Items.ListControl.ScrollEndReached += ListControl_ScrollEndReached;

		Controls.Add(LC_Items);

#if CS2
		Text = "PDX Mods";
#else
		Text = "Steam Workshop";
#endif
	}

	protected override async void OnCreateControl()
	{
		base.OnCreateControl();

		await LC_Items.RefreshItems();
	}

	public override bool KeyPressed(ref Message msg, Keys keyData)
	{
		if (keyData is (Keys.Control | Keys.F))
		{
			LC_Items.TB_Search.Focus();
			LC_Items.TB_Search.SelectAll();

			return true;
		}

		return false;
	}

	protected virtual async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		if (LC_Items.TB_Search.Text.Length is 5 or 6 && ulong.TryParse(LC_Items.TB_Search.Text, out var id))
		{
			var package = await _workshopService.GetInfoAsync(new GenericPackageIdentity(id));

			if (package != null)
			{
				return [package];
			}
		}

		return await GetPackages(0);
	}

	private void ListControl_ScrollEndReached(object sender, EventArgs e)
	{
		if (!listLoading && !endOfPagesReached)
		{
			LC_Items.I_Refresh.Loading = true;

			Task.Run(async () => LC_Items.ListControl.AddRange(await GetPackages(currentPage + 1)));
		}
	}

	private async Task<IEnumerable<IPackageIdentity>> GetPackages(int page)
	{
		listLoading = true;

		try
		{
			IEnumerable<IWorkshopInfo> list;

			if (LC_Items.TB_Search.Text.StartsWith("@"))
			{
				list = await _workshopService.GetWorkshopItemsByUserAsync(
				   LC_Items.TB_Search.Text.Substring(1),
				   (WorkshopQuerySorting)(LC_Items.DD_Sorting.SelectedItem - (int)PackageSorting.WorkshopSorting),
				   null,
				   LC_Items.DD_Tags.SelectedItems.Select(x => x.Value).ToArray(),
				   limit: 30,
				   page: currentPage = page);

			}
			else
			{
				list = await _workshopService.QueryFilesAsync(
				   (WorkshopQuerySorting)(LC_Items.DD_Sorting.SelectedItem - (int)PackageSorting.WorkshopSorting),
				   LC_Items.TB_Search.Text,
				   LC_Items.DD_Tags.SelectedItems.Select(x => x.Value).ToArray(),
				   limit: 30,
				   page: currentPage = page);
			}

			endOfPagesReached = list.Count() < 30;

			return list;
		}
		finally
		{
			listLoading = false;
		}
	}

	protected virtual LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
