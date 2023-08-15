namespace SkyveApp.UserInterface.Panels;

partial class PC_SelectPackage
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
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.OT_ModAsset = new SkyveApp.UserInterface.Generic.ThreeOptionToggle();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.L_Selected = new System.Windows.Forms.Label();
			this.B_Continue = new SlickControls.SlickButton();
			this.L_Totals = new System.Windows.Forms.Label();
			this.FLP_Packages = new SlickControls.SmartFlowPanel();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.OT_ModAsset, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.TB_Search, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(920, 100);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// slickSpacer2
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.slickSpacer2, 2);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(0, 32);
			this.slickSpacer2.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(920, 1);
			this.slickSpacer2.TabIndex = 12;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// OT_ModAsset
			// 
			this.OT_ModAsset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OT_ModAsset.Cursor = System.Windows.Forms.Cursors.Hand;
			this.OT_ModAsset.Image1 = "I_Mods";
			this.OT_ModAsset.Image2 = "I_Assets";
			this.OT_ModAsset.Location = new System.Drawing.Point(544, 3);
			this.OT_ModAsset.Name = "OT_ModAsset";
			this.OT_ModAsset.Option1 = "Mods";
			this.OT_ModAsset.Option2 = "Assets";
			this.OT_ModAsset.OptionStyle1 = Extensions.ColorStyle.Active;
			this.OT_ModAsset.OptionStyle2 = Extensions.ColorStyle.Active;
			this.OT_ModAsset.Size = new System.Drawing.Size(373, 26);
			this.OT_ModAsset.TabIndex = 1;
			this.OT_ModAsset.SelectedValueChanged += new System.EventHandler(this.TB_Search_TextChanged);
			// 
			// TB_Search
			// 
			dynamicIcon1.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon1;
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(372, 26);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// slickSpacer1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.slickSpacer1, 2);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 99);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(920, 1);
			this.slickSpacer1.TabIndex = 1;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.L_Selected, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.B_Continue, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.L_Totals, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.FLP_Packages, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 33);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(920, 66);
			this.tableLayoutPanel2.TabIndex = 13;
			// 
			// L_Selected
			// 
			this.L_Selected.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Selected.AutoSize = true;
			this.L_Selected.Location = new System.Drawing.Point(3, 0);
			this.L_Selected.Name = "L_Selected";
			this.L_Selected.Size = new System.Drawing.Size(71, 30);
			this.L_Selected.TabIndex = 0;
			this.L_Selected.Text = "label1";
			// 
			// B_Continue
			// 
			this.B_Continue.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Continue.ColorShade = null;
			this.B_Continue.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Ok";
			this.B_Continue.ImageName = dynamicIcon2;
			this.B_Continue.Location = new System.Drawing.Point(817, 33);
			this.B_Continue.Name = "B_Continue";
			this.B_Continue.Size = new System.Drawing.Size(100, 30);
			this.B_Continue.SpaceTriggersClick = true;
			this.B_Continue.TabIndex = 2;
			this.B_Continue.Text = "Continue";
			this.B_Continue.Visible = false;
			this.B_Continue.Click += new System.EventHandler(this.B_Continue_Click);
			// 
			// L_Totals
			// 
			this.L_Totals.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.L_Totals.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.L_Totals, 2);
			this.L_Totals.Location = new System.Drawing.Point(917, 0);
			this.L_Totals.Name = "L_Totals";
			this.L_Totals.Size = new System.Drawing.Size(0, 30);
			this.L_Totals.TabIndex = 0;
			// 
			// FLP_Packages
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.FLP_Packages, 2);
			this.FLP_Packages.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Packages.Location = new System.Drawing.Point(0, 30);
			this.FLP_Packages.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Packages.Name = "FLP_Packages";
			this.FLP_Packages.Size = new System.Drawing.Size(814, 0);
			this.FLP_Packages.TabIndex = 2;
			// 
			// PC_SelectPackage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "PC_SelectPackage";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(920, 460);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private Generic.ThreeOptionToggle OT_ModAsset;
	private SlickControls.SlickTextBox TB_Search;
	private SlickControls.SlickSpacer slickSpacer1;
	private SlickControls.SlickSpacer slickSpacer2;
	private System.Windows.Forms.Label L_Selected;
	private SlickControls.SlickButton B_Continue;
	private System.Windows.Forms.Label L_Totals;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
	private SlickControls.SmartFlowPanel FLP_Packages;
}
