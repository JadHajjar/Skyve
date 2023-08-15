﻿using System.Windows.Forms;

namespace SkyveApp.UserInterface.Panels;
internal partial class PC_ContentList<T> : PanelContent where T : IPackage
{
	protected readonly ContentList<T> LC_Items;

	public virtual SkyvePage Page { get; }

	public PC_ContentList() : this(false) { }

	public PC_ContentList(bool load) : base(load)
	{
		InitializeComponent();

		LC_Items = new(Page, !load, GetItems, SetIncluded, SetEnabled, GetItemText, GetCountText)
		{
			TabIndex = 0,
			Dock = DockStyle.Fill
		};

		if (this is not PC_GenericPackageList)
		{
			LC_Items.TLP_Main.SetColumn(LC_Items.FLP_Search, 0);
			LC_Items.TLP_Main.SetColumnSpan(LC_Items.FLP_Search, 2);
			LC_Items.TLP_Main.SetColumn(LC_Items.P_FiltersContainer, 0);
			LC_Items.TLP_Main.SetColumnSpan(LC_Items.P_FiltersContainer, 4);
		}

		Controls.Add(LC_Items);

		if (!load)
		{
			if (!ServiceCenter.Get<INotifier>().IsContentLoaded)
			{
				LC_Items.ListControl.Loading = true;
			}
			else
			{
				LC_Items.RefreshItems();
			}
		}
	}

	public override bool KeyPressed(ref Message msg, Keys keyData)
	{
		if (keyData is (Keys.Control | Keys.F))
		{
			LC_Items.TB_Search.Focus();
			LC_Items.TB_Search.SelectAll();

			return true;
		}

		return false;
	}

	protected virtual IEnumerable<T> GetItems()
	{
		throw new NotImplementedException();
	}

	protected virtual void SetIncluded(IEnumerable<T> filteredItems, bool included)
	{
		ServiceCenter.Get<IBulkUtil>().SetBulkIncluded(filteredItems.SelectWhereNotNull(x => x.LocalPackage)!, included);
	}

	protected virtual void SetEnabled(IEnumerable<T> filteredItems, bool enabled)
	{
		ServiceCenter.Get<IBulkUtil>().SetBulkEnabled(filteredItems.SelectWhereNotNull(x => x.LocalPackage)!, enabled);
	}

	protected virtual LocaleHelper.Translation GetItemText()
	{
		throw new NotImplementedException();
	}

	protected virtual string GetCountText()
	{
		throw new NotImplementedException();
	}
}
