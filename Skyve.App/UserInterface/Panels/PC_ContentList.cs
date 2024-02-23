using Skyve.App.UserInterface.Dropdowns;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_ContentList : PanelContent
{
	protected readonly ContentList LC_Items;
	private readonly bool _itemsReady;

	public virtual SkyvePage Page { get; }

	public PC_ContentList() : this(false) { }

	public PC_ContentList(bool load, bool itemsReady = false) : base(load)
	{
		Padding = new Padding(0, 30, 0, 0);

		LC_Items = new(Page, !load, GetItems, SetIncluded, SetEnabled, GetItemText, GetCountText)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

		//if (this is not PC_GenericPackageList)
		//{
		//	LC_Items.TLP_Main.SetColumn(LC_Items.FLP_Search, 0);
		//	LC_Items.TLP_Main.SetColumnSpan(LC_Items.FLP_Search, 2);
		//	LC_Items.TLP_Main.SetColumn(LC_Items.P_FiltersContainer, 0);
		//	LC_Items.TLP_Main.SetColumnSpan(LC_Items.P_FiltersContainer, 4);
		//}

		Controls.Add(LC_Items);

		if (!load && ServiceCenter.Get<INotifier>().IsContentLoaded)
		{
			_itemsReady = true;
		}
		else
		{
			_itemsReady = itemsReady;
		}
	}

	protected override async void OnCreateControl()
	{
		base.OnCreateControl();

		if (_itemsReady)
		{
			await LC_Items.RefreshItems();
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

	protected virtual Task<IEnumerable<IPackageIdentity>> GetItems()
	{
		throw new NotImplementedException();
	}

	protected virtual async Task SetIncluded(IEnumerable<IPackageIdentity> filteredItems, bool included)
	{
		await ServiceCenter.Get<IPackageUtil>().SetIncluded(filteredItems, included);
	}

	protected virtual async Task SetEnabled(IEnumerable<IPackageIdentity> filteredItems, bool enabled)
	{
		await ServiceCenter.Get<IPackageUtil>().SetEnabled(filteredItems, enabled);
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
