namespace Skyve.App.UserInterface.Panels;

partial class PC_WorkshopPackageSelection
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
			this.base_P_Side = new System.Windows.Forms.Panel();
			this.base_TLP_Side = new SlickControls.RoundedTableLayoutPanel();
			this.base_P_Side.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Location = new System.Drawing.Point(-2, -27);
			this.base_Text.Size = new System.Drawing.Size(150, 39);
			// 
			// base_P_Side
			// 
			this.base_P_Side.Controls.Add(this.base_TLP_Side);
			this.base_P_Side.Dock = System.Windows.Forms.DockStyle.Left;
			this.base_P_Side.Location = new System.Drawing.Point(0, 0);
			this.base_P_Side.Name = "base_P_Side";
			this.base_P_Side.Size = new System.Drawing.Size(165, 438);
			this.base_P_Side.TabIndex = 18;
			// 
			// base_TLP_Side
			// 
			this.base_TLP_Side.BotLeft = true;
			this.base_TLP_Side.ColumnCount = 1;
			this.base_TLP_Side.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.base_TLP_Side.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.base_TLP_Side.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.base_TLP_Side.Dock = System.Windows.Forms.DockStyle.Fill;
			this.base_TLP_Side.Location = new System.Drawing.Point(0, 0);
			this.base_TLP_Side.Name = "base_TLP_Side";
			this.base_TLP_Side.RowCount = 1;
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.base_TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.base_TLP_Side.Size = new System.Drawing.Size(165, 438);
			this.base_TLP_Side.TabIndex = 43;
			this.base_TLP_Side.TopLeft = true;
			// 
			// PC_WorkshopPackageSelection
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.base_P_Side);
			this.Name = "PC_WorkshopPackageSelection";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.base_P_Side, 0);
			this.base_P_Side.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.Panel base_P_Side;
	internal RoundedTableLayoutPanel base_TLP_Side;
}
