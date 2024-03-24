namespace Skyve.App.UserInterface.Panels;

partial class PC_WorkshopPackageSelection
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
			this.base_P_Side = new System.Windows.Forms.Panel();
			this.base_TLP_Side = new SlickControls.RoundedTableLayoutPanel();
			this.B_Confirm = new SlickControls.SlickButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.P_Packages = new SlickControls.SmartPanel();
			this.L_SelectedPackages = new System.Windows.Forms.Label();
			this.L_Info = new System.Windows.Forms.Label();
			this.base_P_Side.SuspendLayout();
			this.base_TLP_Side.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Location = new System.Drawing.Point(-2, -27);
			this.base_Text.Size = new System.Drawing.Size(150, 41);
			// 
			// base_P_Side
			// 
			this.base_P_Side.Controls.Add(this.base_TLP_Side);
			this.base_P_Side.Dock = System.Windows.Forms.DockStyle.Left;
			this.base_P_Side.Location = new System.Drawing.Point(0, 0);
			this.base_P_Side.Name = "base_P_Side";
			this.base_P_Side.Size = new System.Drawing.Size(165, 438);
			this.base_P_Side.TabIndex = 18;
			// 
			// base_TLP_Side
			// 
			this.base_TLP_Side.BotLeft = true;
			this.base_TLP_Side.ColumnCount = 1;
			this.base_TLP_Side.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.base_TLP_Side.Controls.Add(this.B_Confirm, 0, 3);
			this.base_TLP_Side.Controls.Add(this.panel1, 0, 2);
			this.base_TLP_Side.Controls.Add(this.L_SelectedPackages, 0, 0);
			this.base_TLP_Side.Controls.Add(this.L_Info, 0, 1);
			this.base_TLP_Side.Dock = System.Windows.Forms.DockStyle.Fill;
			this.base_TLP_Side.Location = new System.Drawing.Point(0, 0);
			this.base_TLP_Side.Name = "base_TLP_Side";
			this.base_TLP_Side.RowCount = 4;
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F));
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.base_TLP_Side.Size = new System.Drawing.Size(165, 438);
			this.base_TLP_Side.TabIndex = 43;
			this.base_TLP_Side.TopLeft = true;
			// 
			// B_Confirm
			// 
			this.B_Confirm.Dock = System.Windows.Forms.DockStyle.Top;
			this.B_Confirm.AutoSize = true;
			this.B_Confirm.ButtonType = ButtonType.Active;
			this.B_Confirm.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "Ok";
			this.B_Confirm.ImageName = dynamicIcon1;
			this.B_Confirm.Location = new System.Drawing.Point(9, 405);
			this.B_Confirm.Name = "B_Confirm";
			this.B_Confirm.Size = new System.Drawing.Size(147, 30);
			this.B_Confirm.SpaceTriggersClick = true;
			this.B_Confirm.TabIndex = 0;
			this.B_Confirm.Text = "ConfirmSelection";
			this.B_Confirm.Click += new System.EventHandler(this.B_Confirm_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Controls.Add(this.P_Packages);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 42);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(165, 360);
			this.panel1.TabIndex = 1;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.P_Packages;
			this.slickScroll1.Location = new System.Drawing.Point(157, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(8, 360);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 1;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// P_Packages
			// 
			this.P_Packages.Location = new System.Drawing.Point(69, 65);
			this.P_Packages.Name = "P_Packages";
			this.P_Packages.Size = new System.Drawing.Size(200, 0);
			this.P_Packages.TabIndex = 0;
			// 
			// L_SelectedPackages
			// 
			this.L_SelectedPackages.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.L_SelectedPackages.AutoSize = true;
			this.L_SelectedPackages.Location = new System.Drawing.Point(60, 0);
			this.L_SelectedPackages.Name = "L_SelectedPackages";
			this.L_SelectedPackages.Size = new System.Drawing.Size(45, 19);
			this.L_SelectedPackages.TabIndex = 2;
			this.L_SelectedPackages.Text = "label1";
			// 
			// L_Info
			// 
			this.L_Info.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_Info.TextAlign	= System.Drawing.ContentAlignment.MiddleCenter;
			this.L_Info.AutoSize = true;
			this.L_Info.Location = new System.Drawing.Point(60, 19);
			this.L_Info.Name = "L_Info";
			this.L_Info.Size = new System.Drawing.Size(45, 19);
			this.L_Info.TabIndex = 2;
			this.L_Info.Text = "label1";
			// 
			// PC_WorkshopPackageSelection
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.base_P_Side);
			this.Name = "PC_WorkshopPackageSelection";
			this.Padding = new System.Windows.Forms.Padding(0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.base_P_Side, 0);
			this.base_P_Side.ResumeLayout(false);
			this.base_TLP_Side.ResumeLayout(false);
			this.base_TLP_Side.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.Panel base_P_Side;
	internal RoundedTableLayoutPanel base_TLP_Side;
	private SlickButton B_Confirm;
	private System.Windows.Forms.Panel panel1;
	private SlickScroll slickScroll1;
	private SmartPanel P_Packages;
	private System.Windows.Forms.Label L_SelectedPackages;
	private System.Windows.Forms.Label L_Info;
}
