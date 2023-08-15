using SkyveApp.UserInterface.Generic;


namespace SkyveApp.UserInterface.Panels;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PC_Troubleshoot));
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon6 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon7 = new SlickControls.DynamicIcon();
			this.TLP_New = new System.Windows.Forms.TableLayoutPanel();
			this.B_Caused = new SkyveApp.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Missing = new SkyveApp.UserInterface.Generic.BigSelectionOptionControl();
			this.B_New = new SkyveApp.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Cancel = new SlickControls.SlickButton();
			this.L_Title = new System.Windows.Forms.Label();
			this.TLP_ModAsset = new System.Windows.Forms.TableLayoutPanel();
			this.B_Mods = new SkyveApp.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Assets = new SkyveApp.UserInterface.Generic.BigSelectionOptionControl();
			this.slickButton1 = new SlickControls.SlickButton();
			this.L_ModAssetTitle = new System.Windows.Forms.Label();
			this.TLP_Comp = new System.Windows.Forms.TableLayoutPanel();
			this.B_CompView = new SkyveApp.UserInterface.Generic.BigSelectionOptionControl();
			this.B_CompSkip = new SkyveApp.UserInterface.Generic.BigSelectionOptionControl();
			this.slickButton2 = new SlickControls.SlickButton();
			this.L_CompInfo = new System.Windows.Forms.Label();
			this.TLP_New.SuspendLayout();
			this.TLP_ModAsset.SuspendLayout();
			this.TLP_Comp.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Text = "TroubleshootIssues";
			// 
			// TLP_New
			// 
			this.TLP_New.ColumnCount = 3;
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.Controls.Add(this.B_Caused, 1, 1);
			this.TLP_New.Controls.Add(this.B_Missing, 1, 2);
			this.TLP_New.Controls.Add(this.B_New, 1, 3);
			this.TLP_New.Controls.Add(this.B_Cancel, 2, 5);
			this.TLP_New.Controls.Add(this.L_Title, 0, 0);
			this.TLP_New.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_New.Location = new System.Drawing.Point(0, 30);
			this.TLP_New.Name = "TLP_New";
			this.TLP_New.RowCount = 6;
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this.TLP_New.Size = new System.Drawing.Size(1182, 789);
			this.TLP_New.TabIndex = 0;
			// 
			// B_Caused
			// 
			this.B_Caused.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Caused.FromScratch = false;
			dynamicIcon1.Name = "I_Wrench";
			this.B_Caused.ImageName = dynamicIcon1;
			this.B_Caused.Location = new System.Drawing.Point(516, 115);
			this.B_Caused.Name = "B_Caused";
			this.B_Caused.Size = new System.Drawing.Size(150, 150);
			this.B_Caused.TabIndex = 4;
			this.B_Caused.Text = "TroubleshootCaused";
			this.B_Caused.Click += new System.EventHandler(this.B_Caused_Click);
			// 
			// B_Missing
			// 
			this.B_Missing.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Missing.FromScratch = false;
			dynamicIcon2.Name = "I_Search";
			this.B_Missing.ImageName = dynamicIcon2;
			this.B_Missing.Location = new System.Drawing.Point(516, 271);
			this.B_Missing.Name = "B_Missing";
			this.B_Missing.Size = new System.Drawing.Size(150, 150);
			this.B_Missing.TabIndex = 5;
			this.B_Missing.Text = "TroubleshootMissing";
			this.B_Missing.Click += new System.EventHandler(this.B_Missing_Click);
			// 
			// B_New
			// 
			this.B_New.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_New.FromScratch = false;
			dynamicIcon3.Name = "I_UpdateTime";
			this.B_New.ImageName = dynamicIcon3;
			this.B_New.Location = new System.Drawing.Point(516, 427);
			this.B_New.Name = "B_New";
			this.B_New.Size = new System.Drawing.Size(150, 150);
			this.B_New.TabIndex = 16;
			this.B_New.Text = "TroubleshootNew";
			this.B_New.Click += new System.EventHandler(this.B_New_Click);
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Cancel.AutoSize = true;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("B_Cancel.Image")));
			this.B_Cancel.Location = new System.Drawing.Point(1038, 733);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(10);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_Cancel.Size = new System.Drawing.Size(134, 46);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 14;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// L_Title
			// 
			this.L_Title.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_Title.AutoSize = true;
			this.TLP_New.SetColumnSpan(this.L_Title, 3);
			this.L_Title.Location = new System.Drawing.Point(557, 41);
			this.L_Title.Name = "L_Title";
			this.L_Title.Size = new System.Drawing.Size(68, 30);
			this.L_Title.TabIndex = 15;
			this.L_Title.Text = "label1";
			// 
			// TLP_ModAsset
			// 
			this.TLP_ModAsset.ColumnCount = 3;
			this.TLP_ModAsset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_ModAsset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_ModAsset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_ModAsset.Controls.Add(this.B_Mods, 1, 1);
			this.TLP_ModAsset.Controls.Add(this.B_Assets, 1, 2);
			this.TLP_ModAsset.Controls.Add(this.slickButton1, 2, 4);
			this.TLP_ModAsset.Controls.Add(this.L_ModAssetTitle, 0, 0);
			this.TLP_ModAsset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_ModAsset.Location = new System.Drawing.Point(0, 30);
			this.TLP_ModAsset.Name = "TLP_ModAsset";
			this.TLP_ModAsset.RowCount = 5;
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this.TLP_ModAsset.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_ModAsset.Size = new System.Drawing.Size(1182, 789);
			this.TLP_ModAsset.TabIndex = 2;
			this.TLP_ModAsset.Visible = false;
			// 
			// B_Mods
			// 
			this.B_Mods.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Mods.FromScratch = false;
			dynamicIcon4.Name = "I_Mods";
			this.B_Mods.ImageName = dynamicIcon4;
			this.B_Mods.Location = new System.Drawing.Point(516, 169);
			this.B_Mods.Name = "B_Mods";
			this.B_Mods.Size = new System.Drawing.Size(150, 150);
			this.B_Mods.TabIndex = 4;
			this.B_Mods.Text = "Mods";
			this.B_Mods.Click += new System.EventHandler(this.B_Mods_Click);
			// 
			// B_Assets
			// 
			this.B_Assets.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Assets.FromScratch = false;
			dynamicIcon5.Name = "I_Assets";
			this.B_Assets.ImageName = dynamicIcon5;
			this.B_Assets.Location = new System.Drawing.Point(516, 325);
			this.B_Assets.Name = "B_Assets";
			this.B_Assets.Size = new System.Drawing.Size(150, 150);
			this.B_Assets.TabIndex = 5;
			this.B_Assets.Text = "TroubleshootMissing";
			this.B_Assets.Click += new System.EventHandler(this.B_Assets_Click);
			// 
			// slickButton1
			// 
			this.slickButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.slickButton1.AutoSize = true;
			this.slickButton1.ColorShade = null;
			this.slickButton1.ColorStyle = Extensions.ColorStyle.Red;
			this.slickButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.slickButton1.Image = ((System.Drawing.Image)(resources.GetObject("slickButton1.Image")));
			this.slickButton1.Location = new System.Drawing.Point(1038, 733);
			this.slickButton1.Margin = new System.Windows.Forms.Padding(10);
			this.slickButton1.Name = "slickButton1";
			this.slickButton1.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.slickButton1.Size = new System.Drawing.Size(134, 46);
			this.slickButton1.SpaceTriggersClick = true;
			this.slickButton1.TabIndex = 14;
			this.slickButton1.Text = "Cancel";
			this.slickButton1.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// L_ModAssetTitle
			// 
			this.L_ModAssetTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_ModAssetTitle.AutoSize = true;
			this.TLP_ModAsset.SetColumnSpan(this.L_ModAssetTitle, 3);
			this.L_ModAssetTitle.Location = new System.Drawing.Point(557, 68);
			this.L_ModAssetTitle.Name = "L_ModAssetTitle";
			this.L_ModAssetTitle.Size = new System.Drawing.Size(68, 30);
			this.L_ModAssetTitle.TabIndex = 15;
			this.L_ModAssetTitle.Text = "label1";
			// 
			// TLP_Comp
			// 
			this.TLP_Comp.ColumnCount = 3;
			this.TLP_Comp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Comp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Comp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Comp.Controls.Add(this.B_CompView, 1, 1);
			this.TLP_Comp.Controls.Add(this.B_CompSkip, 1, 2);
			this.TLP_Comp.Controls.Add(this.slickButton2, 2, 5);
			this.TLP_Comp.Controls.Add(this.L_CompInfo, 0, 0);
			this.TLP_Comp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Comp.Location = new System.Drawing.Point(0, 30);
			this.TLP_Comp.Name = "TLP_Comp";
			this.TLP_Comp.RowCount = 6;
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Comp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this.TLP_Comp.Size = new System.Drawing.Size(1182, 789);
			this.TLP_Comp.TabIndex = 3;
			this.TLP_Comp.Visible = false;
			// 
			// B_CompView
			// 
			this.B_CompView.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CompView.FromScratch = false;
			dynamicIcon6.Name = "I_CompatibilityReport";
			this.B_CompView.ImageName = dynamicIcon6;
			this.B_CompView.Location = new System.Drawing.Point(516, 169);
			this.B_CompView.Name = "B_CompView";
			this.B_CompView.Size = new System.Drawing.Size(150, 150);
			this.B_CompView.TabIndex = 4;
			this.B_CompView.Text = "TroubleshootViewComp";
			this.B_CompView.Click += new System.EventHandler(this.B_CompView_Click);
			// 
			// B_CompSkip
			// 
			this.B_CompSkip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CompSkip.FromScratch = false;
			dynamicIcon7.Name = "I_Skip";
			this.B_CompSkip.ImageName = dynamicIcon7;
			this.B_CompSkip.Location = new System.Drawing.Point(516, 325);
			this.B_CompSkip.Name = "B_CompSkip";
			this.B_CompSkip.Size = new System.Drawing.Size(150, 150);
			this.B_CompSkip.TabIndex = 5;
			this.B_CompSkip.Text = "TroubleshootSkipComp";
			this.B_CompSkip.Click += new System.EventHandler(this.B_CompSkip_Click);
			// 
			// slickButton2
			// 
			this.slickButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.slickButton2.AutoSize = true;
			this.slickButton2.ColorShade = null;
			this.slickButton2.ColorStyle = Extensions.ColorStyle.Red;
			this.slickButton2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.slickButton2.Image = ((System.Drawing.Image)(resources.GetObject("slickButton2.Image")));
			this.slickButton2.Location = new System.Drawing.Point(1038, 733);
			this.slickButton2.Margin = new System.Windows.Forms.Padding(10);
			this.slickButton2.Name = "slickButton2";
			this.slickButton2.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.slickButton2.Size = new System.Drawing.Size(134, 46);
			this.slickButton2.SpaceTriggersClick = true;
			this.slickButton2.TabIndex = 14;
			this.slickButton2.Text = "Cancel";
			this.slickButton2.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// L_CompInfo
			// 
			this.L_CompInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_CompInfo.AutoSize = true;
			this.TLP_Comp.SetColumnSpan(this.L_CompInfo, 3);
			this.L_CompInfo.Location = new System.Drawing.Point(557, 68);
			this.L_CompInfo.Name = "L_CompInfo";
			this.L_CompInfo.Size = new System.Drawing.Size(68, 30);
			this.L_CompInfo.TabIndex = 15;
			this.L_CompInfo.Text = "label1";
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
	private System.Windows.Forms.Label L_Title;
	private System.Windows.Forms.TableLayoutPanel TLP_ModAsset;
	private BigSelectionOptionControl B_Mods;
	private BigSelectionOptionControl B_Assets;
	private SlickButton slickButton1;
	private System.Windows.Forms.Label L_ModAssetTitle;
	private BigSelectionOptionControl B_New;
	private System.Windows.Forms.TableLayoutPanel TLP_Comp;
	private SlickButton slickButton2;
	private System.Windows.Forms.Label L_CompInfo;
	private BigSelectionOptionControl B_CompView;
	private BigSelectionOptionControl B_CompSkip;
}
