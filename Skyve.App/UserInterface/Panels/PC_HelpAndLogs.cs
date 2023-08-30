using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Generic;

using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_HelpAndLogs : PanelContent
{
	private readonly ILogUtil _logUtil;
	private readonly ILogger _logger;
	private readonly ILocationManager _locationManager;

	public PC_HelpAndLogs() : base(true)
	{
		_logUtil = ServiceCenter.Get<ILogUtil>();
		_logger = ServiceCenter.Get<ILogger>();
		_locationManager = ServiceCenter.Get<ILocationManager>();

		InitializeComponent();

		if (CrossIO.CurrentPlatform is Platform.Windows)
		{
			DD_LogFile.StartingFolder = CrossIO.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
		}

		foreach (var button in TLP_HelpLogs.GetControls<SlickButton>())
		{
			if (button != B_ChangeLog && button != B_Donate)
			{
				SlickTip.SetTo(button, LocaleHelper.GetGlobalText($"{button.Text}_Tip"));
			}
		}

		if (CrossIO.CurrentPlatform is not Platform.Windows)
		{
			B_SaveZip.ButtonType = ButtonType.Active;
			B_CopyLogFile.Visible = B_CopyZip.Visible = B_LotLogCopy.Visible = false;
		}
	}

	protected override void LocaleChanged()
	{
		Text = Locale.HelpLogs;
		L_Info.Text = Locale.DefaultLogViewInfo;
		L_Troubleshoot.Text = Locale.TroubleshootInfo;
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.AccentBackColor;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		I_Info.Size = UI.Scale(new Size(24, 24), UI.FontScale);
		TLP_Main.Padding = UI.Scale(new Padding(3, 0, 7, 0), UI.FontScale);
		P_Troubleshoot.Margin = DD_LogFile.Margin = TLP_Errors.Margin = TLP_LogFolders.Margin = TLP_HelpLogs.Margin = UI.Scale(new Padding(10), UI.UIScale);
		L_Troubleshoot.Font = UI.Font(9F);
		L_Troubleshoot.Margin = UI.Scale(new Padding(3), UI.FontScale);

		foreach (var button in this.GetControls<SlickButton>())
		{
			if (button is not SlickLabel)
			{
				button.Margin = UI.Scale(new Padding(10, 7, 10, 7), UI.UIScale);
			}
		}

		B_CopyLogFile.Margin = B_LotLogCopy.Margin = B_SaveZip.Margin = UI.Scale(new Padding(10, 0, 10, 10), UI.UIScale);
		slickSpacer1.Height = slickSpacer2.Height = slickSpacer3.Height = slickSpacer4.Height = (int)(1.5 * UI.FontScale);
		slickSpacer1.Margin = slickSpacer2.Margin = slickSpacer3.Margin = slickSpacer4.Margin = UI.Scale(new Padding(5), UI.UIScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = design.AccentBackColor;
		TLP_Errors.BackColor = TLP_LogFolders.BackColor = TLP_HelpLogs.BackColor = design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, 1, -1));
	}

	protected override bool LoadData()
	{
		if (!File.Exists(_logUtil.GameLogFile))
		{
			return false;
		}

		var tempName = Path.GetTempFileName();

		File.Copy(_logUtil.GameLogFile, tempName, true);

		var logs = _logUtil.SimplifyLog(tempName, out _);

		this.TryInvoke(() => SetTrace(logs));

		return true;
	}

	private async void B_CopyZip_Click(object sender, EventArgs e)
	{
		B_CopyZip.Loading = true;
		await Task.Run(() =>
		{
			try
			{
				_logUtil.CreateZipFileAndSetToClipboard();
			}
			catch (Exception ex) { ShowPrompt(ex, Locale.FailedToFetchLogs); }
		});
		B_CopyZip.Loading = false;

		B_CopyZip.ImageName = "I_Check";
		await Task.Delay(1500);
		B_CopyZip.ImageName = "I_CopyFile";
	}

	private async void B_SaveZip_Click(object sender, EventArgs e)
	{
		B_SaveZip.Loading = true;

		await Task.Run(() =>
		{
			try
			{
				var folder = CrossIO.Combine(_locationManager.SkyveAppDataPath, "Support Logs");

				Directory.CreateDirectory(folder);

				var fileName = _logUtil.CreateZipFileAndSetToClipboard(folder);

				PlatformUtil.OpenFolder(fileName);
			}
			catch (Exception ex) { ShowPrompt(ex, Locale.FailedToFetchLogs); }
		});

		B_SaveZip.Loading = false;

		B_SaveZip.ImageName = "I_Check";
		await Task.Delay(1500);
		B_SaveZip.ImageName = "I_Log";
	}

	private void DD_LogFile_FileSelected(string file)
	{
		if (!CrossIO.FileExists(file))
		{
			DD_LogFile.SelectedFile = string.Empty;
			return;
		}

		DD_LogFile.Loading = true;

		new BackgroundAction("Simplifying Log", () =>
		{
			var zip = file.ToLower().EndsWith(".zip");

			if (zip)
			{
				try
				{
					using var stream = File.OpenRead(file);
					using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read, false);

					var entry = zipArchive.GetEntry("log.txt");

					if (entry is null)
					{
						DD_LogFile.Loading = false;
						return;
					}

					file = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.txt");

					entry.ExtractToFile(file);
				}
				catch
				{
					DD_LogFile.Loading = false;
					return;
				}
			}

			var logs = _logUtil.SimplifyLog(file, out var simpleLog);

			this.TryInvoke(() => SetTrace(logs));

			if (!zip)
			{
				var simpleLogFile = Path.ChangeExtension(file, "small" + Path.GetExtension(file));
				File.WriteAllText(simpleLogFile, simpleLog);

				DD_LogFile.SelectedFile = simpleLogFile;
			}

			DD_LogFile.Loading = false;
		}).Run();
	}

	private void SetTrace(List<ILogTrace> logs)
	{
		TLP_Errors.Controls.Clear(true);
		TLP_Errors.Controls.Add(new LogTraceControl(logs));
		TLP_Errors.Visible = logs.Count > 0;
	}

	private bool DD_LogFile_ValidFile(object sender, string arg)
	{
		return DD_LogFile.ValidExtensions.Any(x => arg.ToLower().EndsWith(x));
	}

	private void B_OpenLogFolder_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenFolder(Path.GetDirectoryName(_logUtil.GameLogFile));
	}

	private void B_CopyLogFile_Click(object sender, EventArgs e)
	{
		PlatformUtil.SetFileInClipboard(_logUtil.GameLogFile);
	}

	private void B_LotLog_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenFolder(Path.GetDirectoryName(_logger.LogFilePath));
	}

	private void B_LotLogCopy_Click(object sender, EventArgs e)
	{
		PlatformUtil.SetFileInClipboard(_logger.LogFilePath);
	}

	private void B_Discord_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenUrl("https://discord.gg/E4k8ZEtRxd");
	}

	private void B_Guide_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenUrl("https://bit.ly/40x93vk");
	}

	private void B_ChangeLog_Click(object sender, EventArgs e)
	{
		Form.PushPanel(ServiceCenter.Get<IInterfaceService>().ChangelogPanel());
	}

	private void B_Donate_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenUrl("https://www.buymeacoffee.com/tdwsvillage");
	}

	private void slickScroll1_Scroll(object sender, ScrollEventArgs e)
	{
		slickSpacer3.Visible = slickScroll1.Percentage != 0;
	}

	private void B_OpenLog_Click(object sender, EventArgs e)
	{
		ServiceCenter.Get<IIOUtil>().Execute(_logUtil.GameLogFile, string.Empty);
	}

	private void B_OpenAppData_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenFolder(_locationManager.AppDataPath);
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
