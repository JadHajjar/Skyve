namespace Skyve.App.UserInterface.Generic;

partial class CarouselControl
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
			this.MainThumb = new SlickControls.SlickControl();
			this.roundedPanel = new SlickControls.RoundedPanel();
			this.P_Thumbs = new System.Windows.Forms.Panel();
			this.FLP_Thumbs = new System.Windows.Forms.Panel();
			this.slickScroll = new SlickControls.SlickScroll();
			this.roundedPanel.SuspendLayout();
			this.P_Thumbs.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainThumb
			// 
			this.MainThumb.Dock = System.Windows.Forms.DockStyle.Top;
			this.MainThumb.Location = new System.Drawing.Point(0, 0);
			this.MainThumb.Name = "MainThumb";
			this.MainThumb.Size = new System.Drawing.Size(872, 150);
			this.MainThumb.TabIndex = 0;
			this.MainThumb.Paint += new System.Windows.Forms.PaintEventHandler(this.MainThumb_Paint);
			this.MainThumb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainThumb_MouseClick);
			// 
			// roundedPanel
			// 
			this.roundedPanel.AutoSize = true;
			this.roundedPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.roundedPanel.Controls.Add(this.P_Thumbs);
			this.roundedPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.roundedPanel.Location = new System.Drawing.Point(0, 150);
			this.roundedPanel.Name = "roundedPanel";
			this.roundedPanel.Size = new System.Drawing.Size(872, 100);
			this.roundedPanel.TabIndex = 1;
			// 
			// P_Thumbs
			// 
			this.P_Thumbs.Controls.Add(this.FLP_Thumbs);
			this.P_Thumbs.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Thumbs.Location = new System.Drawing.Point(0, 0);
			this.P_Thumbs.Name = "P_Thumbs";
			this.P_Thumbs.Size = new System.Drawing.Size(872, 100);
			this.P_Thumbs.TabIndex = 3;
			// 
			// FLP_Thumbs
			// 
			this.FLP_Thumbs.AutoSize = true;
			this.FLP_Thumbs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FLP_Thumbs.Location = new System.Drawing.Point(44, 21);
			this.FLP_Thumbs.Name = "FLP_Thumbs";
			this.FLP_Thumbs.Size = new System.Drawing.Size(0, 0);
			this.FLP_Thumbs.TabIndex = 0;
			// 
			// slickScroll
			// 
			this.slickScroll.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickScroll.LinkedControl = this.FLP_Thumbs;
			this.slickScroll.Location = new System.Drawing.Point(0, 250);
			this.slickScroll.Name = "slickScroll";
			this.slickScroll.Size = new System.Drawing.Size(872, 20);
			this.slickScroll.Style = SlickControls.StyleType.Horizontal;
			this.slickScroll.TabIndex = 2;
			this.slickScroll.TabStop = false;
			this.slickScroll.Text = "slickScroll1";
			// 
			// CarouselControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.slickScroll);
			this.Controls.Add(this.roundedPanel);
			this.Controls.Add(this.MainThumb);
			this.Name = "CarouselControl";
			this.Size = new System.Drawing.Size(872, 585);
			this.roundedPanel.ResumeLayout(false);
			this.P_Thumbs.ResumeLayout(false);
			this.P_Thumbs.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private SlickControl MainThumb;
	private RoundedPanel roundedPanel;
	private SlickScroll slickScroll;
	private System.Windows.Forms.Panel FLP_Thumbs;
	private System.Windows.Forms.Panel P_Thumbs;
}
