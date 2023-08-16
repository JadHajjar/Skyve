using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Dropdowns;
using Skyve.App.UserInterface.Generic;

namespace Skyve.App.UserInterface.Panels;

partial class PC_RequestReview
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
			this.TLP_Actions = new System.Windows.Forms.TableLayoutPanel();
			this.B_ReportIssue = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_AddStatus = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.B_AddInteraction = new Skyve.App.UserInterface.Generic.BigSelectionOptionControl();
			this.P_Main = new SlickControls.RoundedPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.P_Content = new System.Windows.Forms.Panel();
			this.TLP_MainInfo = new System.Windows.Forms.TableLayoutPanel();
			this.TB_Note2 = new SlickControls.SlickTextBox();
			this.DD_Stability = new Skyve.App.UserInterface.Dropdowns.PackageStabilityDropDown();
			this.DD_DLCs = new Skyve.App.UserInterface.Dropdowns.DlcDropDown();
			this.DD_Usage = new Skyve.App.UserInterface.Dropdowns.PackageUsageDropDown();
			this.DD_PackageType = new Skyve.App.UserInterface.Dropdowns.PackageTypeDropDown();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.TB_Note = new SlickControls.SlickTextBox();
			this.TLP_Top = new System.Windows.Forms.TableLayoutPanel();
			this.PB_Icon = new Skyve.App.UserInterface.Content.PackageIcon();
			this.P_Info = new Skyve.App.UserInterface.Content.PackageDescriptionControl();
			this.PB_LoadingPackage = new SlickControls.SlickPictureBox();
			this.TLP_Button = new System.Windows.Forms.TableLayoutPanel();
			this.B_Apply = new SlickControls.SlickButton();
			this.L_Disclaimer = new System.Windows.Forms.Label();
			this.L_English = new System.Windows.Forms.Label();
			this.TLP_Actions.SuspendLayout();
			this.P_Main.SuspendLayout();
			this.panel1.SuspendLayout();
			this.P_Content.SuspendLayout();
			this.TLP_MainInfo.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.TLP_Top.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_LoadingPackage)).BeginInit();
			this.TLP_Button.SuspendLayout();
			this.SuspendLayout();
			// 
			// TLP_Actions
			// 
			this.TLP_Actions.ColumnCount = 3;
			this.TLP_Actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Actions.Controls.Add(this.B_ReportIssue, 1, 1);
			this.TLP_Actions.Controls.Add(this.B_AddStatus, 1, 2);
			this.TLP_Actions.Controls.Add(this.B_AddInteraction, 1, 3);
			this.TLP_Actions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Actions.Location = new System.Drawing.Point(5, 30);
			this.TLP_Actions.Name = "TLP_Actions";
			this.TLP_Actions.RowCount = 5;
			this.TLP_Actions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Actions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Actions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Actions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Actions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Actions.Size = new System.Drawing.Size(932, 556);
			this.TLP_Actions.TabIndex = 2;
			// 
			// B_ReportIssue
			// 
			this.B_ReportIssue.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ReportIssue.Font = new System.Drawing.Font("Segoe UI", 19.5F, System.Drawing.FontStyle.Bold);
			this.B_ReportIssue.FromScratch = false;
			dynamicIcon1.Name = "I_Remarks";
			this.B_ReportIssue.ImageName = dynamicIcon1;
			this.B_ReportIssue.Location = new System.Drawing.Point(203, 16);
			this.B_ReportIssue.Margin = new System.Windows.Forms.Padding(0, 30, 150, 30);
			this.B_ReportIssue.Name = "B_ReportIssue";
			this.B_ReportIssue.Padding = new System.Windows.Forms.Padding(22);
			this.B_ReportIssue.Size = new System.Drawing.Size(375, 135);
			this.B_ReportIssue.TabIndex = 0;
			this.B_ReportIssue.Text = "RequestOption1";
			this.B_ReportIssue.Click += new System.EventHandler(this.B_ReportIssue_Click);
			// 
			// B_AddStatus
			// 
			this.B_AddStatus.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_AddStatus.Font = new System.Drawing.Font("Segoe UI", 19.5F, System.Drawing.FontStyle.Bold);
			this.B_AddStatus.FromScratch = false;
			dynamicIcon2.Name = "I_Content";
			this.B_AddStatus.ImageName = dynamicIcon2;
			this.B_AddStatus.Location = new System.Drawing.Point(353, 211);
			this.B_AddStatus.Margin = new System.Windows.Forms.Padding(150, 30, 0, 30);
			this.B_AddStatus.Name = "B_AddStatus";
			this.B_AddStatus.Padding = new System.Windows.Forms.Padding(22);
			this.B_AddStatus.Size = new System.Drawing.Size(375, 135);
			this.B_AddStatus.TabIndex = 0;
			this.B_AddStatus.Text = "RequestOption2";
			this.B_AddStatus.Click += new System.EventHandler(this.B_AddStatus_Click);
			// 
			// B_AddInteraction
			// 
			this.B_AddInteraction.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_AddInteraction.Font = new System.Drawing.Font("Segoe UI", 19.5F, System.Drawing.FontStyle.Bold);
			this.B_AddInteraction.FromScratch = false;
			dynamicIcon3.Name = "I_Switch";
			this.B_AddInteraction.ImageName = dynamicIcon3;
			this.B_AddInteraction.Location = new System.Drawing.Point(203, 406);
			this.B_AddInteraction.Margin = new System.Windows.Forms.Padding(0, 30, 150, 30);
			this.B_AddInteraction.Name = "B_AddInteraction";
			this.B_AddInteraction.Padding = new System.Windows.Forms.Padding(22);
			this.B_AddInteraction.Size = new System.Drawing.Size(375, 135);
			this.B_AddInteraction.TabIndex = 0;
			this.B_AddInteraction.Text = "RequestOption3";
			this.B_AddInteraction.Click += new System.EventHandler(this.B_AddInteraction_Click);
			// 
			// P_Main
			// 
			this.P_Main.AddOutline = true;
			this.P_Main.Controls.Add(this.panel1);
			this.P_Main.Controls.Add(this.PB_LoadingPackage);
			this.P_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Main.Location = new System.Drawing.Point(5, 30);
			this.P_Main.Name = "P_Main";
			this.P_Main.Size = new System.Drawing.Size(932, 556);
			this.P_Main.TabIndex = 15;
			this.P_Main.Visible = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.P_Content);
			this.panel1.Controls.Add(this.TLP_Top);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(932, 556);
			this.panel1.TabIndex = 17;
			// 
			// P_Content
			// 
			this.P_Content.Controls.Add(this.TLP_MainInfo);
			this.P_Content.Controls.Add(this.tableLayoutPanel1);
			this.P_Content.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Content.Location = new System.Drawing.Point(0, 100);
			this.P_Content.Name = "P_Content";
			this.P_Content.Size = new System.Drawing.Size(932, 456);
			this.P_Content.TabIndex = 13;
			// 
			// TLP_MainInfo
			// 
			this.TLP_MainInfo.AutoSize = true;
			this.TLP_MainInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_MainInfo.ColumnCount = 2;
			this.TLP_MainInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_MainInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_MainInfo.Controls.Add(this.L_English, 0, 3);
			this.TLP_MainInfo.Controls.Add(this.TB_Note2, 0, 4);
			this.TLP_MainInfo.Controls.Add(this.DD_Stability, 0, 0);
			this.TLP_MainInfo.Controls.Add(this.DD_DLCs, 0, 1);
			this.TLP_MainInfo.Controls.Add(this.DD_Usage, 1, 0);
			this.TLP_MainInfo.Controls.Add(this.DD_PackageType, 1, 1);
			this.TLP_MainInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_MainInfo.Location = new System.Drawing.Point(0, 0);
			this.TLP_MainInfo.Name = "TLP_MainInfo";
			this.TLP_MainInfo.RowCount = 5;
			this.TLP_MainInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_MainInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_MainInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_MainInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_MainInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_MainInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_MainInfo.Size = new System.Drawing.Size(932, 456);
			this.TLP_MainInfo.TabIndex = 19;
			this.TLP_MainInfo.Visible = false;
			// 
			// TB_Note2
			// 
			this.TLP_MainInfo.SetColumnSpan(this.TB_Note2, 2);
			this.TB_Note2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.TB_Note2.LabelText = "ExplainIssue";
			this.TB_Note2.Location = new System.Drawing.Point(3, 279);
			this.TB_Note2.MultiLine = true;
			this.TB_Note2.Name = "TB_Note2";
			this.TB_Note2.Placeholder = "ExplainIssueInfo";
			this.TB_Note2.SelectedText = "";
			this.TB_Note2.SelectionLength = 0;
			this.TB_Note2.SelectionStart = 0;
			this.TB_Note2.Size = new System.Drawing.Size(926, 174);
			this.TB_Note2.TabIndex = 19;
			// 
			// DD_Stability
			// 
			this.DD_Stability.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Stability.Location = new System.Drawing.Point(3, 3);
			this.DD_Stability.Name = "DD_Stability";
			this.DD_Stability.Size = new System.Drawing.Size(375, 56);
			this.DD_Stability.TabIndex = 0;
			this.DD_Stability.Text = "Stability";
			// 
			// DD_DLCs
			// 
			this.DD_DLCs.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_DLCs.Location = new System.Drawing.Point(3, 65);
			this.DD_DLCs.Name = "DD_DLCs";
			this.DD_DLCs.Size = new System.Drawing.Size(375, 54);
			this.DD_DLCs.TabIndex = 17;
			this.DD_DLCs.Text = "RequiredDLCs";
			// 
			// DD_Usage
			// 
			this.DD_Usage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Usage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Usage.Location = new System.Drawing.Point(554, 3);
			this.DD_Usage.Name = "DD_Usage";
			this.DD_Usage.Size = new System.Drawing.Size(375, 54);
			this.DD_Usage.TabIndex = 17;
			this.DD_Usage.Text = "Usage";
			// 
			// DD_PackageType
			// 
			this.DD_PackageType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_PackageType.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_PackageType.Location = new System.Drawing.Point(554, 65);
			this.DD_PackageType.Name = "DD_PackageType";
			this.DD_PackageType.Size = new System.Drawing.Size(375, 54);
			this.DD_PackageType.TabIndex = 17;
			this.DD_PackageType.Text = "PackageType";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.TB_Note, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(932, 456);
			this.tableLayoutPanel1.TabIndex = 4;
			this.tableLayoutPanel1.Visible = false;
			// 
			// TB_Note
			// 
			this.TB_Note.Dock = System.Windows.Forms.DockStyle.Top;
			this.TB_Note.LabelText = "ExplainIssue";
			this.TB_Note.Location = new System.Drawing.Point(3, 3);
			this.TB_Note.MultiLine = true;
			this.TB_Note.Name = "TB_Note";
			this.TB_Note.Placeholder = "ExplainIssueInfo";
			this.TB_Note.SelectedText = "";
			this.TB_Note.SelectionLength = 0;
			this.TB_Note.SelectionStart = 0;
			this.TB_Note.Size = new System.Drawing.Size(926, 151);
			this.TB_Note.TabIndex = 20;
			// 
			// TLP_Top
			// 
			this.TLP_Top.ColumnCount = 2;
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Top.Controls.Add(this.PB_Icon, 0, 0);
			this.TLP_Top.Controls.Add(this.P_Info, 1, 0);
			this.TLP_Top.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_Top.Location = new System.Drawing.Point(0, 0);
			this.TLP_Top.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Top.Name = "TLP_Top";
			this.TLP_Top.RowCount = 2;
			this.TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Top.Size = new System.Drawing.Size(932, 100);
			this.TLP_Top.TabIndex = 0;
			// 
			// PB_Icon
			// 
			this.PB_Icon.Dock = System.Windows.Forms.DockStyle.Left;
			this.PB_Icon.HalfColor = false;
			this.PB_Icon.Location = new System.Drawing.Point(0, 0);
			this.PB_Icon.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Icon.Name = "PB_Icon";
			this.TLP_Top.SetRowSpan(this.PB_Icon, 2);
			this.PB_Icon.Size = new System.Drawing.Size(100, 100);
			this.PB_Icon.TabIndex = 0;
			this.PB_Icon.TabStop = false;
			// 
			// P_Info
			// 
			this.P_Info.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Info.Location = new System.Drawing.Point(105, 0);
			this.P_Info.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.P_Info.Name = "P_Info";
			this.TLP_Top.SetRowSpan(this.P_Info, 2);
			this.P_Info.Size = new System.Drawing.Size(827, 100);
			this.P_Info.TabIndex = 3;
			// 
			// PB_LoadingPackage
			// 
			this.PB_LoadingPackage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PB_LoadingPackage.LoaderSpeed = 1D;
			this.PB_LoadingPackage.Location = new System.Drawing.Point(0, 0);
			this.PB_LoadingPackage.Name = "PB_LoadingPackage";
			this.PB_LoadingPackage.Size = new System.Drawing.Size(932, 556);
			this.PB_LoadingPackage.TabIndex = 18;
			this.PB_LoadingPackage.TabStop = false;
			// 
			// TLP_Button
			// 
			this.TLP_Button.AutoSize = true;
			this.TLP_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Button.ColumnCount = 3;
			this.TLP_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Button.Controls.Add(this.B_Apply, 2, 0);
			this.TLP_Button.Controls.Add(this.L_Disclaimer, 1, 0);
			this.TLP_Button.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.TLP_Button.Location = new System.Drawing.Point(5, 586);
			this.TLP_Button.Name = "TLP_Button";
			this.TLP_Button.RowCount = 1;
			this.TLP_Button.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Button.Size = new System.Drawing.Size(932, 36);
			this.TLP_Button.TabIndex = 16;
			this.TLP_Button.Visible = false;
			// 
			// B_Apply
			// 
			this.B_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Apply.AutoSize = true;
			this.B_Apply.ColorShade = null;
			this.B_Apply.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "I_Link";
			this.B_Apply.ImageName = dynamicIcon4;
			this.B_Apply.Location = new System.Drawing.Point(713, 3);
			this.B_Apply.Name = "B_Apply";
			this.B_Apply.Size = new System.Drawing.Size(216, 30);
			this.B_Apply.SpaceTriggersClick = true;
			this.B_Apply.TabIndex = 16;
			this.B_Apply.Text = "SendReview";
			this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
			// 
			// L_Disclaimer
			// 
			this.L_Disclaimer.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.L_Disclaimer.AutoSize = true;
			this.L_Disclaimer.Location = new System.Drawing.Point(639, 3);
			this.L_Disclaimer.Name = "L_Disclaimer";
			this.L_Disclaimer.Size = new System.Drawing.Size(68, 30);
			this.L_Disclaimer.TabIndex = 18;
			this.L_Disclaimer.Text = "label1";
			// 
			// L_English
			// 
			this.L_English.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.L_English.AutoSize = true;
			this.TLP_MainInfo.SetColumnSpan(this.L_English, 5);
			this.L_English.Location = new System.Drawing.Point(861, 246);
			this.L_English.Name = "L_English";
			this.L_English.Size = new System.Drawing.Size(68, 30);
			this.L_English.TabIndex = 20;
			this.L_English.Text = "label1";
			// 
			// PC_RequestReview
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.P_Main);
			this.Controls.Add(this.TLP_Actions);
			this.Controls.Add(this.TLP_Button);
			this.Name = "PC_RequestReview";
			this.Size = new System.Drawing.Size(942, 627);
			this.Controls.SetChildIndex(this.TLP_Button, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Actions, 0);
			this.Controls.SetChildIndex(this.P_Main, 0);
			this.TLP_Actions.ResumeLayout(false);
			this.P_Main.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.P_Content.ResumeLayout(false);
			this.P_Content.PerformLayout();
			this.TLP_MainInfo.ResumeLayout(false);
			this.TLP_MainInfo.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.TLP_Top.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PB_LoadingPackage)).EndInit();
			this.TLP_Button.ResumeLayout(false);
			this.TLP_Button.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel TLP_Actions;
	private BigSelectionOptionControl B_ReportIssue;
	private BigSelectionOptionControl B_AddStatus;
	private BigSelectionOptionControl B_AddInteraction;
	private SlickControls.RoundedPanel P_Main;
	private System.Windows.Forms.Panel panel1;
	private System.Windows.Forms.Panel P_Content;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickTextBox TB_Note;
	private System.Windows.Forms.TableLayoutPanel TLP_Top;
	private PackageIcon PB_Icon;
	private PackageDescriptionControl P_Info;
	private SlickControls.SlickPictureBox PB_LoadingPackage;
	private System.Windows.Forms.TableLayoutPanel TLP_Button;
	private SlickControls.SlickButton B_Apply;
	private System.Windows.Forms.TableLayoutPanel TLP_MainInfo;
	private SlickControls.SlickTextBox TB_Note2;
	private PackageStabilityDropDown DD_Stability;
	private DlcDropDown DD_DLCs;
	private PackageUsageDropDown DD_Usage;
	private PackageTypeDropDown DD_PackageType;
	private System.Windows.Forms.Label L_Disclaimer;
	private System.Windows.Forms.Label L_English;
}
