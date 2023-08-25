using Skyve.App.Interfaces;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_Options : PanelContent
{
	private bool folderPathsChanged;
	private readonly ILocationManager _locationManager = ServiceCenter.Get<ILocationManager>();
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();

	public PC_Options()
	{
		InitializeComponent();
		ApplyCurrentSettings();

		foreach (var button in this.GetControls<SlickButton>())
		{
			if (button != B_ChangeLog && button is not SlickLabel)
			{
				SlickTip.SetTo(button, LocaleHelper.GetGlobalText($"{button.Text}_Tip"));
			}
		}

		TB_GamePath.Text = _locationManager.GamePath;
		TB_AppDataPath.Text = _locationManager.AppDataPath;
		TB_SteamPath.Text = _locationManager.SteamPath;

		if (CrossIO.CurrentPlatform is Platform.Linux)
		{
			TB_GamePath.Placeholder = "Z:\\...\\Steam\\SteamLibrary\\steamapps\\common\\Cities_Skylines";
			TB_AppDataPath.Placeholder = "Z:\\home\\USERNAME\\.local\\share\\Colossal Order\\Cities_Skylines";
			TB_SteamPath.Placeholder = "/usr/bin/steam";
		}

		if (CrossIO.CurrentPlatform is Platform.MacOSX)
		{
			TB_GamePath.Placeholder = "/Users/USERNAME/Library/Application Support/Steam/steamapps/common/Cities_Skylines";
			TB_AppDataPath.Placeholder = "/Users/USERNAME/Library/Application Support/Colossal Order/Cities_Skylines";
			TB_SteamPath.Placeholder = "/Applications/Steam.app/Contents";
		}

		folderPathsChanged = false;

		DD_Language.Items = LocaleHelper.GetAvailableLanguages().Distinct().ToArray();
		DD_Language.SelectedItem = DD_Language.Items.FirstOrDefault(x => x == LocaleHelper.CurrentCulture.IetfLanguageTag) ?? DD_Language.Items[0];
		DD_Language.SelectedItemChanged += DD_Language_SelectedItemChanged;

		if (!CB_ShowFolderSettings.Checked)
		{
			TLP_Folders.Visible = CB_ShowFolderSettings.Checked;
		}
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.AccentBackColor;
	}

	private void ApplyCurrentSettings()
	{
		foreach (var cb in this.GetControls<SlickCheckbox>())
		{
			if (!string.IsNullOrWhiteSpace(cb.Tag?.ToString()))
			{
				cb.Checked = (bool)_settings.UserSettings.GetType()
					.GetProperty(cb.Tag!.ToString(), BindingFlags.Instance | BindingFlags.Public)
					.GetValue(_settings.UserSettings);

				SlickTip.SetTo(cb, LocaleHelper.GetGlobalText($"{cb.Text}_Tip"));

				if (!IsHandleCreated)
				{
					cb.CheckChanged += CB_CheckChanged;
				}
			}
		}

		if (!Directory.Exists(TB_AppDataPath.Text) || !Directory.Exists(TB_GamePath.Text) || !Directory.Exists(TB_SteamPath.Text))
		{
			CB_ShowFolderSettings.Checked = true;
		}
	}

	protected override void LocaleChanged()
	{
		Text = Locale.Options;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		DD_Language.Width = (int)(220 * UI.FontScale);
		TLP_Main.Padding = UI.Scale(new Padding(3, 0, 7, 0), UI.FontScale);
		B_Theme.Padding = B_HelpTranslate.Padding = B_ClearFolders.Padding = B_ChangeLog.Padding = B_Discord.Padding = B_Guide.Padding = B_Reset.Padding = UI.Scale(new Padding(7), UI.FontScale);
		B_Theme.Margin = TLP_UI.Margin = TLP_Settings.Margin = TLP_Advanced.Margin = B_HelpTranslate.Margin = TLP_HelpLogs.Margin =
			B_ClearFolders.Margin = B_Discord.Margin = B_Guide.Margin = B_Reset.Margin = B_ChangeLog.Margin =
			TLP_Preferences.Margin = TLP_Folders.Margin = UI.Scale(new Padding(10), UI.UIScale);
		DD_Language.Margin = UI.Scale(new Padding(10, 7, 10, 5), UI.UIScale);
		slickSpacer1.Height = slickSpacer2.Height = (int)(1.5 * UI.FontScale);
		slickSpacer1.Margin = slickSpacer2.Margin = UI.Scale(new Padding(5), UI.UIScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = design.AccentBackColor;

		foreach (Control item in TLP_Main.Controls)
		{
			item.BackColor = design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, 1, -1));
		}
	}

	public override bool CanExit(bool toBeDisposed)
	{
		if (folderPathsChanged)
		{
			if (ShowPrompt(Locale.ChangingFoldersRequiresRestart, PromptButtons.OKCancel, PromptIcons.Hand) == System.Windows.Forms.DialogResult.OK)
			{
				_locationManager.SetPaths(TB_GamePath.Text, TB_AppDataPath.Text, TB_SteamPath.Text);

				Process.Start(Program.ExecutablePath);

				folderPathsChanged = false;

				Application.Exit();

				return false;
			}
		}

		return true;
	}

	private void CB_CheckChanged(object sender, EventArgs e)
	{
		TLP_Folders.Visible = CB_ShowFolderSettings.Checked;

		if (!IsHandleCreated)
		{
			return;
		}

		var cb = (sender as SlickCheckbox)!;

		_settings.UserSettings.GetType()
			.GetProperty(cb.Tag!.ToString(), BindingFlags.Instance | BindingFlags.Public)
			.SetValue(_settings.UserSettings, cb.Checked);

		_settings.SessionSettings.Save();
	}

	private void TB_FolderPath_TextChanged(object sender, EventArgs e)
	{
		folderPathsChanged = true;
	}

	private void DD_Language_SelectedItemChanged(object sender, EventArgs e)
	{
		try
		{
			LocaleHelper.SetLanguage(new(DD_Language.SelectedItem));
		}
		catch
		{
			ShowPrompt(Locale.CheckDocumentsFolder, Locale.FailedToSaveLanguage, PromptButtons.OK, PromptIcons.Error);
		}
	}

	private void B_Theme_Click(object sender, EventArgs e)
	{
		try
		{
			Form.PushPanel<PC_ThemeChanger>(null);
		}
		catch
		{
			ShowPrompt(Locale.CheckDocumentsFolder, Locale.FailedToOpenTC, PromptButtons.OK, PromptIcons.Error);
		}
	}

	private void B_HelpTranslate_Click(object sender, EventArgs e)
	{
		try
		{
			PlatformUtil.OpenUrl("https://crowdin.com/project/load-order-mod-2");
		}
		catch { }
	}

	private void B_Discord_Click(object sender, EventArgs e)
	{
		try
		{
			PlatformUtil.OpenUrl("https://discord.gg/E4k8ZEtRxd");
		}
		catch { }
	}

	private void B_Guide_Click(object sender, EventArgs e)
	{
		try
		{
			PlatformUtil.OpenUrl("https://bit.ly/40x93vk");
		}
		catch { }
	}

	private void B_Reset_Click(object sender, EventArgs e)
	{
		_settings.ResetUserSettings();

		ApplyCurrentSettings();
	}

	private void B_ClearFolders_Click(object sender, EventArgs e)
	{
		if (ShowPrompt(Locale.ClearFoldersPrompt + "\r\n\r\n" + Locale.AreYouSure, Locale.ClearFoldersPromptTitle, PromptButtons.OKCancel, PromptIcons.Warning) != DialogResult.OK)
		{
			return;
		}

		CrossIO.DeleteFile(CrossIO.Combine(_locationManager.SkyveAppDataPath, "SetupComplete.txt"));

		_settings.SessionSettings.FirstTimeSetupCompleted = false;
		_settings.SessionSettings.Save();
		_settings.ResetFolderSettings();

		Application.Exit();
	}

	private void B_ChangeLog_Click(object sender, EventArgs e)
	{
		Form.PushPanel(ServiceCenter.Get<IInterfaceService>().ChangelogPanel());
	}

	private void slickScroll1_Scroll(object sender, ScrollEventArgs e)
	{
		slickSpacer3.Visible = slickScroll1.Percentage != 0;
	}

	private void AssumeInternetConnectivity_CheckChanged(object sender, EventArgs e)
	{
		ConnectionHandler.AssumeInternetConnectivity = CB_AssumeInternetConnectivity.Checked;
	}
}
