using Skyve.App.UserInterface.Lists;

namespace Skyve.App.UserInterface.Panels;

partial class PC_ReviewRequests
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
			this.TLP_List = new SlickControls.RoundedTableLayoutPanel();
			this.packageCrList = new Skyve.App.UserInterface.Lists.PackageCrList();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.reviewRequestList1 = new Skyve.App.UserInterface.Lists.ReviewRequestList();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.B_DeleteRequests = new SlickControls.SlickButton();
			this.TLP_List.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// TLP_List
			// 
			this.TLP_List.AddOutline = true;
			this.TLP_List.ColumnCount = 1;
			this.TLP_List.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_List.Controls.Add(this.packageCrList, 0, 1);
			this.TLP_List.Controls.Add(this.TB_Search, 0, 0);
			this.TLP_List.Dock = System.Windows.Forms.DockStyle.Left;
			this.TLP_List.Location = new System.Drawing.Point(5, 30);
			this.TLP_List.Name = "TLP_List";
			this.TLP_List.RowCount = 2;
			this.TLP_List.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_List.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_List.Size = new System.Drawing.Size(200, 403);
			this.TLP_List.TabIndex = 19;
			// 
			// packageCrList
			// 
			this.packageCrList.AutoInvalidate = false;
			this.packageCrList.AutoScroll = true;
			this.packageCrList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.packageCrList.HighlightOnHover = true;
			this.packageCrList.ItemHeight = 32;
			this.packageCrList.Location = new System.Drawing.Point(3, 72);
			this.packageCrList.Name = "packageCrList";
			this.packageCrList.SeparateWithLines = true;
			this.packageCrList.Size = new System.Drawing.Size(194, 328);
			this.packageCrList.TabIndex = 0;
			this.packageCrList.ItemMouseClick += new System.EventHandler<System.Windows.Forms.MouseEventArgs>(this.packageCrList_ItemMouseClick);
			// 
			// TB_Search
			// 
			this.TB_Search.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon1.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon1;
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(0, 24, 0, 24);
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(194, 63);
			this.TB_Search.TabIndex = 1;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// reviewRequestList1
			// 
			this.reviewRequestList1.AutoInvalidate = false;
			this.reviewRequestList1.AutoScroll = true;
			this.reviewRequestList1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.reviewRequestList1.ItemHeight = 64;
			this.reviewRequestList1.Location = new System.Drawing.Point(3, 3);
			this.reviewRequestList1.Name = "reviewRequestList1";
			this.reviewRequestList1.SeparateWithLines = true;
			this.reviewRequestList1.Size = new System.Drawing.Size(567, 361);
			this.reviewRequestList1.TabIndex = 20;
			this.reviewRequestList1.ItemMouseClick += new System.EventHandler<System.Windows.Forms.MouseEventArgs>(this.reviewRequestList1_ItemMouseClick);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.reviewRequestList1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.B_DeleteRequests, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(205, 30);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(573, 403);
			this.tableLayoutPanel1.TabIndex = 21;
			// 
			// B_DeleteRequests
			// 
			this.B_DeleteRequests.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_DeleteRequests.ColorShade = null;
			this.B_DeleteRequests.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Disposable";
			this.B_DeleteRequests.ImageName = dynamicIcon2;
			this.B_DeleteRequests.Location = new System.Drawing.Point(470, 370);
			this.B_DeleteRequests.Name = "B_DeleteRequests";
			this.B_DeleteRequests.Size = new System.Drawing.Size(100, 30);
			this.B_DeleteRequests.SpaceTriggersClick = true;
			this.B_DeleteRequests.TabIndex = 21;
			this.B_DeleteRequests.Text = "Delete these requests";
			this.B_DeleteRequests.Visible = false;
			this.B_DeleteRequests.Click += new System.EventHandler(this.B_DeleteRequests_Click);
			// 
			// PC_ReviewRequests
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.TLP_List);
			this.Name = "PC_ReviewRequests";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_List, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.TLP_List.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private SlickControls.RoundedTableLayoutPanel TLP_List;
	private PackageCrList packageCrList;
	private SlickControls.SlickTextBox TB_Search;
	private ReviewRequestList reviewRequestList1;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickButton B_DeleteRequests;
}
