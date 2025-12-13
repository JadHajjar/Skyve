namespace Skyve.App.UserInterface.Generic;

partial class WorkshopTagsContainer
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.scroll = new SlickControls.SlickScroll();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.title = new System.Windows.Forms.Label();
			this.buttonClear = new SlickControls.SlickButton();
			this.tagControl = new Skyve.App.UserInterface.Generic.WorkshopTagsControl();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.scroll, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.title, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonClear, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 754);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
			this.panel1.Controls.Add(this.tagControl);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(78, 39);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(385, 712);
			this.panel1.TabIndex = 1;
			// 
			// scroll
			// 
			this.scroll.AnimatedValue = 6;
			this.scroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.scroll.LinkedControl = null;
			this.scroll.Location = new System.Drawing.Point(466, 0);
			this.scroll.Margin = new System.Windows.Forms.Padding(0);
			this.scroll.Name = "scroll";
			this.tableLayoutPanel1.SetRowSpan(this.scroll, 2);
			this.scroll.Size = new System.Drawing.Size(6, 754);
			this.scroll.SmallHandle = true;
			this.scroll.Style = SlickControls.StyleType.Vertical;
			this.scroll.TabIndex = 1;
			this.scroll.TabStop = false;
			this.scroll.TargetAnimationValue = 6;
			this.scroll.Text = "slickScroll1";
			this.scroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scroll_Scroll);
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Left;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 0);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.tableLayoutPanel1.SetRowSpan(this.slickSpacer1, 2);
			this.slickSpacer1.Size = new System.Drawing.Size(75, 754);
			this.slickSpacer1.TabIndex = 2;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// title
			// 
			this.title.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.title.AutoSize = true;
			this.title.Location = new System.Drawing.Point(248, 10);
			this.title.Name = "title";
			this.title.Size = new System.Drawing.Size(44, 16);
			this.title.TabIndex = 3;
			this.title.Text = "label1";
			// 
			// buttonClear
			// 
			this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.buttonClear.AutoSize = true;
			this.buttonClear.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonClear.Location = new System.Drawing.Point(469, 3);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(1, 30);
			this.buttonClear.SpaceTriggersClick = true;
			this.buttonClear.TabIndex = 4;
			this.buttonClear.Text = "buttonClear";
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// tagControl
			// 
			this.tagControl.Cursor = System.Windows.Forms.Cursors.Default;
			this.tagControl.Location = new System.Drawing.Point(0, 0);
			this.tagControl.Margin = new System.Windows.Forms.Padding(0);
			this.tagControl.MinimumSize = new System.Drawing.Size(1, 1);
			this.tagControl.Name = "tagControl";
			this.tagControl.Size = new System.Drawing.Size(382, 1);
			this.tagControl.TabIndex = 4;
			// 
			// WorkshopTagsContainer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "WorkshopTagsContainer";
			this.Size = new System.Drawing.Size(472, 754);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickScroll scroll;
	private SlickSpacer slickSpacer1;
	private System.Windows.Forms.Label title;
	private WorkshopTagsControl tagControl;
	private System.Windows.Forms.Panel panel1;
	private SlickButton buttonClear;
}
