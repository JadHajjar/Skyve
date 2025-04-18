﻿using Skyve.App.UserInterface.Generic;
using Skyve.App.UserInterface.Lists;

namespace Skyve.App.UserInterface.Panels;

partial class PC_DLCs
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
			_dlcManager.DlcsLoaded -= SteamUtil_DLCsLoaded;
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
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.T_YourDlcs = new SlickControls.SlickTab();
			this.T_AllDlcs = new SlickControls.SlickTab();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.L_Duplicates = new System.Windows.Forms.Label();
			this.L_Counts = new System.Windows.Forms.Label();
			this.LC_DLCs = new Skyve.App.UserInterface.Lists.DlcListControl();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.TLP_Main.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Location = new System.Drawing.Point(-2, 3);
			this.base_Text.Size = new System.Drawing.Size(150, 31);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 2;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Controls.Add(this.flowLayoutPanel1, 1, 0);
			this.TLP_Main.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.TLP_Main.Controls.Add(this.slickSpacer2, 0, 2);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 0, 4);
			this.TLP_Main.Controls.Add(this.TB_Search, 0, 0);
			this.TLP_Main.Controls.Add(this.tableLayoutPanel3, 0, 3);
			this.TLP_Main.Controls.Add(this.LC_DLCs, 0, 5);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(0, 30);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 5;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Size = new System.Drawing.Size(932, 683);
			this.TLP_Main.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 38);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(155, 0);
			this.tableLayoutPanel1.TabIndex = 16;
			// 
			// T_YourDlcs
			// 
			this.T_YourDlcs.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "User";
			this.T_YourDlcs.IconName = dynamicIcon1;
			this.T_YourDlcs.LinkedControl = null;
			this.T_YourDlcs.Location = new System.Drawing.Point(618, 3);
			this.T_YourDlcs.Name = "T_YourDlcs";
			this.T_YourDlcs.Size = new System.Drawing.Size(150, 26);
			this.T_YourDlcs.TabIndex = 0;
			this.T_YourDlcs.TabStop = false;
			this.T_YourDlcs.Text = "YourDlcs";
			this.T_YourDlcs.TabSelected += new System.EventHandler(this.T_YourDlcs_TabSelected);
			// 
			// T_AllDlcs
			// 
			this.T_AllDlcs.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "Discover";
			this.T_AllDlcs.IconName = dynamicIcon2;
			this.T_AllDlcs.LinkedControl = null;
			this.T_AllDlcs.Location = new System.Drawing.Point(462, 3);
			this.T_AllDlcs.Name = "T_AllDlcs";
			this.T_AllDlcs.Size = new System.Drawing.Size(150, 25);
			this.T_AllDlcs.TabIndex = 0;
			this.T_AllDlcs.TabStop = false;
			this.T_AllDlcs.Text = "AllDlcs";
			this.T_AllDlcs.TabSelected += new System.EventHandler(this.T_YourDlcs_TabSelected);
			// 
			// slickSpacer2
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer2, 2);
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer2.Location = new System.Drawing.Point(0, 38);
			this.slickSpacer2.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(932, 2);
			this.slickSpacer2.TabIndex = 8;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 2);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 53);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(932, 2);
			this.slickSpacer1.TabIndex = 7;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TB_Search
			// 
			this.TB_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			dynamicIcon3.Name = "Search";
			this.TB_Search.ImageName = dynamicIcon3;
			this.TB_Search.LabelText = "Search";
			this.TB_Search.Location = new System.Drawing.Point(3, 3);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(5, 5, 47, 5);
			this.TB_Search.Placeholder = "SearchDlcs";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(149, 32);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			this.TB_Search.IconClicked += new System.EventHandler(this.TB_Search_IconClicked);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel3, 2);
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.L_Duplicates, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.L_Counts, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 40);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(932, 13);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// L_Duplicates
			// 
			this.L_Duplicates.AutoSize = true;
			this.L_Duplicates.Location = new System.Drawing.Point(3, 0);
			this.L_Duplicates.Name = "L_Duplicates";
			this.L_Duplicates.Size = new System.Drawing.Size(38, 13);
			this.L_Duplicates.TabIndex = 2;
			this.L_Duplicates.Text = "label1";
			// 
			// L_Counts
			// 
			this.L_Counts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.L_Counts.AutoSize = true;
			this.L_Counts.Location = new System.Drawing.Point(891, 0);
			this.L_Counts.Name = "L_Counts";
			this.tableLayoutPanel3.SetRowSpan(this.L_Counts, 2);
			this.L_Counts.Size = new System.Drawing.Size(38, 13);
			this.L_Counts.TabIndex = 1;
			this.L_Counts.Text = "label1";
			// 
			// LC_DLCs
			// 
			this.LC_DLCs.AutoInvalidate = false;
			this.LC_DLCs.AutoScroll = true;
			this.TLP_Main.SetColumnSpan(this.LC_DLCs, 2);
			this.LC_DLCs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LC_DLCs.DynamicSizing = true;
			this.LC_DLCs.GridView = true;
			this.LC_DLCs.Location = new System.Drawing.Point(0, 55);
			this.LC_DLCs.Margin = new System.Windows.Forms.Padding(0);
			this.LC_DLCs.Name = "LC_DLCs";
			this.LC_DLCs.Size = new System.Drawing.Size(932, 628);
			this.LC_DLCs.TabIndex = 15;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.T_YourDlcs);
			this.flowLayoutPanel1.Controls.Add(this.T_AllDlcs);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(158, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(771, 32);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// PC_DLCs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Main);
			this.Name = "PC_DLCs";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(932, 713);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.TLP_Main, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.TableLayoutPanel TLP_Main;
	private SlickControls.SlickSpacer slickSpacer2;
	private SlickControls.SlickSpacer slickSpacer1;
	private SlickControls.SlickTextBox TB_Search;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
	private System.Windows.Forms.Label L_Duplicates;
	private System.Windows.Forms.Label L_Counts;
	private DlcListControl LC_DLCs;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private SlickControls.SlickTab T_YourDlcs;
	private SlickControls.SlickTab T_AllDlcs;
	private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
}
