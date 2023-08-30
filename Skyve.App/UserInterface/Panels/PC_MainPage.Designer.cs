using Skyve.App.UserInterface.Bubbles;

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
			_notifier.ContentLoaded -= SetButtonEnabledOnLoad;
			_citiesManager.MonitorTick -= CitiesManager_MonitorTick;
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
			this.P_Container = new System.Windows.Forms.Panel();
			this.P_Board = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.P_Scroll = new System.Windows.Forms.Panel();
			this.P_Container.SuspendLayout();
			this.P_Scroll.SuspendLayout();
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
			this.P_Container.Location = new System.Drawing.Point(0, 30);
			this.P_Container.Name = "P_Container";
			this.P_Container.Size = new System.Drawing.Size(771, 408);
			this.P_Container.TabIndex = 2;
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
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.P_Board;
			this.slickScroll1.Location = new System.Drawing.Point(6, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(6, 408);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 3;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// P_Scroll
			// 
			this.P_Scroll.Controls.Add(this.slickScroll1);
			this.P_Scroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.P_Scroll.Location = new System.Drawing.Point(771, 30);
			this.P_Scroll.Name = "P_Scroll";
			this.P_Scroll.Size = new System.Drawing.Size(12, 408);
			this.P_Scroll.TabIndex = 4;
			// 
			// PC_MainPage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.P_Container);
			this.Controls.Add(this.P_Scroll);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_MainPage";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Text = "Dashboard";
			this.Controls.SetChildIndex(this.P_Scroll, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.P_Container, 0);
			this.P_Container.ResumeLayout(false);
			this.P_Container.PerformLayout();
			this.P_Scroll.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.Panel P_Container;
	private System.Windows.Forms.Panel P_Board;
	private SlickScroll slickScroll1;
	private System.Windows.Forms.Panel P_Scroll;
}
