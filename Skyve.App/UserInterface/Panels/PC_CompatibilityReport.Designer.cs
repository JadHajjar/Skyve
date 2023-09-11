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
			_notifier.CompatibilityReportProcessed -= CompatibilityManager_ReportProcessed;
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
			this.TLP_Buttons = new System.Windows.Forms.TableLayoutPanel();
			this.B_ApplyAll = new SlickControls.SlickButton();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.B_Manage = new SlickControls.SlickButton();
			this.B_YourPackages = new SlickControls.SlickButton();
			this.B_ManageSingle = new SlickControls.SlickButton();
			this.B_Requests = new SlickControls.SlickButton();
			this.PB_Loader = new SlickControls.SlickPictureBox();
			this.LC_Items = new Skyve.App.UserInterface.Lists.CompatibilityReportList();
			this.label1 = new System.Windows.Forms.Label();
			this.TLP_Buttons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).BeginInit();
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
			this.TLP_Buttons.Controls.Add(this.B_ApplyAll, 1, 0);
			this.TLP_Buttons.Controls.Add(this.TB_Search, 0, 0);
			this.TLP_Buttons.Controls.Add(this.B_Manage, 5, 0);
			this.TLP_Buttons.Controls.Add(this.B_YourPackages, 2, 0);
			this.TLP_Buttons.Controls.Add(this.B_ManageSingle, 3, 0);
			this.TLP_Buttons.Controls.Add(this.B_Requests, 4, 0);
			this.TLP_Buttons.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_Buttons.Location = new System.Drawing.Point(0, 30);
			this.TLP_Buttons.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Buttons.Name = "TLP_Buttons";
			this.TLP_Buttons.RowCount = 1;
			this.TLP_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Buttons.Size = new System.Drawing.Size(1312, 32);
			this.TLP_Buttons.TabIndex = 0;
			// 
			// B_ApplyAll
			// 
			this.B_ApplyAll.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_ApplyAll.AutoSize = true;
			this.B_ApplyAll.ButtonType = SlickControls.ButtonType.Active;
			this.B_ApplyAll.ColorShade = null;
			this.B_ApplyAll.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ApplyAll.Enabled = false;
			dynamicIcon1.Name = "I_CompatibilityReport";
			this.B_ApplyAll.ImageName = dynamicIcon1;
			this.B_ApplyAll.Location = new System.Drawing.Point(428, 4);
			this.B_ApplyAll.Name = "B_ApplyAll";
			this.B_ApplyAll.Size = new System.Drawing.Size(189, 24);
			this.B_ApplyAll.SpaceTriggersClick = true;
			this.B_ApplyAll.TabIndex = 1;
			this.B_ApplyAll.Text = "ApplyAllActions";
			this.B_ApplyAll.Click += new System.EventHandler(this.B_ApplyAll_Click);
			// 
			// TB_Search
			// 
			this.TB_Search.Anchor = System.Windows.Forms.AnchorStyles.Left;
			dynamicIcon2.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon2;
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this.TB_Search.Placeholder = "SearchGenericPackages";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(342, 26);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// B_Manage
			// 
			this.B_Manage.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Manage.AutoSize = true;
			this.B_Manage.ColorShade = null;
			this.B_Manage.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_Cog";
			this.B_Manage.ImageName = dynamicIcon3;
			this.B_Manage.Location = new System.Drawing.Point(1109, 4);
			this.B_Manage.Name = "B_Manage";
			this.B_Manage.Size = new System.Drawing.Size(200, 24);
			this.B_Manage.SpaceTriggersClick = true;
			this.B_Manage.TabIndex = 5;
			this.B_Manage.Text = "ManageCompatibilityData";
			this.B_Manage.Click += new System.EventHandler(this.B_Manage_Click);
			// 
			// B_YourPackages
			// 
			this.B_YourPackages.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_YourPackages.AutoSize = true;
			this.B_YourPackages.ColorShade = null;
			this.B_YourPackages.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "I_User";
			this.B_YourPackages.ImageName = dynamicIcon4;
			this.B_YourPackages.Location = new System.Drawing.Point(623, 4);
			this.B_YourPackages.Name = "B_YourPackages";
			this.B_YourPackages.Size = new System.Drawing.Size(171, 24);
			this.B_YourPackages.SpaceTriggersClick = true;
			this.B_YourPackages.TabIndex = 2;
			this.B_YourPackages.Text = "YourPackages";
			this.B_YourPackages.Click += new System.EventHandler(this.B_Manage_Click);
			// 
			// B_ManageSingle
			// 
			this.B_ManageSingle.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_ManageSingle.AutoSize = true;
			this.B_ManageSingle.ColorShade = null;
			this.B_ManageSingle.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon5.Name = "I_Edit";
			this.B_ManageSingle.ImageName = dynamicIcon5;
			this.B_ManageSingle.Location = new System.Drawing.Point(800, 4);
			this.B_ManageSingle.Name = "B_ManageSingle";
			this.B_ManageSingle.Size = new System.Drawing.Size(191, 24);
			this.B_ManageSingle.SpaceTriggersClick = true;
			this.B_ManageSingle.TabIndex = 3;
			this.B_ManageSingle.Text = "ManageSinglePackage";
			this.B_ManageSingle.Click += new System.EventHandler(this.B_ManageSingle_Click);
			// 
			// B_Requests
			// 
			this.B_Requests.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Requests.AutoSize = true;
			this.B_Requests.ColorShade = null;
			this.B_Requests.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon6.Name = "I_RequestReview";
			this.B_Requests.ImageName = dynamicIcon6;
			this.B_Requests.Location = new System.Drawing.Point(997, 4);
			this.B_Requests.Name = "B_Requests";
			this.B_Requests.Size = new System.Drawing.Size(106, 24);
			this.B_Requests.SpaceTriggersClick = true;
			this.B_Requests.TabIndex = 4;
			this.B_Requests.Text = "ViewRequests";
			this.B_Requests.Click += new System.EventHandler(this.B_Requests_Click);
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
			// LC_Items
			// 
			this.LC_Items.AllowDrop = true;
			this.LC_Items.AutoInvalidate = false;
			this.LC_Items.AutoScroll = true;
			this.LC_Items.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LC_Items.DynamicSizing = true;
			this.LC_Items.GridView = true;
			this.LC_Items.ItemHeight = 75;
			this.LC_Items.Location = new System.Drawing.Point(0, 62);
			this.LC_Items.Name = "LC_Items";
			this.LC_Items.Size = new System.Drawing.Size(1312, 755);
			this.LC_Items.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(140, 21);
			this.label1.TabIndex = 103;
			this.label1.Text = "No issues detected";
			this.label1.Visible = false;
			// 
			// PC_CompatibilityReport
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.PB_Loader);
			this.Controls.Add(this.LC_Items);
			this.Controls.Add(this.TLP_Buttons);
			this.LabelBounds = new System.Drawing.Point(-2, 3);
			this.Name = "PC_CompatibilityReport";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1312, 817);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Buttons, 0);
			this.Controls.SetChildIndex(this.LC_Items, 0);
			this.Controls.SetChildIndex(this.PB_Loader, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.TLP_Buttons.ResumeLayout(false);
			this.TLP_Buttons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_Buttons;
	private SlickControls.SlickButton B_Manage;
	private SlickControls.SlickButton B_YourPackages;
	private SlickControls.SlickButton B_ManageSingle;
	private CompatibilityReportList LC_Items;
	private SlickControls.SlickPictureBox PB_Loader;
	private SlickControls.SlickButton B_Requests;
	private SlickTextBox TB_Search;
	private SlickButton B_ApplyAll;
	private System.Windows.Forms.Label label1;
}
