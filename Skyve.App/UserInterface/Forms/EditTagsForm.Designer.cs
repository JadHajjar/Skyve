using Skyve.App.UserInterface.Lists;

namespace Skyve.App.UserInterface.Forms;

partial class EditTagsForm
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.B_Apply = new SlickControls.SlickButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.TLC = new Skyve.App.UserInterface.Lists.TagListControl();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.TB_NewTag = new SlickControls.SlickTextBox();
			this.L_MultipleWarning = new System.Windows.Forms.Label();
			this.base_P_Content.SuspendLayout();
			this.base_P_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.AutoScroll = true;
			this.base_P_Content.Controls.Add(this.tableLayoutPanel1);
			this.base_P_Content.Location = new System.Drawing.Point(1, 51);
			this.base_P_Content.Size = new System.Drawing.Size(387, 437);
			// 
			// base_P_Controls
			// 
			this.base_P_Controls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(235)))), ((int)(((byte)(243)))));
			this.base_P_Controls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.base_P_Controls.Location = new System.Drawing.Point(1, 43);
			// 
			// base_P_Top_Spacer
			// 
			this.base_P_Top_Spacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(235)))), ((int)(((byte)(243)))));
			this.base_P_Top_Spacer.Size = new System.Drawing.Size(387, 6);
			// 
			// base_P_Container
			// 
			this.base_P_Container.Size = new System.Drawing.Size(389, 489);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.B_Apply, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.TB_NewTag, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.L_MultipleWarning, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(387, 437);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// B_Apply
			// 
			this.B_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Apply.AutoSize = true;
			this.B_Apply.ButtonType = SlickControls.ButtonType.Active;
			this.B_Apply.ColorShade = null;
			this.B_Apply.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "Ok";
			this.B_Apply.ImageName = dynamicIcon1;
			this.B_Apply.Location = new System.Drawing.Point(300, 400);
			this.B_Apply.Name = "B_Apply";
			this.B_Apply.Size = new System.Drawing.Size(84, 34);
			this.B_Apply.SpaceTriggersClick = true;
			this.B_Apply.TabIndex = 100;
			this.B_Apply.Text = "Apply";
			this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
			// 
			// panel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
			this.panel1.Controls.Add(this.TLC);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 61);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(381, 333);
			this.panel1.TabIndex = 101;
			// 
			// TLC
			// 
			this.TLC.CurrentSearch = null;
			this.TLC.Location = new System.Drawing.Point(0, 0);
			this.TLC.Name = "TLC";
			this.TLC.Size = new System.Drawing.Size(150, 0);
			this.TLC.TabIndex = 2;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.TLC;
			this.slickScroll1.Location = new System.Drawing.Point(375, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(6, 333);
			this.slickScroll1.SmallHandle = true;
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 1;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// TB_NewTag
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.TB_NewTag, 2);
			this.TB_NewTag.Dock = System.Windows.Forms.DockStyle.Top;
			this.TB_NewTag.EnterTriggersClick = true;
			dynamicIcon2.Name = "Add";
			this.TB_NewTag.ImageName = dynamicIcon2;
			this.TB_NewTag.Location = new System.Drawing.Point(3, 3);
			this.TB_NewTag.Name = "TB_NewTag";
			this.TB_NewTag.Placeholder = "AddTagBox";
			this.TB_NewTag.SelectedText = "";
			this.TB_NewTag.SelectionLength = 0;
			this.TB_NewTag.SelectionStart = 0;
			this.TB_NewTag.ShowLabel = false;
			this.TB_NewTag.Size = new System.Drawing.Size(381, 52);
			this.TB_NewTag.TabIndex = 0;
			this.TB_NewTag.TextChanged += new System.EventHandler(this.TB_NewTag_TextChanged);
			this.TB_NewTag.IconClicked += new System.EventHandler(this.TB_NewTag_IconClicked);
			// 
			// L_MultipleWarning
			// 
			this.L_MultipleWarning.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_MultipleWarning.AutoSize = true;
			this.L_MultipleWarning.Location = new System.Drawing.Point(3, 407);
			this.L_MultipleWarning.Name = "L_MultipleWarning";
			this.L_MultipleWarning.Size = new System.Drawing.Size(45, 19);
			this.L_MultipleWarning.TabIndex = 103;
			this.L_MultipleWarning.Text = "label1";
			// 
			// EditTagsForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(400, 500);
			this.MaximizeBox = false;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 2560, 1380);
			this.MinimizeBox = false;
			this.Name = "EditTagsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AddLinkForm";
			this.base_P_Content.ResumeLayout(false);
			this.base_P_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickButton B_Apply;
	private System.Windows.Forms.Panel panel1;
	private SlickScroll slickScroll1;
	private TagListControl TLC;
	private SlickTextBox TB_NewTag;
	private System.Windows.Forms.Label L_MultipleWarning;
}