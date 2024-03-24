using Skyve.App.UserInterface.Lists;

namespace Skyve.App.UserInterface.Panels;

partial class PC_CompatibilityReport
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
			_notifier.ContentLoaded -= CompatibilityManager_ReportProcessed;
			_notifier.CompatibilityReportProcessed -= CompatibilityManager_ReportProcessed;
			_notifier.SnoozeChanged -= CompatibilityManager_ReportProcessed;
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
			SlickControls.DynamicIcon dynamicIcon21 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon22 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon23 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon24 = new SlickControls.DynamicIcon();
			this.TLP_Buttons = new System.Windows.Forms.TableLayoutPanel();
			this.PB_Loader = new SlickControls.SlickPictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.FLP_Search = new System.Windows.Forms.FlowLayoutPanel();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.I_Refresh = new SlickControls.SlickIcon();
			this.B_Filters = new SlickControls.SlickLabel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.ListControl = new Skyve.App.UserInterface.Lists.CompatibilityReportList();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.DD_Sorting = new Skyve.App.UserInterface.Dropdowns.SortingDropDown();
			this.P_FiltersContainer = new System.Windows.Forms.Panel();
			this.P_Filters = new SlickControls.RoundedGroupTableLayoutPanel();
			this.OT_ModAsset = new Skyve.App.UserInterface.Generic.ThreeOptionToggle();
			this.OT_Workshop = new Skyve.App.UserInterface.Generic.ThreeOptionToggle();
			this.OT_Enabled = new Skyve.App.UserInterface.Generic.ThreeOptionToggle();
			this.OT_Included = new Skyve.App.UserInterface.Generic.ThreeOptionToggle();
			this.I_ClearFilters = new SlickControls.SlickIcon();
			this.DR_SubscribeTime = new SlickControls.SlickDateRange();
			this.DR_ServerTime = new SlickControls.SlickDateRange();
			this.DD_PackageStatus = new Skyve.App.UserInterface.Dropdowns.PackageStatusDropDown();
			this.DD_Tags = new Skyve.App.UserInterface.Dropdowns.TagsDropDown();
			this.DD_Author = new Skyve.App.UserInterface.Dropdowns.AuthorDropDown();
			this.DD_Profile = new Skyve.App.UserInterface.Dropdowns.PlaysetsDropDown();
			this.I_SortOrder = new SlickControls.SlickIcon();
			this.TLP_MiddleBar = new System.Windows.Forms.TableLayoutPanel();
			this.L_Counts = new Skyve.App.UserInterface.Generic.ItemCountControl();
			this.C_ViewTypeControl = new Skyve.App.UserInterface.Generic.ViewTypeControl();
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).BeginInit();
			this.TLP_Main.SuspendLayout();
			this.FLP_Search.SuspendLayout();
			this.P_FiltersContainer.SuspendLayout();
			this.P_Filters.SuspendLayout();
			this.TLP_MiddleBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Location = new System.Drawing.Point(-2, 3);
			this.base_Text.Size = new System.Drawing.Size(150, 32);
			// 
			// TLP_Buttons
			// 
			this.TLP_Buttons.AutoSize = true;
			this.TLP_Buttons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Buttons.ColumnCount = 6;
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.Location = new System.Drawing.Point(124, 427);
			this.TLP_Buttons.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Buttons.Name = "TLP_Buttons";
			this.TLP_Buttons.RowCount = 1;
			this.TLP_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Buttons.Size = new System.Drawing.Size(0, 0);
			this.TLP_Buttons.TabIndex = 0;
			// 
			// PB_Loader
			// 
			this.PB_Loader.LoaderSpeed = 1D;
			this.PB_Loader.Location = new System.Drawing.Point(640, 392);
			this.PB_Loader.Name = "PB_Loader";
			this.PB_Loader.Size = new System.Drawing.Size(32, 32);
			this.PB_Loader.TabIndex = 102;
			this.PB_Loader.TabStop = false;
			this.PB_Loader.Visible = false;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(-94, -4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 13);
			this.label1.TabIndex = 103;
			this.label1.Text = "No issues detected";
			this.label1.Visible = false;
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 3;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Controls.Add(this.TLP_MiddleBar, 0, 5);
			this.TLP_Main.Controls.Add(this.FLP_Search, 0, 1);
			this.TLP_Main.Controls.Add(this.slickSpacer2, 0, 4);
			this.TLP_Main.Controls.Add(this.ListControl, 0, 7);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 0, 6);
			this.TLP_Main.Controls.Add(this.DD_Sorting, 2, 1);
			this.TLP_Main.Controls.Add(this.P_FiltersContainer, 0, 3);
			this.TLP_Main.Controls.Add(this.I_SortOrder, 1, 1);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(0, 30);
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
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Size = new System.Drawing.Size(1124, 778);
			this.TLP_Main.TabIndex = 104;
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
			this.FLP_Search.Size = new System.Drawing.Size(1048, 37);
			this.FLP_Search.TabIndex = 0;
			// 
			// TB_Search
			// 
			dynamicIcon21.Name = "Search";
			this.TB_Search.ImageName = dynamicIcon21;
			this.TB_Search.LabelText = "Search";
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(5, 5, 47, 5);
			this.TB_Search.Placeholder = "SearchGenericPackages";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(253, 31);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// I_Refresh
			// 
			this.I_Refresh.ActiveColor = null;
			this.I_Refresh.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon22.Name = "Refresh";
			this.I_Refresh.ImageName = dynamicIcon22;
			this.I_Refresh.Location = new System.Drawing.Point(262, 3);
			this.I_Refresh.Name = "I_Refresh";
			this.I_Refresh.Size = new System.Drawing.Size(14, 14);
			this.I_Refresh.SpaceTriggersClick = true;
			this.I_Refresh.TabIndex = 1;
			this.I_Refresh.SizeChanged += new System.EventHandler(this.I_Refresh_SizeChanged);
			this.I_Refresh.Click += new System.EventHandler(this.I_Refresh_Click);
			// 
			// B_Filters
			// 
			this.B_Filters.AutoSize = true;
			this.B_Filters.AutoSizeIcon = true;
			this.B_Filters.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon23.Name = "Filter";
			this.B_Filters.ImageName = dynamicIcon23;
			this.B_Filters.Location = new System.Drawing.Point(282, 3);
			this.B_Filters.Name = "B_Filters";
			this.B_Filters.Selected = false;
			this.B_Filters.Size = new System.Drawing.Size(95, 24);
			this.B_Filters.SpaceTriggersClick = true;
			this.B_Filters.TabIndex = 1;
			this.B_Filters.Text = "ShowFilters";
			this.B_Filters.MouseClick += new System.Windows.Forms.MouseEventHandler(this.B_Filters_Click);
			// 
			// slickSpacer2
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer2, 3);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(0, 187);
			this.slickSpacer2.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(1124, 2);
			this.slickSpacer2.TabIndex = 8;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// ListControl
			// 
			this.ListControl.AllowDrop = true;
			this.ListControl.AutoInvalidate = false;
			this.ListControl.AutoScroll = true;
			this.TLP_Main.SetColumnSpan(this.ListControl, 3);
			this.ListControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ListControl.DynamicSizing = true;
			this.ListControl.GridView = true;
			this.ListControl.ItemHeight = 75;
			this.ListControl.Location = new System.Drawing.Point(0, 226);
			this.ListControl.Margin = new System.Windows.Forms.Padding(0);
			this.ListControl.Name = "ListControl";
			this.ListControl.Size = new System.Drawing.Size(1124, 552);
			this.ListControl.TabIndex = 1;
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 3);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 224);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(1124, 2);
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
			this.DD_Sorting.Location = new System.Drawing.Point(1071, 3);
			this.DD_Sorting.Name = "DD_Sorting";
			this.DD_Sorting.Size = new System.Drawing.Size(50, 0);
			this.DD_Sorting.SkyvePage = Skyve.Domain.Enums.SkyvePage.None;
			this.DD_Sorting.TabIndex = 2;
			this.DD_Sorting.Text = "Sort By";
			this.DD_Sorting.SelectedItemChanged += new System.EventHandler(this.DD_Sorting_SelectedItemChanged);
			// 
			// P_FiltersContainer
			// 
			this.TLP_Main.SetColumnSpan(this.P_FiltersContainer, 3);
			this.P_FiltersContainer.Controls.Add(this.P_Filters);
			this.P_FiltersContainer.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_FiltersContainer.Location = new System.Drawing.Point(0, 37);
			this.P_FiltersContainer.Margin = new System.Windows.Forms.Padding(0);
			this.P_FiltersContainer.Name = "P_FiltersContainer";
			this.P_FiltersContainer.Size = new System.Drawing.Size(1124, 150);
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
			this.P_Filters.Controls.Add(this.DD_Author, 3, 1);
			this.P_Filters.Controls.Add(this.DD_Profile, 3, 2);
			this.P_Filters.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Filters.Location = new System.Drawing.Point(0, 0);
			this.P_Filters.Name = "P_Filters";
			this.P_Filters.Padding = new System.Windows.Forms.Padding(6);
			this.P_Filters.RowCount = 5;
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Filters.Size = new System.Drawing.Size(1124, 143);
			this.P_Filters.TabIndex = 0;
			this.P_Filters.Text = "Filters";
			this.P_Filters.UseFirstRowForPadding = true;
			// 
			// OT_ModAsset
			// 
			this.OT_ModAsset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_ModAsset.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_ModAsset.Image1 = "Mods";
			this.OT_ModAsset.Image2 = "Assets";
			this.OT_ModAsset.Location = new System.Drawing.Point(9, 114);
			this.OT_ModAsset.Name = "OT_ModAsset";
			this.OT_ModAsset.Option1 = "Mods";
			this.OT_ModAsset.Option2 = "Assets";
			this.OT_ModAsset.OptionStyle1 = Extensions.ColorStyle.Active;
			this.OT_ModAsset.OptionStyle2 = Extensions.ColorStyle.Active;
			this.OT_ModAsset.Size = new System.Drawing.Size(272, 20);
			this.OT_ModAsset.TabIndex = 10;
			this.OT_ModAsset.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// OT_Workshop
			// 
			this.OT_Workshop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_Workshop.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_Workshop.Image1 = "PC";
			this.OT_Workshop.Image2 = "Steam";
			this.OT_Workshop.Location = new System.Drawing.Point(9, 88);
			this.OT_Workshop.Name = "OT_Workshop";
			this.OT_Workshop.Option1 = "Local";
			this.OT_Workshop.Option2 = "Workshop";
			this.OT_Workshop.OptionStyle1 = Extensions.ColorStyle.Active;
			this.OT_Workshop.OptionStyle2 = Extensions.ColorStyle.Active;
			this.OT_Workshop.Size = new System.Drawing.Size(272, 20);
			this.OT_Workshop.TabIndex = 2;
			this.OT_Workshop.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// OT_Enabled
			// 
			this.OT_Enabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_Enabled.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_Enabled.Image1 = "Ok";
			this.OT_Enabled.Image2 = "Enabled";
			this.OT_Enabled.Location = new System.Drawing.Point(9, 62);
			this.OT_Enabled.Name = "OT_Enabled";
			this.OT_Enabled.Option1 = "Enabled";
			this.OT_Enabled.Option2 = "Disabled";
			this.OT_Enabled.Size = new System.Drawing.Size(272, 20);
			this.OT_Enabled.TabIndex = 1;
			this.OT_Enabled.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// OT_Included
			// 
			this.OT_Included.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_Included.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_Included.Image1 = "Add";
			this.OT_Included.Image2 = "X";
			this.OT_Included.Location = new System.Drawing.Point(9, 36);
			this.OT_Included.Name = "OT_Included";
			this.OT_Included.Option1 = "Included";
			this.OT_Included.Option2 = "Excluded";
			this.OT_Included.Size = new System.Drawing.Size(272, 20);
			this.OT_Included.TabIndex = 0;
			this.OT_Included.SelectedValueChanged += new System.EventHandler(this.FilterChanged);
			// 
			// I_ClearFilters
			// 
			this.I_ClearFilters.ActiveColor = null;
			this.I_ClearFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.I_ClearFilters.ColorStyle = Extensions.ColorStyle.Red;
			this.I_ClearFilters.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon24.Name = "ClearFilter";
			this.I_ClearFilters.ImageName = dynamicIcon24;
			this.I_ClearFilters.Location = new System.Drawing.Point(1085, 9);
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
			this.DR_SubscribeTime.Location = new System.Drawing.Point(287, 36);
			this.DR_SubscribeTime.Name = "DR_SubscribeTime";
			this.DR_SubscribeTime.Size = new System.Drawing.Size(272, 20);
			this.DR_SubscribeTime.TabIndex = 3;
			this.DR_SubscribeTime.RangeChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DR_ServerTime
			// 
			this.DR_ServerTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DR_ServerTime.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DR_ServerTime.Location = new System.Drawing.Point(287, 62);
			this.DR_ServerTime.Name = "DR_ServerTime";
			this.DR_ServerTime.Size = new System.Drawing.Size(272, 20);
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
			this.DD_PackageStatus.Location = new System.Drawing.Point(565, 62);
			this.DD_PackageStatus.Name = "DD_PackageStatus";
			this.DD_PackageStatus.Size = new System.Drawing.Size(272, 20);
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
			this.DD_Tags.Location = new System.Drawing.Point(565, 36);
			this.DD_Tags.Name = "DD_Tags";
			this.DD_Tags.Size = new System.Drawing.Size(272, 20);
			this.DD_Tags.TabIndex = 5;
			this.DD_Tags.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// DD_Author
			// 
			this.DD_Author.AccentBackColor = true;
			this.DD_Author.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Author.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Author.Location = new System.Drawing.Point(843, 36);
			this.DD_Author.Name = "DD_Author";
			this.DD_Author.Size = new System.Drawing.Size(272, 20);
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
			this.DD_Profile.Location = new System.Drawing.Point(843, 62);
			this.DD_Profile.Name = "DD_Profile";
			this.DD_Profile.Size = new System.Drawing.Size(272, 20);
			this.DD_Profile.TabIndex = 9;
			this.DD_Profile.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// I_SortOrder
			// 
			this.I_SortOrder.ActiveColor = null;
			this.I_SortOrder.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_SortOrder.Location = new System.Drawing.Point(1051, 3);
			this.I_SortOrder.Name = "I_SortOrder";
			this.I_SortOrder.Size = new System.Drawing.Size(14, 14);
			this.I_SortOrder.TabIndex = 1;
			this.I_SortOrder.Click += new System.EventHandler(this.I_SortOrder_Click);
			// 
			// TLP_MiddleBar
			// 
			this.TLP_MiddleBar.AutoSize = true;
			this.TLP_MiddleBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_MiddleBar.ColumnCount = 3;
			this.TLP_Main.SetColumnSpan(this.TLP_MiddleBar, 3);
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_MiddleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_MiddleBar.Controls.Add(this.C_ViewTypeControl, 2, 0);
			this.TLP_MiddleBar.Controls.Add(this.L_Counts, 1, 0);
			this.TLP_MiddleBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_MiddleBar.Location = new System.Drawing.Point(0, 189);
			this.TLP_MiddleBar.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_MiddleBar.Name = "TLP_MiddleBar";
			this.TLP_MiddleBar.RowCount = 1;
			this.TLP_MiddleBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_MiddleBar.Size = new System.Drawing.Size(1124, 35);
			this.TLP_MiddleBar.TabIndex = 9;
			// 
			// L_Counts
			// 
			this.L_Counts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.L_Counts.Location = new System.Drawing.Point(0, 0);
			this.L_Counts.Margin = new System.Windows.Forms.Padding(0);
			this.L_Counts.Name = "L_Counts";
			this.L_Counts.Size = new System.Drawing.Size(968, 35);
			this.L_Counts.TabIndex = 4;
			this.L_Counts.Text = "itemCountControl1";
			// 
			// C_ViewTypeControl
			// 
			this.C_ViewTypeControl.Cursor = System.Windows.Forms.Cursors.Hand;
			this.C_ViewTypeControl.Location = new System.Drawing.Point(971, 3);
			this.C_ViewTypeControl.Name = "C_ViewTypeControl";
			this.C_ViewTypeControl.Size = new System.Drawing.Size(150, 29);
			this.C_ViewTypeControl.TabIndex = 3;
			this.C_ViewTypeControl.Visible = false;
			// 
			// PC_CompatibilityReport
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.PB_Loader);
			this.Controls.Add(this.TLP_Buttons);
			this.Controls.Add(this.TLP_Main);
			this.Name = "PC_CompatibilityReport";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1124, 808);
			this.Controls.SetChildIndex(this.TLP_Main, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Buttons, 0);
			this.Controls.SetChildIndex(this.PB_Loader, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).EndInit();
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.FLP_Search.ResumeLayout(false);
			this.FLP_Search.PerformLayout();
			this.P_FiltersContainer.ResumeLayout(false);
			this.P_FiltersContainer.PerformLayout();
			this.P_Filters.ResumeLayout(false);
			this.TLP_MiddleBar.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_Buttons;
	private CompatibilityReportList ListControl;
	private SlickControls.SlickPictureBox PB_Loader;
	private System.Windows.Forms.Label label1;
	public System.Windows.Forms.TableLayoutPanel TLP_Main;
	public System.Windows.Forms.FlowLayoutPanel FLP_Search;
	public SlickTextBox TB_Search;
	internal SlickIcon I_Refresh;
	internal SlickLabel B_Filters;
	internal Dropdowns.SortingDropDown DD_Sorting;
	public System.Windows.Forms.Panel P_FiltersContainer;
	internal RoundedGroupTableLayoutPanel P_Filters;
	internal Generic.ThreeOptionToggle OT_ModAsset;
	internal Generic.ThreeOptionToggle OT_Workshop;
	internal Generic.ThreeOptionToggle OT_Enabled;
	internal Generic.ThreeOptionToggle OT_Included;
	internal SlickIcon I_ClearFilters;
	internal SlickDateRange DR_SubscribeTime;
	internal SlickDateRange DR_ServerTime;
	internal Dropdowns.PackageStatusDropDown DD_PackageStatus;
	internal Dropdowns.TagsDropDown DD_Tags;
	internal Dropdowns.AuthorDropDown DD_Author;
	internal Dropdowns.PlaysetsDropDown DD_Profile;
	internal SlickIcon I_SortOrder;
	internal SlickSpacer slickSpacer2;
	internal SlickSpacer slickSpacer1;
	internal System.Windows.Forms.TableLayoutPanel TLP_MiddleBar;
	private Generic.ItemCountControl L_Counts;
	private Generic.ViewTypeControl C_ViewTypeControl;
}
