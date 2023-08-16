using Skyve.App.UserInterface.Dropdowns;

namespace Skyve.App.UserInterface.CompatibilityReport;

partial class IPackageStatusControl<T, TBase>
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
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			this.CloseIcon = new SlickControls.TopIcon();
			this.P_Main = new SlickControls.RoundedTableLayoutPanel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.TB_Note = new SlickControls.SlickTextBox();
			this.I_Note = new SlickControls.SlickIcon();
			this.L_LinkedPackages = new System.Windows.Forms.Label();
			this.I_AddPackage = new SlickControls.SlickIcon();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.P_Packages = new System.Windows.Forms.Panel();
			this.I_Paste = new SlickControls.SlickIcon();
			this.I_Copy = new SlickControls.SlickIcon();
			this.L_OutputTitle = new System.Windows.Forms.Label();
			this.L_Output = new SlickControls.SlickControl();
			this.DD_Action = new Skyve.App.UserInterface.Dropdowns.PackageActionDropDown();
			((System.ComponentModel.ISupportInitialize)(this.CloseIcon)).BeginInit();
			this.P_Main.SuspendLayout();
			this.SuspendLayout();
			// 
			// CloseIcon
			// 
			this.CloseIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseIcon.AnimatedValue = 0;
			this.CloseIcon.Color = SlickControls.TopIcon.IconStyle.Close;
			this.CloseIcon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CloseIcon.Location = new System.Drawing.Point(367, 3);
			this.CloseIcon.Name = "CloseIcon";
			this.CloseIcon.Size = new System.Drawing.Size(16, 16);
			this.CloseIcon.TabIndex = 0;
			this.CloseIcon.TabStop = false;
			this.CloseIcon.Click += new System.EventHandler(this.CloseIcon_Click);
			// 
			// P_Main
			// 
			this.P_Main.AutoSize = true;
			this.P_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Main.ColumnCount = 4;
			this.P_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Main.Controls.Add(this.slickSpacer2, 0, 6);
			this.P_Main.Controls.Add(this.TB_Note, 0, 2);
			this.P_Main.Controls.Add(this.CloseIcon, 3, 0);
			this.P_Main.Controls.Add(this.DD_Action, 0, 1);
			this.P_Main.Controls.Add(this.I_Note, 3, 1);
			this.P_Main.Controls.Add(this.L_LinkedPackages, 0, 4);
			this.P_Main.Controls.Add(this.I_AddPackage, 3, 4);
			this.P_Main.Controls.Add(this.slickSpacer1, 0, 3);
			this.P_Main.Controls.Add(this.P_Packages, 0, 5);
			this.P_Main.Controls.Add(this.I_Paste, 2, 4);
			this.P_Main.Controls.Add(this.I_Copy, 1, 4);
			this.P_Main.Controls.Add(this.L_OutputTitle, 0, 7);
			this.P_Main.Controls.Add(this.L_Output, 0, 8);
			this.P_Main.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Main.Location = new System.Drawing.Point(0, 0);
			this.P_Main.Name = "P_Main";
			this.P_Main.RowCount = 9;
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Main.Size = new System.Drawing.Size(386, 331);
			this.P_Main.TabIndex = 1;
			// 
			// slickSpacer2
			// 
			this.P_Main.SetColumnSpan(this.slickSpacer2, 4);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(3, 289);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(380, 1);
			this.slickSpacer2.TabIndex = 24;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// TB_Note
			// 
			this.P_Main.SetColumnSpan(this.TB_Note, 4);
			this.TB_Note.Dock = System.Windows.Forms.DockStyle.Top;
			this.TB_Note.LabelText = "Note";
			this.TB_Note.Location = new System.Drawing.Point(3, 75);
			this.TB_Note.MultiLine = true;
			this.TB_Note.Name = "TB_Note";
			this.TB_Note.SelectedText = "";
			this.TB_Note.SelectionLength = 0;
			this.TB_Note.SelectionStart = 0;
			this.TB_Note.Size = new System.Drawing.Size(380, 151);
			this.TB_Note.TabIndex = 19;
			this.TB_Note.Visible = false;
			// 
			// I_Note
			// 
			this.I_Note.ActiveColor = null;
			this.I_Note.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.I_Note.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "I_Note";
			this.I_Note.ImageName = dynamicIcon1;
			this.I_Note.Location = new System.Drawing.Point(309, 25);
			this.I_Note.Name = "I_Note";
			this.I_Note.Size = new System.Drawing.Size(74, 44);
			this.I_Note.TabIndex = 20;
			this.I_Note.Click += new System.EventHandler(this.I_Note_Click);
			// 
			// L_LinkedPackages
			// 
			this.L_LinkedPackages.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_LinkedPackages.AutoSize = true;
			this.L_LinkedPackages.Location = new System.Drawing.Point(3, 253);
			this.L_LinkedPackages.Name = "L_LinkedPackages";
			this.L_LinkedPackages.Size = new System.Drawing.Size(44, 16);
			this.L_LinkedPackages.TabIndex = 21;
			this.L_LinkedPackages.Text = "label1";
			// 
			// I_AddPackage
			// 
			this.I_AddPackage.ActiveColor = null;
			this.I_AddPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.I_AddPackage.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Add";
			this.I_AddPackage.ImageName = dynamicIcon2;
			this.I_AddPackage.Location = new System.Drawing.Point(309, 239);
			this.I_AddPackage.Name = "I_AddPackage";
			this.I_AddPackage.Size = new System.Drawing.Size(74, 44);
			this.I_AddPackage.TabIndex = 20;
			this.I_AddPackage.Click += new System.EventHandler(this.I_AddPackage_Click);
			// 
			// slickSpacer1
			// 
			this.P_Main.SetColumnSpan(this.slickSpacer1, 4);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(3, 232);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(380, 1);
			this.slickSpacer1.TabIndex = 22;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// P_Packages
			// 
			this.P_Packages.AutoSize = true;
			this.P_Packages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Main.SetColumnSpan(this.P_Packages, 4);
			this.P_Packages.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Packages.Location = new System.Drawing.Point(0, 286);
			this.P_Packages.Margin = new System.Windows.Forms.Padding(0);
			this.P_Packages.Name = "P_Packages";
			this.P_Packages.Size = new System.Drawing.Size(386, 0);
			this.P_Packages.TabIndex = 23;
			this.P_Packages.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.RefreshOutput);
			this.P_Packages.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.RefreshOutput);
			// 
			// I_Paste
			// 
			this.I_Paste.ActiveColor = null;
			this.I_Paste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.I_Paste.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_Paste";
			this.I_Paste.ImageName = dynamicIcon3;
			this.I_Paste.Location = new System.Drawing.Point(229, 239);
			this.I_Paste.Name = "I_Paste";
			this.I_Paste.Size = new System.Drawing.Size(74, 44);
			this.I_Paste.TabIndex = 20;
			this.I_Paste.Click += new System.EventHandler(this.I_Paste_Click);
			// 
			// I_Copy
			// 
			this.I_Copy.ActiveColor = null;
			this.I_Copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.I_Copy.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "I_Copy";
			this.I_Copy.ImageName = dynamicIcon4;
			this.I_Copy.Location = new System.Drawing.Point(149, 239);
			this.I_Copy.Name = "I_Copy";
			this.I_Copy.Size = new System.Drawing.Size(74, 44);
			this.I_Copy.TabIndex = 20;
			this.I_Copy.Click += new System.EventHandler(this.I_Copy_Click);
			// 
			// L_OutputTitle
			// 
			this.L_OutputTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.L_OutputTitle.AutoSize = true;
			this.P_Main.SetColumnSpan(this.L_OutputTitle, 4);
			this.L_OutputTitle.Location = new System.Drawing.Point(171, 293);
			this.L_OutputTitle.Name = "L_OutputTitle";
			this.L_OutputTitle.Size = new System.Drawing.Size(44, 16);
			this.L_OutputTitle.TabIndex = 21;
			this.L_OutputTitle.Text = "label1";
			// 
			// L_Output
			// 
			this.P_Main.SetColumnSpan(this.L_Output, 4);
			this.L_Output.Dock = System.Windows.Forms.DockStyle.Top;
			this.L_Output.Location = new System.Drawing.Point(3, 312);
			this.L_Output.Name = "L_Output";
			this.L_Output.Size = new System.Drawing.Size(380, 16);
			this.L_Output.TabIndex = 21;
			this.L_Output.Paint += new System.Windows.Forms.PaintEventHandler(this.L_Output_Paint);
			// 
			// DD_Action
			// 
			this.P_Main.SetColumnSpan(this.DD_Action, 3);
			this.DD_Action.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DD_Action.Dock = System.Windows.Forms.DockStyle.Top;
			this.DD_Action.Location = new System.Drawing.Point(3, 25);
			this.DD_Action.Name = "DD_Action";
			this.DD_Action.Size = new System.Drawing.Size(300, 44);
			this.DD_Action.TabIndex = 1;
			this.DD_Action.Text = "Recommended Action";
			this.DD_Action.SelectedItemChanged += new System.EventHandler(this.RefreshOutput);
			// 
			// IPackageStatusControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.P_Main);
			this.Name = "IPackageStatusControl";
			this.Size = new System.Drawing.Size(386, 360);
			((System.ComponentModel.ISupportInitialize)(this.CloseIcon)).EndInit();
			this.P_Main.ResumeLayout(false);
			this.P_Main.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion
	private SlickControls.TopIcon CloseIcon;
	private SlickControls.RoundedTableLayoutPanel P_Main;
	private PackageActionDropDown DD_Action;
	private SlickControls.SlickTextBox TB_Note;
	private SlickControls.SlickIcon I_Note;
	private SlickControls.SlickIcon I_AddPackage;
	private System.Windows.Forms.Label L_LinkedPackages;
	private SlickControls.SlickSpacer slickSpacer1;
	private System.Windows.Forms.Panel P_Packages;
	private SlickControls.SlickIcon I_Paste;
	private SlickControls.SlickIcon I_Copy;
	private SlickControls.SlickSpacer slickSpacer2;
	private System.Windows.Forms.Label L_OutputTitle;
	private SlickControls.SlickControl L_Output;
}
