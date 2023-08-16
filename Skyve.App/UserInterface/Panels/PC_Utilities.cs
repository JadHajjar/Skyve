using Skyve.App.UserInterface.Content;

using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_Utilities : PanelContent
{
	private readonly ISettings _settings;
	private readonly ICitiesManager _citiesManager;
	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly INotifier _notifier;
	private readonly ILocationManager _locationManager;
	private readonly IPackageManager _contentManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly IDownloadService _downloadService;

	public PC_Utilities()
	{
		ServiceCenter.Get(out _settings, out _citiesManager, out _subscriptionsManager, out _notifier, out _locationManager, out _packageUtil, out _contentManager, out _workshopService, out _downloadService);

		InitializeComponent();

		RefreshModIssues();

		B_LoadCollection.Height = 0;

		_notifier.PackageInformationUpdated += RefreshModIssues;

		DD_BOB.StartingFolder = _locationManager.AppDataPath;
		DD_Missing.StartingFolder = DD_Unused.StartingFolder = LsmUtil.GetReportFolder();
		DD_Missing.ValidExtensions = DD_Unused.ValidExtensions = new[] { ".htm", ".html" };

		SlickTip.SetTo(B_LoadCollection, "LoadCollectionTip");
		SlickTip.SetTo(DD_Missing, "LsmMissingTip");
		SlickTip.SetTo(DD_Unused, "LsmUnusedTip");
		SlickTip.SetTo(B_ReDownload, "FixAllTip");
		SlickTip.SetTo(DD_BOB, "XMLTip");
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.AccentBackColor;
	}

	private void RefreshModIssues()
	{
		var modsOutOfDate = _contentManager.Packages.AllWhere(x => _packageUtil.GetStatus(x, out _) == DownloadStatus.OutOfDate);
		var modsIncomplete = _contentManager.Packages.AllWhere(x => _packageUtil.GetStatus(x, out _) == DownloadStatus.PartiallyDownloaded);

		this.TryInvoke(() =>
		{
			L_OutOfDate.Text = Locale.OutOfDateCount.FormatPlural(modsOutOfDate.Count, Locale.Package.FormatPlural(modsOutOfDate.Count).ToLower());
			L_Incomplete.Text = Locale.IncompleteCount.FormatPlural(modsIncomplete.Count, Locale.Package.FormatPlural(modsIncomplete.Count).ToLower());

			P_OutOfDate.Controls.Clear(true);
			P_Incomplete.Controls.Clear(true);

			foreach (var mod in modsOutOfDate)
			{
				P_OutOfDate.Controls.Add(new MiniPackageControl(mod) { Dock = DockStyle.Top, ReadOnly = true });
			}

			foreach (var mod in modsIncomplete)
			{
				P_Incomplete.Controls.Add(new MiniPackageControl(mod) { Dock = DockStyle.Top, ReadOnly = true });
			}

			P_ModIssues.ColumnStyles[0].Width = modsOutOfDate.Count > 0 ? 50 : 0;
			P_ModIssues.ColumnStyles[1].Width = modsIncomplete.Count > 0 ? 50 : 0;

			L_OutOfDate.Visible = P_OutOfDate.Visible = modsOutOfDate.Count > 0;
			L_Incomplete.Visible = P_Incomplete.Visible = modsIncomplete.Count > 0;
			P_ModIssues.Visible = modsOutOfDate.Count > 0 || modsIncomplete.Count > 0;

			if (B_ReDownload.Loading)
			{
				B_ReDownload.Loading = false;

				ShowPrompt(Locale.RedownloadComplete, LocaleSlickUI.TaskCompleted, icon: PromptIcons.Ok);
			}

			if (B_Cleanup.Loading)
			{
				B_Cleanup.Loading = false;

				ShowPrompt(Locale.CleanupComplete, LocaleSlickUI.TaskCompleted, icon: PromptIcons.Ok);
			}
		});
	}

	protected override void LocaleChanged()
	{
		Text = Locale.Utilities;
		L_CleanupInfo.Text = Locale.CleanupInfo;
		L_Troubleshoot.Text = Locale.TroubleshootInfo;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		B_ReDownload.Margin = UI.Scale(new Padding(5), UI.FontScale);
		B_Troubleshoot.Margin = P_Cleanup.Margin = P_Collections.Margin = P_BOB.Margin = P_LsmReport.Margin = P_Troubleshoot.Margin = P_Reset.Margin = P_Text.Margin = P_ModIssues.Margin = UI.Scale(new Padding(10, 0, 10, 10), UI.FontScale);
		B_ReDownload.Margin = TB_CollectionLink.Margin = B_LoadCollection.Margin = UI.Scale(new Padding(5), UI.FontScale);
		B_ImportClipboard.Margin = UI.Scale(new Padding(10), UI.FontScale);
		L_Troubleshoot.Font = L_CleanupInfo.Font = L_OutOfDate.Font = L_Incomplete.Font = UI.Font(9F);
		L_Troubleshoot.Margin = L_CleanupInfo.Margin = L_OutOfDate.Margin = L_Incomplete.Margin = UI.Scale(new Padding(3), UI.FontScale);

		foreach (Control item in P_Reset.Controls)
		{
			item.Margin = UI.Scale(new Padding(5), UI.FontScale);
		}
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = design.AccentBackColor;
		L_CleanupInfo.ForeColor = design.LabelColor;
		L_OutOfDate.ForeColor = design.YellowColor;
		L_Incomplete.ForeColor = design.RedColor;

		foreach (Control item in TLP_Main.Controls)
		{
			item.BackColor = design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, 1, -1));
		}
	}

	private async void B_LoadCollection_Click(object sender, EventArgs e)
	{
		try
		{
			if (!B_LoadCollection.Loading && this.CheckValidation())
			{
				B_LoadCollection.Loading = true;

				var collectionId = Regex.Match(TB_CollectionLink.Text, TB_CollectionLink.ValidationRegex).Groups[1].Value;

				if (ulong.TryParse(collectionId, out var steamId))
				{
					var contents = await _workshopService.GetPackageAsync(new GenericPackageIdentity(steamId));

					if (contents?.Requirements?.Any() ?? false)
					{
						Form.PushPanel(null, new PC_ViewCollection(contents));

						TB_CollectionLink.Text = string.Empty;
					}
				}

				B_LoadCollection.Loading = false;
			}
		}
		catch (Exception ex)
		{
			B_LoadCollection.Loading = false;
			ShowPrompt(ex, LocaleSlickUI.UnexpectedError);
		}
	}

	private async void B_ReDownload_Click(object sender, EventArgs e)
	{
		B_ReDownload.Loading = true;

		await Task.Run(() => _downloadService.Download(_contentManager.Packages.Where(x => _packageUtil.GetStatus(x, out _) is DownloadStatus.OutOfDate or DownloadStatus.PartiallyDownloaded)));
	}

	private void TB_CollectionLink_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		if (e.KeyData == Keys.Enter)
		{
			B_LoadCollection_Click(this, EventArgs.Empty);

			e.IsInputKey = false;
		}
	}

	private void LSMDragDrop_FileSelected(string obj)
	{
		var assets = LsmUtil.LoadMissingAssets(obj);

		Form.PushPanel(null, new PC_GenericPackageList(assets, false) { Text = Locale.MissingLSMReport });
	}

	private bool LSMDragDrop_ValidFile(object sender, string arg)
	{
		return LsmUtil.IsValidLsmReportFile(arg);
	}

	private void LSM_UnusedDrop_FileSelected(string obj)
	{
		var assets = LsmUtil.LoadUnusedAssets(obj);

		Form.PushPanel(null, new PC_GenericPackageList(assets, false) { Text = Locale.UnusedLSMReport });
	}

	private void DD_BOB_FileSelected(string obj)
	{
		var matches = Regex.Matches(File.ReadAllText(obj), "[\\>\"](\\d{8,20})\\.(.+?)[\\<\"]");
		var assets = new List<IPackageIdentity>();

		foreach (Match item in matches)
		{
			if (ulong.TryParse(item.Groups[1].Value, out var id) && !assets.Any(x => x.Id == id))
			{
				assets.Add(new GenericPackageIdentity(id));
			}
		}

		Form.PushPanel(null, new PC_GenericPackageList(assets, true) { Text = LocaleHelper.GetGlobalText(P_BOB.Text) });
	}

	private bool DD_BOB_ValidFile(object sender, string arg)
	{
		return Path.GetExtension(arg).ToLower() == ".xml";
	}

	private bool DD_TextImport_ValidFile(object sender, string arg)
	{
		return true;
	}

	private void DD_TextImport_FileSelected(string obj)
	{
		var matches = Regex.Matches(File.ReadAllText(obj), "(\\d{8,20})");
		var assets = new List<IPackageIdentity>();

		foreach (Match item in matches)
		{
			if (ulong.TryParse(item.Groups[1].Value, out var id) && !assets.Any(x => x.Id == id))
			{
				assets.Add(new GenericPackageIdentity(id));
			}
		}

		Form.PushPanel(null, new PC_GenericPackageList(assets, true) { Text = LocaleHelper.GetGlobalText(P_Text.Text) });
	}

	private void B_ImportClipboard_Click(object sender, EventArgs e)
	{
		if (!Clipboard.ContainsText())
		{
			return;
		}

		var matches = Regex.Matches(Clipboard.GetText(), "(\\d{8,20})");
		var assets = new List<IPackageIdentity>();

		foreach (Match item in matches)
		{
			if (ulong.TryParse(item.Groups[1].Value, out var id) && !assets.Any(x => x.Id == id))
			{
				assets.Add(new GenericPackageIdentity(id));
			}
		}

		Form.PushPanel(null, new PC_GenericPackageList(assets, true) { Text = LocaleHelper.GetGlobalText(B_ImportClipboard.Text) });
	}

	private void B_Cleanup_Click(object sender, EventArgs e)
	{
		if (_citiesManager.IsRunning())
		{
			MessagePrompt.Show(Locale.CloseCitiesToClean, PromptButtons.OK, PromptIcons.Hand, Program.MainForm);
			return;
		}

		if (!_settings.SessionSettings.CleanupFirstTimeShown)
		{
			MessagePrompt.Show(Locale.CleanupRequiresGameToOpen, Locale.CleanupInfoTitle, PromptButtons.OK, PromptIcons.Info, Program.MainForm);

			_settings.SessionSettings.CleanupFirstTimeShown = true;
			_settings.SessionSettings.Save();
		}

		_citiesManager.RunStub();

		B_Cleanup.Loading = true;
		_subscriptionsManager.Redownload = true;
	}

	private void slickScroll1_Scroll(object sender, ScrollEventArgs e)
	{
		slickSpacer1.Visible = slickScroll1.Percentage != 0;
	}

	private async void B_ReloadAllData_Click(object sender, EventArgs e)
	{
		if (!B_ReloadAllData.Loading)
		{
			B_ReloadAllData.Loading = true;
			await Task.Run(ServiceCenter.Get<ICentralManager>().Start);
			B_ReloadAllData.Loading = false;
			var img = B_ReloadAllData.ImageName;
			B_ReloadAllData.ImageName = "I_Check";
			await Task.Delay(1500);
			B_ReloadAllData.ImageName = img;
		}
	}

	private async void B_ResetSnoozes_Click(object sender, EventArgs e)
	{
		var img = B_ResetSnoozes.ImageName;
		ServiceCenter.Get<ICompatibilityManager>().ResetSnoozes();
		B_ResetSnoozes.ImageName = "I_Check";
		await Task.Delay(1500);
		B_ResetSnoozes.ImageName = img;
	}

	private async void B_ResetModsCache_Click(object sender, EventArgs e)
	{
		ServiceCenter.Get<IModDllManager>().ClearDllCache();
		var img = B_ResetModsCache.ImageName;
		B_ResetModsCache.ImageName = "I_Check";
		await Task.Delay(1500);
		B_ResetModsCache.ImageName = img;
	}

	private async void B_ResetCompatibilityCache_Click(object sender, EventArgs e)
	{
		if (!B_ResetCompatibilityCache.Loading)
		{
			B_ResetCompatibilityCache.Loading = true;
			await Task.Run(ServiceCenter.Get<ICompatibilityManager>().ResetCache);
			B_ResetCompatibilityCache.Loading = false;
			var img = B_ResetCompatibilityCache.ImageName;
			B_ResetCompatibilityCache.ImageName = "I_Check";
			await Task.Delay(1500);
			B_ResetCompatibilityCache.ImageName = img;
		}
	}

	private async void B_ResetImageCache_Click(object sender, EventArgs e)
	{
		if (!B_ResetImageCache.Loading)
		{
			B_ResetImageCache.Loading = true;
			await Task.Run(ServiceCenter.Get<IImageService>().ClearCache);
			B_ResetImageCache.Loading = false;
			var img = B_ResetImageCache.ImageName;
			B_ResetImageCache.ImageName = "I_Check";
			await Task.Delay(1500);
			B_ResetImageCache.ImageName = img;
		}
	}

	private async void B_ResetSteamCache_Click(object sender, EventArgs e)
	{
		var img = B_ResetSteamCache.ImageName;
		_workshopService.ClearCache();
		B_ResetSteamCache.ImageName = "I_Check";
		await Task.Delay(1500);
		B_ResetSteamCache.ImageName = img;
	}

	private async void B_Troubleshoot_Click(object sender, EventArgs e)
	{
		var sys = ServiceCenter.Get<ITroubleshootSystem>();

		if (sys.IsInProgress)
		{
			switch (MessagePrompt.Show(Locale.CancelTroubleshootMessage, Locale.CancelTroubleshootTitle, PromptButtons.YesNoCancel, PromptIcons.Hand, form: Program.MainForm))
			{
				case DialogResult.Yes:
					Hide();
					await Task.Run(() => sys.Stop(true));
					break;
				case DialogResult.No:
					Hide();
					await Task.Run(() => sys.Stop(false));
					break;
			}
		}
		else
		{
			Form.PushPanel<PC_Troubleshoot>();
		}
	}
}
