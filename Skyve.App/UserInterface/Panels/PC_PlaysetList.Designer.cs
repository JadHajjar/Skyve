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
			_notifier.PlaysetUpdated -= LoadPlayset;
			_notifier.PlaysetChanged -= LoadPlayset;
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
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.DD_Sorting = new Skyve.App.UserInterface.Dropdowns.PlaysetSortingDropDown();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.C_ViewTypeControl = new Skyve.App.UserInterface.Generic.ViewTypeControl();
			this.L_Counts = new System.Windows.Forms.Label();
			this.L_FilterCount = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.DD_Usage = new Skyve.App.UserInterface.Dropdowns.PackageUsageDropDown();
			this.roundedPanel = new SlickControls.RoundedPanel();
			this.TLP_Top = new System.Windows.Forms.TableLayoutPanel();
			this.B_AddPlayset = new SlickControls.SlickButton();
			this.TLP_PlaysetName = new SlickControls.RoundedTableLayoutPanel();
			this.B_DeactivatePlayset = new SlickControls.SlickIcon();
			this.L_CurrentPlayset = new System.Windows.Forms.Label();
			this.B_Edit = new SlickControls.SlickIcon();
			this.L_CurrentPlaysetTitle = new System.Windows.Forms.Label();
			this.PB_ActivePlayset = new SlickPictureBox();
			this.B_Discover = new SlickControls.SlickButton();
			this.TLP_Main.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.roundedPanel.SuspendLayout();
			this.TLP_Top.SuspendLayout();
			this.TLP_PlaysetName.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_ActivePlayset)).BeginInit();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 41);
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
			this.TLP_Main.Size = new System.Drawing.Size(1190, 639);
			this.TLP_Main.TabIndex = 0;
			// 
			// slickSpacer2
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer2, 3);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(0, 55);
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
			this.slickSpacer1.Location = new System.Drawing.Point(0, 92);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(1190, 2);
			this.slickSpacer1.TabIndex = 7;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TB_Search
			// 
			dynamicIcon1.Name = "Search";
			this.TB_Search.ImageName = dynamicIcon1;
			this.TB_Search.LabelText = "Search";
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(7, 7, 46, 7);
			this.TB_Search.Placeholder = "SearchPlaysets";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(140, 49);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.FilterChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// DD_Sorting
			// 
			this.DD_Sorting.AccentBackColor = true;
			this.DD_Sorting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Sorting.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Sorting.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_Sorting.HideLabel = true;
			this.DD_Sorting.ItemHeight = 24;
			this.DD_Sorting.Location = new System.Drawing.Point(973, 0);
			this.DD_Sorting.Margin = new System.Windows.Forms.Padding(0);
			this.DD_Sorting.Name = "DD_Sorting";
			this.DD_Sorting.Padding = new System.Windows.Forms.Padding(7);
			this.DD_Sorting.Size = new System.Drawing.Size(217, 55);
			this.DD_Sorting.TabIndex = 4;
			this.DD_Sorting.Text = "Sort By";
			this.DD_Sorting.SelectedItemChanged += new System.EventHandler(this.DD_Sorting_SelectedItemChanged);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 3;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel3, 3);
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add(this.C_ViewTypeControl, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.L_Counts, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.L_FilterCount, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 57);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(1190, 35);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// C_ViewTypeControl
			// 
			this.C_ViewTypeControl.Cursor = System.Windows.Forms.Cursors.Hand;
			this.C_ViewTypeControl.Location = new System.Drawing.Point(1037, 3);
			this.C_ViewTypeControl.Name = "C_ViewTypeControl";
			this.C_ViewTypeControl.Size = new System.Drawing.Size(150, 29);
			this.C_ViewTypeControl.TabIndex = 4;
			this.C_ViewTypeControl.WithCompactList = false;
			this.C_ViewTypeControl.ListClicked += new System.EventHandler(this.B_ListView_Click);
			this.C_ViewTypeControl.GridClicked += new System.EventHandler(this.B_GridView_Click);
			// 
			// L_Counts
			// 
			this.L_Counts.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.L_Counts.AutoSize = true;
			this.L_Counts.Location = new System.Drawing.Point(986, 8);
			this.L_Counts.Name = "L_Counts";
			this.L_Counts.Size = new System.Drawing.Size(45, 19);
			this.L_Counts.TabIndex = 1;
			this.L_Counts.Text = "label1";
			// 
			// L_FilterCount
			// 
			this.L_FilterCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_FilterCount.AutoSize = true;
			this.L_FilterCount.Location = new System.Drawing.Point(3, 8);
			this.L_FilterCount.Name = "L_FilterCount";
			this.L_FilterCount.Size = new System.Drawing.Size(45, 19);
			this.L_FilterCount.TabIndex = 2;
			this.L_FilterCount.Text = "label1";
			// 
			// panel1
			// 
			this.TLP_Main.SetColumnSpan(this.panel1, 3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 94);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1190, 545);
			this.panel1.TabIndex = 14;
			// 
			// DD_Usage
			// 
			this.DD_Usage.AccentBackColor = true;
			this.DD_Usage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Usage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Usage.HideLabel = true;
			this.DD_Usage.ItemHeight = 24;
			this.DD_Usage.Location = new System.Drawing.Point(777, 0);
			this.DD_Usage.Margin = new System.Windows.Forms.Padding(0);
			this.DD_Usage.Name = "DD_Usage";
			this.DD_Usage.Size = new System.Drawing.Size(196, 55);
			this.DD_Usage.TabIndex = 15;
			this.DD_Usage.Text = "PlaysetUsage";
			this.DD_Usage.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// roundedPanel
			// 
			this.roundedPanel.AddOutline = true;
			this.TLP_Top.SetColumnSpan(this.roundedPanel, 2);
			this.roundedPanel.Controls.Add(this.TLP_Main);
			this.roundedPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.roundedPanel.Location = new System.Drawing.Point(3, 47);
			this.roundedPanel.Name = "roundedPanel";
			this.roundedPanel.Size = new System.Drawing.Size(1190, 639);
			this.roundedPanel.TabIndex = 0;
			// 
			// TLP_Top
			// 
			this.TLP_Top.ColumnCount = 2;
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Top.Controls.Add(this.B_AddPlayset, 1, 0);
			this.TLP_Top.Controls.Add(this.TLP_PlaysetName, 0, 0);
			this.TLP_Top.Controls.Add(this.roundedPanel, 0, 1);
			this.TLP_Top.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Top.Location = new System.Drawing.Point(0, 30);
			this.TLP_Top.Name = "TLP_Top";
			this.TLP_Top.RowCount = 2;
			this.TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Top.Size = new System.Drawing.Size(1196, 689);
			this.TLP_Top.TabIndex = 0;
			// 
			// B_AddPlayset
			// 
			this.B_AddPlayset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_AddPlayset.AutoSize = true;
			this.B_AddPlayset.ColorStyle = Extensions.ColorStyle.Green;
			this.B_AddPlayset.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "Add";
			this.B_AddPlayset.ImageName = dynamicIcon2;
			this.B_AddPlayset.LargeImage = true;
			this.B_AddPlayset.Location = new System.Drawing.Point(1020, 9);
			this.B_AddPlayset.Name = "B_AddPlayset";
			this.B_AddPlayset.Size = new System.Drawing.Size(173, 32);
			this.B_AddPlayset.SpaceTriggersClick = true;
			this.B_AddPlayset.TabIndex = 3;
			this.B_AddPlayset.Text = "CreatePlayset";
			this.B_AddPlayset.Click += new System.EventHandler(this.B_AddPlayset_Click);
			// 
			// TLP_PlaysetName
			// 
			this.TLP_PlaysetName.AutoSize = true;
			this.TLP_PlaysetName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_PlaysetName.ColumnCount = 4;
			this.TLP_PlaysetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_PlaysetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_PlaysetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_PlaysetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_PlaysetName.Controls.Add(this.B_DeactivatePlayset, 3, 0);
			this.TLP_PlaysetName.Controls.Add(this.L_CurrentPlayset, 1, 1);
			this.TLP_PlaysetName.Controls.Add(this.B_Edit, 2, 0);
			this.TLP_PlaysetName.Controls.Add(this.L_CurrentPlaysetTitle, 1, 0);
			this.TLP_PlaysetName.Controls.Add(this.PB_ActivePlayset, 0, 0);
			this.TLP_PlaysetName.Location = new System.Drawing.Point(3, 3);
			this.TLP_PlaysetName.Name = "TLP_PlaysetName";
			this.TLP_PlaysetName.RowCount = 2;
			this.TLP_PlaysetName.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_PlaysetName.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_PlaysetName.Size = new System.Drawing.Size(147, 38);
			this.TLP_PlaysetName.TabIndex = 0;
			// 
			// B_DeactivatePlayset
			// 
			this.B_DeactivatePlayset.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_DeactivatePlayset.ColorStyle = Extensions.ColorStyle.Red;
			this.B_DeactivatePlayset.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "Shutdown";
			this.B_DeactivatePlayset.ImageName = dynamicIcon3;
			this.B_DeactivatePlayset.Location = new System.Drawing.Point(103, 3);
			this.B_DeactivatePlayset.Margin = new System.Windows.Forms.Padding(0);
			this.B_DeactivatePlayset.Name = "B_DeactivatePlayset";
			this.B_DeactivatePlayset.Padding = new System.Windows.Forms.Padding(5);
			this.TLP_PlaysetName.SetRowSpan(this.B_DeactivatePlayset, 2);
			this.B_DeactivatePlayset.Size = new System.Drawing.Size(44, 31);
			this.B_DeactivatePlayset.TabIndex = 4;
			this.B_DeactivatePlayset.Click += new System.EventHandler(this.B_Deactivate_Click);
			// 
			// L_CurrentPlayset
			// 
			this.L_CurrentPlayset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_CurrentPlayset.AutoSize = true;
			this.L_CurrentPlayset.Location = new System.Drawing.Point(23, 19);
			this.L_CurrentPlayset.Name = "L_CurrentPlayset";
			this.L_CurrentPlayset.Size = new System.Drawing.Size(45, 19);
			this.L_CurrentPlayset.TabIndex = 1;
			this.L_CurrentPlayset.Text = "label1";
			// 
			// B_Edit
			// 
			this.B_Edit.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Edit.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "Cog";
			this.B_Edit.ImageName = dynamicIcon4;
			this.B_Edit.Location = new System.Drawing.Point(71, 3);
			this.B_Edit.Margin = new System.Windows.Forms.Padding(0);
			this.B_Edit.Name = "B_Edit";
			this.B_Edit.Padding = new System.Windows.Forms.Padding(5);
			this.TLP_PlaysetName.SetRowSpan(this.B_Edit, 2);
			this.B_Edit.Size = new System.Drawing.Size(32, 32);
			this.B_Edit.TabIndex = 3;
			this.B_Edit.Click += new System.EventHandler(this.B_Edit_Click);
			// 
			// L_CurrentPlaysetTitle
			// 
			this.L_CurrentPlaysetTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_CurrentPlaysetTitle.AutoSize = true;
			this.L_CurrentPlaysetTitle.Location = new System.Drawing.Point(23, 0);
			this.L_CurrentPlaysetTitle.Name = "L_CurrentPlaysetTitle";
			this.L_CurrentPlaysetTitle.Size = new System.Drawing.Size(45, 19);
			this.L_CurrentPlaysetTitle.TabIndex = 1;
			this.L_CurrentPlaysetTitle.Text = "label1";
			// 
			// PB_ActivePlayset
			// 
			this.PB_ActivePlayset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.PB_ActivePlayset.Location = new System.Drawing.Point(3, 7);
			this.PB_ActivePlayset.Name = "PB_ActivePlayset";
			this.TLP_PlaysetName.SetRowSpan(this.PB_ActivePlayset, 2);
			this.PB_ActivePlayset.Size = new System.Drawing.Size(14, 24);
			this.PB_ActivePlayset.TabIndex = 5;
			this.PB_ActivePlayset.TabStop = false;
			this.PB_ActivePlayset.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_ActivePlayset_Paint);
			// 
			// B_Discover
			// 
			this.B_Discover.AutoSize = true;
			this.B_Discover.ButtonType = SlickControls.ButtonType.Active;
			this.B_Discover.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon5.Name = "Discover";
			this.B_Discover.ImageName = dynamicIcon5;
			this.B_Discover.LargeImage = true;
			this.B_Discover.Location = new System.Drawing.Point(920, 0);
			this.B_Discover.Name = "B_Discover";
			this.B_Discover.Size = new System.Drawing.Size(151, 32);
			this.B_Discover.SpaceTriggersClick = true;
			this.B_Discover.TabIndex = 3;
			this.B_Discover.Text = "DiscoverPlaysets";
			this.B_Discover.Visible = false;
			this.B_Discover.Click += new System.EventHandler(this.B_Discover_Click);
			// 
			// PC_PlaysetList
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Top);
			this.Controls.Add(this.B_Discover);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_PlaysetList";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1196, 719);
			this.Text = "Mods";
			this.Controls.SetChildIndex(this.B_Discover, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Top, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.roundedPanel.ResumeLayout(false);
			this.TLP_Top.ResumeLayout(false);
			this.TLP_Top.PerformLayout();
			this.TLP_PlaysetName.ResumeLayout(false);
			this.TLP_PlaysetName.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_ActivePlayset)).EndInit();
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
	private System.Windows.Forms.Label L_FilterCount;
	private PackageUsageDropDown DD_Usage;
	private SlickControls.RoundedPanel roundedPanel;
	private System.Windows.Forms.TableLayoutPanel TLP_Top;
	public SlickControls.SlickButton B_Discover;
	private SlickControls.RoundedTableLayoutPanel TLP_PlaysetName;
	private SlickControls.SlickIcon B_DeactivatePlayset;
	private System.Windows.Forms.Label L_CurrentPlayset;
	private SlickIcon B_Edit;
	private Generic.ViewTypeControl C_ViewTypeControl;
	public SlickButton B_AddPlayset;
	private System.Windows.Forms.Label L_CurrentPlaysetTitle;
	private SlickPictureBox PB_ActivePlayset;
}
