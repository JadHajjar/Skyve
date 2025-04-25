using Skyve.App.UserInterface.Content;

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public class PC_WorkshopList : PanelContent
{
	private readonly IWorkshopService _workshopService;
	protected internal readonly WorkshopContentList LC_Items;
	private bool listLoading;

	private static List<ITag> lastSelectedTags = [];

	public PC_WorkshopList() : base(false)
	{
		ServiceCenter.Get(out _workshopService);

		Padding = new Padding(0, 30, 0, 0);

		LC_Items = new(SkyvePage.Workshop, false, GetItems, GetItemText)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

		LC_Items.TagsControl!.SelectedTags.AddRange(lastSelectedTags);
		LC_Items.TagsControl!.SelectedTagChanged += DD_Tags_SelectedItemChanged;
		LC_Items.PaginationControl!.PageSelected += PageChanged;

		Controls.Add(LC_Items);

#if CS2
		Text = "PDX Mods";
#else
		Text = "Steam Workshop";
#endif
	}

	private void DD_Tags_SelectedItemChanged(object sender, EventArgs e)
	{
		lastSelectedTags = LC_Items.TagsControl!.SelectedTags.ToList();
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
		if (LC_Items.TB_Search.Text.Length is 5 or 6 or 7 && ulong.TryParse(LC_Items.TB_Search.Text, out var id))
		{
			var package = await _workshopService.GetInfoAsync(new GenericPackageIdentity(id));

			if (package != null)
			{
				return [package];
			}
		}

		return await GetPackages(LC_Items.PaginationControl!.Page);
	}

	private void PageChanged(object sender, int page)
	{
		LC_Items.I_Refresh.Loading = true;

		Task.Run(LC_Items.RefreshItems);
	}

	private async Task<IEnumerable<IPackageIdentity>> GetPackages(int page)
	{
		listLoading = true;

		try
		{
			(IEnumerable<IWorkshopInfo> Mods, int TotalCount) list;

			if (LC_Items.TB_Search.Text.StartsWith("@"))
			{
				list = await _workshopService.GetWorkshopItemsByUserAsync(
				   LC_Items.TB_Search.Text.Substring(1),
				   (WorkshopQuerySorting)(LC_Items.DD_Sorting.SelectedItem - (int)PackageSorting.WorkshopSorting),
				   null,
				   LC_Items.TagsControl?.SelectedTags.Select(x => x.Value).ToArray(),
				   limit: 30,
				   page: page);
			}
			else
			{
				list = await _workshopService.QueryFilesAsync(
				   (WorkshopQuerySorting)(LC_Items.DD_Sorting.SelectedItem - (int)PackageSorting.WorkshopSorting),
				   LC_Items.DD_SearchTime.SelectedItem,
				   LC_Items.TB_Search.Text,
				   LC_Items.TagsControl?.SelectedTags.Select(x => x.Value).ToArray(),
				   limit: 30,
				   page: page);
			}

			LC_Items.PaginationControl!.SetTotalCount(list.TotalCount);

			return list.Mods;
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

	public void SetSettings(PackageSorting sorting, string[]? selectedTags, WorkshopSearchTime searchTime = WorkshopSearchTime.Month)
	{
		LC_Items.DD_Sorting.SelectedItem = sorting;
		LC_Items.TagsControl!.SelectedTags.Clear();
		LC_Items.TagsControl!.SelectedTags.AddRange(selectedTags?.Select(ServiceCenter.Get<ITagsService>().CreateWorkshopTag));
		LC_Items.DD_SearchTime.SelectedItem = searchTime;
	}
}