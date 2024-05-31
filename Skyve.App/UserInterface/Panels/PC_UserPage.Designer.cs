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
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			this.P_Content = new System.Windows.Forms.Panel();
			this.tabControl = new SlickControls.SlickTabControl();
			this.T_Packages = new SlickControls.SlickTabControl.Tab();
			this.T_Profiles = new SlickControls.SlickTabControl.Tab();
			this.smartFlowPanel1 = new SlickControls.SmartFlowPanel();
			this.roundedGroupPanel1 = new SlickControls.RoundedGroupPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.P_Info = new Skyve.App.UserInterface.Content.UserDescriptionControl();
			this.P_Content.SuspendLayout();
			this.smartFlowPanel1.SuspendLayout();
			this.roundedGroupPanel1.SuspendLayout();
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
			this.P_Content.Location = new System.Drawing.Point(0, 30);
			this.P_Content.Name = "P_Content";
			this.P_Content.Size = new System.Drawing.Size(700, 487);
			this.P_Content.TabIndex = 13;
			// 
			// tabControl
			// 
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl.Name = "tabControl";
			this.tabControl.Size = new System.Drawing.Size(700, 487);
			this.tabControl.TabIndex = 0;
			this.tabControl.Tabs = new SlickControls.SlickTabControl.Tab[] {
        this.T_Packages,
        this.T_Profiles};
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
			this.T_Packages.Size = new System.Drawing.Size(125, 60);
			this.T_Packages.TabIndex = 0;
			this.T_Packages.TabStop = false;
			this.T_Packages.Text = "Packages";
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
			this.T_Profiles.Location = new System.Drawing.Point(125, 5);
			this.T_Profiles.Name = "T_Profiles";
			this.T_Profiles.Size = new System.Drawing.Size(125, 60);
			this.T_Profiles.TabIndex = 0;
			this.T_Profiles.TabStop = false;
			this.T_Profiles.Text = "Profiles";
			this.T_Profiles.Visible = false;
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
			this.roundedGroupPanel1.Padding = new System.Windows.Forms.Padding(34, 43, 6, 6);
			this.roundedGroupPanel1.Size = new System.Drawing.Size(501, 206);
			this.roundedGroupPanel1.TabIndex = 0;
			this.roundedGroupPanel1.Text = "WorkshopPackageSubmissions";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(34, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// P_Info
			// 
			this.P_Info.Dock = System.Windows.Forms.DockStyle.Right;
			this.P_Info.Location = new System.Drawing.Point(700, 30);
			this.P_Info.Margin = new System.Windows.Forms.Padding(0);
			this.P_Info.Name = "P_Info";
			this.P_Info.Size = new System.Drawing.Size(216, 487);
			this.P_Info.TabIndex = 3;
			// 
			// PC_UserPage
			// 
			this.Controls.Add(this.P_Content);
			this.Controls.Add(this.P_Info);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_UserPage";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(916, 517);
			this.Text = "Back";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.P_Info, 0);
			this.Controls.SetChildIndex(this.P_Content, 0);
			this.P_Content.ResumeLayout(false);
			this.smartFlowPanel1.ResumeLayout(false);
			this.roundedGroupPanel1.ResumeLayout(false);
			this.roundedGroupPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.Panel P_Content;
	private UserDescriptionControl P_Info;
	private SlickControls.SlickTabControl tabControl;
	public SlickControls.SlickTabControl.Tab T_Packages;
	private SlickControls.SlickTabControl.Tab T_Profiles;
	private SlickControls.SmartFlowPanel smartFlowPanel1;
	private SlickControls.RoundedGroupPanel roundedGroupPanel1;
	private System.Windows.Forms.Label label1;
}
