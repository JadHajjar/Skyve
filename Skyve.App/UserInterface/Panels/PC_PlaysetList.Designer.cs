using Skyve.App.UserInterface.Dropdowns;

namespace Skyve.App.UserInterface.Panels;

partial class PC_PlaysetList
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
			_notifier.PlaysetChanged -= LoadProfile;
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
			SlickControls.DynamicIcon dynamicIcon9 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon10 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon11 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon12 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon13 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon14 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon15 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon16 = new SlickControls.DynamicIcon();
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.DD_Sorting = new Skyve.App.UserInterface.Dropdowns.PlaysetSortingDropDown();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.B_ListView = new SlickControls.SlickIcon();
			this.B_GridView = new SlickControls.SlickIcon();
			this.L_Counts = new System.Windows.Forms.Label();
			this.L_FilterCount = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.FLP_Profiles = new System.Windows.Forms.FlowLayoutPanel();
			this.DD_Usage = new Skyve.App.UserInterface.Dropdowns.PackageUsageDropDown();
			this.roundedPanel = new SlickControls.RoundedPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.B_AddProfile = new SlickControls.RoundedTableLayoutPanel();
			this.slickIcon2 = new SlickControls.SlickIcon();
			this.B_TempProfile = new SlickControls.RoundedTableLayoutPanel();
			this.slickIcon1 = new SlickControls.SlickIcon();
			this.B_Discover = new SlickControls.SlickButton();
			this.TLP_ProfileName = new SlickControls.RoundedTableLayoutPanel();
			this.I_ProfileIcon = new SlickControls.SlickIcon();
			this.B_Save = new SlickControls.SlickIcon();
			this.I_Favorite = new SlickControls.SlickIcon();
			this.L_CurrentProfile = new System.Windows.Forms.Label();
			this.B_EditName = new SlickControls.SlickIcon();
			this.TLP_Main.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.roundedPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.B_AddProfile.SuspendLayout();
			this.B_TempProfile.SuspendLayout();
			this.TLP_ProfileName.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 31);
			this.base_Text.Text = "Mods";
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 3;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.Controls.Add(this.slickSpacer2, 0, 1);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 0, 3);
			this.TLP_Main.Controls.Add(this.TB_Search, 0, 0);
			this.TLP_Main.Controls.Add(this.DD_Sorting, 2, 0);
			this.TLP_Main.Controls.Add(this.tableLayoutPanel3, 0, 2);
			this.TLP_Main.Controls.Add(this.panel1, 0, 4);
			this.TLP_Main.Controls.Add(this.DD_Usage, 1, 0);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(0, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 5;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Size = new System.Drawing.Size(1190, 631);
			this.TLP_Main.TabIndex = 0;
			// 
			// slickSpacer2
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer2, 3);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(0, 27);
			this.slickSpacer2.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(1190, 2);
			this.slickSpacer2.TabIndex = 8;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 3);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 61);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(1190, 2);
			this.slickSpacer1.TabIndex = 7;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TB_Search
			// 
			dynamicIcon9.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon9;
			this.TB_Search.LabelText = "Search";
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.TB_Search.Placeholder = "SearchPlaysets";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(140, 21);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.FilterChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// DD_Sorting
			// 
			this.DD_Sorting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Sorting.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Sorting.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_Sorting.Location = new System.Drawing.Point(973, 0);
			this.DD_Sorting.Margin = new System.Windows.Forms.Padding(0);
			this.DD_Sorting.Name = "DD_Sorting";
			this.DD_Sorting.Padding = new System.Windows.Forms.Padding(7);
			this.DD_Sorting.Size = new System.Drawing.Size(217, 27);
			this.DD_Sorting.TabIndex = 4;
			this.DD_Sorting.SelectedItemChanged += new System.EventHandler(this.DD_Sorting_SelectedItemChanged);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 4;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel3, 3);
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.B_ListView, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.B_GridView, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.L_Counts, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.L_FilterCount, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 29);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(1190, 32);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// B_ListView
			// 
			this.B_ListView.ActiveColor = null;
			this.B_ListView.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_ListView.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon10.Name = "I_List";
			this.B_ListView.ImageName = dynamicIcon10;
			this.B_ListView.Location = new System.Drawing.Point(1120, 1);
			this.B_ListView.Margin = new System.Windows.Forms.Padding(1);
			this.B_ListView.Name = "B_ListView";
			this.B_ListView.Size = new System.Drawing.Size(34, 30);
			this.B_ListView.SpaceTriggersClick = true;
			this.B_ListView.TabIndex = 7;
			this.B_ListView.Click += new System.EventHandler(this.B_ListView_Click);
			// 
			// B_GridView
			// 
			this.B_GridView.ActiveColor = null;
			this.B_GridView.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_GridView.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon11.Name = "I_Grid";
			this.B_GridView.ImageName = dynamicIcon11;
			this.B_GridView.Location = new System.Drawing.Point(1156, 1);
			this.B_GridView.Margin = new System.Windows.Forms.Padding(1);
			this.B_GridView.Name = "B_GridView";
			this.B_GridView.Selected = true;
			this.B_GridView.Size = new System.Drawing.Size(33, 30);
			this.B_GridView.SpaceTriggersClick = true;
			this.B_GridView.TabIndex = 6;
			this.B_GridView.Click += new System.EventHandler(this.B_GridView_Click);
			// 
			// L_Counts
			// 
			this.L_Counts.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.L_Counts.AutoSize = true;
			this.L_Counts.Location = new System.Drawing.Point(1078, 9);
			this.L_Counts.Name = "L_Counts";
			this.L_Counts.Size = new System.Drawing.Size(38, 13);
			this.L_Counts.TabIndex = 1;
			this.L_Counts.Text = "label1";
			// 
			// L_FilterCount
			// 
			this.L_FilterCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_FilterCount.AutoSize = true;
			this.L_FilterCount.Location = new System.Drawing.Point(3, 9);
			this.L_FilterCount.Name = "L_FilterCount";
			this.L_FilterCount.Size = new System.Drawing.Size(38, 13);
			this.L_FilterCount.TabIndex = 2;
			this.L_FilterCount.Text = "label1";
			// 
			// panel1
			// 
			this.TLP_Main.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Controls.Add(this.FLP_Profiles);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 63);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1190, 568);
			this.panel1.TabIndex = 14;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.FLP_Profiles;
			this.slickScroll1.Location = new System.Drawing.Point(1182, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(8, 568);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 0;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// FLP_Profiles
			// 
			this.FLP_Profiles.AutoSize = true;
			this.FLP_Profiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FLP_Profiles.Location = new System.Drawing.Point(100, 28);
			this.FLP_Profiles.Name = "FLP_Profiles";
			this.FLP_Profiles.Size = new System.Drawing.Size(0, 0);
			this.FLP_Profiles.TabIndex = 1;
			// 
			// DD_Usage
			// 
			this.DD_Usage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Usage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Usage.HideLabel = true;
			this.DD_Usage.Location = new System.Drawing.Point(777, 0);
			this.DD_Usage.Margin = new System.Windows.Forms.Padding(0);
			this.DD_Usage.Name = "DD_Usage";
			this.DD_Usage.Size = new System.Drawing.Size(196, 27);
			this.DD_Usage.TabIndex = 15;
			this.DD_Usage.Text = "Usage";
			this.DD_Usage.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// roundedPanel
			// 
			this.roundedPanel.AddOutline = true;
			this.tableLayoutPanel1.SetColumnSpan(this.roundedPanel, 4);
			this.roundedPanel.Controls.Add(this.TLP_Main);
			this.roundedPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.roundedPanel.Location = new System.Drawing.Point(3, 55);
			this.roundedPanel.Name = "roundedPanel";
			this.roundedPanel.Size = new System.Drawing.Size(1190, 631);
			this.roundedPanel.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.B_AddProfile, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.B_TempProfile, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.B_Discover, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.TLP_ProfileName, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.roundedPanel, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1196, 689);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// B_AddProfile
			// 
			this.B_AddProfile.AutoSize = true;
			this.B_AddProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_AddProfile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.B_AddProfile.Controls.Add(this.slickIcon2, 0, 0);
			this.B_AddProfile.Location = new System.Drawing.Point(266, 10);
			this.B_AddProfile.Margin = new System.Windows.Forms.Padding(10);
			this.B_AddProfile.Name = "B_AddProfile";
			this.B_AddProfile.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.B_AddProfile.Size = new System.Drawing.Size(44, 31);
			this.B_AddProfile.TabIndex = 7;
			// 
			// slickIcon2
			// 
			this.slickIcon2.ActiveColor = null;
			this.slickIcon2.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon12.Name = "I_Add";
			this.slickIcon2.ImageName = dynamicIcon12;
			this.slickIcon2.Location = new System.Drawing.Point(0, 0);
			this.slickIcon2.Margin = new System.Windows.Forms.Padding(0);
			this.slickIcon2.Name = "slickIcon2";
			this.slickIcon2.Padding = new System.Windows.Forms.Padding(5);
			this.slickIcon2.Size = new System.Drawing.Size(44, 31);
			this.slickIcon2.TabIndex = 4;
			this.slickIcon2.TabStop = false;
			this.slickIcon2.Click += new System.EventHandler(this.B_AddProfile_Click);
			// 
			// B_TempProfile
			// 
			this.B_TempProfile.AutoSize = true;
			this.B_TempProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_TempProfile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.B_TempProfile.Controls.Add(this.slickIcon1, 0, 0);
			this.B_TempProfile.Location = new System.Drawing.Point(202, 10);
			this.B_TempProfile.Margin = new System.Windows.Forms.Padding(10);
			this.B_TempProfile.Name = "B_TempProfile";
			this.B_TempProfile.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.B_TempProfile.Size = new System.Drawing.Size(44, 31);
			this.B_TempProfile.TabIndex = 6;
			// 
			// slickIcon1
			// 
			this.slickIcon1.ActiveColor = null;
			this.slickIcon1.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon13.Name = "I_TempProfile";
			this.slickIcon1.ImageName = dynamicIcon13;
			this.slickIcon1.Location = new System.Drawing.Point(0, 0);
			this.slickIcon1.Margin = new System.Windows.Forms.Padding(0);
			this.slickIcon1.Name = "slickIcon1";
			this.slickIcon1.Padding = new System.Windows.Forms.Padding(5);
			this.slickIcon1.Size = new System.Drawing.Size(44, 31);
			this.slickIcon1.TabIndex = 4;
			this.slickIcon1.TabStop = false;
			this.slickIcon1.Click += new System.EventHandler(this.B_TempProfile_Click);
			// 
			// B_Discover
			// 
			this.B_Discover.AutoSize = true;
			this.B_Discover.ButtonType = SlickControls.ButtonType.Active;
			this.B_Discover.ColorShade = null;
			this.B_Discover.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Discover.Dock = System.Windows.Forms.DockStyle.Right;
			dynamicIcon14.Name = "I_Discover";
			this.B_Discover.ImageName = dynamicIcon14;
			this.B_Discover.LargeImage = true;
			this.B_Discover.Location = new System.Drawing.Point(1068, 3);
			this.B_Discover.Name = "B_Discover";
			this.B_Discover.Size = new System.Drawing.Size(125, 46);
			this.B_Discover.SpaceTriggersClick = true;
			this.B_Discover.TabIndex = 3;
			this.B_Discover.Text = "DiscoverPlaysets";
			this.B_Discover.Click += new System.EventHandler(this.B_Discover_Click);
			// 
			// TLP_ProfileName
			// 
			this.TLP_ProfileName.AutoSize = true;
			this.TLP_ProfileName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_ProfileName.ColumnCount = 6;
			this.TLP_ProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ProfileName.Controls.Add(this.I_ProfileIcon, 1, 0);
			this.TLP_ProfileName.Controls.Add(this.B_Save, 5, 0);
			this.TLP_ProfileName.Controls.Add(this.I_Favorite, 0, 0);
			this.TLP_ProfileName.Controls.Add(this.L_CurrentProfile, 4, 0);
			this.TLP_ProfileName.Controls.Add(this.B_EditName, 2, 0);
			this.TLP_ProfileName.Location = new System.Drawing.Point(10, 10);
			this.TLP_ProfileName.Margin = new System.Windows.Forms.Padding(10);
			this.TLP_ProfileName.Name = "TLP_ProfileName";
			this.TLP_ProfileName.RowCount = 1;
			this.TLP_ProfileName.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_ProfileName.Size = new System.Drawing.Size(172, 32);
			this.TLP_ProfileName.TabIndex = 5;
			// 
			// I_ProfileIcon
			// 
			this.I_ProfileIcon.ActiveColor = null;
			this.I_ProfileIcon.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.I_ProfileIcon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_ProfileIcon.Location = new System.Drawing.Point(32, 0);
			this.I_ProfileIcon.Margin = new System.Windows.Forms.Padding(0);
			this.I_ProfileIcon.Name = "I_ProfileIcon";
			this.I_ProfileIcon.Padding = new System.Windows.Forms.Padding(5);
			this.I_ProfileIcon.Size = new System.Drawing.Size(32, 32);
			this.I_ProfileIcon.TabIndex = 0;
			this.I_ProfileIcon.TabStop = false;
			this.I_ProfileIcon.Click += new System.EventHandler(this.I_ProfileIcon_Click);
			// 
			// B_Save
			// 
			this.B_Save.ActiveColor = null;
			this.B_Save.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_Save.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon15.Name = "I_Save";
			this.B_Save.ImageName = dynamicIcon15;
			this.B_Save.Location = new System.Drawing.Point(140, 0);
			this.B_Save.Margin = new System.Windows.Forms.Padding(0);
			this.B_Save.Name = "B_Save";
			this.B_Save.Padding = new System.Windows.Forms.Padding(5);
			this.B_Save.Size = new System.Drawing.Size(32, 32);
			this.B_Save.TabIndex = 4;
			this.B_Save.TabStop = false;
			this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
			// 
			// I_Favorite
			// 
			this.I_Favorite.ActiveColor = null;
			this.I_Favorite.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.I_Favorite.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_Favorite.Location = new System.Drawing.Point(0, 0);
			this.I_Favorite.Margin = new System.Windows.Forms.Padding(0);
			this.I_Favorite.Name = "I_Favorite";
			this.I_Favorite.Padding = new System.Windows.Forms.Padding(5);
			this.I_Favorite.Size = new System.Drawing.Size(32, 32);
			this.I_Favorite.TabIndex = 0;
			this.I_Favorite.TabStop = false;
			this.I_Favorite.Click += new System.EventHandler(this.I_Favorite_Click);
			// 
			// L_CurrentProfile
			// 
			this.L_CurrentProfile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_CurrentProfile.AutoSize = true;
			this.L_CurrentProfile.Location = new System.Drawing.Point(99, 9);
			this.L_CurrentProfile.Name = "L_CurrentProfile";
			this.L_CurrentProfile.Size = new System.Drawing.Size(38, 13);
			this.L_CurrentProfile.TabIndex = 1;
			this.L_CurrentProfile.Text = "label1";
			// 
			// B_EditName
			// 
			this.B_EditName.ActiveColor = null;
			this.B_EditName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_EditName.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon16.Name = "I_Cog";
			this.B_EditName.ImageName = dynamicIcon16;
			this.B_EditName.Location = new System.Drawing.Point(64, 0);
			this.B_EditName.Margin = new System.Windows.Forms.Padding(0);
			this.B_EditName.Name = "B_EditName";
			this.B_EditName.Padding = new System.Windows.Forms.Padding(5);
			this.B_EditName.Size = new System.Drawing.Size(32, 32);
			this.B_EditName.TabIndex = 3;
			this.B_EditName.TabStop = false;
			this.B_EditName.Click += new System.EventHandler(this.B_EditName_Click);
			// 
			// PC_PlaysetList
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_PlaysetList";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1196, 719);
			this.Text = "Mods";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.roundedPanel.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.B_AddProfile.ResumeLayout(false);
			this.B_TempProfile.ResumeLayout(false);
			this.TLP_ProfileName.ResumeLayout(false);
			this.TLP_ProfileName.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel TLP_Main;
	private SlickControls.SlickTextBox TB_Search;
	private PlaysetSortingDropDown DD_Sorting;
	private SlickControls.SlickSpacer slickSpacer1;
	private SlickControls.SlickSpacer slickSpacer2;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
	private System.Windows.Forms.Label L_Counts;
	private System.Windows.Forms.Panel panel1;
	private System.Windows.Forms.FlowLayoutPanel FLP_Profiles;
	private SlickControls.SlickScroll slickScroll1;
	private System.Windows.Forms.Label L_FilterCount;
	private PackageUsageDropDown DD_Usage;
	protected SlickControls.SlickIcon B_ListView;
	protected SlickControls.SlickIcon B_GridView;
	private SlickControls.RoundedPanel roundedPanel;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	public SlickControls.SlickButton B_Discover;
	private SlickControls.RoundedTableLayoutPanel TLP_ProfileName;
	private SlickControls.SlickIcon I_ProfileIcon;
	private System.Windows.Forms.Label L_CurrentProfile;
	private SlickControls.SlickIcon B_EditName;
	private SlickControls.SlickIcon B_Save;
	private SlickControls.SlickIcon I_Favorite;
	private SlickControls.RoundedTableLayoutPanel B_AddProfile;
	private SlickControls.SlickIcon slickIcon2;
	private SlickControls.RoundedTableLayoutPanel B_TempProfile;
	private SlickControls.SlickIcon slickIcon1;
}
