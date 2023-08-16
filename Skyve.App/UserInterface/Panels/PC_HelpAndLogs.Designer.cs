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
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon10 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon6 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon7 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon8 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon9 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon11 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon12 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon13 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon14 = new SlickControls.DynamicIcon();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.P_Troubleshoot = new SlickControls.RoundedGroupTableLayoutPanel();
			this.B_Troubleshoot = new SlickControls.SlickButton();
			this.L_Troubleshoot = new System.Windows.Forms.Label();
			this.B_OpenLog = new SlickControls.SlickButton();
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
			this.slickSpacer4 = new SlickControls.SlickSpacer();
			this.B_LotLogCopy = new SlickControls.SlickButton();
			this.B_LotLog = new SlickControls.SlickButton();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.B_OpenLogFolder = new SlickControls.SlickButton();
			this.B_CopyLogFile = new SlickControls.SlickButton();
			this.DD_LogFile = new Skyve.App.UserInterface.Generic.DragAndDropControl();
			this.TLP_Errors = new SlickControls.RoundedGroupTableLayoutPanel();
			this.I_Info = new SlickControls.SlickIcon();
			this.L_Info = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickSpacer3 = new SlickControls.SlickSpacer();
			this.TLP_Main.SuspendLayout();
			this.P_Troubleshoot.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.TLP_HelpLogs.SuspendLayout();
			this.TLP_LogFolders.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 32);
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.TLP_Main;
			this.slickScroll1.Location = new System.Drawing.Point(1161, 31);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(10, 706);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 17;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			this.slickScroll1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.slickScroll1_Scroll);
			// 
			// TLP_Main
			// 
			this.TLP_Main.AutoSize = true;
			this.TLP_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.ColumnCount = 4;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.Controls.Add(this.P_Troubleshoot, 1, 0);
			this.TLP_Main.Controls.Add(this.B_OpenLog, 3, 2);
			this.TLP_Main.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.TLP_Main.Controls.Add(this.DD_LogFile, 1, 1);
			this.TLP_Main.Controls.Add(this.TLP_Errors, 1, 3);
			this.TLP_Main.Controls.Add(this.I_Info, 1, 2);
			this.TLP_Main.Controls.Add(this.L_Info, 2, 2);
			this.TLP_Main.Location = new System.Drawing.Point(0, 0);
			this.TLP_Main.MaximumSize = new System.Drawing.Size(1100, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 4;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.Size = new System.Drawing.Size(1100, 652);
			this.TLP_Main.TabIndex = 13;
			// 
			// P_Troubleshoot
			// 
			this.P_Troubleshoot.AddOutline = true;
			this.P_Troubleshoot.AutoSize = true;
			this.P_Troubleshoot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Troubleshoot.ColorStyle = Extensions.ColorStyle.Yellow;
			this.P_Troubleshoot.ColumnCount = 2;
			this.TLP_Main.SetColumnSpan(this.P_Troubleshoot, 3);
			this.P_Troubleshoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Troubleshoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Troubleshoot.Controls.Add(this.B_Troubleshoot, 1, 0);
			this.P_Troubleshoot.Controls.Add(this.L_Troubleshoot, 0, 1);
			this.P_Troubleshoot.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon2.Name = "I_Wrench";
			this.P_Troubleshoot.ImageName = dynamicIcon2;
			this.P_Troubleshoot.Location = new System.Drawing.Point(333, 3);
			this.P_Troubleshoot.Name = "P_Troubleshoot";
			this.P_Troubleshoot.Padding = new System.Windows.Forms.Padding(9);
			this.P_Troubleshoot.RowCount = 2;
			this.P_Troubleshoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
			this.P_Troubleshoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Troubleshoot.Size = new System.Drawing.Size(764, 102);
			this.P_Troubleshoot.TabIndex = 24;
			this.P_Troubleshoot.Text = "TroubleshootIssues";
			this.P_Troubleshoot.UseFirstRowForPadding = true;
			// 
			// B_Troubleshoot
			// 
			this.B_Troubleshoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Troubleshoot.AutoSize = true;
			this.B_Troubleshoot.ColorShade = null;
			this.B_Troubleshoot.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "I_ArrowRight";
			this.B_Troubleshoot.ImageName = dynamicIcon1;
			this.B_Troubleshoot.Location = new System.Drawing.Point(489, 57);
			this.B_Troubleshoot.Name = "B_Troubleshoot";
			this.P_Troubleshoot.SetRowSpan(this.B_Troubleshoot, 2);
			this.B_Troubleshoot.Size = new System.Drawing.Size(263, 33);
			this.B_Troubleshoot.SpaceTriggersClick = true;
			this.B_Troubleshoot.TabIndex = 14;
			this.B_Troubleshoot.Text = "ViewTroubleshootOptions";
			this.B_Troubleshoot.Click += new System.EventHandler(this.B_Troubleshoot_Click);
			// 
			// L_Troubleshoot
			// 
			this.L_Troubleshoot.AutoSize = true;
			this.L_Troubleshoot.Location = new System.Drawing.Point(12, 63);
			this.L_Troubleshoot.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.L_Troubleshoot.Name = "L_Troubleshoot";
			this.L_Troubleshoot.Size = new System.Drawing.Size(68, 30);
			this.L_Troubleshoot.TabIndex = 16;
			this.L_Troubleshoot.Text = "label1";
			// 
			// B_OpenLog
			// 
			this.B_OpenLog.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_OpenLog.AutoSize = true;
			this.B_OpenLog.ColorShade = null;
			this.B_OpenLog.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_Link";
			this.B_OpenLog.ImageName = dynamicIcon3;
			this.B_OpenLog.Location = new System.Drawing.Point(993, 272);
			this.B_OpenLog.Name = "B_OpenLog";
			this.B_OpenLog.Size = new System.Drawing.Size(104, 33);
			this.B_OpenLog.SpaceTriggersClick = true;
			this.B_OpenLog.TabIndex = 20;
			this.B_OpenLog.Text = "OpenLog";
			this.B_OpenLog.Click += new System.EventHandler(this.B_OpenLog_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.TLP_HelpLogs, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.TLP_LogFolders, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.TLP_Main.SetRowSpan(this.tableLayoutPanel1, 4);
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(330, 652);
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
			dynamicIcon10.Name = "I_AskHelp";
			this.TLP_HelpLogs.ImageName = dynamicIcon10;
			this.TLP_HelpLogs.Location = new System.Drawing.Point(3, 3);
			this.TLP_HelpLogs.Name = "TLP_HelpLogs";
			this.TLP_HelpLogs.Padding = new System.Windows.Forms.Padding(9, 53, 9, 9);
			this.TLP_HelpLogs.RowCount = 7;
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_HelpLogs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_HelpLogs.Size = new System.Drawing.Size(324, 325);
			this.TLP_HelpLogs.TabIndex = 0;
			this.TLP_HelpLogs.Text = "HelpSupport";
			// 
			// B_Donate
			// 
			this.B_Donate.AutoSize = true;
			this.B_Donate.ColorShade = null;
			this.B_Donate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Donate.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon4.Name = "I_Donate";
			this.B_Donate.ImageName = dynamicIcon4;
			this.B_Donate.Location = new System.Drawing.Point(12, 173);
			this.B_Donate.Name = "B_Donate";
			this.B_Donate.Size = new System.Drawing.Size(300, 33);
			this.B_Donate.SpaceTriggersClick = true;
			this.B_Donate.TabIndex = 3;
			this.B_Donate.Text = "Donate";
			this.B_Donate.Click += new System.EventHandler(this.B_Donate_Click);
			// 
			// B_ChangeLog
			// 
			this.B_ChangeLog.AutoSize = true;
			this.B_ChangeLog.ColorShade = null;
			this.B_ChangeLog.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ChangeLog.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon5.Name = "I_Versions";
			this.B_ChangeLog.ImageName = dynamicIcon5;
			this.B_ChangeLog.Location = new System.Drawing.Point(12, 134);
			this.B_ChangeLog.Name = "B_ChangeLog";
			this.B_ChangeLog.Size = new System.Drawing.Size(300, 33);
			this.B_ChangeLog.SpaceTriggersClick = true;
			this.B_ChangeLog.TabIndex = 2;
			this.B_ChangeLog.Text = "OpenChangelog";
			this.B_ChangeLog.Click += new System.EventHandler(this.B_ChangeLog_Click);
			// 
			// B_SaveZip
			// 
			this.B_SaveZip.AutoSize = true;
			this.B_SaveZip.ColorShade = null;
			this.B_SaveZip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_SaveZip.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon6.Name = "I_Log";
			this.B_SaveZip.ImageName = dynamicIcon6;
			this.B_SaveZip.Location = new System.Drawing.Point(12, 241);
			this.B_SaveZip.Name = "B_SaveZip";
			this.B_SaveZip.Size = new System.Drawing.Size(300, 33);
			this.B_SaveZip.SpaceTriggersClick = true;
			this.B_SaveZip.TabIndex = 4;
			this.B_SaveZip.Text = "LogZipFile";
			this.B_SaveZip.Click += new System.EventHandler(this.B_SaveZip_Click);
			// 
			// B_CopyZip
			// 
			this.B_CopyZip.AutoSize = true;
			this.B_CopyZip.ButtonType = SlickControls.ButtonType.Active;
			this.B_CopyZip.ColorShade = null;
			this.B_CopyZip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CopyZip.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon7.Name = "I_CopyFile";
			this.B_CopyZip.ImageName = dynamicIcon7;
			this.B_CopyZip.Location = new System.Drawing.Point(12, 280);
			this.B_CopyZip.Name = "B_CopyZip";
			this.B_CopyZip.Size = new System.Drawing.Size(300, 33);
			this.B_CopyZip.SpaceTriggersClick = true;
			this.B_CopyZip.TabIndex = 5;
			this.B_CopyZip.Text = "LogZipCopy";
			this.B_CopyZip.Click += new System.EventHandler(this.B_CopyZip_Click);
			// 
			// B_Discord
			// 
			this.B_Discord.AutoSize = true;
			this.B_Discord.ColorShade = null;
			this.B_Discord.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Discord.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon8.Name = "I_Discord";
			this.B_Discord.ImageName = dynamicIcon8;
			this.B_Discord.Location = new System.Drawing.Point(12, 56);
			this.B_Discord.Name = "B_Discord";
			this.B_Discord.Size = new System.Drawing.Size(300, 33);
			this.B_Discord.SpaceTriggersClick = true;
			this.B_Discord.TabIndex = 0;
			this.B_Discord.Text = "JoinDiscord";
			this.B_Discord.Click += new System.EventHandler(this.B_Discord_Click);
			// 
			// B_Guide
			// 
			this.B_Guide.AutoSize = true;
			this.B_Guide.ColorShade = null;
			this.B_Guide.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Guide.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon9.Name = "I_Guide";
			this.B_Guide.ImageName = dynamicIcon9;
			this.B_Guide.Location = new System.Drawing.Point(12, 95);
			this.B_Guide.Name = "B_Guide";
			this.B_Guide.Size = new System.Drawing.Size(300, 33);
			this.B_Guide.SpaceTriggersClick = true;
			this.B_Guide.TabIndex = 1;
			this.B_Guide.Text = "OpenGuide";
			this.B_Guide.Click += new System.EventHandler(this.B_Guide_Click);
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(12, 212);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(300, 23);
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
			this.TLP_LogFolders.Controls.Add(this.B_OpenAppData, 0, 6);
			this.TLP_LogFolders.Controls.Add(this.slickSpacer4, 0, 5);
			this.TLP_LogFolders.Controls.Add(this.B_LotLogCopy, 0, 4);
			this.TLP_LogFolders.Controls.Add(this.B_LotLog, 0, 3);
			this.TLP_LogFolders.Controls.Add(this.slickSpacer2, 0, 2);
			this.TLP_LogFolders.Controls.Add(this.B_OpenLogFolder, 0, 0);
			this.TLP_LogFolders.Controls.Add(this.B_CopyLogFile, 0, 1);
			this.TLP_LogFolders.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_LogFolders.ImageName = dynamicIcon12;
			this.TLP_LogFolders.Location = new System.Drawing.Point(3, 334);
			this.TLP_LogFolders.Name = "TLP_LogFolders";
			this.TLP_LogFolders.Padding = new System.Windows.Forms.Padding(9, 53, 9, 9);
			this.TLP_LogFolders.RowCount = 7;
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_LogFolders.Size = new System.Drawing.Size(324, 315);
			this.TLP_LogFolders.TabIndex = 1;
			this.TLP_LogFolders.Text = "LogFolders";
			// 
			// B_OpenAppData
			// 
			this.B_OpenAppData.AutoSize = true;
			this.B_OpenAppData.ColorShade = null;
			this.B_OpenAppData.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OpenAppData.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon11.Name = "I_Folder";
			this.B_OpenAppData.ImageName = dynamicIcon11;
			this.B_OpenAppData.Location = new System.Drawing.Point(12, 270);
			this.B_OpenAppData.Name = "B_OpenAppData";
			this.B_OpenAppData.Size = new System.Drawing.Size(300, 33);
			this.B_OpenAppData.SpaceTriggersClick = true;
			this.B_OpenAppData.TabIndex = 20;
			this.B_OpenAppData.Text = "OpenCitiesAppData";
			this.B_OpenAppData.Click += new System.EventHandler(this.B_OpenAppData_Click);
			// 
			// slickSpacer4
			// 
			this.slickSpacer4.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer4.Location = new System.Drawing.Point(12, 241);
			this.slickSpacer4.Name = "slickSpacer4";
			this.slickSpacer4.Size = new System.Drawing.Size(300, 23);
			this.slickSpacer4.TabIndex = 19;
			this.slickSpacer4.TabStop = false;
			this.slickSpacer4.Text = "slickSpacer4";
			// 
			// B_LotLogCopy
			// 
			this.B_LotLogCopy.AutoSize = true;
			this.B_LotLogCopy.ColorShade = null;
			this.B_LotLogCopy.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_LotLogCopy.Dock = System.Windows.Forms.DockStyle.Top;
			this.B_LotLogCopy.ImageName = dynamicIcon7;
			this.B_LotLogCopy.Location = new System.Drawing.Point(12, 202);
			this.B_LotLogCopy.Name = "B_LotLogCopy";
			this.B_LotLogCopy.Size = new System.Drawing.Size(300, 33);
			this.B_LotLogCopy.SpaceTriggersClick = true;
			this.B_LotLogCopy.TabIndex = 3;
			this.B_LotLogCopy.Text = "CopyLOTLogFile";
			this.B_LotLogCopy.Click += new System.EventHandler(this.B_LotLogCopy_Click);
			// 
			// B_LotLog
			// 
			this.B_LotLog.AutoSize = true;
			this.B_LotLog.ColorShade = null;
			this.B_LotLog.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_LotLog.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon12.Name = "I_Folder";
			this.B_LotLog.ImageName = dynamicIcon12;
			this.B_LotLog.Location = new System.Drawing.Point(12, 163);
			this.B_LotLog.Name = "B_LotLog";
			this.B_LotLog.Size = new System.Drawing.Size(300, 33);
			this.B_LotLog.SpaceTriggersClick = true;
			this.B_LotLog.TabIndex = 2;
			this.B_LotLog.Text = "OpenLOTLogFolder";
			this.B_LotLog.Click += new System.EventHandler(this.B_LotLog_Click);
			// 
			// slickSpacer2
			// 
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(12, 134);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(300, 23);
			this.slickSpacer2.TabIndex = 18;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// B_OpenLogFolder
			// 
			this.B_OpenLogFolder.AutoSize = true;
			this.B_OpenLogFolder.ColorShade = null;
			this.B_OpenLogFolder.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OpenLogFolder.Dock = System.Windows.Forms.DockStyle.Top;
			this.B_OpenLogFolder.ImageName = dynamicIcon12;
			this.B_OpenLogFolder.Location = new System.Drawing.Point(12, 56);
			this.B_OpenLogFolder.Name = "B_OpenLogFolder";
			this.B_OpenLogFolder.Size = new System.Drawing.Size(300, 33);
			this.B_OpenLogFolder.SpaceTriggersClick = true;
			this.B_OpenLogFolder.TabIndex = 0;
			this.B_OpenLogFolder.Text = "OpenLogFolder";
			this.B_OpenLogFolder.Click += new System.EventHandler(this.B_OpenLogFolder_Click);
			// 
			// B_CopyLogFile
			// 
			this.B_CopyLogFile.AutoSize = true;
			this.B_CopyLogFile.ColorShade = null;
			this.B_CopyLogFile.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CopyLogFile.Dock = System.Windows.Forms.DockStyle.Top;
			this.B_CopyLogFile.ImageName = dynamicIcon7;
			this.B_CopyLogFile.Location = new System.Drawing.Point(12, 95);
			this.B_CopyLogFile.Name = "B_CopyLogFile";
			this.B_CopyLogFile.Size = new System.Drawing.Size(300, 33);
			this.B_CopyLogFile.SpaceTriggersClick = true;
			this.B_CopyLogFile.TabIndex = 1;
			this.B_CopyLogFile.Text = "CopyLogFile";
			this.B_CopyLogFile.Click += new System.EventHandler(this.B_CopyLogFile_Click);
			// 
			// DD_LogFile
			// 
			this.DD_LogFile.AllowDrop = true;
			this.TLP_Main.SetColumnSpan(this.DD_LogFile, 3);
			this.DD_LogFile.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_LogFile.Dock = System.Windows.Forms.DockStyle.Top;
			this.DD_LogFile.Location = new System.Drawing.Point(333, 111);
			this.DD_LogFile.Name = "DD_LogFile";
			this.DD_LogFile.Size = new System.Drawing.Size(764, 150);
			this.DD_LogFile.TabIndex = 1;
			this.DD_LogFile.Text = "LogFileDrop";
			this.DD_LogFile.ValidExtensions = new string[] {
        ".txt",
        ".log",
        ".zip"};
			this.DD_LogFile.FileSelected += new System.Action<string>(this.DD_LogFile_FileSelected);
			this.DD_LogFile.ValidFile += new System.Func<object, string, bool>(this.DD_LogFile_ValidFile);
			// 
			// TLP_Errors
			// 
			this.TLP_Errors.AddOutline = true;
			this.TLP_Errors.AutoSize = true;
			this.TLP_Errors.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Errors.ColumnCount = 1;
			this.TLP_Main.SetColumnSpan(this.TLP_Errors, 3);
			this.TLP_Errors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Errors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Errors.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon13.Name = "I_Errors";
			this.TLP_Errors.ImageName = dynamicIcon13;
			this.TLP_Errors.Location = new System.Drawing.Point(333, 317);
			this.TLP_Errors.Name = "TLP_Errors";
			this.TLP_Errors.Padding = new System.Windows.Forms.Padding(9, 53, 9, 9);
			this.TLP_Errors.RowCount = 1;
			this.TLP_Errors.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Errors.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Errors.Size = new System.Drawing.Size(764, 62);
			this.TLP_Errors.TabIndex = 2;
			this.TLP_Errors.Text = "ErrorsInLog";
			this.TLP_Errors.Visible = false;
			// 
			// I_Info
			// 
			this.I_Info.ActiveColor = null;
			this.I_Info.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.I_Info.ColorStyle = Extensions.ColorStyle.Icon;
			this.I_Info.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_Info.Enabled = false;
			dynamicIcon14.Name = "I_Info";
			this.I_Info.ImageName = dynamicIcon14;
			this.I_Info.Location = new System.Drawing.Point(340, 273);
			this.I_Info.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			this.I_Info.Name = "I_Info";
			this.I_Info.Selected = true;
			this.I_Info.Size = new System.Drawing.Size(32, 32);
			this.I_Info.TabIndex = 18;
			this.I_Info.TabStop = false;
			// 
			// L_Info
			// 
			this.L_Info.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Info.AutoSize = true;
			this.L_Info.Location = new System.Drawing.Point(378, 274);
			this.L_Info.Margin = new System.Windows.Forms.Padding(3, 10, 10, 10);
			this.L_Info.Name = "L_Info";
			this.L_Info.Size = new System.Drawing.Size(68, 30);
			this.L_Info.TabIndex = 19;
			this.L_Info.Text = "label1";
			this.L_Info.UseMnemonic = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.TLP_Main);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 31);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1171, 706);
			this.panel1.TabIndex = 16;
			// 
			// slickSpacer3
			// 
			this.slickSpacer3.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer3.Location = new System.Drawing.Point(0, 30);
			this.slickSpacer3.Name = "slickSpacer3";
			this.slickSpacer3.Size = new System.Drawing.Size(1171, 1);
			this.slickSpacer3.TabIndex = 18;
			this.slickSpacer3.TabStop = false;
			this.slickSpacer3.Text = "slickSpacer3";
			// 
			// PC_HelpAndLogs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.slickScroll1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.slickSpacer3);
			this.Name = "PC_HelpAndLogs";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1171, 737);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.slickSpacer3, 0);
			this.Controls.SetChildIndex(this.panel1, 0);
			this.Controls.SetChildIndex(this.slickScroll1, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.P_Troubleshoot.ResumeLayout(false);
			this.P_Troubleshoot.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.TLP_HelpLogs.ResumeLayout(false);
			this.TLP_HelpLogs.PerformLayout();
			this.TLP_LogFolders.ResumeLayout(false);
			this.TLP_LogFolders.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private DragAndDropControl DD_LogFile;
	private SlickControls.SlickScroll slickScroll1;
	private System.Windows.Forms.TableLayoutPanel TLP_Main;
	private SlickControls.RoundedGroupTableLayoutPanel TLP_Errors;
	private SlickControls.RoundedGroupTableLayoutPanel TLP_HelpLogs;
	private SlickControls.SlickButton B_Discord;
	private SlickControls.SlickButton B_Guide;
	private System.Windows.Forms.Panel panel1;
	private SlickControls.SlickButton B_SaveZip;
	private SlickControls.SlickButton B_CopyZip;
	private SlickControls.SlickSpacer slickSpacer1;
	private SlickControls.SlickIcon I_Info;
	private System.Windows.Forms.Label L_Info;
	private SlickControls.RoundedGroupTableLayoutPanel TLP_LogFolders;
	private SlickControls.SlickButton B_LotLogCopy;
	private SlickControls.SlickButton B_LotLog;
	private SlickControls.SlickSpacer slickSpacer2;
	private SlickControls.SlickButton B_OpenLogFolder;
	private SlickControls.SlickButton B_CopyLogFile;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickButton B_ChangeLog;
	private SlickControls.SlickButton B_Donate;
	private SlickControls.SlickSpacer slickSpacer3;
	private SlickControls.SlickButton B_OpenLog;
	private SlickControls.SlickButton B_OpenAppData;
	private SlickControls.SlickSpacer slickSpacer4;
	private RoundedGroupTableLayoutPanel P_Troubleshoot;
	private SlickButton B_Troubleshoot;
	private System.Windows.Forms.Label L_Troubleshoot;
}
