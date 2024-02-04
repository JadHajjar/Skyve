namespace Skyve.App.UserInterface.Panels;

partial class PC_MainPage
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
			_control?.Dispose();
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
			this.P_Container = new System.Windows.Forms.Panel();
			this.P_Board = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.P_Scroll = new System.Windows.Forms.Panel();
			this.TLP_FirstTime = new System.Windows.Forms.TableLayoutPanel();
			this.L_Info = new System.Windows.Forms.Label();
			this.I_Info = new SlickControls.SlickIcon();
			this.B_Dismiss = new SlickControls.SlickButton();
			this.P_Container.SuspendLayout();
			this.P_Scroll.SuspendLayout();
			this.TLP_FirstTime.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(150, 32);
			this.base_Text.Text = "Dashboard";
			// 
			// P_Container
			// 
			this.P_Container.Controls.Add(this.P_Board);
			this.P_Container.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Container.Location = new System.Drawing.Point(0, 74);
			this.P_Container.Name = "P_Container";
			this.P_Container.Size = new System.Drawing.Size(771, 364);
			this.P_Container.TabIndex = 2;
			this.P_Container.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Container_Paint);
			this.P_Container.Layout += new System.Windows.Forms.LayoutEventHandler(this.P_Board_Layout);
			// 
			// P_Board
			// 
			this.P_Board.AutoSize = true;
			this.P_Board.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Board.Location = new System.Drawing.Point(0, 0);
			this.P_Board.Name = "P_Board";
			this.P_Board.Size = new System.Drawing.Size(0, 0);
			this.P_Board.TabIndex = 0;
			this.P_Board.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Container_Paint);
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.P_Board;
			this.slickScroll1.Location = new System.Drawing.Point(2, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(10, 364);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 3;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// P_Scroll
			// 
			this.P_Scroll.Controls.Add(this.slickScroll1);
			this.P_Scroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.P_Scroll.Location = new System.Drawing.Point(771, 74);
			this.P_Scroll.Name = "P_Scroll";
			this.P_Scroll.Size = new System.Drawing.Size(12, 364);
			this.P_Scroll.TabIndex = 4;
			// 
			// TLP_FirstTime
			// 
			this.TLP_FirstTime.AutoSize = true;
			this.TLP_FirstTime.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_FirstTime.ColumnCount = 3;
			this.TLP_FirstTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_FirstTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_FirstTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_FirstTime.Controls.Add(this.L_Info, 1, 0);
			this.TLP_FirstTime.Controls.Add(this.I_Info, 0, 0);
			this.TLP_FirstTime.Controls.Add(this.B_Dismiss, 2, 0);
			this.TLP_FirstTime.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_FirstTime.Location = new System.Drawing.Point(0, 24);
			this.TLP_FirstTime.Name = "TLP_FirstTime";
			this.TLP_FirstTime.RowCount = 1;
			this.TLP_FirstTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_FirstTime.Size = new System.Drawing.Size(783, 50);
			this.TLP_FirstTime.TabIndex = 5;
			// 
			// L_Info
			// 
			this.L_Info.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Info.AutoSize = true;
			this.L_Info.Location = new System.Drawing.Point(48, 10);
			this.L_Info.Margin = new System.Windows.Forms.Padding(3, 10, 10, 10);
			this.L_Info.Name = "L_Info";
			this.L_Info.Size = new System.Drawing.Size(68, 30);
			this.L_Info.TabIndex = 21;
			this.L_Info.Text = "label1";
			this.L_Info.UseMnemonic = false;
			// 
			// I_Info
			// 
			this.I_Info.ActiveColor = null;
			this.I_Info.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.I_Info.ColorStyle = Extensions.ColorStyle.Icon;
			this.I_Info.Cursor = System.Windows.Forms.Cursors.Hand;
			this.I_Info.Enabled = false;
			dynamicIcon1.Name = "I_Info";
			this.I_Info.ImageName = dynamicIcon1;
			this.I_Info.Location = new System.Drawing.Point(10, 9);
			this.I_Info.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			this.I_Info.Name = "I_Info";
			this.I_Info.Selected = true;
			this.I_Info.Size = new System.Drawing.Size(32, 32);
			this.I_Info.TabIndex = 20;
			this.I_Info.TabStop = false;
			// 
			// B_Dismiss
			// 
			this.B_Dismiss.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Dismiss.AutoSize = true;
			this.B_Dismiss.ColorShade = null;
			this.B_Dismiss.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Ok";
			this.B_Dismiss.ImageName = dynamicIcon2;
			this.B_Dismiss.Location = new System.Drawing.Point(741, 9);
			this.B_Dismiss.Name = "B_Dismiss";
			this.B_Dismiss.Size = new System.Drawing.Size(39, 32);
			this.B_Dismiss.SpaceTriggersClick = true;
			this.B_Dismiss.TabIndex = 22;
			this.B_Dismiss.Text = "Ok";
			this.B_Dismiss.Click += new System.EventHandler(this.B_Dismiss_Click);
			// 
			// PC_MainPage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.P_Container);
			this.Controls.Add(this.P_Scroll);
			this.Controls.Add(this.TLP_FirstTime);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_MainPage";
			this.Padding = new System.Windows.Forms.Padding(0, 24, 0, 0);
			this.Text = "Dashboard";
			this.Controls.SetChildIndex(this.TLP_FirstTime, 0);
			this.Controls.SetChildIndex(this.P_Scroll, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.P_Container, 0);
			this.P_Container.ResumeLayout(false);
			this.P_Container.PerformLayout();
			this.P_Scroll.ResumeLayout(false);
			this.TLP_FirstTime.ResumeLayout(false);
			this.TLP_FirstTime.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.Panel P_Container;
	private System.Windows.Forms.Panel P_Board;
	private SlickScroll slickScroll1;
	private System.Windows.Forms.Panel P_Scroll;
	private System.Windows.Forms.TableLayoutPanel TLP_FirstTime;
	private System.Windows.Forms.Label L_Info;
	private SlickIcon I_Info;
	private SlickButton B_Dismiss;
}
