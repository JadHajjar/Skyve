﻿using SkyveApp.Systems.CS1.Utilities;
using SkyveApp.UserInterface.Content;
using SkyveApp.UserInterface.Lists;

using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Panels;
public partial class PC_SelectPackage : PanelContent
{
	private readonly ItemListControl<GenericWorkshopPackage> LC_Items;
	private readonly DelayedAction<TicketBooth.Ticket> _delayedSearch;
	private readonly TicketBooth _ticketBooth = new();
	private bool searchEmpty = true;
	private readonly List<string> searchTermsOr = new();
	private readonly List<string> searchTermsAnd = new();
	private readonly List<string> searchTermsExclude = new();
	public event Action<IEnumerable<ulong>>? PackageSelected;

	private readonly IWorkshopService _workshopService = ServiceCenter.Get<IWorkshopService>();

	public PC_SelectPackage()
	{
		InitializeComponent();

		_delayedSearch = new(350, DelayedSearch);

		TB_Search.Placeholder = LocaleHelper.GetGlobalText("Search") + "..";

		Text = LocaleHelper.GetGlobalText("Add Packages");

		L_Selected.Text = Locale.ControlToSelectMultiplePackages;

		LC_Items = new(SkyvePage.None)
		{
			Loading = true,
			IsSelection = true,
			IsGenericPage = true,
			Dock = DockStyle.Fill
		};

		LC_Items.PackageSelected += LC_Items_PackageSelected;

		LC_Items.SetSorting(PackageSorting.None, false);

		Controls.Add(LC_Items);

		LC_Items.BringToFront();

		OT_ModAsset.SelectedValue = Generic.ThreeOptionToggle.Value.Option1;
		TB_Search.Loading = true;
		_delayedSearch.Run(_ticketBooth.GetTicket());
	}

	public PC_SelectPackage(string search, Generic.ThreeOptionToggle.Value value) : this()
	{
		TB_Search.Text = search;
		LC_Items.IsSelection = false;
		OT_ModAsset.SelectedValue = value;
		L_Selected.Visible = false;
	}

	private void LC_Items_PackageSelected(GenericWorkshopPackage obj)
	{
		if (ModifierKeys.HasFlag(Keys.Control))
		{
			B_Continue.Visible = true;
			if (!FLP_Packages.Controls.OfType<MiniPackageControl>().Any(x => x.Id == obj.Id))
			{
				FLP_Packages.Controls.Add(new MiniPackageControl(obj.Id));
			}

			return;
		}

		if (FLP_Packages.Controls.Count > 0)
		{
			if (!FLP_Packages.Controls.OfType<MiniPackageControl>().Any(x => x.Id == obj.Id))
			{
				FLP_Packages.Controls.Add(new MiniPackageControl(obj.Id));
			}

			Form.PushBack();
			PackageSelected?.Invoke(FLP_Packages.Controls.OfType<MiniPackageControl>().Select(x => x.Id));
			return;
		}

		Form.PushBack();
		PackageSelected?.Invoke(new[] { obj.Id });
	}

	private bool DoNotDraw(IWorkshopInfo item)
	{
		if (!searchEmpty)
		{
			for (var i = 0; i < searchTermsExclude.Count; i++)
			{
				if (Search(searchTermsExclude[i], item))
				{
					return true;
				}
			}

			var orMatched = searchTermsOr.Count == 0;

			for (var i = 0; i < searchTermsOr.Count; i++)
			{
				if (Search(searchTermsOr[i], item))
				{
					orMatched = true;
					break;
				}
			}

			if (!orMatched)
			{
				return true;
			}

			for (var i = 0; i < searchTermsAnd.Count; i++)
			{
				if (!Search(searchTermsAnd[i], item))
				{
					return true;
				}
			}

			return false;
		}

		return false;
	}

