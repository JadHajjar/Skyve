using Skyve.App.UserInterface.Generic;

namespace Skyve.App.UserInterface.Panels;

partial class PC_Utilities
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
			_notifier.PackageInformationUpdated -= RefreshModIssues;
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
			SlickControls.DynamicIcon dynamicIcon19 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon18 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon9 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon6 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon7 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon8 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon11 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon10 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon13 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon12 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon14 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon15 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon17 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon16 = new SlickControls.DynamicIcon();
			this.TB_CollectionLink = new SlickControls.SlickTextBox();
			this.P_Collections = new SlickControls.RoundedGroupPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.B_LoadCollection = new SlickControls.SlickButton();
			this.TLP_Main = new SmartTablePanel();
			this.P_Troubleshoot = new SlickControls.RoundedGroupTableLayoutPanel();
			this.B_Troubleshoot = new SlickControls.SlickButton();
			this.L_Troubleshoot = new System.Windows.Forms.Label();
			this.P_Reset = new SlickControls.RoundedGroupTableLayoutPanel();
			this.B_ResetImageCache = new SlickControls.SlickButton();
			this.B_ResetSnoozes = new SlickControls.SlickButton();
			this.B_ReloadAllData = new SlickControls.SlickButton();
			this.B_ResetSteamCache = new SlickControls.SlickButton();
			this.B_ResetCompatibilityCache = new SlickControls.SlickButton();
			this.B_ResetModsCache = new SlickControls.SlickButton();
			this.P_Cleanup = new SlickControls.RoundedGroupTableLayoutPanel();
			this.L_CleanupInfo = new System.Windows.Forms.Label();
			this.B_Cleanup = new SlickControls.SlickButton();
			this.P_Text = new SlickControls.RoundedGroupPanel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.DD_TextImport = new Skyve.App.UserInterface.Generic.DragAndDropControl();
			this.B_ImportClipboard = new SlickControls.SlickButton();
			this.P_BOB = new SlickControls.RoundedGroupPanel();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.DD_BOB = new Skyve.App.UserInterface.Generic.DragAndDropControl();
			this.P_LsmReport = new SlickControls.RoundedGroupPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.DD_Unused = new Skyve.App.UserInterface.Generic.DragAndDropControl();
			this.DD_Missing = new Skyve.App.UserInterface.Generic.DragAndDropControl();
			this.P_ModIssues = new SlickControls.RoundedGroupTableLayoutPanel();
			this.P_OutOfDate = new System.Windows.Forms.Panel();
			this.B_ReDownload = new SlickControls.SlickButton();
			this.L_OutOfDate = new System.Windows.Forms.Label();
			this.L_Incomplete = new System.Windows.Forms.Label();
			this.P_Incomplete = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.P_Container = new System.Windows.Forms.Panel();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.P_Collections.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.TLP_Main.SuspendLayout();
			this.P_Troubleshoot.SuspendLayout();
			this.P_Reset.SuspendLayout();
			this.P_Cleanup.SuspendLayout();
			this.P_Text.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.P_BOB.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.P_LsmReport.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.P_ModIssues.SuspendLayout();
			this.P_Container.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 25);
			// 
			// TB_CollectionLink
			// 
			this.TB_CollectionLink.Dock = System.Windows.Forms.DockStyle.Top;
			this.TB_CollectionLink.LabelText = "CollectionLink";
			this.TB_CollectionLink.Location = new System.Drawing.Point(3, 3);
			this.TB_CollectionLink.Name = "TB_CollectionLink";
			this.TB_CollectionLink.Placeholder = "PasteCollection";
			this.TB_CollectionLink.Required = true;
			this.TB_CollectionLink.SelectedText = "";
			this.TB_CollectionLink.SelectionLength = 0;
			this.TB_CollectionLink.SelectionStart = 0;
			this.TB_CollectionLink.Size = new System.Drawing.Size(564, 49);
			this.TB_CollectionLink.TabIndex = 13;
			this.TB_CollectionLink.Validation = SlickControls.ValidationType.Regex;
			this.TB_CollectionLink.ValidationRegex = "^(?:https:\\/\\/steamcommunity\\.com\\/(?:(?:sharedfiles)|(?:workshop))\\/filedetails\\" +
    "/\\?id=)?(\\d{8,20})";
			this.TB_CollectionLink.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TB_CollectionLink_PreviewKeyDown);
			// 
			// P_Collecttions
			// 
			this.P_Collections.AddOutline = true;
			this.P_Collections.AutoSize = true;
			this.P_Collections.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.SetColumnSpan(this.P_Collections, 2);
			this.P_Collections.Controls.Add(this.tableLayoutPanel1);
			this.P_Collections.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon19.Name = "I_Steam";
			this.P_Collections.ImageName = dynamicIcon19;
			this.P_Collections.Location = new System.Drawing.Point(3, 227);
			this.P_Collections.Name = "P_Collecttions";
			this.P_Collections.Padding = new System.Windows.Forms.Padding(9, 54, 9, 9);
			this.P_Collections.Size = new System.Drawing.Size(694, 118);
			this.P_Collections.TabIndex = 15;
			this.P_Collections.Text = "CollectionTitle";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.B_LoadCollection, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.TB_CollectionLink, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 54);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(676, 55);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// B_LoadCollection
			// 
			this.B_LoadCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.B_LoadCollection.AutoSize = true;
			this.B_LoadCollection.ColorShade = null;
			this.B_LoadCollection.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon18.Name = "I_Import";
			this.B_LoadCollection.ImageName = dynamicIcon18;
			this.B_LoadCollection.Location = new System.Drawing.Point(573, 3);
			this.B_LoadCollection.Name = "B_LoadCollection";
			this.B_LoadCollection.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_LoadCollection.Size = new System.Drawing.Size(100, 49);
			this.B_LoadCollection.SpaceTriggersClick = true;
			this.B_LoadCollection.TabIndex = 15;
			this.B_LoadCollection.Text = "LoadCollection";
			this.B_LoadCollection.Click += new System.EventHandler(this.B_LoadCollection_Click);
			// 
			// TLP_Main
			// 
			this.TLP_Main.AutoSize = true;
			this.TLP_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.ColumnCount = 2;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Main.Controls.Add(this.P_Troubleshoot, 0, 5);
			this.TLP_Main.Controls.Add(this.P_Reset, 0, 6);
			this.TLP_Main.Controls.Add(this.P_Cleanup, 0, 1);
			this.TLP_Main.Controls.Add(this.P_Text, 0, 4);
			this.TLP_Main.Controls.Add(this.P_BOB, 1, 4);
			this.TLP_Main.Controls.Add(this.P_LsmReport, 0, 3);
			this.TLP_Main.Controls.Add(this.P_ModIssues, 0, 0);
			this.TLP_Main.Controls.Add(this.P_Collections, 0, 2);
			this.TLP_Main.Location = new System.Drawing.Point(0, 0);
			this.TLP_Main.MinimumSize = new System.Drawing.Size(700, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 7;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.Size = new System.Drawing.Size(700, 1015);
			this.TLP_Main.TabIndex = 17;
			// 
			// P_Troubleshoot
			// 
			this.P_Troubleshoot.AddOutline = true;
			this.P_Troubleshoot.AutoSize = true;
			this.P_Troubleshoot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Troubleshoot.ColorStyle = Extensions.ColorStyle.Yellow;
			this.P_Troubleshoot.ColumnCount = 2;
			this.TLP_Main.SetColumnSpan(this.P_Troubleshoot, 2);
			this.P_Troubleshoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Troubleshoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Troubleshoot.Controls.Add(this.B_Troubleshoot, 1, 0);
			this.P_Troubleshoot.Controls.Add(this.L_Troubleshoot, 0, 1);
			this.P_Troubleshoot.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon2.Name = "I_Wrench";
			this.P_Troubleshoot.ImageName = dynamicIcon2;
			this.P_Troubleshoot.Location = new System.Drawing.Point(3, 800);
			this.P_Troubleshoot.Name = "P_Troubleshoot";
			this.P_Troubleshoot.Padding = new System.Windows.Forms.Padding(9);
			this.P_Troubleshoot.RowCount = 2;
			this.P_Troubleshoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.P_Troubleshoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Troubleshoot.Size = new System.Drawing.Size(694, 103);
			this.P_Troubleshoot.TabIndex = 23;
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
			this.B_Troubleshoot.Location = new System.Drawing.Point(532, 46);
			this.B_Troubleshoot.Name = "B_Troubleshoot";
			this.P_Troubleshoot.SetRowSpan(this.B_Troubleshoot, 2);
			this.B_Troubleshoot.Size = new System.Drawing.Size(150, 45);
			this.B_Troubleshoot.SpaceTriggersClick = true;
			this.B_Troubleshoot.TabIndex = 14;
			this.B_Troubleshoot.Text = "ViewTroubleshootOptions";
			this.B_Troubleshoot.Click += new System.EventHandler(this.B_Troubleshoot_Click);
			// 
			// L_Troubleshoot
			// 
			this.L_Troubleshoot.AutoSize = true;
			this.L_Troubleshoot.Location = new System.Drawing.Point(12, 64);
			this.L_Troubleshoot.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.L_Troubleshoot.Name = "L_Troubleshoot";
			this.L_Troubleshoot.Size = new System.Drawing.Size(71, 30);
			this.L_Troubleshoot.TabIndex = 16;
			this.L_Troubleshoot.Text = "label1";
			// 
			// P_Reset
			// 
			this.P_Reset.AddOutline = true;
			this.P_Reset.AutoSize = true;
			this.P_Reset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Reset.ColorStyle = Extensions.ColorStyle.Red;
			this.P_Reset.ColumnCount = 3;
			this.TLP_Main.SetColumnSpan(this.P_Reset, 2);
			this.P_Reset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.P_Reset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.P_Reset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.P_Reset.Controls.Add(this.B_ResetImageCache, 2, 0);
			this.P_Reset.Controls.Add(this.B_ResetSnoozes, 0, 1);
			this.P_Reset.Controls.Add(this.B_ReloadAllData, 0, 0);
			this.P_Reset.Controls.Add(this.B_ResetSteamCache, 2, 1);
			this.P_Reset.Controls.Add(this.B_ResetCompatibilityCache, 1, 1);
			this.P_Reset.Controls.Add(this.B_ResetModsCache, 1, 0);
			this.P_Reset.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon9.Name = "I_Undo";
			this.P_Reset.ImageName = dynamicIcon9;
			this.P_Reset.Info = "ResetInfo";
			this.P_Reset.Location = new System.Drawing.Point(3, 909);
			this.P_Reset.Name = "P_Reset";
			this.P_Reset.Padding = new System.Windows.Forms.Padding(9, 54, 9, 9);
			this.P_Reset.RowCount = 2;
			this.P_Reset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Reset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Reset.Size = new System.Drawing.Size(694, 103);
			this.P_Reset.TabIndex = 22;
			this.P_Reset.Text = "Reset";
			// 
			// B_ResetImageCache
			// 
			this.B_ResetImageCache.AutoSize = true;
			this.B_ResetImageCache.ColorShade = null;
			this.B_ResetImageCache.ColorStyle = Extensions.ColorStyle.Red;
			this.B_ResetImageCache.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_RemoveImage";
			this.B_ResetImageCache.ImageName = dynamicIcon3;
			this.B_ResetImageCache.Location = new System.Drawing.Point(462, 57);
			this.B_ResetImageCache.Name = "B_ResetImageCache";
			this.B_ResetImageCache.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_ResetImageCache.Size = new System.Drawing.Size(103, 14);
			this.B_ResetImageCache.SpaceTriggersClick = true;
			this.B_ResetImageCache.TabIndex = 15;
			this.B_ResetImageCache.Text = "ResetImageCache";
			this.B_ResetImageCache.Click += new System.EventHandler(this.B_ResetImageCache_Click);
			// 
			// B_ResetSnoozes
			// 
			this.B_ResetSnoozes.AutoSize = true;
			this.B_ResetSnoozes.ColorShade = null;
			this.B_ResetSnoozes.ColorStyle = Extensions.ColorStyle.Yellow;
			this.B_ResetSnoozes.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "I_Snooze";
			this.B_ResetSnoozes.ImageName = dynamicIcon4;
			this.B_ResetSnoozes.Location = new System.Drawing.Point(12, 77);
			this.B_ResetSnoozes.Name = "B_ResetSnoozes";
			this.B_ResetSnoozes.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_ResetSnoozes.Size = new System.Drawing.Size(103, 14);
			this.B_ResetSnoozes.SpaceTriggersClick = true;
			this.B_ResetSnoozes.TabIndex = 15;
			this.B_ResetSnoozes.Text = "ResetSnoozes";
			this.B_ResetSnoozes.Click += new System.EventHandler(this.B_ResetSnoozes_Click);
			// 
			// B_ReloadAllData
			// 
			this.B_ReloadAllData.AutoSize = true;
			this.B_ReloadAllData.ColorShade = null;
			this.B_ReloadAllData.ColorStyle = Extensions.ColorStyle.Yellow;
			this.B_ReloadAllData.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon5.Name = "I_Refresh";
			this.B_ReloadAllData.ImageName = dynamicIcon5;
			this.B_ReloadAllData.Location = new System.Drawing.Point(12, 57);
			this.B_ReloadAllData.Name = "B_ReloadAllData";
			this.B_ReloadAllData.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_ReloadAllData.Size = new System.Drawing.Size(103, 14);
			this.B_ReloadAllData.SpaceTriggersClick = true;
			this.B_ReloadAllData.TabIndex = 15;
			this.B_ReloadAllData.Text = "ReloadAllData";
			this.B_ReloadAllData.Click += new System.EventHandler(this.B_ReloadAllData_Click);
			// 
			// B_ResetSteamCache
			// 
			this.B_ResetSteamCache.AutoSize = true;
			this.B_ResetSteamCache.ColorShade = null;
			this.B_ResetSteamCache.ColorStyle = Extensions.ColorStyle.Red;
			this.B_ResetSteamCache.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon6.Name = "I_RemoveSteam";
			this.B_ResetSteamCache.ImageName = dynamicIcon6;
			this.B_ResetSteamCache.Location = new System.Drawing.Point(462, 77);
			this.B_ResetSteamCache.Name = "B_ResetSteamCache";
			this.B_ResetSteamCache.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_ResetSteamCache.Size = new System.Drawing.Size(103, 14);
			this.B_ResetSteamCache.SpaceTriggersClick = true;
			this.B_ResetSteamCache.TabIndex = 15;
			this.B_ResetSteamCache.Text = "ResetSteamCache";
			this.B_ResetSteamCache.Click += new System.EventHandler(this.B_ResetSteamCache_Click);
			// 
			// B_ResetCompatibilityCache
			// 
			this.B_ResetCompatibilityCache.AutoSize = true;
			this.B_ResetCompatibilityCache.ColorShade = null;
			this.B_ResetCompatibilityCache.ColorStyle = Extensions.ColorStyle.Orange;
			this.B_ResetCompatibilityCache.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon7.Name = "I_CompatibilityReport";
			this.B_ResetCompatibilityCache.ImageName = dynamicIcon7;
			this.B_ResetCompatibilityCache.Location = new System.Drawing.Point(237, 77);
			this.B_ResetCompatibilityCache.Name = "B_ResetCompatibilityCache";
			this.B_ResetCompatibilityCache.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_ResetCompatibilityCache.Size = new System.Drawing.Size(103, 14);
			this.B_ResetCompatibilityCache.SpaceTriggersClick = true;
			this.B_ResetCompatibilityCache.TabIndex = 15;
			this.B_ResetCompatibilityCache.Text = "ResetCompatibilityCache";
			this.B_ResetCompatibilityCache.Click += new System.EventHandler(this.B_ResetCompatibilityCache_Click);
			// 
			// B_ResetModsCache
			// 
			this.B_ResetModsCache.AutoSize = true;
			this.B_ResetModsCache.ColorShade = null;
			this.B_ResetModsCache.ColorStyle = Extensions.ColorStyle.Orange;
			this.B_ResetModsCache.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon8.Name = "I_Mods";
			this.B_ResetModsCache.ImageName = dynamicIcon8;
			this.B_ResetModsCache.Location = new System.Drawing.Point(237, 57);
			this.B_ResetModsCache.Name = "B_ResetModsCache";
			this.B_ResetModsCache.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_ResetModsCache.Size = new System.Drawing.Size(103, 14);
			this.B_ResetModsCache.SpaceTriggersClick = true;
			this.B_ResetModsCache.TabIndex = 15;
			this.B_ResetModsCache.Text = "ResetModsCache";
			this.B_ResetModsCache.Click += new System.EventHandler(this.B_ResetModsCache_Click);
			// 
			// P_Cleanup
			// 
			this.P_Cleanup.AddOutline = true;
			this.P_Cleanup.AutoSize = true;
			this.P_Cleanup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Cleanup.ColumnCount = 2;
			this.TLP_Main.SetColumnSpan(this.P_Cleanup, 2);
			this.P_Cleanup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Cleanup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Cleanup.Controls.Add(this.L_CleanupInfo, 0, 1);
			this.P_Cleanup.Controls.Add(this.B_Cleanup, 1, 0);
			this.P_Cleanup.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon11.Name = "I_Broom";
			this.P_Cleanup.ImageName = dynamicIcon11;
			this.P_Cleanup.Location = new System.Drawing.Point(3, 118);
			this.P_Cleanup.Name = "P_Cleanup";
			this.P_Cleanup.Padding = new System.Windows.Forms.Padding(9);
			this.P_Cleanup.RowCount = 2;
			this.P_Cleanup.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.P_Cleanup.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Cleanup.Size = new System.Drawing.Size(694, 103);
			this.P_Cleanup.TabIndex = 21;
			this.P_Cleanup.Text = "CleanupTitle";
			this.P_Cleanup.UseFirstRowForPadding = true;
			// 
			// L_CleanupInfo
			// 
			this.L_CleanupInfo.AutoSize = true;
			this.L_CleanupInfo.Location = new System.Drawing.Point(12, 64);
			this.L_CleanupInfo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.L_CleanupInfo.Name = "L_CleanupInfo";
			this.L_CleanupInfo.Size = new System.Drawing.Size(71, 30);
			this.L_CleanupInfo.TabIndex = 17;
			this.L_CleanupInfo.Text = "label1";
			// 
			// B_Cleanup
			// 
			this.B_Cleanup.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cleanup.AutoSize = true;
			this.B_Cleanup.ColorShade = null;
			this.B_Cleanup.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon10.Name = "I_AppIcon";
			this.B_Cleanup.ImageName = dynamicIcon10;
			this.B_Cleanup.Location = new System.Drawing.Point(582, 39);
			this.B_Cleanup.Name = "B_Cleanup";
			this.B_Cleanup.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.P_Cleanup.SetRowSpan(this.B_Cleanup, 2);
			this.B_Cleanup.Size = new System.Drawing.Size(100, 25);
			this.B_Cleanup.SpaceTriggersClick = true;
			this.B_Cleanup.TabIndex = 16;
			this.B_Cleanup.Text = "RunCleanup";
			this.B_Cleanup.Click += new System.EventHandler(this.B_Cleanup_Click);
			// 
			// P_Text
			// 
			this.P_Text.AddOutline = true;
			this.P_Text.AutoSize = true;
			this.P_Text.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Text.Controls.Add(this.tableLayoutPanel3);
			this.P_Text.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon13.Name = "I_Text";
			this.P_Text.ImageName = dynamicIcon13;
			this.P_Text.Info = "ImportFromTextInfo";
			this.P_Text.Location = new System.Drawing.Point(3, 576);
			this.P_Text.Name = "P_Text";
			this.P_Text.Padding = new System.Windows.Forms.Padding(9, 54, 9, 9);
			this.P_Text.Size = new System.Drawing.Size(344, 218);
			this.P_Text.TabIndex = 20;
			this.P_Text.Text = "ImportFromText";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add(this.DD_TextImport, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.B_ImportClipboard, 0, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(9, 54);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(326, 155);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// DD_TextImport
			// 
			this.DD_TextImport.AllowDrop = true;
			this.DD_TextImport.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_TextImport.Dock = System.Windows.Forms.DockStyle.Top;
			this.DD_TextImport.Location = new System.Drawing.Point(3, 3);
			this.DD_TextImport.Name = "DD_TextImport";
			this.DD_TextImport.Size = new System.Drawing.Size(320, 104);
			this.DD_TextImport.TabIndex = 17;
			this.DD_TextImport.Text = "TextImportMissingInfo";
			this.DD_TextImport.ValidExtensions = new string[] {
        ".txt"};
			this.DD_TextImport.FileSelected += new System.Action<string>(this.DD_TextImport_FileSelected);
			this.DD_TextImport.ValidFile += new System.Func<object, string, bool>(this.DD_TextImport_ValidFile);
			// 
			// B_ImportClipboard
			// 
			this.B_ImportClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.B_ImportClipboard.AutoSize = true;
			this.B_ImportClipboard.ColorShade = null;
			this.B_ImportClipboard.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon12.Name = "I_Copy";
			this.B_ImportClipboard.ImageName = dynamicIcon12;
			this.B_ImportClipboard.Location = new System.Drawing.Point(216, 113);
			this.B_ImportClipboard.Name = "B_ImportClipboard";
			this.B_ImportClipboard.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_ImportClipboard.Size = new System.Drawing.Size(107, 39);
			this.B_ImportClipboard.SpaceTriggersClick = true;
			this.B_ImportClipboard.TabIndex = 15;
			this.B_ImportClipboard.Text = "ImportFromClipboard";
			this.B_ImportClipboard.Click += new System.EventHandler(this.B_ImportClipboard_Click);
			// 
			// P_BOB
			// 
			this.P_BOB.AddOutline = true;
			this.P_BOB.AutoSize = true;
			this.P_BOB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_BOB.Controls.Add(this.tableLayoutPanel6);
			this.P_BOB.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon14.Name = "I_XML";
			this.P_BOB.ImageName = dynamicIcon14;
			this.P_BOB.Info = "XMLImportInfo";
			this.P_BOB.Location = new System.Drawing.Point(353, 576);
			this.P_BOB.Name = "P_BOB";
			this.P_BOB.Padding = new System.Windows.Forms.Padding(9, 87, 9, 9);
			this.P_BOB.Size = new System.Drawing.Size(344, 209);
			this.P_BOB.TabIndex = 19;
			this.P_BOB.Text = "XMLImport";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 1;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.Controls.Add(this.DD_BOB, 0, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(9, 87);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 1;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(326, 113);
			this.tableLayoutPanel6.TabIndex = 0;
			// 
			// DD_BOB
			// 
			this.DD_BOB.AllowDrop = true;
			this.DD_BOB.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_BOB.Dock = System.Windows.Forms.DockStyle.Top;
			this.DD_BOB.Location = new System.Drawing.Point(3, 3);
			this.DD_BOB.Name = "DD_BOB";
			this.DD_BOB.Size = new System.Drawing.Size(320, 107);
			this.DD_BOB.TabIndex = 16;
			this.DD_BOB.Text = "XMLImportMissingInfo";
			this.DD_BOB.ValidExtensions = new string[] {
        ".xml"};
			this.DD_BOB.FileSelected += new System.Action<string>(this.DD_BOB_FileSelected);
			this.DD_BOB.ValidFile += new System.Func<object, string, bool>(this.DD_BOB_ValidFile);
			// 
			// P_LsmReport
			// 
			this.P_LsmReport.AddOutline = true;
			this.P_LsmReport.AutoSize = true;
			this.P_LsmReport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.SetColumnSpan(this.P_LsmReport, 2);
			this.P_LsmReport.Controls.Add(this.tableLayoutPanel4);
			this.P_LsmReport.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon15.Name = "I_LSM";
			this.P_LsmReport.ImageName = dynamicIcon15;
			this.P_LsmReport.Info = "LsmImportInfo";
			this.P_LsmReport.Location = new System.Drawing.Point(3, 351);
			this.P_LsmReport.Name = "P_LsmReport";
			this.P_LsmReport.Padding = new System.Windows.Forms.Padding(9, 54, 9, 9);
			this.P_LsmReport.Size = new System.Drawing.Size(694, 219);
			this.P_LsmReport.TabIndex = 18;
			this.P_LsmReport.Text = "LsmImport";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Controls.Add(this.DD_Unused, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.DD_Missing, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(9, 54);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(676, 156);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// DD_Unused
			// 
			this.DD_Unused.AllowDrop = true;
			this.DD_Unused.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Unused.Dock = System.Windows.Forms.DockStyle.Top;
			this.DD_Unused.Location = new System.Drawing.Point(341, 3);
			this.DD_Unused.Name = "DD_Unused";
			this.DD_Unused.Size = new System.Drawing.Size(332, 150);
			this.DD_Unused.TabIndex = 17;
			this.DD_Unused.Text = "LsmImportUnusedInfo";
			this.DD_Unused.FileSelected += new System.Action<string>(this.LSM_UnusedDrop_FileSelected);
			this.DD_Unused.ValidFile += new System.Func<object, string, bool>(this.LSMDragDrop_ValidFile);
			// 
			// DD_Missing
			// 
			this.DD_Missing.AllowDrop = true;
			this.DD_Missing.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Missing.Dock = System.Windows.Forms.DockStyle.Top;
			this.DD_Missing.Location = new System.Drawing.Point(3, 3);
			this.DD_Missing.Name = "DD_Missing";
			this.DD_Missing.Size = new System.Drawing.Size(332, 150);
			this.DD_Missing.TabIndex = 16;
			this.DD_Missing.Text = "LsmImportMissingInfo";
			this.DD_Missing.FileSelected += new System.Action<string>(this.LSMDragDrop_FileSelected);
			this.DD_Missing.ValidFile += new System.Func<object, string, bool>(this.LSMDragDrop_ValidFile);
			// 
			// P_ModIssues
			// 
			this.P_ModIssues.AddOutline = true;
			this.P_ModIssues.AutoSize = true;
			this.P_ModIssues.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_ModIssues.ColorStyle = Extensions.ColorStyle.Yellow;
			this.P_ModIssues.ColumnCount = 3;
			this.TLP_Main.SetColumnSpan(this.P_ModIssues, 2);
			this.P_ModIssues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.P_ModIssues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.P_ModIssues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_ModIssues.Controls.Add(this.P_OutOfDate, 0, 1);
			this.P_ModIssues.Controls.Add(this.B_ReDownload, 2, 0);
			this.P_ModIssues.Controls.Add(this.L_OutOfDate, 0, 0);
			this.P_ModIssues.Controls.Add(this.L_Incomplete, 1, 0);
			this.P_ModIssues.Controls.Add(this.P_Incomplete, 1, 1);
			this.P_ModIssues.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon17.Name = "I_ModWarning";
			this.P_ModIssues.ImageName = dynamicIcon17;
			this.P_ModIssues.Location = new System.Drawing.Point(3, 3);
			this.P_ModIssues.Name = "P_ModIssues";
			this.P_ModIssues.Padding = new System.Windows.Forms.Padding(9, 54, 9, 9);
			this.P_ModIssues.RowCount = 2;
			this.P_ModIssues.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_ModIssues.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_ModIssues.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.P_ModIssues.Size = new System.Drawing.Size(694, 109);
			this.P_ModIssues.TabIndex = 16;
			this.P_ModIssues.Text = "DetectedIssues";
			// 
			// P_OutOfDate
			// 
			this.P_OutOfDate.AutoSize = true;
			this.P_OutOfDate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_OutOfDate.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_OutOfDate.Location = new System.Drawing.Point(12, 97);
			this.P_OutOfDate.Name = "P_OutOfDate";
			this.P_OutOfDate.Size = new System.Drawing.Size(273, 0);
			this.P_OutOfDate.TabIndex = 19;
			// 
			// B_ReDownload
			// 
			this.B_ReDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_ReDownload.AutoSize = true;
			this.B_ReDownload.ColorShade = null;
			this.B_ReDownload.ColorStyle = Extensions.ColorStyle.Green;
			this.B_ReDownload.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon16.Name = "I_Tools";
			this.B_ReDownload.ImageName = dynamicIcon16;
			this.B_ReDownload.Location = new System.Drawing.Point(571, 57);
			this.B_ReDownload.Name = "B_ReDownload";
			this.P_ModIssues.SetRowSpan(this.B_ReDownload, 2);
			this.B_ReDownload.Size = new System.Drawing.Size(111, 40);
			this.B_ReDownload.SpaceTriggersClick = true;
			this.B_ReDownload.TabIndex = 14;
			this.B_ReDownload.Text = "FixAllIssues";
			this.B_ReDownload.Click += new System.EventHandler(this.B_ReDownload_Click);
			// 
			// L_OutOfDate
			// 
			this.L_OutOfDate.AutoSize = true;
			this.L_OutOfDate.Location = new System.Drawing.Point(12, 64);
			this.L_OutOfDate.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.L_OutOfDate.Name = "L_OutOfDate";
			this.L_OutOfDate.Size = new System.Drawing.Size(71, 30);
			this.L_OutOfDate.TabIndex = 16;
			this.L_OutOfDate.Text = "label1";
			// 
			// L_Incomplete
			// 
			this.L_Incomplete.AutoSize = true;
			this.L_Incomplete.Location = new System.Drawing.Point(291, 64);
			this.L_Incomplete.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.L_Incomplete.Name = "L_Incomplete";
			this.L_Incomplete.Size = new System.Drawing.Size(71, 30);
			this.L_Incomplete.TabIndex = 17;
			this.L_Incomplete.Text = "label1";
			// 
			// P_Incomplete
			// 
			this.P_Incomplete.AutoSize = true;
			this.P_Incomplete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Incomplete.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Incomplete.Location = new System.Drawing.Point(291, 97);
			this.P_Incomplete.Name = "P_Incomplete";
			this.P_Incomplete.Size = new System.Drawing.Size(273, 0);
			this.P_Incomplete.TabIndex = 18;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.TLP_Main;
			this.slickScroll1.Location = new System.Drawing.Point(781, 31);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(10, 1046);
			this.slickScroll1.SmallHandle = true;
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 18;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			this.slickScroll1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.slickScroll1_Scroll);
			// 
			// P_Container
			// 
			this.P_Container.Controls.Add(this.TLP_Main);
			this.P_Container.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Container.Location = new System.Drawing.Point(0, 31);
			this.P_Container.Name = "P_Container";
			this.P_Container.Size = new System.Drawing.Size(781, 1046);
			this.P_Container.TabIndex = 19;
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 30);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(791, 1);
			this.slickSpacer1.TabIndex = 20;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// PC_Utilities
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.P_Container);
			this.Controls.Add(this.slickScroll1);
			this.Controls.Add(this.slickSpacer1);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_Utilities";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(791, 1077);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.slickSpacer1, 0);
			this.Controls.SetChildIndex(this.slickScroll1, 0);
			this.Controls.SetChildIndex(this.P_Container, 0);
			this.P_Collections.ResumeLayout(false);
			this.P_Collections.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.P_Troubleshoot.ResumeLayout(false);
			this.P_Troubleshoot.PerformLayout();
			this.P_Reset.ResumeLayout(false);
			this.P_Reset.PerformLayout();
			this.P_Cleanup.ResumeLayout(false);
			this.P_Cleanup.PerformLayout();
			this.P_Text.ResumeLayout(false);
			this.P_Text.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.P_BOB.ResumeLayout(false);
			this.P_BOB.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.P_LsmReport.ResumeLayout(false);
			this.P_LsmReport.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.P_ModIssues.ResumeLayout(false);
			this.P_ModIssues.PerformLayout();
			this.P_Container.ResumeLayout(false);
			this.P_Container.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private SlickControls.SlickTextBox TB_CollectionLink;
	private SlickControls.RoundedGroupPanel P_Collections;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickButton B_LoadCollection;
	private SmartTablePanel TLP_Main;
	private SlickControls.RoundedGroupTableLayoutPanel P_ModIssues;
	private SlickControls.SlickScroll slickScroll1;
	private System.Windows.Forms.Panel P_Container;
	private SlickControls.SlickButton B_ReDownload;
	private SlickControls.RoundedGroupPanel P_LsmReport;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
	private DragAndDropControl DD_Missing;
	private DragAndDropControl DD_Unused;
	private SlickControls.RoundedGroupPanel P_BOB;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
	private DragAndDropControl DD_BOB;
	private SlickControls.RoundedGroupPanel P_Text;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
	private DragAndDropControl DD_TextImport;
	private SlickControls.SlickButton B_ImportClipboard;
	private SlickControls.RoundedGroupTableLayoutPanel P_Cleanup;
	private SlickControls.SlickButton B_Cleanup;
	private System.Windows.Forms.Label L_CleanupInfo;
	private SlickControls.SlickSpacer slickSpacer1;
	private System.Windows.Forms.Label L_Incomplete;
	private System.Windows.Forms.Label L_OutOfDate;
	private System.Windows.Forms.Panel P_OutOfDate;
	private System.Windows.Forms.Panel P_Incomplete;
	private SlickControls.RoundedGroupTableLayoutPanel P_Reset;
	private SlickControls.SlickButton B_ResetImageCache;
	private SlickControls.SlickButton B_ResetSnoozes;
	private SlickControls.SlickButton B_ReloadAllData;
	private SlickControls.SlickButton B_ResetSteamCache;
	private SlickControls.SlickButton B_ResetCompatibilityCache;
	private SlickControls.SlickButton B_ResetModsCache;
	private RoundedGroupTableLayoutPanel P_Troubleshoot;
	private System.Windows.Forms.Label L_Troubleshoot;
	private SlickButton B_Troubleshoot;
}
