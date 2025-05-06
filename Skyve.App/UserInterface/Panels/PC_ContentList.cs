using Skyve.App.UserInterface.Dropdowns;

using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_ContentList : PanelContent
{
	protected internal readonly ContentList LC_Items;
	private readonly IPackageUtil _packageUtil;
	private readonly bool _itemsReady;

	public virtual SkyvePage Page { get; }

	public PC_ContentList() : this(false) { }

	public PC_ContentList(bool load, bool itemsReady = false, IPackageUtil? customPackageUtil = null) : base(load)
	{
		Padding = new Padding(0, 30, 0, 0);

		_packageUtil = customPackageUtil ?? ServiceCenter.Get<IPackageUtil>();

		LC_Items = new(Page, !load, GetItems, GetItemText, customPackageUtil)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

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

			if (!ServiceCenter.Get<ISettings>().UserSettings.ExcludeTipShown && ExtensionClass.RNG.NextDouble() > 0.6)
			{
				new Timer
				{
					Interval = 4000,
					Enabled = true,
				}.Tick += (s, e) =>
				{
					((Timer)s).Enabled = false;
					((Timer)s).Dispose();

					if (Form.CurrentPanel == this)
					{
						ShowExcludedTip();
					}
				};
			}
		}
	}

	private void ShowExcludedTip()
	{
		var userSettings = ServiceCenter.Get<ISettings>().UserSettings;

		userSettings.ExcludeTipShown = true;
		userSettings.Save();

		Notification.Create(Locale.ExclusionTutorialTitle, Locale.ExclusionTutorialText, PromptIcons.Info, size: new Size(300, 140)).Show(Form, 20);
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

	protected virtual Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	protected virtual async Task SetIncluded(IEnumerable<IPackageIdentity> filteredItems, bool included)
	{
		await _packageUtil.SetIncluded(filteredItems, included);
	}

	protected virtual async Task SetEnabled(IEnumerable<IPackageIdentity> filteredItems, bool enabled)
	{
		await _packageUtil.SetEnabled(filteredItems, enabled);
	}

	protected virtual LocaleHelper.Translation GetItemText()
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
