using Skyve.App.UserInterface.Dropdowns;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_ContentList : PanelContent
{
	protected readonly ContentList LC_Items;

	public virtual SkyvePage Page { get; }

	public PC_ContentList() : this(false) { }

	public PC_ContentList(bool load) : base(load)
	{
		InitializeComponent();

		LC_Items = new(Page, !load, GetItems, SetIncluded, SetEnabled, GetItemText, GetCountText)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

		if (this is not PC_GenericPackageList)
		{
			LC_Items.TLP_Main.SetColumn(LC_Items.FLP_Search, 0);
			LC_Items.TLP_Main.SetColumnSpan(LC_Items.FLP_Search, 2);
			LC_Items.TLP_Main.SetColumn(LC_Items.P_FiltersContainer, 0);
			LC_Items.TLP_Main.SetColumnSpan(LC_Items.P_FiltersContainer, 4);
		}

		Controls.Add(LC_Items);

		if (!load)
		{
			if (!ServiceCenter.Get<INotifier>().IsContentLoaded)
			{
				LC_Items.ListControl.Loading = true;
			}
			else
			{
				LC_Items.RefreshItems();
			}
		}
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

	protected virtual IEnumerable<IPackageIdentity> GetItems()
	{
		throw new NotImplementedException();
	}

	protected virtual async Task SetIncluded(IEnumerable<IPackageIdentity> filteredItems, bool included)
	{
		await ServiceCenter.Get<IPackageUtil>().SetIncluded(filteredItems.Cast<Domain.IPackageIdentity>(), included);
	}

	protected virtual async Task SetEnabled(IEnumerable<IPackageIdentity> filteredItems, bool enabled)
	{
		await ServiceCenter.Get<IPackageUtil>().SetEnabled(filteredItems.Cast<Domain.IPackageIdentity>(), enabled);
	}

	protected virtual LocaleHelper.Translation GetItemText()
	{
		throw new NotImplementedException();
	}

	protected virtual string GetCountText()
	{
		throw new NotImplementedException();
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
