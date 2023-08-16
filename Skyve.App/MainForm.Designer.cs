using Skyve.App.UserInterface.Content;

namespace Skyve.App
{
	partial class MainForm
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

		#region Windows Form Designer generated code

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.PI_Dashboard = new SlickControls.PanelItem();
			this.PI_Mods = new SlickControls.PanelItem();
			this.PI_Assets = new SlickControls.PanelItem();
			this.PI_Profiles = new SlickControls.PanelItem();
			this.PI_Options = new SlickControls.PanelItem();
			this.PI_Compatibility = new SlickControls.PanelItem();
			this.PI_ModUtilities = new SlickControls.PanelItem();
			this.PI_Troubleshoot = new SlickControls.PanelItem();
			this.PI_Packages = new SlickControls.PanelItem();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.L_Text = new System.Windows.Forms.Label();
			this.L_Version = new System.Windows.Forms.Label();
			this.subscriptionInfoControl1 = new Skyve.App.UserInterface.Content.SubscriptionInfoControl();
			this.PI_DLCs = new SlickControls.PanelItem();
			this.downloadsInfoControl1 = new Skyve.App.UserInterface.Content.DownloadsInfoControl();
			this.TroubleshootInfoControl =	new UserInterface.Content.TroubleshootInfoControl();
			this.base_P_SideControls.SuspendLayout();
			this.base_P_Container.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
			this.base_P_Content.Size = new System.Drawing.Size(987, 562);
			// 
			// base_P_SideControls
			// 
			this.base_P_SideControls.Controls.Add(this.tableLayoutPanel1);
			this.base_P_SideControls.Font = new System.Drawing.Font("Nirmala UI", 6.75F);
			this.base_P_SideControls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(129)))), ((int)(((byte)(150)))));
			this.base_P_SideControls.Location = new System.Drawing.Point(7, 522);
			this.base_P_SideControls.Size = new System.Drawing.Size(236, 19);
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(203)))), ((int)(((byte)(145)))));
			this.base_P_Container.Size = new System.Drawing.Size(989, 564);
			// 
			// PI_Dashboard
			// 
			this.PI_Dashboard.Data = null;
			this.PI_Dashboard.ForceReopen = false;
			this.PI_Dashboard.Group = "";
			this.PI_Dashboard.Highlighted = false;
			dynamicIcon1.Name = "I_Dashboard";
			this.PI_Dashboard.IconName = dynamicIcon1;
			this.PI_Dashboard.Selected = false;
			this.PI_Dashboard.Text = "Dashboard";
			this.PI_Dashboard.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Dashboard_OnClick);
			// 
			// PI_Mods
			// 
			this.PI_Mods.Data = null;
			this.PI_Mods.ForceReopen = false;
			this.PI_Mods.Group = "Content";
			this.PI_Mods.Highlighted = false;
			dynamicIcon2.Name = "I_Mods";
			this.PI_Mods.IconName = dynamicIcon2;
			this.PI_Mods.Selected = false;
			this.PI_Mods.Text = "Mods";
			this.PI_Mods.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Mods_OnClick);
			// 
			// PI_Assets
			// 
			this.PI_Assets.Data = null;
			this.PI_Assets.ForceReopen = false;
			this.PI_Assets.Group = "Content";
			this.PI_Assets.Highlighted = false;
			dynamicIcon3.Name = "I_Assets";
			this.PI_Assets.IconName = dynamicIcon3;
			this.PI_Assets.Selected = false;
			this.PI_Assets.Text = "Assets";
			this.PI_Assets.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Assets_OnClick);
			// 
			// PI_Profiles
			// 
			this.PI_Profiles.Data = null;
			this.PI_Profiles.ForceReopen = false;
			this.PI_Profiles.Group = "";
			this.PI_Profiles.Highlighted = false;
			dynamicIcon4.Name = "I_ProfileSettings";
			this.PI_Profiles.IconName = dynamicIcon4;
			this.PI_Profiles.Selected = false;
			this.PI_Profiles.Text = "Profiles";
			this.PI_Profiles.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Profiles_OnClick);
			// 
			// PI_Options
			// 
			this.PI_Options.Data = null;
			this.PI_Options.ForceReopen = false;
			this.PI_Options.Group = "Other";
			this.PI_Options.Highlighted = false;
			dynamicIcon5.Name = "I_UserOptions";
			this.PI_Options.IconName = dynamicIcon5;
			this.PI_Options.Selected = false;
			this.PI_Options.Text = "Options";
			this.PI_Options.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Options_OnClick);
			// 
			// PI_Compatibility
			// 
			this.PI_Compatibility.Data = null;
			this.PI_Compatibility.ForceReopen = false;
			this.PI_Compatibility.Group = "Maintenance";
			this.PI_Compatibility.Highlighted = false;
			dynamicIcon6.Name = "I_CompatibilityReport";
			this.PI_Compatibility.IconName = dynamicIcon6;
			this.PI_Compatibility.Selected = false;
			this.PI_Compatibility.Text = "CompatibilityReport";
			this.PI_Compatibility.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Compatibility_OnClick);
			// 
			// PI_ModUtilities
			// 
			this.PI_ModUtilities.Data = null;
			this.PI_ModUtilities.ForceReopen = false;
			this.PI_ModUtilities.Group = "Maintenance";
			this.PI_ModUtilities.Highlighted = false;
			dynamicIcon7.Name = "I_Wrench";
			this.PI_ModUtilities.IconName = dynamicIcon7;
			this.PI_ModUtilities.Selected = false;
			this.PI_ModUtilities.Text = "Utilities";
			this.PI_ModUtilities.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_ModReview_OnClick);
			// 
			// PI_Troubleshoot
			// 
			this.PI_Troubleshoot.Data = null;
			this.PI_Troubleshoot.ForceReopen = false;
			this.PI_Troubleshoot.Group = "Maintenance";
			this.PI_Troubleshoot.Highlighted = false;
			dynamicIcon8.Name = "I_AskHelp";
			this.PI_Troubleshoot.IconName = dynamicIcon8;
			this.PI_Troubleshoot.Selected = false;
			this.PI_Troubleshoot.Text = "HelpLogs";
			this.PI_Troubleshoot.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Troubleshoot_OnClick);
			// 
			// PI_Packages
			// 
			this.PI_Packages.Data = null;
			this.PI_Packages.ForceReopen = false;
			this.PI_Packages.Group = "Content";
			this.PI_Packages.Highlighted = false;
			dynamicIcon9.Name = "I_Package";
			this.PI_Packages.IconName = dynamicIcon9;
			this.PI_Packages.Selected = false;
			this.PI_Packages.Text = "Packages";
			this.PI_Packages.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_Packages_OnClick);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.L_Text, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.L_Version, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.TroubleshootInfoControl, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.subscriptionInfoControl1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.downloadsInfoControl1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(236, 19);
			this.tableLayoutPanel1.TabIndex = 34;
			// 
			// L_Text
			// 
			this.L_Text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.L_Text.AutoSize = true;
			this.L_Text.Location = new System.Drawing.Point(0, 0);
			this.L_Text.Margin = new System.Windows.Forms.Padding(0);
			this.L_Text.Name = "L_Text";
			this.L_Text.Padding = new System.Windows.Forms.Padding(2);
			this.L_Text.Size = new System.Drawing.Size(41, 19);
			this.L_Text.TabIndex = 31;
			this.L_Text.Text = "Skyve";
			// 
			// L_Version
			// 
			this.L_Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.L_Version.AutoSize = true;
			this.L_Version.Location = new System.Drawing.Point(187, 0);
			this.L_Version.Margin = new System.Windows.Forms.Padding(0);
			this.L_Version.Name = "L_Version";
			this.L_Version.Padding = new System.Windows.Forms.Padding(2);
			this.L_Version.Size = new System.Drawing.Size(49, 19);
			this.L_Version.TabIndex = 30;
			this.L_Version.Text = "Version";
			// 
			// subscriptionInfoControl1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.subscriptionInfoControl1, 2);
			this.subscriptionInfoControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.subscriptionInfoControl1.Location = new System.Drawing.Point(3, 3);
			this.subscriptionInfoControl1.Name = "subscriptionInfoControl1";
			this.subscriptionInfoControl1.Size = new System.Drawing.Size(230, 25);
			this.subscriptionInfoControl1.TabIndex = 32;
			this.subscriptionInfoControl1.Visible = false;
			// 
			// PI_DLCs
			// 
			this.PI_DLCs.Data = null;
			this.PI_DLCs.ForceReopen = false;
			this.PI_DLCs.Group = "Content";
			this.PI_DLCs.Highlighted = false;
			dynamicIcon10.Name = "I_Dlc";
			this.PI_DLCs.IconName = dynamicIcon10;
			this.PI_DLCs.Selected = false;
			this.PI_DLCs.Text = "DLCs";
			this.PI_DLCs.OnClick += new System.Windows.Forms.MouseEventHandler(this.PI_DLCs_OnClick);
			// 
			// downloadsInfoControl1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.downloadsInfoControl1, 2);
			this.downloadsInfoControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.downloadsInfoControl1.Location = new System.Drawing.Point(0, 0);
			this.downloadsInfoControl1.Name = "downloadsInfoControl1";
			this.downloadsInfoControl1.Size = new System.Drawing.Size(150, 43);
			this.downloadsInfoControl1.TabIndex = 33;
			this.downloadsInfoControl1.Visible = false;

			this.tableLayoutPanel1.SetColumnSpan(this.TroubleshootInfoControl, 2);
			this.TroubleshootInfoControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.TroubleshootInfoControl.Location = new System.Drawing.Point(0, 0);
			this.TroubleshootInfoControl.Name = "TroubleshootInfoControl";
			this.TroubleshootInfoControl.Size = new System.Drawing.Size(150, 43);
			this.TroubleshootInfoControl.TabIndex = 33;
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1000, 575);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IconBounds = new System.Drawing.Rectangle(111, 31, 14, 42);
			this.MaximizeBox = true;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 1920, 1032);
			this.MinimizeBox = true;
			this.Name = "MainForm";
			this.SidebarItems = new SlickControls.PanelItem[] {
        this.PI_Dashboard,
        this.PI_Profiles,
        this.PI_Packages,
        this.PI_Mods,
        this.PI_Assets,
        this.PI_DLCs,
        this.PI_ModUtilities,
        this.PI_Compatibility,
        this.PI_Troubleshoot,
        this.PI_Options};
			this.Text = "Skyve";
			this.base_P_SideControls.ResumeLayout(false);
			this.base_P_SideControls.PerformLayout();
			this.base_P_Container.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		internal SlickControls.PanelItem PI_Dashboard;
		internal SlickControls.PanelItem PI_Mods;
		internal SlickControls.PanelItem PI_Assets;
		internal SlickControls.PanelItem PI_Profiles;
		internal SlickControls.PanelItem PI_Options;
		internal SlickControls.PanelItem PI_Compatibility;
		internal SlickControls.PanelItem PI_ModUtilities;
		internal SlickControls.PanelItem PI_Troubleshoot;
		internal SlickControls.PanelItem PI_Packages;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label L_Text;
		private System.Windows.Forms.Label L_Version;
		internal SlickControls.PanelItem PI_DLCs;
		private SubscriptionInfoControl subscriptionInfoControl1;
		private DownloadsInfoControl downloadsInfoControl1;
		private TroubleshootInfoControl TroubleshootInfoControl;
	}
}