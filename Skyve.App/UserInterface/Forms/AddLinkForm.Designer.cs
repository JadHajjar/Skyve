namespace Skyve.App.UserInterface.Forms;

partial class AddLinkForm
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
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.B_AddLink = new SlickControls.SlickButton();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.B_Apply = new SlickControls.SlickButton();
			this.base_P_Content.SuspendLayout();
			this.base_P_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			this.TLP.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.AutoScroll = true;
			this.base_P_Content.Controls.Add(this.tableLayoutPanel1);
			this.base_P_Content.Location = new System.Drawing.Point(1, 43);
			this.base_P_Content.Size = new System.Drawing.Size(367, 345);
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
			this.base_P_Top_Spacer.Size = new System.Drawing.Size(367, 6);
			// 
			// base_P_Container
			// 
			this.base_P_Container.Size = new System.Drawing.Size(369, 389);
			// 
			// TLP
			// 
			this.TLP.AutoSize = true;
			this.TLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.ColumnCount = 1;
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP.Controls.Add(this.B_AddLink, 0, 0);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP.Location = new System.Drawing.Point(3, 3);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 1;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.Size = new System.Drawing.Size(361, 36);
			this.TLP.TabIndex = 0;
			// 
			// B_AddLink
			// 
			this.B_AddLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.B_AddLink.ColorShade = null;
			this.B_AddLink.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Add";
			this.B_AddLink.ImageName = dynamicIcon2;
			this.B_AddLink.Location = new System.Drawing.Point(3, 3);
			this.B_AddLink.Name = "B_AddLink";
			this.B_AddLink.Size = new System.Drawing.Size(355, 30);
			this.B_AddLink.SpaceTriggersClick = true;
			this.B_AddLink.TabIndex = 0;
			this.B_AddLink.Text = "Add Link";
			this.B_AddLink.Click += new System.EventHandler(this.B_AddLink_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.B_Apply, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.TLP, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(367, 345);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// B_Apply
			// 
			this.B_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Apply.AutoSize = true;
			this.B_Apply.ColorShade = null;
			this.B_Apply.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "I_Ok";
			this.B_Apply.ImageName = dynamicIcon1;
			this.B_Apply.Location = new System.Drawing.Point(314, 319);
			this.B_Apply.Name = "B_Apply";
			this.B_Apply.Size = new System.Drawing.Size(50, 23);
			this.B_Apply.SpaceTriggersClick = true;
			this.B_Apply.TabIndex = 100;
			this.B_Apply.Text = "Apply";
			this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
			// 
			// AddLinkForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(380, 400);
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 2560, 1380);
			this.Name = "AddLinkForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AddLinkForm";
			this.base_P_Content.ResumeLayout(false);
			this.base_P_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			this.TLP.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel TLP;
	private SlickControls.SlickButton B_AddLink;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickButton B_Apply;
}