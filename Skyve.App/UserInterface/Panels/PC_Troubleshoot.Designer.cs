using Skyve.App.UserInterface.Generic;


namespace Skyve.App.UserInterface.Panels;

partial class PC_Troubleshoot
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
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon6 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon7 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon8 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon9 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon10 = new SlickControls.DynamicIcon();
			this.TLP_New = new System.Windows.Forms.TableLayoutPanel();
			this.B_Caused = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Cancel = new SlickControls.SlickButton();
			this.B_Missing = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_New = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.L_Title = new System.Windows.Forms.Label();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.TLP_ModAsset = new System.Windows.Forms.TableLayoutPanel();
			this.B_Mods = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Cancel2 = new SlickControls.SlickButton();
			this.L_ModAssetTitle = new System.Windows.Forms.Label();
			this.B_Assets = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.TLP_Comp = new System.Windows.Forms.TableLayoutPanel();
			this.B_CompView = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_CompSkip = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Cancel3 = new SlickControls.SlickButton();
			this.L_CompInfo = new System.Windows.Forms.Label();
			this.slickSpacer3 = new SlickControls.SlickSpacer();
			this.TLP_New.SuspendLayout();
			this.TLP_ModAsset.SuspendLayout();
			this.TLP_Comp.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(193, 39);
			this.base_Text.Text = "TroubleshootIssues";
			// 
			// TLP_New
			// 
			this.TLP_New.ColumnCount = 5;
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.Controls.Add(this.B_Caused, 1, 4);
			this.TLP_New.Controls.Add(this.B_Cancel, 4, 5);
			this.TLP_New.Controls.Add(this.B_Missing, 2, 4);
			this.TLP_New.Controls.Add(this.B_New, 3, 4);
			this.TLP_New.Controls.Add(this.L_Title, 0, 1);
			this.TLP_New.Controls.Add(this.slickSpacer1, 1, 2);
			this.TLP_New.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_New.Location = new System.Drawing.Point(0, 30);
			this.TLP_New.Name = "TLP_New";
			this.TLP_New.RowCount = 6;
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.TLP_New.Size = new System.Drawing.Size(1182, 789);
			this.TLP_New.TabIndex = 0;
			// 
			// B_Caused
			// 
			this.B_Caused.ButtonText = "Select";
			this.B_Caused.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Caused.Highlighted = true;
			dynamicIcon1.Name = "Wrench";
			this.B_Caused.ImageName = dynamicIcon1;
			this.B_Caused.Location = new System.Drawing.Point(360, 204);
			this.B_Caused.Name = "B_Caused";
			this.B_Caused.Size = new System.Drawing.Size(150, 317);
			this.B_Caused.TabIndex = 0;
			this.B_Caused.Text = "TroubleshootCaused";
			this.B_Caused.Title = "GenericIssue";
			this.B_Caused.Click += new System.EventHandler(this.B_Caused_Click);
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Cancel.AutoSize = true;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "Disposable";
			this.B_Cancel.ImageName = dynamicIcon2;
			this.B_Cancel.Location = new System.Drawing.Point(1097, 757);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Size = new System.Drawing.Size(85, 32);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 3;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// B_Missing
			// 
			this.B_Missing.ButtonText = "Select";
			this.B_Missing.ColorStyle = Extensions.ColorStyle.Yellow;
			this.B_Missing.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "MissingMod";
			this.B_Missing.ImageName = dynamicIcon3;
			this.B_Missing.Location = new System.Drawing.Point(516, 204);
			this.B_Missing.Name = "B_Missing";
			this.B_Missing.Size = new System.Drawing.Size(150, 317);
			this.B_Missing.TabIndex = 1;
			this.B_Missing.Text = "TroubleshootMissing";
			this.B_Missing.Title = "MissingPackage";
			this.B_Missing.Click += new System.EventHandler(this.B_Missing_Click);
			// 
			// B_New
			// 
			this.B_New.ButtonText = "Select";
			this.B_New.ColorStyle = Extensions.ColorStyle.Orange;
			this.B_New.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "OutOfDate";
			this.B_New.ImageName = dynamicIcon4;
			this.B_New.Location = new System.Drawing.Point(672, 204);
			this.B_New.Name = "B_New";
			this.B_New.Size = new System.Drawing.Size(150, 317);
			this.B_New.TabIndex = 2;
			this.B_New.Text = "TroubleshootNew";
			this.B_New.Title = "BrokenUpdate";
			this.B_New.Click += new System.EventHandler(this.B_New_Click);
			// 
			// L_Title
			// 
			this.L_Title.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_Title.AutoSize = true;
			this.TLP_New.SetColumnSpan(this.L_Title, 5);
			this.L_Title.Location = new System.Drawing.Point(568, 87);
			this.L_Title.Name = "L_Title";
			this.L_Title.Size = new System.Drawing.Size(45, 19);
			this.L_Title.TabIndex = 15;
			this.L_Title.Text = "label1";
			this.L_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// slickSpacer1
			// 
			this.TLP_New.SetColumnSpan(this.slickSpacer1, 3);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(360, 109);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(462, 2);
			this.slickSpacer1.TabIndex = 17;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TLP_ModAsset
			// 
			this.TLP_ModAsset.ColumnCount = 4;
			this.TLP_ModAsset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_ModAsset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ModAsset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ModAsset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_ModAsset.Controls.Add(this.B_Mods, 1, 4);
			this.TLP_ModAsset.Controls.Add(this.B_Cancel2, 3, 5);
			this.TLP_ModAsset.Controls.Add(this.L_ModAssetTitle, 0, 1);
			this.TLP_ModAsset.Controls.Add(this.B_Assets, 2, 4);
			this.TLP_ModAsset.Controls.Add(this.slickSpacer2, 1, 2);
			this.TLP_ModAsset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_ModAsset.Location = new System.Drawing.Point(0, 30);
			this.TLP_ModAsset.Name = "TLP_ModAsset";
			this.TLP_ModAsset.RowCount = 6;
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.TLP_ModAsset.Size = new System.Drawing.Size(1182, 789);
			this.TLP_ModAsset.TabIndex = 2;
			this.TLP_ModAsset.Visible = false;
			// 
			// B_Mods
			// 
			this.B_Mods.ButtonText = "Select";
			this.B_Mods.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Mods.Highlighted = true;
			dynamicIcon5.Name = "Mods";
			this.B_Mods.ImageName = dynamicIcon5;
			this.B_Mods.Location = new System.Drawing.Point(438, 232);
			this.B_Mods.Name = "B_Mods";
			this.B_Mods.Size = new System.Drawing.Size(150, 247);
			this.B_Mods.TabIndex = 0;
			this.B_Mods.Text = "InvestigateMods";
			this.B_Mods.Title = "Mods";
			this.B_Mods.Click += new System.EventHandler(this.B_Mods_Click);
			// 
			// B_Cancel2
			// 
			this.B_Cancel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Cancel2.AutoSize = true;
			this.B_Cancel2.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel2.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon6.Name = "Disposable";
			this.B_Cancel2.ImageName = dynamicIcon6;
			this.B_Cancel2.Location = new System.Drawing.Point(1097, 757);
			this.B_Cancel2.Margin = new System.Windows.Forms.Padding(0);
			this.B_Cancel2.Name = "B_Cancel2";
			this.B_Cancel2.Size = new System.Drawing.Size(85, 32);
			this.B_Cancel2.SpaceTriggersClick = true;
			this.B_Cancel2.TabIndex = 2;
			this.B_Cancel2.Text = "Cancel";
			this.B_Cancel2.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// L_ModAssetTitle
			// 
			this.L_ModAssetTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_ModAssetTitle.AutoSize = true;
			this.TLP_ModAsset.SetColumnSpan(this.L_ModAssetTitle, 4);
			this.L_ModAssetTitle.Location = new System.Drawing.Point(568, 101);
			this.L_ModAssetTitle.Name = "L_ModAssetTitle";
			this.L_ModAssetTitle.Size = new System.Drawing.Size(45, 19);
			this.L_ModAssetTitle.TabIndex = 15;
			this.L_ModAssetTitle.Text = "label1";
			this.L_ModAssetTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// B_Assets
			// 
			this.B_Assets.ButtonText = "Select";
			this.B_Assets.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon7.Name = "Assets";
			this.B_Assets.ImageName = dynamicIcon7;
			this.B_Assets.Location = new System.Drawing.Point(594, 232);
			this.B_Assets.Name = "B_Assets";
			this.B_Assets.Size = new System.Drawing.Size(150, 247);
			this.B_Assets.TabIndex = 1;
			this.B_Assets.Text = "InvestigateAssets";
			this.B_Assets.Title = "Assets";
			this.B_Assets.Click += new System.EventHandler(this.B_Assets_Click);
			// 
			// slickSpacer2
			// 
			this.TLP_ModAsset.SetColumnSpan(this.slickSpacer2, 2);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(438, 123);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(306, 2);
			this.slickSpacer2.TabIndex = 16;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// TLP_Comp
			// 
			this.TLP_Comp.ColumnCount = 4;
			this.TLP_Comp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Comp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Comp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Comp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Comp.Controls.Add(this.B_CompView, 1, 4);
			this.TLP_Comp.Controls.Add(this.B_CompSkip, 2, 4);
			this.TLP_Comp.Controls.Add(this.B_Cancel3, 3, 5);
			this.TLP_Comp.Controls.Add(this.L_CompInfo, 0, 1);
			this.TLP_Comp.Controls.Add(this.slickSpacer3, 1, 2);
			this.TLP_Comp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Comp.Location = new System.Drawing.Point(0, 30);
			this.TLP_Comp.Name = "TLP_Comp";
			this.TLP_Comp.RowCount = 6;
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Comp.Size = new System.Drawing.Size(1182, 789);
			this.TLP_Comp.TabIndex = 3;
			this.TLP_Comp.Visible = false;
			// 
			// B_CompView
			// 
			this.B_CompView.ButtonText = "Select";
			this.B_CompView.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CompView.Highlighted = true;
			dynamicIcon8.Name = "CompatibilityReport";
			this.B_CompView.ImageName = dynamicIcon8;
			this.B_CompView.Location = new System.Drawing.Point(438, 226);
			this.B_CompView.Name = "B_CompView";
			this.B_CompView.Size = new System.Drawing.Size(150, 265);
			this.B_CompView.TabIndex = 0;
			this.B_CompView.Text = "TroubleshootViewComp";
			this.B_CompView.Title = "Cancel";
			this.B_CompView.Click += new System.EventHandler(this.B_CompView_Click);
			// 
			// B_CompSkip
			// 
			this.B_CompSkip.ButtonText = "Select";
			this.B_CompSkip.ColorStyle = Extensions.ColorStyle.Red;
			this.B_CompSkip.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon9.Name = "Skip";
			this.B_CompSkip.ImageName = dynamicIcon9;
			this.B_CompSkip.Location = new System.Drawing.Point(594, 226);
			this.B_CompSkip.Name = "B_CompSkip";
			this.B_CompSkip.Size = new System.Drawing.Size(150, 265);
			this.B_CompSkip.TabIndex = 1;
			this.B_CompSkip.Text = "TroubleshootSkipComp";
			this.B_CompSkip.Title = "Ignore";
			this.B_CompSkip.Click += new System.EventHandler(this.B_CompSkip_Click);
			// 
			// B_Cancel3
			// 
			this.B_Cancel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Cancel3.AutoSize = true;
			this.B_Cancel3.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel3.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon10.Name = "Disposable";
			this.B_Cancel3.ImageName = dynamicIcon10;
			this.B_Cancel3.Location = new System.Drawing.Point(1097, 757);
			this.B_Cancel3.Margin = new System.Windows.Forms.Padding(0);
			this.B_Cancel3.Name = "B_Cancel3";
			this.B_Cancel3.Size = new System.Drawing.Size(85, 32);
			this.B_Cancel3.SpaceTriggersClick = true;
			this.B_Cancel3.TabIndex = 2;
			this.B_Cancel3.Text = "Cancel";
			this.B_Cancel3.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// L_CompInfo
			// 
			this.L_CompInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_CompInfo.AutoSize = true;
			this.TLP_Comp.SetColumnSpan(this.L_CompInfo, 4);
			this.L_CompInfo.Location = new System.Drawing.Point(568, 98);
			this.L_CompInfo.Name = "L_CompInfo";
			this.L_CompInfo.Size = new System.Drawing.Size(45, 19);
			this.L_CompInfo.TabIndex = 15;
			this.L_CompInfo.Text = "label1";
			this.L_CompInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// slickSpacer3
			// 
			this.TLP_Comp.SetColumnSpan(this.slickSpacer3, 2);
			this.slickSpacer3.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer3.Location = new System.Drawing.Point(438, 120);
			this.slickSpacer3.Name = "slickSpacer3";
			this.slickSpacer3.Size = new System.Drawing.Size(306, 2);
			this.slickSpacer3.TabIndex = 16;
			this.slickSpacer3.TabStop = false;
			this.slickSpacer3.Text = "slickSpacer3";
			// 
			// PC_Troubleshoot
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_New);
			this.Controls.Add(this.TLP_ModAsset);
			this.Controls.Add(this.TLP_Comp);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_Troubleshoot";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1182, 819);
			this.Text = "TroubleshootIssues";
			this.Controls.SetChildIndex(this.TLP_Comp, 0);
			this.Controls.SetChildIndex(this.TLP_ModAsset, 0);
			this.Controls.SetChildIndex(this.TLP_New, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.TLP_New.ResumeLayout(false);
			this.TLP_New.PerformLayout();
			this.TLP_ModAsset.ResumeLayout(false);
			this.TLP_ModAsset.PerformLayout();
			this.TLP_Comp.ResumeLayout(false);
			this.TLP_Comp.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_New;
	private SlickControls.SlickButton B_Cancel;
	private BigSelectionOptionControl B_Caused;
	private BigSelectionOptionControl B_Missing;
	private System.Windows.Forms.TableLayoutPanel TLP_ModAsset;
	private BigSelectionOptionControl B_Mods;
	private BigSelectionOptionControl B_Assets;
	private SlickButton B_Cancel2;
	private System.Windows.Forms.Label L_ModAssetTitle;
	private BigSelectionOptionControl B_New;
	private System.Windows.Forms.TableLayoutPanel TLP_Comp;
	private SlickButton B_Cancel3;
	private System.Windows.Forms.Label L_CompInfo;
	private BigSelectionOptionControl B_CompView;
	private BigSelectionOptionControl B_CompSkip;
	private System.Windows.Forms.Label L_Title;
	private SlickSpacer slickSpacer1;
	private SlickSpacer slickSpacer2;
	private SlickSpacer slickSpacer3;
}
