using Skyve.App.Interfaces;
using Skyve.App.Utilities;

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
	private readonly ISettings _settings;
	private readonly ILocationService _locationManager;
	private readonly System.Timers.Timer timer;
	private bool orderAsc;
	private List<ILogTrace>? selectedFileLogs;
	private List<ILogTrace>? defaultLogs;

	public PC_HelpAndLogs() : base(true)
	{
		ServiceCenter.Get(out _logger, out _logUtil, out _settings, out _locationManager);

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
			B_CopyZip.Visible = false;
		}

		timer = new System.Timers.Timer(5000);
		timer.Elapsed += Timer_Elapsed;
		timer.Start();
	}

	private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		if (CrossIO.FileExists(DD_LogFile.SelectedFile))
		{
			return;
		}

		LoadData();
	}

	protected override void LocaleChanged()
	{
		Text = Locale.HelpLogs;
		L_Troubleshoot.Text = Locale.TroubleshootInfo;
		TB_Search.Placeholder = LocaleSlickUI.Search + "..";
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 2 : -5);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		I_Sort.Size = UI.Scale(new Size(24, 24), UI.FontScale);
		I_Sort.Padding = UI.Scale(new Padding(3), UI.FontScale);
		TLP_Main.Padding = UI.Scale(new Padding(3, 0, 7, 0), UI.FontScale);
		TLP_LogFiles.Margin = TLP_LogFolders.Margin = TLP_HelpLogs.Margin = UI.Scale(new Padding(10, 10, 10, 0), UI.UIScale);
		TLP_LogFiles.Padding = TLP_Errors.Padding =  UI.Scale(new Padding(12), UI.UIScale);
		DD_LogFile.Margin = UI.Scale(new Padding(12,14,12,6), UI.UIScale);
		P_Troubleshoot.Margin = UI.Scale(new Padding(10, 10, 5, 0), UI.UIScale);
		TLP_Errors.Margin = UI.Scale(new Padding(10, 10, 5, 10), UI.UIScale);
		L_Troubleshoot.Font = UI.Font(9F);
		CB_OnlyShowErrors.Font = UI.Font(7.5F);
		CB_OnlyShowErrors.Padding = TB_Search.Margin = B_OpenSkyveLog.Margin = B_OpenLog.Margin = UI.Scale(new Padding(3), UI.FontScale);
		tableLayoutPanel1.Width = (int)(250 * UI.FontScale);
		B_OpenSkyveLog.Height = B_OpenLog.Height = (int)(48 * UI.FontScale);

		foreach (var button in this.GetControls<SlickButton>())
		{
			if (button is not SlickLabel && button != B_Troubleshoot)
			{
				button.Margin = UI.Scale(new Padding(10, 7, 10, 7), UI.UIScale);
			}
		}

		TLP_Main.RowStyles[0].Height = (int)(125 * UI.FontScale);
		TB_Search.Width = (int)(150 * UI.FontScale);
		B_SaveZip.Margin = UI.Scale(new Padding(10, 7, 10, 10), UI.UIScale);
		slickSpacer1.Height = (int)(1.5 * UI.FontScale);
		slickSpacer1.Margin = UI.Scale(new Padding(5), UI.UIScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		P_Troubleshoot.BackColor = TLP_LogFiles.BackColor = TLP_Errors.BackColor = TLP_LogFolders.BackColor = TLP_HelpLogs.BackColor = design.BackColor;
	}

	protected override bool LoadData()
	{
		try
		{
			defaultLogs = _logUtil.GetCurrentLogsTrace();

			this.TryInvoke(() => SetTrace(defaultLogs));

			return true;
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, "Failed to get current logs trace");
			return false;
		}
	}

	private async void B_CopyZip_Click(object sender, EventArgs e)
	{
		B_CopyZip.Loading = true;
		await Task.Run(() =>
		{
			try
			{
				var file = _logUtil.CreateZipFile();

				PlatformUtil.SetFileInClipboard(file);
			}
			catch (Exception ex)
			{
				ShowPrompt(ex, Locale.FailedToFetchLogs);
			}
		});
		B_CopyZip.Loading = false;

		B_CopyZip.ImageName = "Check";
		await Task.Delay(1500);
		B_CopyZip.ImageName = "CopyFile";
	}

	private async void B_SaveZip_Click(object sender, EventArgs e)
	{
		B_SaveZip.Loading = true;

		await Task.Run(() =>
		{
			try
			{
				var folder = CrossIO.Combine(_locationManager.SkyveDataPath, ".SupportLogs");

				Directory.CreateDirectory(folder);

				var fileName = _logUtil.CreateZipFile(folder);

				PlatformUtil.OpenFolder(fileName);
			}
			catch (Exception ex)
			{
				ShowPrompt(ex, Locale.FailedToFetchLogs);
			}
		});

		B_SaveZip.Loading = false;

		B_SaveZip.ImageName = "Check";
		await Task.Delay(1500);
		B_SaveZip.ImageName = "Log";
	}

	private async void DD_LogFile_FileSelected(string file)
	{
		if (!CrossIO.FileExists(file))
		{
			DD_LogFile.SelectedFile = string.Empty;
			return;
		}

		DD_LogFile.Loading = true;

		selectedFileLogs = await Task.Run(() =>
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
						return null;
					}

					file = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.txt");

					entry.ExtractToFile(file);
				}
				catch
				{
					return null;
				}
			}

