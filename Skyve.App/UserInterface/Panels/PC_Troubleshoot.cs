﻿using Skyve.Compatibility.Domain.Enums;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_Troubleshoot : PanelContent
{
	private readonly TroubleshootSettings _settings = new();

	public PC_Troubleshoot()
	{
		InitializeComponent();

		L_Title.Text = Locale.TroubleshootSelection;
		L_ModAssetTitle.Text = Locale.TroubleshootModOrAsset;
		B_Mods.Title = Locale.Mod.Plural;
		B_Assets.Title = Locale.Asset.Plural;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		L_CompInfo.Font = L_Title.Font = L_ModAssetTitle.Font = UI.Font(12.75F, System.Drawing.FontStyle.Bold);
		L_CompInfo.Margin = L_Title.Margin = L_ModAssetTitle.Margin = UI.Scale(new Padding(6));
		B_Cancel.Font = B_Cancel2.Font = B_Cancel3.Font = UI.Font(9.75F);
		B_Cancel.Margin = B_Cancel2.Margin = B_Cancel3.Margin = UI.Scale(new Padding(10));
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
			B_CompView.Focus();
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

		await StartTroubleshooting();
	}

	private async void B_Assets_Click(object sender, EventArgs e)
	{
		if (B_Mods.Loading || B_Assets.Loading)
		{
			return;
		}

		B_Assets.Loading = true;

		await StartTroubleshooting();
	}

	private async Task StartTroubleshooting()
	{
		var applyResult = await ServiceCenter.Get<ITroubleshootSystem>().Start(_settings);

		if (applyResult >= TroubleshootResult.Error)
		{
			MessagePrompt.Show(Locale.TroubleshootActionFailed
				+ "\r\n\r\n"
				+ LocaleHelper.GetGlobalText($"Troubleshoot{applyResult}"), PromptButtons.OK, PromptIcons.Error, Program.MainForm);
		}

		PushBack();
	}

	private void B_CompView_Click(object sender, EventArgs e)
	{
		Form.PushPanel<PC_CompatibilityReport>();
	}

	private void B_CompSkip_Click(object sender, EventArgs e)
	{
		Next2();
	}

	private void Next2()
	{
#if CS1
		if (_settings.ItemIsCausingIssues || _settings.NewItemCausingIssues)
		{
			var faultyPackages = ServiceCenter.Get<IPackageManager>().Packages.AllWhere(x => x.IsIncluded() && CheckStrict(x));

			if (faultyPackages.Count > 0 && ShowPrompt(Locale.SkyveDetectedFaultyPackages, Locale.FaultyPackagesTitle, PromptButtons.YesNo, PromptIcons.Warning) == DialogResult.Yes)
			{
				new BackgroundAction(() =>
				{
					ServiceCenter.Get<ITroubleshootSystem>().CleanDownload(faultyPackages.ToList(x => x.LocalData!));
				}).Run();

				PushBack();
				return;
			}
		}
#endif

		TLP_ModAsset.Show();
		TLP_Comp.Hide();
		TLP_New.Hide();
		B_Mods.Focus();
	}

#if CS1
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
#endif

	private class TroubleshootSettings : ITroubleshootSettings
	{
		public bool Mods { get; set; }
		public bool ItemIsCausingIssues { get; set; }
		public bool ItemIsMissing { get; set; }
		public bool NewItemCausingIssues { get; set; }
	}
}
