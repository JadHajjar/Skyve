using Skyve.App.UserInterface.Content;
using Skyve.Domain;
using Skyve.Domain.Systems;

namespace Skyve.App.UserInterface.Panels;

partial class PC_PackagePage
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
			ServiceCenter.Get<INotifier>().PackageInformationUpdated -= CentralManager_PackageInformationUpdated;
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
			this.P_Content = new System.Windows.Forms.Panel();
			this.slickTabControl1 = new SlickControls.SlickTabControl();
			this.T_Info = new SlickControls.SlickTabControl.Tab();
			this.TLP_Info = new System.Windows.Forms.TableLayoutPanel();
			this.P_List = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.TLP_About = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.FLP_Links = new SlickControls.SmartFlowPanel();
			this.FLP_Tags = new SlickControls.SmartFlowPanel();
			this.T_CR = new SlickControls.SlickTabControl.Tab();
			this.T_References = new SlickControls.SlickTabControl.Tab();
			this.T_Profiles = new SlickControls.SlickTabControl.Tab();
			this.TLP_Profiles = new System.Windows.Forms.TableLayoutPanel();
			this.TLP_Top = new System.Windows.Forms.TableLayoutPanel();
			this.PB_Icon = new Skyve.App.UserInterface.Content.PackageIcon();
			this.P_Back = new System.Windows.Forms.Panel();
			this.P_Info = new Skyve.App.UserInterface.Content.PackageDescriptionControl();
			this.L_Requirements = new System.Windows.Forms.Label();
			this.FLP_Requirements = new SlickControls.SmartFlowPanel();
			this.P_Content.SuspendLayout();
			this.TLP_Info.SuspendLayout();
			this.panel1.SuspendLayout();
			this.TLP_About.SuspendLayout();
			this.TLP_Top.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Text = "Back";
			// 
			// P_Content
			// 
			this.P_Content.Controls.Add(this.slickTabControl1);
			this.P_Content.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Content.Location = new System.Drawing.Point(0, 130);
			this.P_Content.Name = "P_Content";
			this.P_Content.Size = new System.Drawing.Size(783, 308);
			this.P_Content.TabIndex = 13;
			// 
			// slickTabControl1
			// 
			this.slickTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.slickTabControl1.Location = new System.Drawing.Point(0, 0);
			this.slickTabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.slickTabControl1.Name = "slickTabControl1";
			this.slickTabControl1.Size = new System.Drawing.Size(783, 308);
			this.slickTabControl1.TabIndex = 0;
			this.slickTabControl1.Tabs = new SlickControls.SlickTabControl.Tab[] {
        this.T_Info,
        this.T_CR,
        this.T_References,
        this.T_Profiles};
			// 
			// T_Info
			// 
			this.T_Info.Cursor = System.Windows.Forms.Cursors.Hand;
			this.T_Info.Dock = System.Windows.Forms.DockStyle.Left;
			this.T_Info.FillTab = true;
			this.T_Info.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dynamicIcon1.Name = "I_Content";
			this.T_Info.IconName = dynamicIcon1;
			this.T_Info.LinkedControl = this.TLP_Info;
			this.T_Info.Location = new System.Drawing.Point(0, 5);
			this.T_Info.Name = "T_Info";
			this.T_Info.Selected = true;
			this.T_Info.Size = new System.Drawing.Size(195, 25);
			this.T_Info.TabIndex = 0;
			this.T_Info.TabStop = false;
			this.T_Info.Text = "ContentAndInfo";
			// 
			// TLP_Info
			// 
			this.TLP_Info.ColumnCount = 2;
			this.TLP_Info.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.TLP_Info.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.TLP_Info.Controls.Add(this.P_List, 0, 0);
			this.TLP_Info.Controls.Add(this.panel1, 1, 0);
			this.TLP_Info.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Info.Location = new System.Drawing.Point(0, 0);
			this.TLP_Info.Name = "TLP_Info";
			this.TLP_Info.RowCount = 1;
			this.TLP_Info.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Info.Size = new System.Drawing.Size(783, 277);
			this.TLP_Info.TabIndex = 14;
			// 
			// P_List
			// 
			this.P_List.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_List.Location = new System.Drawing.Point(0, 0);
			this.P_List.Margin = new System.Windows.Forms.Padding(0);
			this.P_List.Name = "P_List";
			this.P_List.Size = new System.Drawing.Size(469, 277);
			this.P_List.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.slickSpacer1);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Controls.Add(this.TLP_About);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(469, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(314, 277);
			this.panel1.TabIndex = 3;
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Left;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(1, 277);
			this.slickSpacer1.TabIndex = 4;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.TLP_About;
			this.slickScroll1.Location = new System.Drawing.Point(304, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(10, 277);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 3;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// TLP_About
			// 
			this.TLP_About.AutoSize = true;
			this.TLP_About.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_About.ColumnCount = 1;
			this.TLP_About.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_About.Controls.Add(this.label1, 0, 0);
			this.TLP_About.Controls.Add(this.label2, 0, 1);
			this.TLP_About.Controls.Add(this.label3, 0, 2);
			this.TLP_About.Controls.Add(this.label4, 0, 3);
			this.TLP_About.Controls.Add(this.label5, 0, 4);
			this.TLP_About.Controls.Add(this.label6, 0, 6);
			this.TLP_About.Controls.Add(this.FLP_Links, 0, 5);
			this.TLP_About.Controls.Add(this.FLP_Tags, 0, 7);
			this.TLP_About.Controls.Add(this.L_Requirements, 0, 8);
			this.TLP_About.Controls.Add(this.FLP_Requirements, 0, 9);
			this.TLP_About.Location = new System.Drawing.Point(3, 6);
			this.TLP_About.Name = "TLP_About";
			this.TLP_About.RowCount = 10;
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_About.Size = new System.Drawing.Size(206, 228);
			this.TLP_About.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 30);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 30);
			this.label2.TabIndex = 1;
			this.label2.Text = "label1";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 60);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 30);
			this.label3.TabIndex = 1;
			this.label3.Text = "label1";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 90);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 30);
			this.label4.TabIndex = 1;
			this.label4.Text = "label1";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(68, 30);
			this.label5.TabIndex = 1;
			this.label5.Text = "label1";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(3, 156);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(68, 30);
			this.label6.TabIndex = 1;
			this.label6.Text = "label1";
			// 
			// FLP_Links
			// 
			this.FLP_Links.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Links.Location = new System.Drawing.Point(3, 153);
			this.FLP_Links.Name = "FLP_Links";
			this.FLP_Links.Size = new System.Drawing.Size(200, 0);
			this.FLP_Links.TabIndex = 2;
			// 
			// FLP_Tags
			// 
			this.FLP_Tags.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Tags.Location = new System.Drawing.Point(3, 189);
			this.FLP_Tags.Name = "FLP_Tags";
			this.FLP_Tags.Size = new System.Drawing.Size(200, 0);
			this.FLP_Tags.TabIndex = 2;
			// 
			// T_CR
			// 
			this.T_CR.Cursor = System.Windows.Forms.Cursors.Hand;
			this.T_CR.Dock = System.Windows.Forms.DockStyle.Left;
			this.T_CR.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dynamicIcon2.Name = "I_CompatibilityReport";
			this.T_CR.IconName = dynamicIcon2;
			this.T_CR.LinkedControl = null;
			this.T_CR.Location = new System.Drawing.Point(195, 5);
			this.T_CR.Name = "T_CR";
			this.T_CR.Selected = false;
			this.T_CR.Size = new System.Drawing.Size(195, 25);
			this.T_CR.TabIndex = 0;
			this.T_CR.TabStop = false;
			this.T_CR.Text = "CompatibilityInfo";
			// 
			// T_References
			// 
			this.T_References.Cursor = System.Windows.Forms.Cursors.Hand;
			this.T_References.Dock = System.Windows.Forms.DockStyle.Left;
			this.T_References.FillTab = true;
			dynamicIcon3.Name = "I_Share";
			this.T_References.IconName = dynamicIcon3;
			this.T_References.LinkedControl = null;
			this.T_References.Location = new System.Drawing.Point(390, 5);
			this.T_References.Name = "T_References";
			this.T_References.Selected = false;
			this.T_References.Size = new System.Drawing.Size(195, 25);
			this.T_References.TabIndex = 1;
			this.T_References.TabStop = false;
			this.T_References.Text = "References";
			// 
			// T_Profiles
			// 
			this.T_Profiles.Cursor = System.Windows.Forms.Cursors.Hand;
			this.T_Profiles.Dock = System.Windows.Forms.DockStyle.Left;
			this.T_Profiles.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dynamicIcon4.Name = "I_ProfileSettings";
			this.T_Profiles.IconName = dynamicIcon4;
			this.T_Profiles.LinkedControl = this.TLP_Profiles;
			this.T_Profiles.Location = new System.Drawing.Point(585, 5);
			this.T_Profiles.Name = "T_Profiles";
			this.T_Profiles.Selected = false;
			this.T_Profiles.Size = new System.Drawing.Size(195, 25);
			this.T_Profiles.TabIndex = 0;
			this.T_Profiles.TabStop = false;
			this.T_Profiles.Text = "OtherPlaysets";
			// 
			// TLP_Profiles
			// 
			this.TLP_Profiles.ColumnCount = 2;
			this.TLP_Profiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Profiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Profiles.Location = new System.Drawing.Point(0, 0);
			this.TLP_Profiles.MaximumSize = new System.Drawing.Size(773, 2147483647);
			this.TLP_Profiles.MinimumSize = new System.Drawing.Size(773, 0);
			this.TLP_Profiles.Name = "TLP_Profiles";
			this.TLP_Profiles.RowCount = 2;
			this.TLP_Profiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Profiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Profiles.Size = new System.Drawing.Size(773, 208);
			this.TLP_Profiles.TabIndex = 16;
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
			this.TLP_Top.Size = new System.Drawing.Size(783, 100);
			this.TLP_Top.TabIndex = 0;
			// 
			// PB_Icon
			// 
			this.PB_Icon.Dock = System.Windows.Forms.DockStyle.Left;
			this.PB_Icon.HalfColor = false;
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
			this.P_Info.Size = new System.Drawing.Size(651, 100);
			this.P_Info.TabIndex = 3;
			// 
			// L_Requirements
			// 
			this.L_Requirements.AutoSize = true;
			this.L_Requirements.Location = new System.Drawing.Point(3, 192);
			this.L_Requirements.Name = "L_Requirements";
			this.L_Requirements.Size = new System.Drawing.Size(68, 30);
			this.L_Requirements.TabIndex = 1;
			this.L_Requirements.Text = "label1";
			// 
			// FLP_Requirements
			// 
			this.FLP_Requirements.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Requirements.Location = new System.Drawing.Point(3, 225);
			this.FLP_Requirements.Name = "FLP_Requirements";
			this.FLP_Requirements.Size = new System.Drawing.Size(200, 0);
			this.FLP_Requirements.TabIndex = 3;
			// 
			// PC_PackagePage
			// 
			this.Controls.Add(this.P_Content);
			this.Controls.Add(this.TLP_Top);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_PackagePage";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Text = "Back";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Top, 0);
			this.Controls.SetChildIndex(this.P_Content, 0);
			this.P_Content.ResumeLayout(false);
			this.TLP_Info.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.TLP_About.ResumeLayout(false);
			this.TLP_About.PerformLayout();
			this.TLP_Top.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_Top;
	private PackageIcon PB_Icon;
	private System.Windows.Forms.Panel P_Content;
	private System.Windows.Forms.Panel P_Back;
	private PackageDescriptionControl P_Info;
	private SlickControls.SlickTabControl slickTabControl1;
	private SlickControls.SlickTabControl.Tab T_Info;
	internal SlickControls.SlickTabControl.Tab T_CR;
	private SlickControls.SlickTabControl.Tab T_Profiles;
	private System.Windows.Forms.TableLayoutPanel TLP_Profiles;
	private System.Windows.Forms.TableLayoutPanel TLP_Info;
	private System.Windows.Forms.Panel P_List;
	private System.Windows.Forms.Panel panel1;
	private SlickControls.SlickScroll slickScroll1;
	private System.Windows.Forms.TableLayoutPanel TLP_About;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.Label label2;
	private System.Windows.Forms.Label label3;
	private System.Windows.Forms.Label label4;
	private System.Windows.Forms.Label label5;
	private System.Windows.Forms.Label label6;
	private SlickControls.SmartFlowPanel FLP_Links;
	private SlickControls.SmartFlowPanel FLP_Tags;
	private SlickControls.SlickSpacer slickSpacer1;
	private SlickControls.SlickTabControl.Tab T_References;
	private System.Windows.Forms.Label L_Requirements;
	private SmartFlowPanel FLP_Requirements;
}
