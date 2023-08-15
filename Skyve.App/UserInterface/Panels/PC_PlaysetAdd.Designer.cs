using SkyveApp.UserInterface.Generic;


namespace SkyveApp.UserInterface.Panels;

partial class PC_PlaysetAdd
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PC_PlaysetAdd));
			this.TLP_New = new System.Windows.Forms.TableLayoutPanel();
			this.newProfileOptionControl1 = new SkyveApp.UserInterface.Generic.NewProfileOptionControl();
			this.newProfileOptionControl2 = new SkyveApp.UserInterface.Generic.NewProfileOptionControl();
			this.newProfileOptionControl3 = new SkyveApp.UserInterface.Generic.NewProfileOptionControl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.B_Cancel = new SlickControls.SlickButton();
			this.DAD_NewProfile = new SkyveApp.UserInterface.Generic.DragAndDropControl();
			this.TLP_New.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Text = "AddProfile";
			// 
			// TLP_New
			// 
			this.TLP_New.ColumnCount = 3;
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_New.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_New.Controls.Add(this.newProfileOptionControl1, 1, 1);
			this.TLP_New.Controls.Add(this.newProfileOptionControl2, 1, 2);
			this.TLP_New.Controls.Add(this.newProfileOptionControl3, 1, 3);
			this.TLP_New.Controls.Add(this.tableLayoutPanel1, 0, 4);
			this.TLP_New.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_New.Location = new System.Drawing.Point(0, 30);
			this.TLP_New.Name = "TLP_New";
			this.TLP_New.RowCount = 5;
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this.TLP_New.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_New.Size = new System.Drawing.Size(1182, 789);
			this.TLP_New.TabIndex = 0;
			// 
			// newProfileOptionControl1
			// 
			this.newProfileOptionControl1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.newProfileOptionControl1.FromLink = false;
			this.newProfileOptionControl1.FromScratch = true;
			this.newProfileOptionControl1.Location = new System.Drawing.Point(516, 143);
			this.newProfileOptionControl1.Name = "newProfileOptionControl1";
			this.newProfileOptionControl1.Size = new System.Drawing.Size(150, 150);
			this.newProfileOptionControl1.TabIndex = 0;
			this.newProfileOptionControl1.Click += new System.EventHandler(this.NewProfile_Click);
			// 
			// newProfileOptionControl2
			// 
			this.newProfileOptionControl2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.newProfileOptionControl2.FromLink = false;
			this.newProfileOptionControl2.FromScratch = false;
			this.newProfileOptionControl2.Location = new System.Drawing.Point(516, 299);
			this.newProfileOptionControl2.Name = "newProfileOptionControl2";
			this.newProfileOptionControl2.Size = new System.Drawing.Size(150, 110);
			this.newProfileOptionControl2.TabIndex = 1;
			this.newProfileOptionControl2.Click += new System.EventHandler(this.CopyProfile_Click);
			// 
			// newProfileOptionControl3
			// 
			this.newProfileOptionControl3.Cursor = System.Windows.Forms.Cursors.Hand;
			this.newProfileOptionControl3.FromLink = true;
			this.newProfileOptionControl3.FromScratch = false;
			this.newProfileOptionControl3.Location = new System.Drawing.Point(516, 415);
			this.newProfileOptionControl3.Name = "newProfileOptionControl3";
			this.newProfileOptionControl3.Size = new System.Drawing.Size(150, 110);
			this.newProfileOptionControl3.TabIndex = 2;
			this.newProfileOptionControl3.Click += new System.EventHandler(this.B_ImportLink_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.TLP_New.SetColumnSpan(this.tableLayoutPanel1, 3);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.B_Cancel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.DAD_NewProfile, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 531);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1176, 255);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Cancel.AutoSize = true;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("B_Cancel.Image")));
			this.B_Cancel.Location = new System.Drawing.Point(1086, 213);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(10);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
			this.B_Cancel.Size = new System.Drawing.Size(80, 32);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 14;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// DAD_NewProfile
			// 
			this.DAD_NewProfile.AllowDrop = true;
			this.DAD_NewProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.DAD_NewProfile.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DAD_NewProfile.Location = new System.Drawing.Point(3, 102);
			this.DAD_NewProfile.Name = "DAD_NewProfile";
			this.DAD_NewProfile.Size = new System.Drawing.Size(150, 150);
			this.DAD_NewProfile.TabIndex = 15;
			this.DAD_NewProfile.Text = "DropNewPlayset";
			this.DAD_NewProfile.ValidExtensions = new string[] {
        ".json",
        ".xml"};
			this.DAD_NewProfile.FileSelected += new System.Action<string>(this.DAD_NewProfile_FileSelected);
			// 
			// PC_ProfileAdd
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_New);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_ProfileAdd";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1182, 819);
			this.Text = "AddProfile";
			this.Controls.SetChildIndex(this.TLP_New, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.TLP_New.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private System.Windows.Forms.TableLayoutPanel TLP_New;
	private NewProfileOptionControl newProfileOptionControl1;
	private NewProfileOptionControl newProfileOptionControl2;
	private SlickControls.SlickButton B_Cancel;
	private DragAndDropControl DAD_NewProfile;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private NewProfileOptionControl newProfileOptionControl3;
}
