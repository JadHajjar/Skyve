﻿using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_DLCs : PanelContent
{
	private readonly IDlcManager _dlcManager;
	public PC_DLCs()
	{
		ServiceCenter.Get(out _dlcManager);

		InitializeComponent();

		LC_DLCs.CanDrawItem += LC_DLCs_CanDrawItem;

		LC_DLCs.SetItems(_dlcManager.Dlcs);

		if (LC_DLCs.ItemCount == 0)
			LC_DLCs.Loading = true;

		T_YourDlcs.Selected = true;

		_dlcManager.DlcsLoaded += SteamUtil_DLCsLoaded;
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		tableLayoutPanel3.BackColor = design.AccentBackColor;
		L_Counts.ForeColor = design.InfoColor;
		L_Duplicates.ForeColor = design.YellowColor.MergeColor(design.ForeColor, 90);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		flowLayoutPanel1.Margin = 
		TB_Search.Margin = L_Duplicates.Margin = L_Counts.Margin = UI.Scale(new Padding(5));
		L_Duplicates.Font = L_Counts.Font = UI.Font(7.5F, FontStyle.Bold);
		TB_Search.Width = UI.Scale(250);
	}

	private void SteamUtil_DLCsLoaded()
	{
		LC_DLCs.SetItems(_dlcManager.Dlcs);

		this.TryInvoke(RefreshCounts);
	}

	private void RefreshCounts()
	{
		var total = _dlcManager.Dlcs.Count(x => _dlcManager.IsAvailable(x.Id));

		L_Counts.Text = string.Format(Locale.DlcCount, total);
	}

	private void LC_DLCs_CanDrawItem(object sender, CanDrawItemEventArgs<IDlcInfo> e)
	{
		if (T_YourDlcs.Selected && !_dlcManager.IsAvailable(e.Item.Id))
		{
			e.DoNotDraw = true;
		}

		if (!string.IsNullOrWhiteSpace(TB_Search.Text) && !TB_Search.Text.SearchCheck(e.Item.Name))
		{
			e.DoNotDraw = true;
		}
	}

	protected override void LocaleChanged()
	{
		Text = $"{Locale.DLCs} - {ServiceCenter.Get<IPlaysetManager>().CurrentPlayset?.Name ?? Locale.NoActivePlayset}";

		L_Duplicates.Text = Locale.DlcUpdateNotice;
	}

	private void TB_Search_TextChanged(object sender, EventArgs e)
	{
		TB_Search.ImageName = string.IsNullOrWhiteSpace(TB_Search.Text) ? "Search" : "ClearSearch";
		LC_DLCs.FilterChanged();
		RefreshCounts();
	}

	public override bool KeyPressed(ref Message msg, Keys keyData)
	{
		if (keyData is (Keys.Control | Keys.F))
		{
			TB_Search.Focus();
			TB_Search.SelectAll();

			return true;
		}

		return false;
	}

	private void B_ExInclude_RightClicked(object sender, EventArgs e)
	{
		foreach (var item in LC_DLCs.FilteredItems)
		{
			_dlcManager.SetIncluded(item, false);
		}

		LC_DLCs.Invalidate();
	}

	private void B_ExInclude_LeftClicked(object sender, EventArgs e)
	{
		foreach (var item in LC_DLCs.FilteredItems)
		{
			_dlcManager.SetIncluded(item, true);
		}

		LC_DLCs.Invalidate();
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private void T_YourDlcs_TabSelected(object sender, EventArgs e)
	{
		LC_DLCs.FilterChanged();
		RefreshCounts();
	}
}
