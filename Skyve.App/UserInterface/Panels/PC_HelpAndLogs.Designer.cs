using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Generic;

namespace Skyve.App.UserInterface.Panels;

partial class PC_HelpAndLogs
{
	/// <summary> 
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary> 
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			timer.Dispose();
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Component Designer generated code

	/// <summary> 
	/// Required method for Designer support - do not modify 
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon12 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon6 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon7 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon8 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon9 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon10 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon11 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon16 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon13 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon14 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon15 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon17 = new SlickControls.DynamicIcon();
			this.TLP_Main = new SlickControls.SmartTablePanel();
			this.TLP_Errors = new SlickControls.RoundedTableLayoutPanel();
			this.CB_OnlyShowErrors = new SlickControls.SlickCheckbox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.L_Info = new SlickControls.AutoSizeLabel();
			this.slickSpacer3 = new SlickControls.SlickSpacer();
			this.logTraceControl = new Skyve.App.UserInterface.Generic.LogTraceControl();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.I_Sort = new SlickControls.SlickIcon();
			this.P_Troubleshoot = new SlickControls.RoundedGroupTableLayoutPanel();
			this.B_Troubleshoot = new SlickControls.SlickButton();
			this.L_Troubleshoot = new SlickControls.AutoSizeLabel();
			this.DD_LogFile = new Skyve.App.UserInterface.Generic.DragAndDropControl();
			this.B_OpenLog = new Skyve.App.UserInterface.Content.IconTopButton();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.TLP_HelpLogs = new SlickControls.RoundedGroupTableLayoutPanel();
			this.B_Donate = new SlickControls.SlickButton();
			this.B_ChangeLog = new SlickControls.SlickButton();
			this.B_SaveZip = new SlickControls.SlickButton();
			this.B_CopyZip = new SlickControls.SlickButton();
			this.B_Discord = new SlickControls.SlickButton();
			this.B_Guide = new SlickControls.SlickButton();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.TLP_LogFolders = new SlickControls.RoundedGroupTableLayoutPanel();
			this.B_OpenAppData = new SlickControls.SlickButton();
			this.B_LotLog = new SlickControls.SlickButton();
			this.B_OpenLogFolder = new SlickControls.SlickButton();
			this.TLP_LogFiles = new SlickControls.RoundedTableLayoutPanel();
			this.B_OpenSkyveLog = new Skyve.App.UserInterface.Content.IconTopButton();
			this.TLP_Main.SuspendLayout();
			this.TLP_Errors.SuspendLayout();
			this.panel1.SuspendLayout();
			this.P_Troubleshoot.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.TLP_HelpLogs.SuspendLayout();
			this.TLP_LogFolders.SuspendLayout();
			this.TLP_LogFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 39);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 2;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Main.Controls.Add(this.TLP_Errors, 0, 1);
			this.TLP_Main.Controls.Add(this.P_Troubleshoot, 0, 0);
			this.TLP_Main.Controls.Add(this.DD_LogFile, 1, 0);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(329, 30);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 2;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Size = new System.Drawing.Size(842, 707);
			this.TLP_Main.TabIndex = 13;
			// 
			// TLP_Errors
			// 
			this.TLP_Errors.AddOutline = true;
			this.TLP_Errors.ColumnCount = 4;
			this.TLP_Main.SetColumnSpan(this.TLP_Errors, 2);
			this.TLP_Errors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Errors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Errors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Errors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Errors.Controls.Add(this.CB_OnlyShowErrors, 1, 0);
			this.TLP_Errors.Controls.Add(this.panel1, 0, 1);
			this.TLP_Errors.Controls.Add(this.TB_Search, 0, 0);
			this.TLP_Errors.Controls.Add(this.I_Sort, 3, 0);
			this.TLP_Errors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Errors.Location = new System.Drawing.Point(3, 159);
			this.TLP_Errors.Name = "TLP_Errors";
			this.TLP_Errors.Padding = new System.Windows.Forms.Padding(8);
			this.TLP_Errors.RowCount = 2;
			this.TLP_Errors.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Errors.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Errors.Size = new System.Drawing.Size(836, 545);
			this.TLP_Errors.TabIndex = 2;
			this.TLP_Errors.Text = "ErrorsInLog";
			// 
			// CB_OnlyShowErrors
			// 
			this.CB_OnlyShowErrors.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.CB_OnlyShowErrors.AutoSize = true;
			this.CB_OnlyShowErrors.Checked = true;
			this.CB_OnlyShowErrors.CheckedText = null;
			this.CB_OnlyShowErrors.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_OnlyShowErrors.DefaultValue = true;
			this.CB_OnlyShowErrors.EnterTriggersClick = false;
			this.CB_OnlyShowErrors.Location = new System.Drawing.Point(167, 11);
			this.CB_OnlyShowErrors.Name = "CB_OnlyShowErrors";
			this.CB_OnlyShowErrors.Size = new System.Drawing.Size(159, 44);
			this.CB_OnlyShowErrors.SpaceTriggersClick = true;
			this.CB_OnlyShowErrors.TabIndex = 25;
			this.CB_OnlyShowErrors.Text = "OnlyShowErrors";
			this.CB_OnlyShowErrors.UncheckedText = null;
			this.CB_OnlyShowErrors.CheckChanged += new System.EventHandler(this.CB_OnlyShowErrors_CheckChanged);
			// 
			// panel1
			// 
			this.TLP_Errors.SetColumnSpan(this.panel1, 4);
			this.panel1.Controls.Add(this.L_Info);
			this.panel1.Controls.Add(this.slickSpacer3);
			this.panel1.Controls.Add(this.logTraceControl);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(8, 59);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(820, 478);
			this.panel1.TabIndex = 27;
			// 
			// L_Info
			// 
			this.L_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.L_Info.HorizontalAlignment = System.Drawing.StringAlignment.Center;
			this.L_Info.Location = new System.Drawing.Point(0, 205);
			this.L_Info.Margin = new System.Windows.Forms.Padding(3, 10, 10, 10);
			this.L_Info.Name = "L_Info";
			this.L_Info.Size = new System.Drawing.Size(784, 69);
			this.L_Info.TabIndex = 20;
			this.L_Info.VerticalAlignment = System.Drawing.StringAlignment.Center;
			// 
			// slickSpacer3
			// 
			this.slickSpacer3.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer3.Location = new System.Drawing.Point(0, 0);
			this.slickSpacer3.Name = "slickSpacer3";
			this.slickSpacer3.Size = new System.Drawing.Size(812, 1);
			this.slickSpacer3.TabIndex = 19;
			this.slickSpacer3.TabStop = false;
			this.slickSpacer3.Text = "slickSpacer3";
			// 
			// logTraceControl
			// 
			this.logTraceControl.AutoInvalidate = false;
			this.logTraceControl.Cursor = System.Windows.Forms.Cursors.Default;
			this.logTraceControl.Location = new System.Drawing.Point(0, 0);
			this.logTraceControl.Name = "logTraceControl";
			this.logTraceControl.Size = new System.Drawing.Size(812, 1);
			this.logTraceControl.TabIndex = 1;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.logTraceControl;
			this.slickScroll1.Location = new System.Drawing.Point(812, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(8, 478);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 0;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			this.slickScroll1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.slickScroll1_Scroll);
			// 
			// TB_Search
			// 
			this.TB_Search.Anchor = System.Windows.Forms.AnchorStyles.Right;
			dynamicIcon1.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon1;
			this.TB_Search.LabelText = "";
			this.TB_Search.Location = new System.Drawing.Point(11, 11);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(6, 6, 44, 6);
			this.TB_Search.Placeholder = "Search";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(150, 45);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// I_Sort
			// 
			this.I_Sort.ActiveColor = null;
			this.I_Sort.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.I_Sort.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_SortDesc";
			this.I_Sort.ImageName = dynamicIcon2;
			this.I_Sort.Location = new System.Drawing.Point(793, 26);
			this.I_Sort.Name = "I_Sort";
			this.I_Sort.Size = new System.Drawing.Size(32, 14);
			this.I_Sort.TabIndex = 26;
			this.I_Sort.Click += new System.EventHandler(this.I_Sort_Click);
			// 
			// P_Troubleshoot
			// 
			this.P_Troubleshoot.AddOutline = true;
			this.P_Troubleshoot.ColorStyle = Extensions.ColorStyle.Yellow;
			this.P_Troubleshoot.ColumnCount = 2;
			this.P_Troubleshoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Troubleshoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Troubleshoot.Controls.Add(this.B_Troubleshoot, 1, 0);
			this.P_Troubleshoot.Controls.Add(this.L_Troubleshoot, 0, 1);
			this.P_Troubleshoot.Dock = System.Windows.Forms.DockStyle.Fill;
			dynamicIcon4.Name = "I_Wrench";
			this.P_Troubleshoot.ImageName = dynamicIcon4;
			this.P_Troubleshoot.Location = new System.Drawing.Point(3, 3);
			this.P_Troubleshoot.Name = "P_Troubleshoot";
			this.P_Troubleshoot.Padding = new System.Windows.Forms.Padding(8);
			this.P_Troubleshoot.RowCount = 2;
			this.P_Troubleshoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
			this.P_Troubleshoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Troubleshoot.Size = new System.Drawing.Size(415, 150);
			this.P_Troubleshoot.TabIndex = 24;
			this.P_Troubleshoot.Text = "TroubleshootIssues";
			this.P_Troubleshoot.UseFirstRowForPadding = true;
			// 
			// B_Troubleshoot
			// 
			this.B_Troubleshoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Troubleshoot.AutoSize = true;
			this.B_Troubleshoot.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_ArrowRight";
			this.B_Troubleshoot.ImageName = dynamicIcon3;
			this.B_Troubleshoot.Location = new System.Drawing.Point(175, 107);
			this.B_Troubleshoot.Name = "B_Troubleshoot";
			this.P_Troubleshoot.SetRowSpan(this.B_Troubleshoot, 2);
			this.B_Troubleshoot.Size = new System.Drawing.Size(229, 32);
			this.B_Troubleshoot.SpaceTriggersClick = true;
			this.B_Troubleshoot.TabIndex = 14;
			this.B_Troubleshoot.Text = "ViewTroubleshootOptions";
			this.B_Troubleshoot.Click += new System.EventHandler(this.B_Troubleshoot_Click);
			// 
			// L_Troubleshoot
			// 
			this.L_Troubleshoot.AutoSize = true;
			this.L_Troubleshoot.Location = new System.Drawing.Point(11, 56);
			this.L_Troubleshoot.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.L_Troubleshoot.Name = "L_Troubleshoot";
			this.L_Troubleshoot.Size = new System.Drawing.Size(158, 0);
			this.L_Troubleshoot.TabIndex = 16;
			// 
			// DD_LogFile
			// 
			this.DD_LogFile.AllowDrop = true;
			this.DD_LogFile.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_LogFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DD_LogFile.Location = new System.Drawing.Point(424, 3);
			this.DD_LogFile.Name = "DD_LogFile";
			this.DD_LogFile.Size = new System.Drawing.Size(415, 150);
			this.DD_LogFile.TabIndex = 1;
			this.DD_LogFile.Text = "LogFileDrop";
			this.DD_LogFile.ValidExtensions = new string[] {
        ".txt",
        ".log",
        ".zip"};
			this.DD_LogFile.FileSelected += new System.Action<string>(this.DD_LogFile_FileSelected);
			this.DD_LogFile.ValidFile += new System.Func<object, string, bool>(this.DD_LogFile_ValidFile);
			// 
			// B_OpenLog
			// 
			this.B_OpenLog.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OpenLog.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon5.Name = "I_CS";
			this.B_OpenLog.ImageName = dynamicIcon5;
			this.B_OpenLog.Location = new System.Drawing.Point(164, 11);
			this.B_OpenLog.Name = "B_OpenLog";
			this.B_OpenLog.Padding = new System.Windows.Forms.Padding(8);
			this.B_OpenLog.Size = new System.Drawing.Size(148, 58);
			this.B_OpenLog.SpaceTriggersClick = true;
			this.B_OpenLog.TabIndex = 20;
			this.B_OpenLog.Text = "OpenLog";
			this.B_OpenLog.Click += new System.EventHandler(this.B_OpenLog_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.TLP_HelpLogs, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.TLP_LogFolders, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.TLP_LogFiles, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(329, 707);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// TLP_HelpLogs
			// 
			this.TLP_HelpLogs.AddOutline = true;
			this.TLP_HelpLogs.AutoSize = true;
			this.TLP_HelpLogs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_HelpLogs.ColumnCount = 1;
			this.TLP_HelpLogs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_HelpLogs.Controls.Add(this.B_Donate, 0, 3);
			this.TLP_HelpLogs.Controls.Add(this.B_ChangeLog, 0, 2);
			this.TLP_HelpLogs.Controls.Add(this.B_SaveZip, 0, 5);
			this.TLP_HelpLogs.Controls.Add(this.B_CopyZip, 0, 6);
			this.TLP_HelpLogs.Controls.Add(this.B_Discord, 0, 0);
			this.TLP_HelpLogs.Controls.Add(this.B_Guide, 0, 1);
			this.TLP_HelpLogs.Controls.Add(this.slickSpacer1, 0, 4);
			this.TLP_HelpLogs.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon12.Name = "I_AskHelp";
			this.TLP_HelpLogs.ImageName = dynamicIcon12;
			this.TLP_HelpLogs.Location = new System.Drawing.Point(3, 3);
			this.TLP_HelpLogs.Name = "TLP_HelpLogs";
			this.TLP_HelpLogs.Padding = new System.Windows.Forms.Padding(8, 46, 8, 8);
			this.TLP_HelpLogs.RowCount = 7;
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_HelpLogs.Size = new System.Drawing.Size(323, 311);
			this.TLP_HelpLogs.TabIndex = 0;
			this.TLP_HelpLogs.Text = "HelpSupport";
			// 
			// B_Donate
			// 
			this.B_Donate.AutoSize = true;
			this.B_Donate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Donate.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon6.Name = "I_Donate";
			this.B_Donate.ImageName = dynamicIcon6;
			this.B_Donate.Location = new System.Drawing.Point(11, 163);
			this.B_Donate.Name = "B_Donate";
			this.B_Donate.Size = new System.Drawing.Size(301, 32);
			this.B_Donate.SpaceTriggersClick = true;
			this.B_Donate.TabIndex = 3;
			this.B_Donate.Text = "Donate";
			this.B_Donate.Click += new System.EventHandler(this.B_Donate_Click);
			// 
			// B_ChangeLog
			// 
			this.B_ChangeLog.AutoSize = true;
			this.B_ChangeLog.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ChangeLog.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon7.Name = "I_Versions";
			this.B_ChangeLog.ImageName = dynamicIcon7;
			this.B_ChangeLog.Location = new System.Drawing.Point(11, 125);
			this.B_ChangeLog.Name = "B_ChangeLog";
			this.B_ChangeLog.Size = new System.Drawing.Size(301, 32);
			this.B_ChangeLog.SpaceTriggersClick = true;
			this.B_ChangeLog.TabIndex = 2;
			this.B_ChangeLog.Text = "OpenChangelog";
			this.B_ChangeLog.Click += new System.EventHandler(this.B_ChangeLog_Click);
			// 
			// B_SaveZip
			// 
			this.B_SaveZip.AutoSize = true;
			this.B_SaveZip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_SaveZip.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon8.Name = "I_Log";
			this.B_SaveZip.ImageName = dynamicIcon8;
			this.B_SaveZip.Location = new System.Drawing.Point(11, 230);
			this.B_SaveZip.Name = "B_SaveZip";
			this.B_SaveZip.Size = new System.Drawing.Size(301, 32);
			this.B_SaveZip.SpaceTriggersClick = true;
			this.B_SaveZip.TabIndex = 4;
			this.B_SaveZip.Text = "LogZipFile";
			this.B_SaveZip.Click += new System.EventHandler(this.B_SaveZip_Click);
			// 
			// B_CopyZip
			// 
			this.B_CopyZip.AutoSize = true;
			this.B_CopyZip.ButtonType = SlickControls.ButtonType.Active;
			this.B_CopyZip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CopyZip.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon9.Name = "I_CopyFile";
			this.B_CopyZip.ImageName = dynamicIcon9;
			this.B_CopyZip.Location = new System.Drawing.Point(11, 268);
			this.B_CopyZip.Name = "B_CopyZip";
			this.B_CopyZip.Size = new System.Drawing.Size(301, 32);
			this.B_CopyZip.SpaceTriggersClick = true;
			this.B_CopyZip.TabIndex = 5;
			this.B_CopyZip.Text = "LogZipCopy";
			this.B_CopyZip.Click += new System.EventHandler(this.B_CopyZip_Click);
			// 
			// B_Discord
			// 
			this.B_Discord.AutoSize = true;
			this.B_Discord.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Discord.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon10.Name = "I_Discord";
			this.B_Discord.ImageName = dynamicIcon10;
			this.B_Discord.Location = new System.Drawing.Point(11, 49);
			this.B_Discord.Name = "B_Discord";
			this.B_Discord.Size = new System.Drawing.Size(301, 32);
			this.B_Discord.SpaceTriggersClick = true;
			this.B_Discord.TabIndex = 0;
			this.B_Discord.Text = "JoinDiscord";
			this.B_Discord.Click += new System.EventHandler(this.B_Discord_Click);
			// 
			// B_Guide
			// 
			this.B_Guide.AutoSize = true;
			this.B_Guide.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Guide.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon11.Name = "I_Guide";
			this.B_Guide.ImageName = dynamicIcon11;
			this.B_Guide.Location = new System.Drawing.Point(11, 87);
			this.B_Guide.Name = "B_Guide";
			this.B_Guide.Size = new System.Drawing.Size(301, 32);
			this.B_Guide.SpaceTriggersClick = true;
			this.B_Guide.TabIndex = 1;
			this.B_Guide.Text = "OpenGuide";
			this.B_Guide.Click += new System.EventHandler(this.B_Guide_Click);
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(11, 201);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(301, 23);
			this.slickSpacer1.TabIndex = 2;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TLP_LogFolders
			// 
			this.TLP_LogFolders.AddOutline = true;
			this.TLP_LogFolders.AutoSize = true;
			this.TLP_LogFolders.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_LogFolders.ColumnCount = 1;
			this.TLP_LogFolders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_LogFolders.Controls.Add(this.B_OpenAppData, 0, 0);
			this.TLP_LogFolders.Controls.Add(this.B_LotLog, 0, 2);
			this.TLP_LogFolders.Controls.Add(this.B_OpenLogFolder, 0, 1);
			this.TLP_LogFolders.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon16.Name = "I_Log";
			this.TLP_LogFolders.ImageName = dynamicIcon16;
			this.TLP_LogFolders.Location = new System.Drawing.Point(3, 320);
			this.TLP_LogFolders.Name = "TLP_LogFolders";
			this.TLP_LogFolders.Padding = new System.Windows.Forms.Padding(8, 46, 8, 8);
			this.TLP_LogFolders.RowCount = 3;
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_LogFolders.Size = new System.Drawing.Size(323, 172);
			this.TLP_LogFolders.TabIndex = 1;
			this.TLP_LogFolders.Text = "LogFolders";
			// 
			// B_OpenAppData
			// 
			this.B_OpenAppData.AutoSize = true;
			this.B_OpenAppData.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OpenAppData.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon13.Name = "I_Folder";
			this.B_OpenAppData.ImageName = dynamicIcon13;
			this.B_OpenAppData.Location = new System.Drawing.Point(11, 49);
			this.B_OpenAppData.Name = "B_OpenAppData";
			this.B_OpenAppData.Size = new System.Drawing.Size(301, 32);
			this.B_OpenAppData.SpaceTriggersClick = true;
			this.B_OpenAppData.TabIndex = 20;
			this.B_OpenAppData.Text = "OpenCitiesAppData";
			this.B_OpenAppData.Click += new System.EventHandler(this.B_OpenAppData_Click);
			// 
			// B_LotLog
			// 
			this.B_LotLog.AutoSize = true;
			this.B_LotLog.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_LotLog.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon14.Name = "I_Folder";
			this.B_LotLog.ImageName = dynamicIcon14;
			this.B_LotLog.Location = new System.Drawing.Point(11, 129);
			this.B_LotLog.Name = "B_LotLog";
			this.B_LotLog.Size = new System.Drawing.Size(301, 32);
			this.B_LotLog.SpaceTriggersClick = true;
			this.B_LotLog.TabIndex = 2;
			this.B_LotLog.Text = "OpenLOTLogFolder";
			this.B_LotLog.Click += new System.EventHandler(this.B_LotLog_Click);
			// 
			// B_OpenLogFolder
			// 
			this.B_OpenLogFolder.AutoSize = true;
			this.B_OpenLogFolder.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OpenLogFolder.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon15.Name = "I_Folder";
			this.B_OpenLogFolder.ImageName = dynamicIcon15;
			this.B_OpenLogFolder.Location = new System.Drawing.Point(11, 87);
			this.B_OpenLogFolder.Name = "B_OpenLogFolder";
			this.B_OpenLogFolder.Padding = new System.Windows.Forms.Padding(8);
			this.B_OpenLogFolder.Size = new System.Drawing.Size(301, 36);
			this.B_OpenLogFolder.SpaceTriggersClick = true;
			this.B_OpenLogFolder.TabIndex = 0;
			this.B_OpenLogFolder.Text = "OpenLogFolder";
			this.B_OpenLogFolder.Click += new System.EventHandler(this.B_OpenLogFolder_Click);
			// 
			// TLP_LogFiles
			// 
			this.TLP_LogFiles.AddOutline = true;
			this.TLP_LogFiles.AutoSize = true;
			this.TLP_LogFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_LogFiles.ColumnCount = 2;
			this.TLP_LogFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_LogFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_LogFiles.Controls.Add(this.B_OpenSkyveLog, 0, 0);
			this.TLP_LogFiles.Controls.Add(this.B_OpenLog, 1, 0);
			this.TLP_LogFiles.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_LogFiles.Location = new System.Drawing.Point(3, 498);
			this.TLP_LogFiles.Name = "TLP_LogFiles";
			this.TLP_LogFiles.Padding = new System.Windows.Forms.Padding(8);
			this.TLP_LogFiles.RowCount = 1;
			this.TLP_LogFiles.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFiles.Size = new System.Drawing.Size(323, 80);
			this.TLP_LogFiles.TabIndex = 19;
			// 
			// B_OpenSkyveLog
			// 
			this.B_OpenSkyveLog.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OpenSkyveLog.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon17.Name = "I_AppIcon";
			this.B_OpenSkyveLog.ImageName = dynamicIcon17;
			this.B_OpenSkyveLog.Location = new System.Drawing.Point(11, 11);
			this.B_OpenSkyveLog.Name = "B_OpenSkyveLog";
			this.B_OpenSkyveLog.Padding = new System.Windows.Forms.Padding(8);
			this.B_OpenSkyveLog.Size = new System.Drawing.Size(147, 58);
			this.B_OpenSkyveLog.SpaceTriggersClick = true;
			this.B_OpenSkyveLog.TabIndex = 20;
			this.B_OpenSkyveLog.Text = "OpenSkyveLog";
			this.B_OpenSkyveLog.Click += new System.EventHandler(this.B_OpenSkyveLog_Click);
			// 
			// PC_HelpAndLogs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Main);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "PC_HelpAndLogs";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1171, 737);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.Controls.SetChildIndex(this.TLP_Main, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Errors.ResumeLayout(false);
			this.TLP_Errors.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.P_Troubleshoot.ResumeLayout(false);
			this.P_Troubleshoot.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.TLP_HelpLogs.ResumeLayout(false);
			this.TLP_HelpLogs.PerformLayout();
			this.TLP_LogFolders.ResumeLayout(false);
			this.TLP_LogFolders.PerformLayout();
			this.TLP_LogFiles.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private SmartTablePanel TLP_Main;
	private SlickControls.RoundedGroupTableLayoutPanel TLP_HelpLogs;
	private SlickControls.SlickButton B_Discord;
	private SlickControls.SlickButton B_Guide;
	private SlickControls.SlickButton B_SaveZip;
	private SlickControls.SlickButton B_CopyZip;
	private SlickControls.SlickSpacer slickSpacer1;
	private SlickControls.RoundedGroupTableLayoutPanel TLP_LogFolders;
	private SlickControls.SlickButton B_LotLog;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickButton B_ChangeLog;
	private SlickControls.SlickButton B_Donate;
	private IconTopButton B_OpenLog;
	private SlickButton B_OpenAppData;
	private SlickControls.SlickButton B_OpenLogFolder;
	private RoundedGroupTableLayoutPanel P_Troubleshoot;
	private SlickButton B_Troubleshoot;
	private AutoSizeLabel L_Troubleshoot;
	private DragAndDropControl DD_LogFile;
	private SlickCheckbox CB_OnlyShowErrors;
	private SlickIcon I_Sort;
	private IconTopButton B_OpenSkyveLog;
	private SlickTextBox TB_Search;
	private RoundedTableLayoutPanel TLP_LogFiles;
	private System.Windows.Forms.Panel panel1;
	private SlickScroll slickScroll1;
	private LogTraceControl logTraceControl;
	private SlickSpacer slickSpacer3;
	private RoundedTableLayoutPanel TLP_Errors;
	private AutoSizeLabel L_Info;
}
