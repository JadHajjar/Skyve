using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Dropdowns;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public class PC_WorkshopList : PanelContent
{
	private readonly IWorkshopService _workshopService;
	protected internal readonly WorkshopContentList LC_Items;
	private int currentPage;
	private bool listLoading;

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

	protected virtual async Task<IEnumerable<IPackageIdentity>> GetItems()
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
		if (!listLoading)
		{
			Task.Run(async () => LC_Items.ListControl.AddRange(await GetPackages(currentPage + 1)));
		}
	}

	private async Task<IEnumerable<IPackageIdentity>> GetPackages(int page)
	{
		listLoading = true;

		try
		{
			var list = await _workshopService.QueryFilesAsync(
				WorkshopQuerySorting.Popularity,
				LC_Items.TB_Search.Text,
				LC_Items.DD_Tags.SelectedItems.Select(x => x.Value).ToArray(),
				limit: 30,
				page: currentPage = page);

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

	public void SetSorting(PackageSorting packageSorting, bool desc)
	{
		LC_Items.ListControl.SetSorting(packageSorting, desc);
	}

	public void SetCompatibilityFilter(CompatibilityNotificationFilter filter)
	{
		LC_Items.DD_ReportSeverity.SelectedItem = filter;
	}

	public void SetIncludedFilter(Generic.ThreeOptionToggle.Value filter)
	{
		LC_Items.OT_Included.SelectedValue = filter;
	}
}
