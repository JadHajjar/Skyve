using Skyve.App.UserInterface.Content;

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
	
	private static List<ITag> lastSelectedTags = new();

	public PC_WorkshopList() : base(false)
	{
		ServiceCenter.Get(out _workshopService);

		Padding = new Padding(0, 30, 0, 0);

		LC_Items = new(SkyvePage.Workshop, false, GetItems, GetItemText)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

		LC_Items.ListControl.ScrollUpdate += ListControl_ScrollUpdate;
		LC_Items.DD_Tags.SelectedItems = lastSelectedTags;
		LC_Items.DD_Tags.SelectedItemChanged += DD_Tags_SelectedItemChanged;

		Controls.Add(LC_Items);

#if CS2
		Text = "PDX Mods";
#else
		Text = "Steam Workshop";
#endif
	}

	private void DD_Tags_SelectedItemChanged(object sender, EventArgs e)
	{
		lastSelectedTags = LC_Items.DD_Tags.SelectedItems.ToList();
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

	private void ListControl_ScrollUpdate(object sender, double scrollIndex, double maxScroll)
	{
		if (scrollIndex > maxScroll - 4.5 && !listLoading && !endOfPagesReached)
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
				   LC_Items.DD_SearchTime.SelectedItem,
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

	public void SetSettings(PackageSorting sorting, string[]? selectedTags)
	{
		LC_Items.DD_Sorting.SelectedItem = sorting;
		LC_Items.DD_Tags.SelectedItems = selectedTags?.Select(ServiceCenter.Get<ITagsService>().CreateWorkshopTag);
	}

	private class TagItem(string value, string icon, bool isCustom) : ITag
	{
		public string Value { get; } = value;
		public string Icon { get; } = icon;
		public bool IsCustom { get; } = isCustom;
	}

	private void InitializeComponent()
	{
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 39);
			// 
			// PC_WorkshopList
			// 
			this.Name = "PC_WorkshopList";
			this.ResumeLayout(false);
			this.PerformLayout();

	}
}
