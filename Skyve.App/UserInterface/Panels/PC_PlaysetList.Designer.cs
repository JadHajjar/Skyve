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
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.L_Counts = new System.Windows.Forms.Label();
			this.L_FilterCount = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.roundedPanel = new SlickControls.RoundedPanel();
			this.TLP_Top = new System.Windows.Forms.TableLayoutPanel();
			this.B_AddPlayset = new SlickControls.RoundedTableLayoutPanel();
			this.B_Add = new SlickControls.SlickIcon();
			this.B_DeactivatePlayset = new SlickControls.RoundedTableLayoutPanel();
			this.B_Deactivate = new SlickControls.SlickIcon();
			this.B_Discover = new SlickControls.SlickButton();
			this.TLP_PlaysetName = new SlickControls.RoundedTableLayoutPanel();
			this.L_CurrentPlayset = new System.Windows.Forms.Label();
			this.B_Edit = new SlickControls.SlickIcon();
			this.DD_Sorting = new Skyve.App.UserInterface.Dropdowns.PlaysetSortingDropDown();
			this.C_ViewTypeControl = new Skyve.App.UserInterface.Generic.ViewTypeControl();
			this.DD_Usage = new Skyve.App.UserInterface.Dropdowns.PackageUsageDropDown();
			this.TLP_Main.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.roundedPanel.SuspendLayout();
			this.TLP_Top.SuspendLayout();
			this.B_AddPlayset.SuspendLayout();
			this.B_DeactivatePlayset.SuspendLayout();
			this.TLP_PlaysetName.SuspendLayout();
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
			this.TLP_Main.Size = new System.Drawing.Size(1190, 645);
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
			this.slickSpacer1.Location = new System.Drawing.Point(0, 64);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(1190, 2);
			this.slickSpacer1.TabIndex = 7;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TB_Search
			// 
			dynamicIcon1.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon1;
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
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 29);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(1190, 35);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// L_Counts
			// 
			this.L_Counts.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.L_Counts.AutoSize = true;
			this.L_Counts.Location = new System.Drawing.Point(993, 11);
			this.L_Counts.Name = "L_Counts";
			this.L_Counts.Size = new System.Drawing.Size(38, 13);
			this.L_Counts.TabIndex = 1;
			this.L_Counts.Text = "label1";
			// 
			// L_FilterCount
			// 
			this.L_FilterCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_FilterCount.AutoSize = true;
			this.L_FilterCount.Location = new System.Drawing.Point(3, 11);
			this.L_FilterCount.Name = "L_FilterCount";
			this.L_FilterCount.Size = new System.Drawing.Size(38, 13);
			this.L_FilterCount.TabIndex = 2;
			this.L_FilterCount.Text = "label1";
			// 
			// panel1
			// 
			this.TLP_Main.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 66);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1190, 579);
			this.panel1.TabIndex = 14;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = null;
			this.slickScroll1.Location = new System.Drawing.Point(1182, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(8, 579);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 0;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// roundedPanel
			// 
			this.roundedPanel.AddOutline = true;
			this.TLP_Top.SetColumnSpan(this.roundedPanel, 5);
			this.roundedPanel.Controls.Add(this.TLP_Main);
			this.roundedPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.roundedPanel.Location = new System.Drawing.Point(3, 41);
			this.roundedPanel.Name = "roundedPanel";
			this.roundedPanel.Size = new System.Drawing.Size(1190, 645);
			this.roundedPanel.TabIndex = 0;
			// 
			// TLP_Top
			// 
			this.TLP_Top.ColumnCount = 5;
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Top.Controls.Add(this.B_AddPlayset, 2, 0);
			this.TLP_Top.Controls.Add(this.B_DeactivatePlayset, 1, 0);
			this.TLP_Top.Controls.Add(this.B_Discover, 4, 0);
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
			this.B_AddPlayset.AutoSize = true;
			this.B_AddPlayset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_AddPlayset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.B_AddPlayset.Controls.Add(this.B_Add, 0, 0);
			this.B_AddPlayset.Location = new System.Drawing.Point(135, 3);
			this.B_AddPlayset.Name = "B_AddPlayset";
			this.B_AddPlayset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.B_AddPlayset.Size = new System.Drawing.Size(44, 31);
			this.B_AddPlayset.TabIndex = 7;
			// 
			// B_Add
			// 
			this.B_Add.ActiveColor = null;
			this.B_Add.ColorStyle = Extensions.ColorStyle.Green;
			this.B_Add.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Add";
			this.B_Add.ImageName = dynamicIcon2;
			this.B_Add.Location = new System.Drawing.Point(0, 0);
			this.B_Add.Margin = new System.Windows.Forms.Padding(0);
			this.B_Add.Name = "B_Add";
			this.B_Add.Padding = new System.Windows.Forms.Padding(5);
			this.B_Add.Size = new System.Drawing.Size(44, 31);
			this.B_Add.TabIndex = 4;
			this.B_Add.TabStop = false;
			this.B_Add.Click += new System.EventHandler(this.B_AddProfile_Click);
			// 
			// B_DeactivatePlayset
			// 
			this.B_DeactivatePlayset.AutoSize = true;
			this.B_DeactivatePlayset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_DeactivatePlayset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.B_DeactivatePlayset.Controls.Add(this.B_Deactivate, 0, 0);
			this.B_DeactivatePlayset.Location = new System.Drawing.Point(85, 3);
			this.B_DeactivatePlayset.Name = "B_DeactivatePlayset";
			this.B_DeactivatePlayset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.B_DeactivatePlayset.Size = new System.Drawing.Size(44, 31);
			this.B_DeactivatePlayset.TabIndex = 6;
			// 
			// B_Deactivate
			// 
			this.B_Deactivate.ActiveColor = null;
			this.B_Deactivate.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Deactivate.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_Cancel";
			this.B_Deactivate.ImageName = dynamicIcon3;
			this.B_Deactivate.Location = new System.Drawing.Point(0, 0);
			this.B_Deactivate.Margin = new System.Windows.Forms.Padding(0);
			this.B_Deactivate.Name = "B_Deactivate";
			this.B_Deactivate.Padding = new System.Windows.Forms.Padding(5);
			this.B_Deactivate.Size = new System.Drawing.Size(44, 31);
			this.B_Deactivate.TabIndex = 4;
			this.B_Deactivate.TabStop = false;
			this.B_Deactivate.Click += new System.EventHandler(this.B_Deactivate_Click);
			// 
			// B_Discover
			// 
			this.B_Discover.AutoSize = true;
			this.B_Discover.ButtonType = SlickControls.ButtonType.Active;
			this.B_Discover.ColorShade = null;
			this.B_Discover.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Discover.Dock = System.Windows.Forms.DockStyle.Right;
			dynamicIcon4.Name = "I_Discover";
			this.B_Discover.ImageName = dynamicIcon4;
			this.B_Discover.LargeImage = true;
			this.B_Discover.Location = new System.Drawing.Point(1068, 3);
			this.B_Discover.Name = "B_Discover";
			this.B_Discover.Size = new System.Drawing.Size(125, 32);
			this.B_Discover.SpaceTriggersClick = true;
			this.B_Discover.TabIndex = 3;
			this.B_Discover.Text = "DiscoverPlaysets";
			this.B_Discover.Click += new System.EventHandler(this.B_Discover_Click);
			// 
			// TLP_PlaysetName
			// 
			this.TLP_PlaysetName.AutoSize = true;
			this.TLP_PlaysetName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_PlaysetName.ColumnCount = 2;
			this.TLP_PlaysetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_PlaysetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_PlaysetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_PlaysetName.Controls.Add(this.L_CurrentPlayset, 0, 0);
			this.TLP_PlaysetName.Controls.Add(this.B_Edit, 1, 0);
			this.TLP_PlaysetName.Location = new System.Drawing.Point(3, 3);
			this.TLP_PlaysetName.Name = "TLP_PlaysetName";
			this.TLP_PlaysetName.RowCount = 1;
			this.TLP_PlaysetName.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_PlaysetName.Size = new System.Drawing.Size(76, 32);
			this.TLP_PlaysetName.TabIndex = 5;
			// 
			// L_CurrentPlayset
			// 
			this.L_CurrentPlayset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_CurrentPlayset.AutoSize = true;
			this.L_CurrentPlayset.Location = new System.Drawing.Point(3, 9);
			this.L_CurrentPlayset.Name = "L_CurrentPlayset";
			this.L_CurrentPlayset.Size = new System.Drawing.Size(38, 13);
			this.L_CurrentPlayset.TabIndex = 1;
			this.L_CurrentPlayset.Text = "label1";
			// 
			// B_Edit
			// 
			this.B_Edit.ActiveColor = null;
			this.B_Edit.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_Edit.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon5.Name = "I_Cog";
			this.B_Edit.ImageName = dynamicIcon5;
			this.B_Edit.Location = new System.Drawing.Point(44, 0);
			this.B_Edit.Margin = new System.Windows.Forms.Padding(0);
			this.B_Edit.Name = "B_Edit";
			this.B_Edit.Padding = new System.Windows.Forms.Padding(5);
			this.B_Edit.Size = new System.Drawing.Size(32, 32);
			this.B_Edit.TabIndex = 3;
			this.B_Edit.TabStop = false;
			this.B_Edit.Click += new System.EventHandler(this.B_Edit_Click);
			// 
			// DD_Sorting
			// 
			this.DD_Sorting.AccentBackColor = true;
			this.DD_Sorting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Sorting.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Sorting.Font = new System.Drawing.Font("Nirmala UI", 15F);
			this.DD_Sorting.HideLabel = true;
			this.DD_Sorting.Location = new System.Drawing.Point(973, 0);
			this.DD_Sorting.Margin = new System.Windows.Forms.Padding(0);
			this.DD_Sorting.Name = "DD_Sorting";
			this.DD_Sorting.Padding = new System.Windows.Forms.Padding(7);
			this.DD_Sorting.Size = new System.Drawing.Size(217, 27);
			this.DD_Sorting.TabIndex = 4;
			this.DD_Sorting.Text = "Sort By";
			this.DD_Sorting.SelectedItemChanged += new System.EventHandler(this.DD_Sorting_SelectedItemChanged);
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
			// DD_Usage
			// 
			this.DD_Usage.AccentBackColor = true;
			this.DD_Usage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Usage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Usage.HideLabel = true;
			this.DD_Usage.Location = new System.Drawing.Point(777, 0);
			this.DD_Usage.Margin = new System.Windows.Forms.Padding(0);
			this.DD_Usage.Name = "DD_Usage";
			this.DD_Usage.Size = new System.Drawing.Size(196, 27);
			this.DD_Usage.TabIndex = 15;
			this.DD_Usage.Text = "PlaysetUsage";
			this.DD_Usage.SelectedItemChanged += new System.EventHandler(this.FilterChanged);
			// 
			// PC_PlaysetList
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Top);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_PlaysetList";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1196, 719);
			this.Text = "Mods";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Top, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.roundedPanel.ResumeLayout(false);
			this.TLP_Top.ResumeLayout(false);
			this.TLP_Top.PerformLayout();
			this.B_AddPlayset.ResumeLayout(false);
			this.B_DeactivatePlayset.ResumeLayout(false);
			this.TLP_PlaysetName.ResumeLayout(false);
			this.TLP_PlaysetName.PerformLayout();
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
	private SlickControls.SlickScroll slickScroll1;
	private System.Windows.Forms.Label L_FilterCount;
	private PackageUsageDropDown DD_Usage;
	private SlickControls.RoundedPanel roundedPanel;
	private System.Windows.Forms.TableLayoutPanel TLP_Top;
	public SlickControls.SlickButton B_Discover;
	private SlickControls.RoundedTableLayoutPanel TLP_PlaysetName;
	private SlickControls.RoundedTableLayoutPanel B_AddPlayset;
	private SlickControls.SlickIcon B_Add;
	private SlickControls.RoundedTableLayoutPanel B_DeactivatePlayset;
	private SlickControls.SlickIcon B_Deactivate;
	private System.Windows.Forms.Label L_CurrentPlayset;
	private SlickIcon B_Edit;
	private Generic.ViewTypeControl C_ViewTypeControl;
}
