using Skyve.App.UserInterface.Content;

namespace Skyve.App.UserInterface.Panels;

partial class PC_UserPage
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
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			this.P_Content = new System.Windows.Forms.Panel();
			this.tabControl = new SlickControls.SlickTabControl();
			this.T_Profiles = new SlickControls.SlickTabControl.Tab();
			this.T_Packages = new SlickControls.SlickTabControl.Tab();
			this.smartFlowPanel1 = new SlickControls.SmartFlowPanel();
			this.roundedGroupPanel1 = new SlickControls.RoundedGroupPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.TLP_Top = new System.Windows.Forms.TableLayoutPanel();
			this.PB_Icon = new Skyve.App.UserInterface.Content.UserIcon();
			this.P_Back = new System.Windows.Forms.Panel();
			this.P_Info = new Skyve.App.UserInterface.Content.UserDescriptionControl();
			this.P_Content.SuspendLayout();
			this.smartFlowPanel1.SuspendLayout();
			this.roundedGroupPanel1.SuspendLayout();
			this.TLP_Top.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 39);
			this.base_Text.Text = "Back";
			// 
			// P_Content
			// 
			this.P_Content.Controls.Add(this.tabControl);
			this.P_Content.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Content.Location = new System.Drawing.Point(0, 130);
			this.P_Content.Name = "P_Content";
			this.P_Content.Size = new System.Drawing.Size(916, 387);
			this.P_Content.TabIndex = 13;
			// 
			// tabControl
			// 
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl.Name = "tabControl";
			this.tabControl.Size = new System.Drawing.Size(916, 387);
			this.tabControl.TabIndex = 0;
			this.tabControl.Tabs = new SlickControls.SlickTabControl.Tab[] {
        this.T_Packages,
        this.T_Profiles};
			// 
			// T_Profiles
			// 
			this.T_Profiles.Cursor = System.Windows.Forms.Cursors.Hand;
			this.T_Profiles.Dock = System.Windows.Forms.DockStyle.Left;
			this.T_Profiles.FillTab = true;
			this.T_Profiles.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dynamicIcon2.Name = "PlaysetSettings";
			this.T_Profiles.IconName = dynamicIcon2;
			this.T_Profiles.LinkedControl = null;
			this.T_Profiles.Location = new System.Drawing.Point(156, 5);
			this.T_Profiles.Name = "T_Profiles";
			this.T_Profiles.Size = new System.Drawing.Size(156, 75);
			this.T_Profiles.TabIndex = 0;
			this.T_Profiles.TabStop = false;
			this.T_Profiles.Text = "Profiles";
			this.T_Profiles.Visible = false;
			// 
			// T_Packages
			// 
			this.T_Packages.Cursor = System.Windows.Forms.Cursors.Hand;
			this.T_Packages.Dock = System.Windows.Forms.DockStyle.Left;
			this.T_Packages.FillTab = true;
			this.T_Packages.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dynamicIcon1.Name = "Package";
			this.T_Packages.IconName = dynamicIcon1;
			this.T_Packages.LinkedControl = null;
			this.T_Packages.Location = new System.Drawing.Point(0, 5);
			this.T_Packages.Name = "T_Packages";
			this.T_Packages.Size = new System.Drawing.Size(156, 75);
			this.T_Packages.TabIndex = 0;
			this.T_Packages.TabStop = false;
			this.T_Packages.Text = "Packages";
			// 
			// smartFlowPanel1
			// 
			this.smartFlowPanel1.Controls.Add(this.roundedGroupPanel1);
			this.smartFlowPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.smartFlowPanel1.Location = new System.Drawing.Point(0, 0);
			this.smartFlowPanel1.Name = "smartFlowPanel1";
			this.smartFlowPanel1.Size = new System.Drawing.Size(916, 212);
			this.smartFlowPanel1.TabIndex = 14;
			// 
			// roundedGroupPanel1
			// 
			this.roundedGroupPanel1.AddPaddingForIcon = true;
			this.roundedGroupPanel1.Controls.Add(this.label1);
			this.roundedGroupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon3.Name = "Package";
			this.roundedGroupPanel1.ImageName = dynamicIcon3;
			this.roundedGroupPanel1.Location = new System.Drawing.Point(3, 3);
			this.roundedGroupPanel1.Name = "roundedGroupPanel1";
			this.roundedGroupPanel1.Padding = new System.Windows.Forms.Padding(36, 45, 7, 7);
			this.roundedGroupPanel1.Size = new System.Drawing.Size(501, 206);
			this.roundedGroupPanel1.TabIndex = 0;
			this.roundedGroupPanel1.Text = "WorkshopPackageSubmissions";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(36, 45);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// TLP_Top
			// 
			this.TLP_Top.ColumnCount = 3;
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Top.Controls.Add(this.PB_Icon, 1, 0);
			this.TLP_Top.Controls.Add(this.P_Back, 0, 1);
			this.TLP_Top.Controls.Add(this.P_Info, 2, 0);
			this.TLP_Top.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_Top.Location = new System.Drawing.Point(0, 30);
			this.TLP_Top.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Top.Name = "TLP_Top";
			this.TLP_Top.RowCount = 2;
			this.TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Top.Size = new System.Drawing.Size(916, 100);
			this.TLP_Top.TabIndex = 0;
			// 
			// PB_Icon
			// 
			this.PB_Icon.Dock = System.Windows.Forms.DockStyle.Left;
			this.PB_Icon.Location = new System.Drawing.Point(32, 0);
			this.PB_Icon.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Icon.Name = "PB_Icon";
			this.TLP_Top.SetRowSpan(this.PB_Icon, 2);
			this.PB_Icon.Size = new System.Drawing.Size(100, 100);
			this.PB_Icon.TabIndex = 0;
			this.PB_Icon.TabStop = false;
			// 
			// P_Back
			// 
			this.P_Back.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Back.Location = new System.Drawing.Point(0, 50);
			this.P_Back.Margin = new System.Windows.Forms.Padding(0);
			this.P_Back.Name = "P_Back";
			this.P_Back.Size = new System.Drawing.Size(32, 50);
			this.P_Back.TabIndex = 2;
			// 
			// P_Info
			// 
			this.P_Info.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Info.Location = new System.Drawing.Point(132, 0);
			this.P_Info.Margin = new System.Windows.Forms.Padding(0);
			this.P_Info.Name = "P_Info";
			this.TLP_Top.SetRowSpan(this.P_Info, 2);
			this.P_Info.Size = new System.Drawing.Size(784, 100);
			this.P_Info.TabIndex = 3;
			// 
			// PC_UserPage
			// 
			this.Controls.Add(this.P_Content);
			this.Controls.Add(this.TLP_Top);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_UserPage";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(916, 517);
			this.Text = "Back";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Top, 0);
			this.Controls.SetChildIndex(this.P_Content, 0);
			this.P_Content.ResumeLayout(false);
			this.smartFlowPanel1.ResumeLayout(false);
			this.roundedGroupPanel1.ResumeLayout(false);
			this.roundedGroupPanel1.PerformLayout();
			this.TLP_Top.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_Top;
	private UserIcon PB_Icon;
	private System.Windows.Forms.Panel P_Content;
	private System.Windows.Forms.Panel P_Back;
	private UserDescriptionControl P_Info;
	private SlickControls.SlickTabControl tabControl;
	public SlickControls.SlickTabControl.Tab T_Packages;
	private SlickControls.SlickTabControl.Tab T_Profiles;
	private SlickControls.SmartFlowPanel smartFlowPanel1;
	private SlickControls.RoundedGroupPanel roundedGroupPanel1;
	private System.Windows.Forms.Label label1;
}
