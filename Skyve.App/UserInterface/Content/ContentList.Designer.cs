﻿using SkyveApp.UserInterface.Dropdowns;
using SkyveApp.UserInterface.Generic;

namespace SkyveApp.UserInterface.Panels;

partial class ContentList<T>
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
			_notifier.CompatibilityReportProcessed -= CentralManager_WorkshopInfoUpdated;
			_notifier.ContentLoaded -= CentralManager_ContentLoaded;
			_notifier.ContentLoaded -= CentralManager_WorkshopInfoUpdated;
			_notifier.WorkshopInfoUpdated -= CentralManager_WorkshopInfoUpdated;
			_notifier.PackageInformationUpdated -= CentralManager_WorkshopInfoUpdated;
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
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon6 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon7 = new SlickControls.DynamicIcon();
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.FLP_Search = new System.Windows.Forms.FlowLayoutPanel();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.I_Refresh = new SlickControls.SlickIcon();
			this.B_Filters = new SlickControls.SlickLabel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.DD_Sorting = new SkyveApp.UserInterface.Dropdowns.SortingDropDown();
			this.TLP_MiddleBar = new System.Windows.Forms.TableLayoutPanel();
			this.B_CompactList = new SlickControls.SlickIcon();
			this.L_Counts = new System.Windows.Forms.Label();
			this.L_FilterCount = new System.Windows.Forms.Label();
			this.B_ListView = new SlickControls.SlickIcon();
			this.B_GridView = new SlickControls.SlickIcon();
			this.P_FiltersContainer = new System.Windows.Forms.Panel();
			this.P_Filters = new SlickControls.RoundedGroupTableLayoutPanel();
			this.OT_ModAsset = new SkyveApp.UserInterface.Generic.ThreeOptionToggle();
			this.OT_Workshop = new SkyveApp.UserInterface.Generic.ThreeOptionToggle();
			this.OT_Enabled = new SkyveApp.UserInterface.Generic.ThreeOptionToggle();
			this.OT_Included = new SkyveApp.UserInterface.Generic.ThreeOptionToggle();
			this.I_ClearFilters = new SlickControls.SlickIcon();
			this.DR_SubscribeTime = new SlickControls.SlickDateRange();
			this.DR_ServerTime = new SlickControls.SlickDateRange();
			this.DD_PackageStatus = new SkyveApp.UserInterface.Dropdowns.PackageStatusDropDown();
			this.DD_Tags = new SkyveApp.UserInterface.Dropdowns.TagsDropDown();
			this.DD_ReportSeverity = new SkyveApp.UserInterface.Dropdowns.ReportSeverityDropDown();
			this.DD_Author = new SkyveApp.UserInterface.Dropdowns.AuthorDropDown();
			this.DD_Profile = new SkyveApp.UserInterface.Dropdowns.ProfilesDropDown();
			this.I_SortOrder = new SlickControls.SlickIcon();
			this.TLP_Main.SuspendLayout();
			this.FLP_Search.SuspendLayout();
			this.TLP_MiddleBar.SuspendLayout();
			this.P_FiltersContainer.SuspendLayout();
			this.P_Filters.SuspendLayout();
			this.SuspendLayout();
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 4;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Controls.Add(this.FLP_Search, 1, 1);
			this.TLP_Main.Controls.Add(this.slickSpacer2, 0, 4);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 0, 6);
			this.TLP_Main.Controls.Add(this.DD_Sorting, 3, 1);
			this.TLP_Main.Controls.Add(this.TLP_MiddleBar, 0, 5);
			this.TLP_Main.Controls.Add(this.P_FiltersContainer, 1, 3);
			this.TLP_Main.Controls.Add(this.I_SortOrder, 2, 1);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(0, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 8;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Size = new System.Drawing.Size(895, 516);
			this.TLP_Main.TabIndex = 0;
			// 
			// FLP_Search
			// 
			this.FLP_Search.AutoSize = true;
			this.FLP_Search.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FLP_Search.Controls.Add(this.TB_Search);
			this.FLP_Search.Controls.Add(this.I_Refresh);
			this.FLP_Search.Controls.Add(this.B_Filters);
			this.FLP_Search.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Search.Location = new System.Drawing.Point(0, 0);
			this.FLP_Search.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Search.Name = "FLP_Search";
			this.TLP_Main.SetRowSpan(this.FLP_Search, 2);
			this.FLP_Search.Size = new System.Drawing.Size(819, 20);
			this.FLP_Search.TabIndex = 0;
			// 
			// TB_Search
			// 
			dynamicIcon1.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon1;
			this.TB_Search.LabelText = "Search";
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Placeholder = "SearchPackages";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(14, 14);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// I_Refresh
			// 
			this.I_Refresh.ActiveColor = null;
			this.I_Refresh.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Refresh";
			this.I_Refresh.ImageName = dynamicIcon2;
			this.I_Refresh.Location = new System.Drawing.Point(23, 3);
			this.I_Refresh.Name = "I_Refresh";
			this.I_Refresh.Size = new System.Drawing.Size(14, 14);
			this.I_Refresh.SpaceTriggersClick = true;
			this.I_Refresh.TabIndex = 1;
			this.I_Refresh.SizeChanged += new System.EventHandler(this.Icon_SizeChanged);
			this.I_Refresh.Click += new System.EventHandler(this.FilterChanged);
			// 
			// B_Filters
			// 
			this.B_Filters.AutoHideText = false;
			this.B_Filters.AutoSize = true;
			this.B_Filters.AutoSizeIcon = true;
			this.B_Filters.ColorShade = null;
			this.B_Filters.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_Filter";
			this.B_Filters.ImageName = dynamicIcon3;
			this.B_Filters.Location = new System.Drawing.Point(43, 3);
			this.B_Filters.Name = "B_Filters";
			this.B_Filters.Selected = false;
			this.B_Filters.Size = new System.Drawing.Size(50, 14);
			this.B_Filters.SpaceTriggersClick = true;
			this.B_Filters.TabIndex = 1;
			this.B_Filters.Text = "ShowFilters";
			this.B_Filters.MouseClick += new System.Windows.Forms.MouseEventHandler(this.B_Filters_Click);
			// 
			// slickSpacer2
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer2, 4);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(0, 158);
			this.slickSpacer2.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(895, 2);
			this.slickSpacer2.TabIndex = 8;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 4);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 192);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(895, 2);
			this.slickSpacer1.TabIndex = 7;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// DD_Sorting
			// 
			this.DD_Sorting.AccentBackColor = true;
			this.DD_Sorting.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Sorting.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_Sorting.HideLabel = true;
			this.DD_Sorting.Location = new System.Drawing.Point(842, 3);
			this.DD_Sorting.Name = "DD_Sorting";
			this.DD_Sorting.Size = new System.Drawing.Size(50, 0);
			this.DD_Sorting.SkyvePage = SkyveApp.Domain.Enums.SkyvePage.None;
			this.DD_Sorting.TabIndex = 2;
			this.DD_Sorting.Text = "Sort By";
			this.DD_Sorting.SelectedItemChanged += new System.EventHandler(this.DD_Sorting_SelectedItemChanged);
			// 
			// TLP_MiddleBar
			// 
			this.TLP_MiddleBar.AutoSize = true;
			this.TLP_MiddleBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_MiddleBar.ColumnCount = 7;
			this.TLP_Main.SetColumnSpan(this.TLP_MiddleBar, 4);
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.Controls.Add(this.B_CompactList, 4, 0);
			this.TLP_MiddleBar.Controls.Add(this.L_Counts, 3, 0);
			this.TLP_MiddleBar.Controls.Add(this.L_FilterCount, 1, 0);
			this.TLP_MiddleBar.Controls.Add(this.B_ListView, 5, 0);
			this.TLP_MiddleBar.Controls.Add(this.B_GridView, 6, 0);
			this.TLP_MiddleBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_MiddleBar.Location = new System.Drawing.Point(0, 160);
			this.TLP_MiddleBar.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_MiddleBar.Name = "TLP_MiddleBar";
			this.TLP_MiddleBar.RowCount = 1;
			this.TLP_MiddleBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_MiddleBar.Size = new System.Drawing.Size(895, 32);
			this.TLP_MiddleBar.TabIndex = 6;
			// 
			// B_CompactList
			// 
			this.B_CompactList.ActiveColor = null;
			this.B_CompactList.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_CompactList.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "I_CompactList";
			this.B_CompactList.ImageName = dynamicIcon4;
			this.B_CompactList.Location = new System.Drawing.Point(790, 1);
			this.B_CompactList.Margin = new System.Windows.Forms.Padding(1);
			this.B_CompactList.Name = "B_CompactList";
			this.B_CompactList.Size = new System.Drawing.Size(33, 30);
			this.B_CompactList.SpaceTriggersClick = true;
			this.B_CompactList.TabIndex = 10;
			this.B_CompactList.Click += new System.EventHandler(this.B_CompactList_Click);
			// 
			// L_Counts
			// 
			this.L_Counts.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.L_Counts.AutoSize = true;
			this.L_Counts.Location = new System.Drawing.Point(742, 8);
			this.L_Counts.Name = "L_Counts";
			this.L_Counts.Size = new System.Drawing.Size(44, 16);
			this.L_Counts.TabIndex = 1;
			this.L_Counts.Text = "label1";
			this.L_Counts.UseMnemonic = false;
			// 
			// L_FilterCount
			// 
			this.L_FilterCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_FilterCount.AutoSize = true;
			this.L_FilterCount.Location = new System.Drawing.Point(3, 8);
			this.L_FilterCount.Name = "L_FilterCount";
			this.L_FilterCount.Size = new System.Drawing.Size(44, 16);
			this.L_FilterCount.TabIndex = 2;
			this.L_FilterCount.Text = "label1";
			this.L_FilterCount.UseMnemonic = false;
			// 
			// B_ListView
			// 
			this.B_ListView.ActiveColor = null;
			this.B_ListView.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_ListView.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon5.Name = "I_List";
			this.B_ListView.ImageName = dynamicIcon5;
			this.B_ListView.Location = new System.Drawing.Point(825, 1);
			this.B_ListView.Margin = new System.Windows.Forms.Padding(1);
			this.B_ListView.Name = "B_ListView";
			this.B_ListView.Selected = true;
			this.B_ListView.Size = new System.Drawing.Size(34, 30);
			this.B_ListView.SpaceTriggersClick = true;
			this.B_ListView.TabIndex = 9;
			this.B_ListView.Click += new System.EventHandler(this.B_ListView_Click);
			// 
			// B_GridView
			// 
			this.B_GridView.ActiveColor = null;
			this.B_GridView.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_GridView.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon6.Name = "I_Grid";
			this.B_GridView.ImageName = dynamicIcon6;
			this.B_GridView.Location = new System.Drawing.Point(861, 1);
			this.B_GridView.Margin = new System.Windows.Forms.Padding(1);
			this.B_GridView.Name = "B_GridView";
			this.B_GridView.Size = new System.Drawing.Size(33, 30);
			this.B_GridView.SpaceTriggersClick = true;
			this.B_GridView.TabIndex = 8;
			this.B_GridView.Click += new System.EventHandler(this.B_GridView_Click);
			// 
			// P_FiltersContainer
			// 
			this.TLP_Main.SetColumnSpan(this.P_FiltersContainer, 3);
			this.P_FiltersContainer.Controls.Add(this.P_Filters);
			this.P_FiltersContainer.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_FiltersContainer.Location = new System.Drawing.Point(0, 20);
			this.P_FiltersContainer.Margin = new System.Windows.Forms.Padding(0);
			this.P_FiltersContainer.Name = "P_FiltersContainer";
			this.P_FiltersContainer.Size = new System.Drawing.Size(895, 138);
			this.P_FiltersContainer.TabIndex = 3;
			this.P_FiltersContainer.Visible = false;
			// 
			// P_Filters
			// 
			this.P_Filters.AddOutline = true;
			this.P_Filters.AutoSize = true;
			this.P_Filters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Filters.ColumnCount = 4;
			this.P_Filters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.P_Filters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.P_Filters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.P_Filters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.P_Filters.Controls.Add(this.OT_ModAsset, 0, 4);
			this.P_Filters.Controls.Add(this.OT_Workshop, 0, 3);
			this.P_Filters.Controls.Add(this.OT_Enabled, 0, 2);
			this.P_Filters.Controls.Add(this.OT_Included, 0, 1);
			this.P_Filters.Controls.Add(this.I_ClearFilters, 3, 0);
			this.P_Filters.Controls.Add(this.DR_SubscribeTime, 1, 1);
			this.P_Filters.Controls.Add(this.DR_ServerTime, 1, 2);
			this.P_Filters.Controls.Add(this.DD_PackageStatus, 2, 2);
			this.P_Filters.Controls.Add(this.DD_Tags, 2, 1);
			this.P_Filters.Controls.Add(this.DD_ReportSeverity, 3, 2);
			this.P_Filters.Controls.Add(this.DD_Author, 3, 1);
			this.P_Filters.Controls.Add(this.DD_Profile, 2, 3);
			this.P_Filters.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Filters.Location = new System.Drawing.Point(0, 0);
			this.P_Filters.Name = "P_Filters";
			this.P_Filters.Padding = new System.Windows.Forms.Padding(9);
			this.P_Filters.RowCount = 5;
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.Size = new System.Drawing.Size(895, 149);
			this.P_Filters.TabIndex = 0;
			this.P_Filters.Text = "Filters";
			this.P_Filters.UseFirstRowForPadding = true;
			// 
			// OT_ModAsset
			// 
			this.OT_ModAsset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_ModAsset.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_ModAsset.Image1 = "I_Mods";
			this.OT_ModAsset.Image2 = "I_Assets";
			this.OT_ModAsset.Location = new System.Drawing.Point(12, 117);
			this.OT_ModAsset.Name = "OT_ModAsset";
			this.OT_ModAsset.Option1 = "Mods";
			this.OT_ModAsset.Option2 = "Assets";
			this.OT_ModAsset.OptionStyle1 = Extensions.ColorStyle.Active;
			this.OT_ModAsset.OptionStyle2 = Extensions.ColorStyle.Active;
			this.OT_ModAsset.Size = new System.Drawing.Size(213, 20);
			this.OT_ModAsset.TabIndex = 10;
			this.OT_ModAsset.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// OT_Workshop
			// 
			this.OT_Workshop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_Workshop.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_Workshop.Image1 = "I_PC";
			this.OT_Workshop.Image2 = "I_Steam";
			this.OT_Workshop.Location = new System.Drawing.Point(12, 91);
			this.OT_Workshop.Name = "OT_Workshop";
			this.OT_Workshop.Option1 = "Local";
			this.OT_Workshop.Option2 = "Workshop";
			this.OT_Workshop.OptionStyle1 = Extensions.ColorStyle.Active;
			this.OT_Workshop.OptionStyle2 = Extensions.ColorStyle.Active;
			this.OT_Workshop.Size = new System.Drawing.Size(213, 20);
			this.OT_Workshop.TabIndex = 2;
			this.OT_Workshop.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// OT_Enabled
			// 
			this.OT_Enabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_Enabled.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_Enabled.Image1 = "I_Checked";
			this.OT_Enabled.Image2 = "I_Checked_OFF";
			this.OT_Enabled.Location = new System.Drawing.Point(12, 65);
			this.OT_Enabled.Name = "OT_Enabled";
			this.OT_Enabled.Option1 = "Enabled";
			this.OT_Enabled.Option2 = "Disabled";
			this.OT_Enabled.Size = new System.Drawing.Size(213, 20);
			this.OT_Enabled.TabIndex = 1;
			this.OT_Enabled.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// OT_Included
			// 
			this.OT_Included.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_Included.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_Included.Image1 = "I_Ok";
			this.OT_Included.Image2 = "I_Enabled";
			this.OT_Included.Location = new System.Drawing.Point(12, 39);
			this.OT_Included.Name = "OT_Included";
			this.OT_Included.Option1 = "Included";
			this.OT_Included.Option2 = "Excluded";
			this.OT_Included.Size = new System.Drawing.Size(213, 20);
			this.OT_Included.TabIndex = 0;
			this.OT_Included.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// I_ClearFilters
			// 
			this.I_ClearFilters.ActiveColor = null;
			this.I_ClearFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.I_ClearFilters.ColorStyle = Extensions.ColorStyle.Red;
			this.I_ClearFilters.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon7.Name = "I_ClearFilter";
			this.I_ClearFilters.ImageName = dynamicIcon7;
			this.I_ClearFilters.Location = new System.Drawing.Point(853, 12);
			this.I_ClearFilters.Name = "I_ClearFilters";
			this.I_ClearFilters.Size = new System.Drawing.Size(30, 21);
			this.I_ClearFilters.TabIndex = 1;
			this.I_ClearFilters.Click += new System.EventHandler(this.I_ClearFilters_Click);
			// 
			// DR_SubscribeTime
			// 
			this.DR_SubscribeTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DR_SubscribeTime.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DR_SubscribeTime.Location = new System.Drawing.Point(231, 39);
			this.DR_SubscribeTime.Name = "DR_SubscribeTime";
			this.DR_SubscribeTime.Size = new System.Drawing.Size(213, 20);
			this.DR_SubscribeTime.TabIndex = 3;
			this.DR_SubscribeTime.RangeChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DR_ServerTime
			// 
			this.DR_ServerTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DR_ServerTime.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DR_ServerTime.Location = new System.Drawing.Point(231, 65);
			this.DR_ServerTime.Name = "DR_ServerTime";
			this.DR_ServerTime.Size = new System.Drawing.Size(213, 20);
			this.DR_ServerTime.TabIndex = 4;
			this.DR_ServerTime.RangeChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DD_PackageStatus
			// 
			this.DD_PackageStatus.AccentBackColor = true;
			this.DD_PackageStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_PackageStatus.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_PackageStatus.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_PackageStatus.Location = new System.Drawing.Point(450, 65);
			this.DD_PackageStatus.Name = "DD_PackageStatus";
			this.DD_PackageStatus.Size = new System.Drawing.Size(213, 20);
			this.DD_PackageStatus.TabIndex = 7;
			this.DD_PackageStatus.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DD_Tags
			// 
			this.DD_Tags.AccentBackColor = true;
			this.DD_Tags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Tags.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Tags.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_Tags.Location = new System.Drawing.Point(450, 39);
			this.DD_Tags.Name = "DD_Tags";
			this.DD_Tags.Size = new System.Drawing.Size(213, 20);
			this.DD_Tags.TabIndex = 5;
			this.DD_Tags.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DD_ReportSeverity
			// 
			this.DD_ReportSeverity.AccentBackColor = true;
			this.DD_ReportSeverity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_ReportSeverity.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_ReportSeverity.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_ReportSeverity.Location = new System.Drawing.Point(669, 65);
			this.DD_ReportSeverity.Name = "DD_ReportSeverity";
			this.DD_ReportSeverity.Size = new System.Drawing.Size(214, 20);
			this.DD_ReportSeverity.TabIndex = 8;
			this.DD_ReportSeverity.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DD_Author
			// 
			this.DD_Author.AccentBackColor = true;
			this.DD_Author.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Author.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Author.Location = new System.Drawing.Point(669, 39);
			this.DD_Author.Name = "DD_Author";
			this.DD_Author.Size = new System.Drawing.Size(214, 20);
			this.DD_Author.TabIndex = 6;
			this.DD_Author.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DD_Profile
			// 
			this.DD_Profile.AccentBackColor = true;
			this.DD_Profile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Profile.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Profile.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_Profile.Location = new System.Drawing.Point(450, 91);
			this.DD_Profile.Name = "DD_Profile";
			this.DD_Profile.Size = new System.Drawing.Size(213, 20);
			this.DD_Profile.TabIndex = 9;
			this.DD_Profile.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// I_SortOrder
			// 
			this.I_SortOrder.ActiveColor = null;
			this.I_SortOrder.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_SortOrder.Location = new System.Drawing.Point(822, 3);
			this.I_SortOrder.Name = "I_SortOrder";
			this.I_SortOrder.Size = new System.Drawing.Size(14, 14);
			this.I_SortOrder.TabIndex = 1;
			this.I_SortOrder.SizeChanged += new System.EventHandler(this.Icon_SizeChanged);
			this.I_SortOrder.Click += new System.EventHandler(this.I_SortOrder_Click);
			// 
			// ContentList
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Main);
			this.Name = "ContentList";
			this.Size = new System.Drawing.Size(895, 516);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.FLP_Search.ResumeLayout(false);
			this.FLP_Search.PerformLayout();
			this.TLP_MiddleBar.ResumeLayout(false);
			this.TLP_MiddleBar.PerformLayout();
			this.P_FiltersContainer.ResumeLayout(false);
			this.P_FiltersContainer.PerformLayout();
			this.P_Filters.ResumeLayout(false);
			this.ResumeLayout(false);

	}

	#endregion
	internal SlickControls.SlickTextBox TB_Search;
	private ThreeOptionToggle OT_Workshop;
	private ThreeOptionToggle OT_Enabled;
	private ThreeOptionToggle OT_Included;
	private SlickControls.RoundedGroupTableLayoutPanel P_Filters;
	private ReportSeverityDropDown DD_ReportSeverity;
	private PackageStatusDropDown DD_PackageStatus;
	private SlickControls.SlickIcon I_ClearFilters;
	private System.Windows.Forms.Label L_Counts;
	private SortingDropDown DD_Sorting;
	private System.Windows.Forms.TableLayoutPanel TLP_MiddleBar;
	private SlickControls.SlickSpacer slickSpacer1;
	private SlickControls.SlickSpacer slickSpacer2;
	internal System.Windows.Forms.Panel P_FiltersContainer;
	private SlickControls.SlickLabel B_Filters;
	private TagsDropDown DD_Tags;
	private ProfilesDropDown DD_Profile;
	private SlickControls.SlickIcon I_Refresh;
	private SlickControls.SlickDateRange DR_SubscribeTime;
	private SlickControls.SlickDateRange DR_ServerTime;
	private AuthorDropDown DD_Author;
	private SlickControls.SlickIcon I_SortOrder;
	private System.Windows.Forms.Label L_FilterCount;
	internal System.Windows.Forms.FlowLayoutPanel FLP_Search;
	private ThreeOptionToggle OT_ModAsset;
	internal System.Windows.Forms.TableLayoutPanel TLP_Main;
	protected SlickIcon B_ListView;
	protected SlickIcon B_GridView;
	protected SlickIcon B_CompactList;
}
