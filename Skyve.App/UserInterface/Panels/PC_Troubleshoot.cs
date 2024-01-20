using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_Troubleshoot : PanelContent
{
	private readonly TroubleshootSettings _settings = new();
	private readonly IModLogicManager _modLogicManager;
	private readonly IModUtil _modUtil;
	private readonly ILogger _logger;

	public PC_Troubleshoot()
	{
		ServiceCenter.Get(out _modLogicManager, out _modUtil, out _logger);

		InitializeComponent();

		L_Title.Text = Locale.TroubleshootSelection;
		L_ModAssetTitle.Text = Locale.TroubleshootModOrAsset;
		B_Mods.Text = Locale.Mod.Plural;
		B_Assets.Text = Locale.Asset.Plural;
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		L_Title.ForeColor = L_ModAssetTitle.ForeColor = design.ActiveColor;
		L_CompInfo.ForeColor = design.RedColor;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		L_CompInfo.Font = L_Title.Font = L_ModAssetTitle.Font = UI.Font(10.5F, System.Drawing.FontStyle.Bold);
		B_Cancel.Font = UI.Font(9.75F);
	}

	private void B_Cancel_Click(object sender, EventArgs e)
	{
		PushBack();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			PushBack();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void B_Caused_Click(object sender, EventArgs e)
	{
		_settings.ItemIsCausingIssues = true;

		Next();
	}

	private void B_Missing_Click(object sender, EventArgs e)
	{
		_settings.ItemIsMissing = true;

		Next();
	}

	private void B_New_Click(object sender, EventArgs e)
	{
		_settings.NewItemCausingIssues = true;

		Next();
	}

	private void Next()
	{
		var showComp = ServiceCenter.Get<IPackageManager>().Packages.Count(x => x.IsIncluded() && x.GetCompatibilityInfo().GetNotification() > NotificationType.Warning);

		if (showComp > 0)
		{
			L_CompInfo.Text = Locale.TroubleshootCompAsk.FormatPlural(showComp);
			TLP_Comp.Show();
			TLP_New.Hide();
		}
		else
		{
			Next2();
		}
	}

	private async void B_Mods_Click(object sender, EventArgs e)
	{
		if (B_Mods.Loading || B_Assets.Loading)
		{
			return;
		}

		B_Mods.Loading = true;

		_settings.Mods = true;

		await Task.Run(() => ServiceCenter.Get<ITroubleshootSystem>().Start(_settings));

		//Form.SetPanel<PC_MainPage>(Program.MainForm.PI_Dashboard);
	}

	private async void B_Assets_Click(object sender, EventArgs e)
	{
		if (B_Mods.Loading || B_Assets.Loading)
		{
			return;
		}

		B_Assets.Loading = true;

		await Task.Run(() => ServiceCenter.Get<ITroubleshootSystem>().Start(_settings));

		//Form.SetPanel<PC_MainPage>(Program.MainForm.PI_Dashboard);
	}

	private void B_CompView_Click(object sender, EventArgs e)
	{
		//Form.SetPanel<PC_CompatibilityReport>(Program.MainForm.PI_Compatibility);
	}

	private void B_CompSkip_Click(object sender, EventArgs e)
	{
		Next2();
	}

	private void Next2()
	{
		if (_settings.ItemIsCausingIssues || _settings.NewItemCausingIssues)
		{
			var faultyPackages = ServiceCenter.Get<IPackageManager>().Packages.AllWhere(x => x.IsIncluded() && CheckStrict(x));

			if (faultyPackages.Count > 0 && ShowPrompt(Locale.SkyveDetectedFaultyPackages, Locale.FaultyPackagesTitle, PromptButtons.YesNo, PromptIcons.Warning) == DialogResult.Yes)
			{
				new BackgroundAction(() =>
				{
					ServiceCenter.Get<ITroubleshootSystem>().CleanDownload(faultyPackages.ToList(x=>x.LocalData!));
				}).Run();

				PushBack();
				return;
			}
		}

		TLP_ModAsset.Show();
		TLP_Comp.Hide();
		TLP_New.Hide();
	}

	private bool CheckStrict(IPackage localPackage)
	{
		var workshopInfo = localPackage.GetWorkshopInfo();

		if (localPackage.IsLocal)
		{
			return false;
		}

		if (localPackage.IsCodeMod && _modLogicManager.IsRequired(localPackage.LocalData, _modUtil))
		{
			return false;
		}

		if (workshopInfo is null)
		{
			return false;
		}

		var sizeServer = workshopInfo.ServerSize;
		var localSize = localPackage.LocalData.FileSize;

		if (sizeServer != 0 && localSize != 0 && sizeServer != localSize)
		{
			return true;
		}

		var updatedServer = workshopInfo.ServerTime;
		var updatedLocal = localPackage.LocalData.LocalTime;

		if (updatedServer != default && updatedLocal != default && updatedServer > updatedLocal)
		{
			return true;
		}

		return false;
	}

	private class TroubleshootSettings : ITroubleshootSettings
	{
		public bool Mods { get; set; }
		public bool ItemIsCausingIssues { get; set; }
		public bool ItemIsMissing { get; set; }
		public bool NewItemCausingIssues { get; set; }
	}
}
