﻿namespace Skyve.App.UserInterface.Panels;

partial class DashboardPanelControl
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
		if (disposing)
		{
			Program.MainForm.GlobalMouseMove -= GlobalMouseMove;
			Program.MainForm.Deactivate -= Form_Deactivate;
			Program.MainForm.WindowStateChanged -= Form_WindowStateChanged;

			_control?.Dispose();
			components?.Dispose();
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
			this.slickScroll = new SlickControls.SlickScroll();
			this.P_Board = new SlickControls.SlickControl();
			this.P_Scroll = new System.Windows.Forms.Panel();
			this.FLP_AvailableWidgets = new SlickControls.RoundedGroupFlowLayoutPanel();
			this.P_AvailableWidgets = new System.Windows.Forms.Panel();
			this.P_Container = new SlickControls.SlickControl();
			this.P_Scroll.SuspendLayout();
			this.P_AvailableWidgets.SuspendLayout();
			this.P_Container.SuspendLayout();
			this.SuspendLayout();
			// 
			// slickScroll
			// 
			this.slickScroll.AnimatedValue = 8;
			this.slickScroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll.LinkedControl = this.P_Board;
			this.slickScroll.Location = new System.Drawing.Point(-4, 0);
			this.slickScroll.Name = "slickScroll";
			this.slickScroll.Size = new System.Drawing.Size(16, 382);
			this.slickScroll.Style = SlickControls.StyleType.Vertical;
			this.slickScroll.TabIndex = 3;
			this.slickScroll.TabStop = false;
			this.slickScroll.TargetAnimationValue = 8;
			this.slickScroll.Text = "slickScroll1";
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
			this.P_Board.MouseClick += new System.Windows.Forms.MouseEventHandler(this.P_Board_MouseClick);
			this.P_Board.MouseMove += new System.Windows.Forms.MouseEventHandler(this.P_Board_MouseMove);
			// 
			// P_Scroll
			// 
			this.P_Scroll.Controls.Add(this.slickScroll);
			this.P_Scroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.P_Scroll.Location = new System.Drawing.Point(775, 24);
			this.P_Scroll.Name = "P_Scroll";
			this.P_Scroll.Size = new System.Drawing.Size(12, 382);
			this.P_Scroll.TabIndex = 4;
			// 
			// FLP_AvailableWidgets
			// 
			this.FLP_AvailableWidgets.AddOutline = true;
			this.FLP_AvailableWidgets.AddShadow = true;
			this.FLP_AvailableWidgets.AutoSize = true;
			this.FLP_AvailableWidgets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FLP_AvailableWidgets.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon1.Name = "Grid";
			this.FLP_AvailableWidgets.ImageName = dynamicIcon1;
			this.FLP_AvailableWidgets.Location = new System.Drawing.Point(0, 0);
			this.FLP_AvailableWidgets.Name = "FLP_AvailableWidgets";
			this.FLP_AvailableWidgets.Padding = new System.Windows.Forms.Padding(16, 53, 16, 16);
			this.FLP_AvailableWidgets.Size = new System.Drawing.Size(787, 69);
			this.FLP_AvailableWidgets.TabIndex = 6;
			this.FLP_AvailableWidgets.Text = "AvailableWidgets";
			// 
			// P_AvailableWidgets
			// 
			this.P_AvailableWidgets.AutoSize = true;
			this.P_AvailableWidgets.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_AvailableWidgets.Controls.Add(this.FLP_AvailableWidgets);
			this.P_AvailableWidgets.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.P_AvailableWidgets.Location = new System.Drawing.Point(0, 406);
			this.P_AvailableWidgets.Name = "P_AvailableWidgets";
			this.P_AvailableWidgets.Size = new System.Drawing.Size(787, 69);
			this.P_AvailableWidgets.TabIndex = 7;
			this.P_AvailableWidgets.Visible = false;
			// 
			// P_Container
			// 
			this.P_Container.Controls.Add(this.P_Board);
			this.P_Container.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Container.Location = new System.Drawing.Point(0, 24);
			this.P_Container.Name = "P_Container";
			this.P_Container.Size = new System.Drawing.Size(775, 382);
			this.P_Container.TabIndex = 2;
			this.P_Container.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Container_Paint);
			this.P_Container.Layout += new System.Windows.Forms.LayoutEventHandler(this.P_Board_Layout);
			this.P_Container.MouseClick += new System.Windows.Forms.MouseEventHandler(this.P_Board_MouseClick);
			this.P_Container.MouseMove += new System.Windows.Forms.MouseEventHandler(this.P_Board_MouseMove);
			this.P_Container.Resize += new System.EventHandler(this.P_Container_Resize);
			// 
			// DashboardPanelControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.P_Container);
			this.Controls.Add(this.P_Scroll);
			this.Controls.Add(this.P_AvailableWidgets);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "DashboardPanelControl";
			this.Size = new System.Drawing.Size(787, 475);
			this.P_Scroll.ResumeLayout(false);
			this.P_AvailableWidgets.ResumeLayout(false);
			this.P_AvailableWidgets.PerformLayout();
			this.P_Container.ResumeLayout(false);
			this.P_Container.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private SlickScroll slickScroll;
	private System.Windows.Forms.Panel P_Scroll;
	private RoundedGroupFlowLayoutPanel FLP_AvailableWidgets;
	private System.Windows.Forms.Panel P_AvailableWidgets;
	private SlickControl P_Board;
	private SlickControl P_Container;
}
