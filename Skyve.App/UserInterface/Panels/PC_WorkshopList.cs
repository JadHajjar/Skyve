using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Dropdowns;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public class PC_WorkshopList : PanelContent
{
	private readonly IWorkshopService _workshopService;
	protected internal readonly WorkshopContentList LC_Items;

	public PC_WorkshopList() : base(false)
	{
		ServiceCenter.Get(out _workshopService);

		Padding = new Padding(0, 30, 0, 0);

		LC_Items = new(SkyvePage.Workshop, false, GetItems, GetItemText, GetCountText)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

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
		if (ulong.TryParse(LC_Items.TB_Search.Text, out var id))
		{
			var package = await _workshopService.GetInfoAsync(new GenericPackageIdentity(id));

			if (package != null)
			{
				return [package];
			}
		}

		return await _workshopService.QueryFilesAsync(WorkshopQuerySorting.Popularity, LC_Items.TB_Search.Text, LC_Items.DD_Tags.SelectedItems.Select(x => x.Value).ToArray());
	}

	protected virtual LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}

	protected virtual string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in LC_Items.Items)
		{
			var package = item.GetLocalPackage();

			if (package is null)
			{
				continue;
			}

			if (package.IsIncluded())
			{
				packagesIncluded++;

				if (package.Package.IsCodeMod)
				{
					modsIncluded++;

					if (package.IsEnabled())
					{
						modsEnabled++;
					}
				}
			}
		}

		var total = LC_Items.ItemCount;

		if (!ServiceCenter.Get<ISettings>().UserSettings.AdvancedIncludeEnable)
		{
			return string.Format(Locale.PackageIncludedTotal, packagesIncluded, total);
		}

		return modsIncluded == modsEnabled
			? string.Format(Locale.PackageIncludedAndEnabledTotal, packagesIncluded, total)
			: string.Format(Locale.PackageIncludedEnabledTotal, packagesIncluded, modsIncluded, modsEnabled, total);
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
