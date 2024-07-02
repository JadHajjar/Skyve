namespace Skyve.App.UserInterface.Content;

partial class UserDescriptionControl
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
			_notifier.WorkshopUsersInfoLoaded -= _notifier_WorkshopUsersInfoLoaded;
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
			this.TLP_Side = new System.Windows.Forms.TableLayoutPanel();
			this.TLP_Bio = new SlickControls.RoundedTableLayoutPanel();
			this.L_Bio = new System.Windows.Forms.Label();
			this.L_BioLabel = new System.Windows.Forms.Label();
			this.TLP_Links = new SlickControls.RoundedTableLayoutPanel();
			this.L_Links = new System.Windows.Forms.Label();
			this.FLP_Package_Links = new SlickControls.SmartFlowPanel();
			this.TLP_TopInfo = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.I_Verified = new SlickControls.SlickIcon();
			this.L_Name = new System.Windows.Forms.Label();
			this.PB_Icon = new Skyve.App.UserInterface.Content.UserIcon();
			this.I_More = new SlickControls.SlickButton();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.I_Followers = new SlickControls.SlickIcon();
			this.L_Followers = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.I_Packages = new SlickControls.SlickIcon();
			this.L_Packages = new System.Windows.Forms.Label();
			this.base_slickSpacer = new SlickControls.SlickSpacer();
			this.base_slickScroll = new SlickControls.SlickScroll();
			this.TLP_Side.SuspendLayout();
			this.TLP_Bio.SuspendLayout();
			this.TLP_Links.SuspendLayout();
			this.TLP_TopInfo.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// TLP_Side
			// 
			this.TLP_Side.AutoSize = true;
			this.TLP_Side.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Side.ColumnCount = 1;
			this.TLP_Side.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Side.Controls.Add(this.TLP_Bio, 0, 2);
			this.TLP_Side.Controls.Add(this.TLP_Links, 0, 3);
			this.TLP_Side.Controls.Add(this.TLP_TopInfo, 0, 0);
			this.TLP_Side.Controls.Add(this.base_slickSpacer, 0, 1);
			this.TLP_Side.Location = new System.Drawing.Point(0, 0);
			this.TLP_Side.Name = "TLP_Side";
			this.TLP_Side.RowCount = 4;
			this.TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Side.Size = new System.Drawing.Size(390, 167);
			this.TLP_Side.TabIndex = 1;
			// 
			// TLP_Bio
			// 
			this.TLP_Bio.AutoSize = true;
			this.TLP_Bio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Bio.ColumnCount = 1;
			this.TLP_Bio.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Bio.Controls.Add(this.L_Bio, 0, 1);
			this.TLP_Bio.Controls.Add(this.L_BioLabel, 0, 0);
			this.TLP_Bio.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_Bio.Location = new System.Drawing.Point(0, 122);
			this.TLP_Bio.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Bio.Name = "TLP_Bio";
			this.TLP_Bio.RowCount = 2;
			this.TLP_Bio.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Bio.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Bio.Size = new System.Drawing.Size(390, 26);
			this.TLP_Bio.TabIndex = 21;
			this.TLP_Bio.Visible = false;
			// 
			// L_Bio
			// 
			this.L_Bio.AutoSize = true;
			this.L_Bio.Location = new System.Drawing.Point(3, 13);
			this.L_Bio.Name = "L_Bio";
			this.L_Bio.Size = new System.Drawing.Size(35, 13);
			this.L_Bio.TabIndex = 4;
			this.L_Bio.Text = "label1";
			// 
			// L_BioLabel
			// 
			this.L_BioLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_BioLabel.AutoSize = true;
			this.L_BioLabel.Location = new System.Drawing.Point(177, 0);
			this.L_BioLabel.Name = "L_BioLabel";
			this.L_BioLabel.Size = new System.Drawing.Size(35, 13);
			this.L_BioLabel.TabIndex = 0;
			this.L_BioLabel.Text = "label4";
			// 
			// TLP_Links
			// 
			this.TLP_Links.AutoSize = true;
			this.TLP_Links.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Links.ColumnCount = 1;
			this.TLP_Links.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Links.Controls.Add(this.L_Links, 0, 0);
			this.TLP_Links.Controls.Add(this.FLP_Package_Links, 0, 1);
			this.TLP_Links.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_Links.Location = new System.Drawing.Point(0, 148);
			this.TLP_Links.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Links.Name = "TLP_Links";
			this.TLP_Links.RowCount = 2;
			this.TLP_Links.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Links.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Links.Size = new System.Drawing.Size(390, 19);
			this.TLP_Links.TabIndex = 20;
			this.TLP_Links.Visible = false;
			// 
			// L_Links
			// 
			this.L_Links.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.L_Links.AutoSize = true;
			this.L_Links.Location = new System.Drawing.Point(177, 0);
			this.L_Links.Name = "L_Links";
			this.L_Links.Size = new System.Drawing.Size(35, 13);
			this.L_Links.TabIndex = 0;
			this.L_Links.Text = "label4";
			// 
			// FLP_Package_Links
			// 
			this.FLP_Package_Links.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Package_Links.Location = new System.Drawing.Point(3, 16);
			this.FLP_Package_Links.Name = "FLP_Package_Links";
			this.FLP_Package_Links.Size = new System.Drawing.Size(384, 0);
			this.FLP_Package_Links.TabIndex = 1;
			this.FLP_Package_Links.SizeChanged += new System.EventHandler(this.FLP_Package_Links_SizeChanged);
			// 
			// TLP_TopInfo
			// 
			this.TLP_TopInfo.AutoSize = true;
			this.TLP_TopInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_TopInfo.ColumnCount = 3;
			this.TLP_TopInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_TopInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_TopInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_TopInfo.Controls.Add(this.tableLayoutPanel3, 1, 0);
			this.TLP_TopInfo.Controls.Add(this.PB_Icon, 0, 0);
			this.TLP_TopInfo.Controls.Add(this.I_More, 2, 0);
			this.TLP_TopInfo.Controls.Add(this.tableLayoutPanel1, 1, 1);
			this.TLP_TopInfo.Controls.Add(this.tableLayoutPanel2, 1, 2);
			this.TLP_TopInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_TopInfo.Location = new System.Drawing.Point(0, 0);
			this.TLP_TopInfo.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_TopInfo.Name = "TLP_TopInfo";
			this.TLP_TopInfo.RowCount = 4;
			this.TLP_TopInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_TopInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_TopInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_TopInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_TopInfo.Size = new System.Drawing.Size(390, 102);
			this.TLP_TopInfo.TabIndex = 0;
			this.TLP_TopInfo.Tag = "NoMouseDown";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.I_Verified, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.L_Name, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(90, 0);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(276, 24);
			this.tableLayoutPanel3.TabIndex = 9;
			// 
			// I_Verified
			// 
			this.I_Verified.ActiveColor = null;
			this.I_Verified.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.I_Verified.ColorStyle = Extensions.ColorStyle.Green;
			this.I_Verified.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_Verified.Enabled = false;
			dynamicIcon1.Name = "Ok";
			this.I_Verified.ImageName = dynamicIcon1;
			this.I_Verified.Location = new System.Drawing.Point(3, 3);
			this.I_Verified.Name = "I_Verified";
			this.I_Verified.Selected = true;
			this.I_Verified.Size = new System.Drawing.Size(23, 18);
			this.I_Verified.TabIndex = 7;
			this.I_Verified.Visible = false;
			// 
			// L_Name
			// 
			this.L_Name.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Name.AutoSize = true;
			this.L_Name.Location = new System.Drawing.Point(32, 5);
			this.L_Name.Name = "L_Name";
			this.L_Name.Size = new System.Drawing.Size(35, 13);
			this.L_Name.TabIndex = 3;
			this.L_Name.Text = "label1";
			// 
			// PB_Icon
			// 
			this.PB_Icon.Location = new System.Drawing.Point(0, 0);
			this.PB_Icon.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Icon.Name = "PB_Icon";
			this.TLP_TopInfo.SetRowSpan(this.PB_Icon, 4);
			this.PB_Icon.Size = new System.Drawing.Size(90, 102);
			this.PB_Icon.TabIndex = 0;
			this.PB_Icon.TabStop = false;
			// 
			// I_More
			// 
			this.I_More.AutoSize = true;
			this.I_More.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_More.Location = new System.Drawing.Point(366, 0);
			this.I_More.Margin = new System.Windows.Forms.Padding(0);
			this.I_More.Name = "I_More";
			this.I_More.Size = new System.Drawing.Size(24, 24);
			this.I_More.SpaceTriggersClick = true;
			this.I_More.TabIndex = 2;
			this.I_More.Click += new System.EventHandler(this.I_More_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.TLP_TopInfo.SetColumnSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.I_Followers, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.L_Followers, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(90, 24);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 24);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// I_Followers
			// 
			this.I_Followers.ActiveColor = null;
			this.I_Followers.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.I_Followers.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_Followers.Enabled = false;
			dynamicIcon2.Name = "People";
			this.I_Followers.ImageName = dynamicIcon2;
			this.I_Followers.Location = new System.Drawing.Point(3, 3);
			this.I_Followers.Name = "I_Followers";
			this.I_Followers.Size = new System.Drawing.Size(23, 18);
			this.I_Followers.TabIndex = 7;
			this.I_Followers.Visible = false;
			// 
			// L_Followers
			// 
			this.L_Followers.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Followers.AutoSize = true;
			this.L_Followers.Location = new System.Drawing.Point(32, 5);
			this.L_Followers.Name = "L_Followers";
			this.L_Followers.Size = new System.Drawing.Size(35, 13);
			this.L_Followers.TabIndex = 3;
			this.L_Followers.Text = "label1";
			this.L_Followers.Visible = false;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.TLP_TopInfo.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.I_Packages, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.L_Packages, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(90, 48);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(300, 27);
			this.tableLayoutPanel2.TabIndex = 9;
			// 
			// I_Packages
			// 
			this.I_Packages.ActiveColor = null;
			this.I_Packages.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.I_Packages.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_Packages.Enabled = false;
			dynamicIcon3.Name = "Package";
			this.I_Packages.ImageName = dynamicIcon3;
			this.I_Packages.Location = new System.Drawing.Point(3, 3);
			this.I_Packages.Name = "I_Packages";
			this.I_Packages.Size = new System.Drawing.Size(14, 21);
			this.I_Packages.TabIndex = 7;
			// 
			// L_Packages
			// 
			this.L_Packages.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Packages.AutoSize = true;
			this.L_Packages.Location = new System.Drawing.Point(23, 7);
			this.L_Packages.Name = "L_Packages";
			this.L_Packages.Size = new System.Drawing.Size(35, 13);
			this.L_Packages.TabIndex = 3;
			this.L_Packages.Text = "label1";
			// 
			// base_slickSpacer
			// 
			this.base_slickSpacer.Dock = System.Windows.Forms.DockStyle.Top;
			this.base_slickSpacer.Location = new System.Drawing.Point(3, 105);
			this.base_slickSpacer.Name = "base_slickSpacer";
			this.base_slickSpacer.Size = new System.Drawing.Size(384, 14);
			this.base_slickSpacer.TabIndex = 1;
			this.base_slickSpacer.TabStop = false;
			this.base_slickSpacer.Text = "slickSpacer1";
			// 
			// base_slickScroll
			// 
			this.base_slickScroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.base_slickScroll.LinkedControl = this.TLP_Side;
			this.base_slickScroll.Location = new System.Drawing.Point(424, 0);
			this.base_slickScroll.Name = "base_slickScroll";
			this.base_slickScroll.Size = new System.Drawing.Size(6, 261);
			this.base_slickScroll.Style = SlickControls.StyleType.Vertical;
			this.base_slickScroll.TabIndex = 2;
			this.base_slickScroll.TabStop = false;
			this.base_slickScroll.Text = "slickScroll1";
			// 
			// UserDescriptionControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.base_slickScroll);
			this.Controls.Add(this.TLP_Side);
			this.Name = "UserDescriptionControl";
			this.Size = new System.Drawing.Size(430, 261);
			this.TLP_Side.ResumeLayout(false);
			this.TLP_Side.PerformLayout();
			this.TLP_Bio.ResumeLayout(false);
			this.TLP_Bio.PerformLayout();
			this.TLP_Links.ResumeLayout(false);
			this.TLP_Links.PerformLayout();
			this.TLP_TopInfo.ResumeLayout(false);
			this.TLP_TopInfo.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_Side;
	private RoundedTableLayoutPanel TLP_Links;
	private System.Windows.Forms.Label L_Links;
	private SmartFlowPanel FLP_Package_Links;
	private System.Windows.Forms.TableLayoutPanel TLP_TopInfo;
	protected UserIcon PB_Icon;
	private SlickButton I_More;
	private SlickSpacer base_slickSpacer;
	private System.Windows.Forms.Label L_Name;
	private RoundedTableLayoutPanel TLP_Bio;
	private System.Windows.Forms.Label L_Bio;
	private System.Windows.Forms.Label L_BioLabel;
	private SlickIcon I_Followers;
	private SlickIcon I_Packages;
	private System.Windows.Forms.Label L_Followers;
	private System.Windows.Forms.Label L_Packages;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
	private SlickScroll base_slickScroll;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
	private SlickIcon I_Verified;
}