#if CS2
			var logs = _logUtil.ExtractTrace(file, file);

			DD_LogFile.SelectedFile = file;
#else
			var logs = _logUtil.SimplifyLog(file, file, out var simpleLog);

			if (!zip)
			{
				var simpleLogFile = Path.ChangeExtension(file, "small" + Path.GetExtension(file));
				File.WriteAllText(simpleLogFile, simpleLog);

				DD_LogFile.SelectedFile = simpleLogFile;
			}
#endif

			return logs;
		});

		this.TryInvoke(() => SetTrace(selectedFileLogs ?? defaultLogs ?? []));

		DD_LogFile.Loading = false;
	}

	private void CB_OnlyShowErrors_CheckChanged(object sender, EventArgs e)
	{
		if (CrossIO.FileExists(DD_LogFile.SelectedFile))
		{
			SetTrace(selectedFileLogs ?? []);
		}
		else
		{
			SetTrace(defaultLogs ?? []);
		}
	}

	private void I_Sort_Click(object sender, EventArgs e)
	{
		orderAsc = !orderAsc;

		I_Sort.ImageName = orderAsc ? "SortAsc" : "SortDesc";

		CB_OnlyShowErrors_CheckChanged(sender, e);
	}

	private void SetTrace(List<ILogTrace> logs)
	{
		IEnumerable<ILogTrace> logEnumerable = logs;

		if (CB_OnlyShowErrors.Checked)
		{
			logEnumerable = logEnumerable.Where(x => x.Type is not "INFO" and not "DEBUG");
		}

		if (!string.IsNullOrWhiteSpace(TB_Search.Text))
		{
			logEnumerable = logEnumerable.Where(SearchLog);
		}

		if (orderAsc)
		{
			logEnumerable = logEnumerable.OrderBy(x => x.Timestamp);
		}
		else
		{
			logEnumerable = logEnumerable.OrderByDescending(x => x.Timestamp);
		}

		logTraceControl.SetItems(logEnumerable);
		logTraceControl.Invalidate();
		P_ErrorsContainer.Invalidate();
	}

	private bool SearchLog(ILogTrace trace)
	{
		if (trace.Type.Contains(TB_Search.Text, StringComparison.InvariantCultureIgnoreCase)
			|| Path.GetFileName(trace.SourceFile).Contains(TB_Search.Text, StringComparison.InvariantCultureIgnoreCase)
			|| TB_Search.Text.SearchCheck(trace.Title))
		{
			return true;
		}

		for (var i = 0; i < trace.Trace.Count; i++)
		{
			if (TB_Search.Text.SearchCheck(trace.Trace[i]))
			{
				return true;
			}
		}

		return false;
	}

	private bool DD_LogFile_ValidFile(object sender, string arg)
	{
		return DD_LogFile.ValidExtensions.Any(x => arg.ToLower().EndsWith(x));
	}

	private void B_OpenLogFolder_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenFolder(_logUtil.GameLogFolder);
	}

	private void B_LotLog_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenFolder(Path.GetDirectoryName(_logger.LogFilePath));
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
		Form.PushPanel(ServiceCenter.Get<IAppInterfaceService>().ChangelogPanel());
	}

	private void B_Donate_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenUrl("https://www.buymeacoffee.com/tdwsvillage");
	}

	private void B_OpenLog_Click(object sender, EventArgs e)
	{
		ServiceCenter.Get<IIOUtil>().Execute(_logUtil.GameLogFile, string.Empty);
	}

	private void B_OpenAppData_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenFolder(_settings.FolderSettings.AppDataPath);
	}

	private async void B_Troubleshoot_Click(object sender, EventArgs e)
	{
		ShowPrompt("Coming soon...", icon: PromptIcons.Info);return;
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

	private void B_OpenSkyveLog_Click(object sender, EventArgs e)
	{
		ServiceCenter.Get<IIOUtil>().Execute(_logger.LogFilePath, string.Empty);
	}

	private void slickScroll1_Scroll(object sender, ScrollEventArgs e)
	{
		slickSpacer3.Visible = slickScroll1.Percentage != 0;
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private void TB_Search_TextChanged(object sender, EventArgs e)
	{
		TB_Search.ImageName = string.IsNullOrWhiteSpace(TB_Search.Text) ? "Search" : "ClearSearch";

		CB_OnlyShowErrors_CheckChanged(sender, e);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == (Keys.Control | Keys.F))
		{
			TB_Search.Focus();
			TB_Search.SelectAll();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void P_ErrorsContainer_Paint(object sender, PaintEventArgs e)
	{
		if (logTraceControl.Items.Any())
			return;

		using var font = UI.Font(9.75F, FontStyle.Italic);
		using var brush = new SolidBrush(FormDesign.Design.InfoColor);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };

		e.Graphics.DrawString(Locale.DefaultLogViewInfo, font, brush, P_ErrorsContainer.ClientRectangle.Pad(Math.Min(P_ErrorsContainer.Height, P_ErrorsContainer. Width) / 3), format);
	}
}