	private bool Search(string searchTerm, IWorkshopInfo item)
	{
		return searchTerm.SearchCheck(item.ToString())
			|| searchTerm.SearchCheck(item.Author?.Name)
			|| item.Id.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		L_Totals.Margin = L_Selected.Margin = UI.Scale(new Padding(5), UI.FontScale);
		L_Totals.Font = L_Selected.Font = UI.Font(7.5F, FontStyle.Bold);
		OT_ModAsset.Width = (int)(200 * UI.FontScale);
		TB_Search.Width = (int)(250 * UI.FontScale);
		TB_Search.Margin = OT_ModAsset.Margin = UI.Scale(new Padding(10), UI.FontScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		tableLayoutPanel2.BackColor = design.AccentBackColor;
	}

	private void TB_Search_TextChanged(object sender, EventArgs e)
	{
		TB_Search.ImageName = (searchEmpty = string.IsNullOrWhiteSpace(TB_Search.Text)) ? "I_Search" : "I_ClearSearch";
		TB_Search.Loading = true;

		if (Regex.IsMatch(TB_Search.Text, @"filedetails/\?id=(\d+)"))
		{
			TB_Search.Text = Regex.Match(TB_Search.Text, @"filedetails/\?id=(\d+)").Groups[1].Value;
			return;
		}

		var searchText = TB_Search.Text.Trim();

		searchTermsAnd.Clear();
		searchTermsExclude.Clear();
		searchTermsOr.Clear();

		LC_Items.IsTextSearchNotEmpty = !searchEmpty;

		if (!searchEmpty)
		{
			var matches = Regex.Matches(searchText, @"(?:^|,)?\s*([+-]?)\s*([^,\-\+]+)");
			foreach (Match item in matches)
			{
				switch (item.Groups[1].Value)
				{
					case "+":
						if (!string.IsNullOrWhiteSpace(item.Groups[2].Value))
						{
							searchTermsAnd.Add(item.Groups[2].Value.Trim());
						}

						break;
					case "-":
						if (!string.IsNullOrWhiteSpace(item.Groups[2].Value))
						{
							searchTermsExclude.Add(item.Groups[2].Value.Trim());
						}

						break;
					default:
						searchTermsOr.Add(item.Groups[2].Value.Trim());
						break;
				}
			}
		}

		_delayedSearch.Run(_ticketBooth.GetTicket());
	}

	private async void DelayedSearch(TicketBooth.Ticket ticket)
	{
		Dictionary<ulong, IWorkshopInfo> items;

		if (TB_Search.Text.Trim().Length > 7 && ulong.TryParse(TB_Search.Text.Trim(), out var steamId))
		{
			var item = await _workshopService.GetInfoAsync(new GenericPackageIdentity(steamId));

			if (item is not null)
			{
				items = new() { [steamId] = item };
			}
			else
			{
				items = new();
			}
		}
		else
		{
			items = (await _workshopService.QueryFilesAsync(PackageSorting.Default,
				TB_Search.Text,
				OT_ModAsset.SelectedValue == Generic.ThreeOptionToggle.Value.Option1 ? new[] { "Mod" } : null,
			 	OT_ModAsset.SelectedValue == Generic.ThreeOptionToggle.Value.Option2 ? new[] { "Mod" } : null)).ToDictionary(x => x.Id);
		}

		if (!_ticketBooth.IsLast(ticket))
		{
			return;
		}

		LC_Items.SetItems(items.Values.Where(x => !DoNotDraw(x)).Select(x => new GenericWorkshopPackage(x)));
		LC_Items.Loading = false;

		this.TryInvoke(() => L_Totals.Text = Locale.ShowingCount.FormatPlural(LC_Items.ItemCount, Locale.Package.FormatPlural(LC_Items.ItemCount)));

		TB_Search.Loading = false;
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private void B_Continue_Click(object sender, EventArgs e)
	{
		Form.PushBack();
		PackageSelected?.Invoke(FLP_Packages.Controls.OfType<MiniPackageControl>().Select(x => x.Id));
	}
}
