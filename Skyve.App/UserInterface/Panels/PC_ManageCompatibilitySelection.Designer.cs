using Skyve.App.UserInterface.Generic;


namespace Skyve.App.UserInterface.Panels;

partial class PC_ManageCompatibilitySelection
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PC_ManageCompatibilitySelection));
			this.TLP_New = new System.Windows.Forms.TableLayoutPanel();
			this.B_Manage = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_YourPackages = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_ManageSingle = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Requests = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_Cancel = new SlickControls.SlickButton();
			this.TLP_New.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(200, 30);
			this.base_Text.Text = "ManageCompatibilityData";
			// 
			// TLP_New
			// 
			this.TLP_New.ColumnCount = 3;
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.Controls.Add(this.B_Manage, 1, 4);
			this.TLP_New.Controls.Add(this.B_YourPackages, 1, 1);
			this.TLP_New.Controls.Add(this.B_ManageSingle, 1, 2);
			this.TLP_New.Controls.Add(this.B_Requests, 1, 3);
			this.TLP_New.Controls.Add(this.B_Cancel, 2, 5);
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
			// B_Manage
			// 
			this.B_Manage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Manage.FromScratch = false;
			dynamicIcon1.Name = "I_Cog";
			this.B_Manage.ImageName = dynamicIcon1;
			this.B_Manage.Location = new System.Drawing.Point(516, 528);
			this.B_Manage.Name = "B_Manage";
			this.B_Manage.Size = new System.Drawing.Size(150, 150);
			this.B_Manage.TabIndex = 17;
			this.B_Manage.Text = "ManageCompatibilityData";
			this.B_Manage.Click += new System.EventHandler(this.B_Manage_Click);
			// 
			// B_YourPackages
			// 
			this.B_YourPackages.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_YourPackages.FromScratch = false;
			dynamicIcon2.Name = "I_User";
			this.B_YourPackages.ImageName = dynamicIcon2;
			this.B_YourPackages.Location = new System.Drawing.Point(516, 60);
			this.B_YourPackages.Name = "B_YourPackages";
			this.B_YourPackages.Size = new System.Drawing.Size(150, 150);
			this.B_YourPackages.TabIndex = 4;
			this.B_YourPackages.Text = "YourPackages";
			this.B_YourPackages.Click += new System.EventHandler(this.B_YourPackages_Click);
			// 
			// B_ManageSingle
			// 
			this.B_ManageSingle.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ManageSingle.FromScratch = false;
			dynamicIcon3.Name = "I_Edit";
			this.B_ManageSingle.ImageName = dynamicIcon3;
			this.B_ManageSingle.Location = new System.Drawing.Point(516, 216);
			this.B_ManageSingle.Name = "B_ManageSingle";
			this.B_ManageSingle.Size = new System.Drawing.Size(150, 150);
			this.B_ManageSingle.TabIndex = 5;
			this.B_ManageSingle.Text = "ManageSinglePackage";
			this.B_ManageSingle.Click += new System.EventHandler(this.B_ManageSingle_Click);
			// 
			// B_Requests
			// 
			this.B_Requests.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Requests.FromScratch = false;
			dynamicIcon4.Name = "I_RequestReview";
			this.B_Requests.ImageName = dynamicIcon4;
			this.B_Requests.Location = new System.Drawing.Point(516, 372);
			this.B_Requests.Name = "B_Requests";
			this.B_Requests.Size = new System.Drawing.Size(150, 150);
			this.B_Requests.TabIndex = 16;
			this.B_Requests.Text = "ViewRequests";
			this.B_Requests.Click += new System.EventHandler(this.B_Requests_Click);
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Cancel.AutoSize = true;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("B_Cancel.Image")));
			this.B_Cancel.Location = new System.Drawing.Point(1057, 733);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(10);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_Cancel.Size = new System.Drawing.Size(115, 46);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 14;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// PC_ManageCompatibilitySelection
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_New);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_ManageCompatibilitySelection";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1182, 819);
			this.Text = "ManageCompatibilityData";
			this.Controls.SetChildIndex(this.TLP_New, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.TLP_New.ResumeLayout(false);
			this.TLP_New.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_New;
	private SlickControls.SlickButton B_Cancel;
	private BigSelectionOptionControl B_YourPackages;
	private BigSelectionOptionControl B_ManageSingle;
	private BigSelectionOptionControl B_Requests;
	private BigSelectionOptionControl B_Manage;
}
