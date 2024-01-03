using Skyve.App.UserInterface.Dropdowns;
using Skyve.App.UserInterface.Generic;

using Skyve.App.UserInterface.Lists;

using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class ContentList<T> : SlickControl where T : IPackageIdentity
{
	private bool clearingFilters = true;
	private bool firstFilterPassed;
	private readonly DelayedAction _delayedSearch;
	private readonly DelayedAction _delayedAuthorTagsRefresh;
	public readonly ItemListControl<T> ListControl;
	private readonly IncludeAllButton<T> I_Actions;
	protected int UsageFilteredOut;
	private bool searchEmpty = true;
	private readonly List<string> searchTermsOr = new();
	private readonly List<string> searchTermsAnd = new();
	private readonly List<string> searchTermsExclude = new();

	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPlaysetManager _profileManager;
	private readonly ITagsService _tagUtil;
	private readonly IPackageUtil _packageUtil;
	private readonly IDownloadService _downloadService;

	private readonly Func<IEnumerable<T>> GetItems;
	private readonly Action<IEnumerable<T>, bool> SetIncluded;
	private readonly Action<IEnumerable<T>, bool> SetEnabled;
	private readonly Func<LocaleHelper.Translation> GetItemText;
	private readonly Func<string> GetCountText;

	public SkyvePage Page { get; }
	public int ItemCount => ListControl.ItemCount;

	public bool IsGenericPage { get => ListControl.IsGenericPage; set => ListControl.IsGenericPage = value; }
	public IEnumerable<T> Items => ListControl.Items;

	public void Remove(T item)
	{
		ListControl.Remove(item);
	}

	public ContentList(SkyvePage page, bool loaded, Func<IEnumerable<T>> getItems, Action<IEnumerable<T>, bool> setIncluded, Action<IEnumerable<T>, bool> setEnabled, Func<LocaleHelper.Translation> getItemText, Func<string> getCountText)
	{
		Page = page;
		GetItems = getItems;
		SetIncluded = setIncluded;
		SetEnabled = setEnabled;
		GetItemText = getItemText;
		GetCountText = getCountText;

		ServiceCenter.Get(out _settings, out _notifier, out _compatibilityManager, out _profileManager, out _tagUtil, out _packageUtil, out _downloadService);

		if (_settings.UserSettings.ExtendedListInfo)
			ListControl = new ItemListControl<T>.Complex(Page) { Dock = DockStyle.Fill, Margin = new() };
		else
			ListControl = new ItemListControl<T>.Simple(Page) { Dock = DockStyle.Fill, Margin = new() };

		InitializeComponent();

		DD_Sorting.SkyvePage = Page;

		I_Actions = new IncludeAllButton<T>(() => ListControl.FilteredItems.ToList());
		I_Actions.ActionClicked += I_Actions_Click;
		I_Actions.IncludeAllClicked += IncludeAll;
		I_Actions.ExcludeAllClicked += ExcludeAll;
		I_Actions.EnableAllClicked += EnableAll;
		I_Actions.DisableAllClicked += DisableAll;
		I_Actions.SubscribeAllClicked += SubscribeAll;

		TLP_Main.Controls.Add(ListControl, 0, TLP_Main.RowCount - 1);
		TLP_Main.SetColumnSpan(ListControl, TLP_Main.ColumnCount);
		TLP_MiddleBar.Controls.Add(I_Actions, 0, 0);

		OT_Workshop.Visible = _profileManager.CurrentPlayset is not null && !_profileManager.CurrentPlayset.DisableWorkshop;
		OT_ModAsset.Visible = this is not PC_Assets and not PC_Mods;

		ListControl.FilterRequested += FilterChanged;
		ListControl.CanDrawItem += LC_Items_CanDrawItem;
		ListControl.DownloadStatusSelected += LC_Items_DownloadStatusSelected;
		ListControl.CompatibilityReportSelected += LC_Items_CompatibilityReportSelected;
		ListControl.DateSelected += LC_Items_DateSelected;
		ListControl.TagSelected += LC_Items_TagSelected;
		ListControl.AuthorSelected += LC_Items_AuthorSelected;
		ListControl.FilterByEnabled += LC_Items_FilterByEnabled;
		ListControl.FilterByIncluded += LC_Items_FilterByIncluded;
		ListControl.AddToSearch += LC_Items_AddToSearch;
		ListControl.OpenWorkshopSearch += LC_Items_OpenWorkshopSearch;
		ListControl.OpenWorkshopSearchInBrowser += LC_Items_OpenWorkshopSearchInBrowser;
		ListControl.SelectedItemsChanged += (_, _) => RefreshCounts();

		_delayedSearch = new(350, DelayedSearch);
		_delayedAuthorTagsRefresh = new(350, RefreshAuthorAndTags);

		if (!_settings.UserSettings.AdvancedIncludeEnable || this is PC_Assets)
		{
			OT_Enabled.Hide();
			P_Filters.SetRow(OT_Workshop, 2);
			P_Filters.SetRow(OT_ModAsset, 3);
		}

		clearingFilters = false;

		I_SortOrder.ImageName = ListControl.SortDescending ? "I_SortDesc" : "I_SortAsc";
		B_GridView.Selected = ListControl.GridView;
		B_ListView.Selected = !ListControl.GridView && !ListControl.CompactList;
		B_CompactList.Selected = !ListControl.GridView && ListControl.CompactList;

		if (loaded)
		{
			_notifier.ContentLoaded += CentralManager_ContentLoaded;
		}
		else
		{
			_notifier.ContentLoaded += CentralManager_WorkshopInfoUpdated;
		}

		_notifier.WorkshopInfoUpdated += CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated += CentralManager_WorkshopInfoUpdated;
		_notifier.CompatibilityReportProcessed += CentralManager_WorkshopInfoUpdated;

		if (loaded)
		{
			new BackgroundAction("Getting tag list", RefreshAuthorAndTags).Run();
		}

		SlickTip.SetTo(B_GridView, "Switch to Grid-View");
		SlickTip.SetTo(B_CompactList, "Switch to Compact-View");
		SlickTip.SetTo(B_ListView, "Switch to List-View");
	}

	protected void RefreshAuthorAndTags()
	{
		var items = new List<T>(ListControl.Items);

		DD_Author.SetItems(items);
		DD_Tags.Items = _tagUtil.GetDistinctTags().ToArray();
	}

	private void LC_Items_OpenWorkshopSearch()
	{
		Program.MainForm.PushPanel(null, new PC_SelectPackage(TB_Search.Text, this is PC_Mods ? ThreeOptionToggle.Value.Option1 : this is PC_Assets ? ThreeOptionToggle.Value.Option2 : ThreeOptionToggle.Value.None));
	}

	private void LC_Items_OpenWorkshopSearchInBrowser()
	{
		PlatformUtil.OpenUrl($"https://steamcommunity.com/workshop/browse/?appid=255710&searchtext={WebUtility.UrlEncode(TB_Search.Text)}&browsesort=trend&section=readytouseitems&actualsort=trend&p=1&days=365" + (this is PC_Mods ? "&requiredtags%5B0%5D=Mod" : ""));
	}

	private void LC_Items_AddToSearch(string obj)
	{
		if (TB_Search.Text.Length == 0)
		{
			TB_Search.Text = obj;
		}
		else
		{
			TB_Search.Text += $",+{obj}";
		}
	}

	private void LC_Items_FilterByIncluded(bool obj)
	{
		OT_Included.SelectedValue = obj ? ThreeOptionToggle.Value.Option1 : ThreeOptionToggle.Value.Option2;

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	private void LC_Items_FilterByEnabled(bool obj)
	{
		OT_Enabled.SelectedValue = obj ? ThreeOptionToggle.Value.Option1 : ThreeOptionToggle.Value.Option2;

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	private void LC_Items_AuthorSelected(IUser obj)
	{
		DD_Author.Select(obj);

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	private void LC_Items_TagSelected(ITag obj)
	{
		DD_Tags.Select(obj);

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	private void LC_Items_DateSelected(DateTime obj)
	{
		DR_ServerTime.SetValue(DateRangeType.After, obj);

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	private void LC_Items_CompatibilityReportSelected(NotificationType obj)
	{
		if ((int)DD_ReportSeverity.SelectedItem == (int)obj)
		{
			DD_ReportSeverity.SelectedItem = CompatibilityNotificationFilter.Any;
		}
		else
		{
			DD_ReportSeverity.SelectedItem = (CompatibilityNotificationFilter)obj;
		}

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	private void LC_Items_DownloadStatusSelected(DownloadStatus obj)
	{
		if (DD_PackageStatus.SelectedItem == (DownloadStatusFilter)(obj + 1))
		{
			DD_PackageStatus.SelectedItem = DownloadStatusFilter.Any;
		}
		else
		{
			DD_PackageStatus.SelectedItem = (DownloadStatusFilter)(obj + 1);
		}

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_MiddleBar.BackColor = design.AccentBackColor;
		P_Filters.BackColor = design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, -1, 1));
		ListControl.BackColor = design.BackColor;
		L_Counts.ForeColor = L_FilterCount.ForeColor = design.InfoColor;
	}

	protected override void LocaleChanged()
	{
		DD_PackageStatus.Text = Locale.PackageStatus;
		DD_ReportSeverity.Text = Locale.CompatibilityStatus;
		DD_Tags.Text = Locale.Tags;
		DD_Profile.Text = Locale.PlaysetFilter;
		DR_SubscribeTime.Text = Locale.DateSubscribed;
		DR_ServerTime.Text = Locale.DateUpdated;
		DD_Author.Text = Locale.Author;
		OT_ModAsset.Option1 = Locale.Mod.Plural;
		OT_ModAsset.Option2 = Locale.Asset.Plural;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		RefreshCounts();

		firstFilterPassed = true;

		if (_settings.UserSettings.AlwaysOpenFiltersAndActions)
		{
			P_FiltersContainer.Visible = true;
			P_FiltersContainer.AutoSize = true;
			B_Filters.Text = "HideFilters";
		}
		else
		{
			P_FiltersContainer.Height = 0;
			P_FiltersContainer.Visible = true;
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		P_FiltersContainer.Padding = TB_Search.Margin = I_Refresh.Padding = B_Filters.Padding
			= L_Counts.Margin = L_FilterCount.Margin = I_SortOrder.Padding
			= B_Filters.Margin = I_SortOrder.Margin = I_Refresh.Margin = DD_Sorting.Margin = UI.Scale(new Padding(5), UI.FontScale);

		B_Filters.Size = B_Filters.GetAutoSize(true);

		OT_Enabled.Margin = OT_Included.Margin = OT_Workshop.Margin = OT_ModAsset.Margin
			= DD_ReportSeverity.Margin = DR_SubscribeTime.Margin = DR_ServerTime.Margin
			= DD_Author.Margin = DD_PackageStatus.Margin = DD_Profile.Margin = DD_Tags.Margin = UI.Scale(new Padding(4, 2, 4, 2), UI.FontScale);

		I_ClearFilters.Size = UI.Scale(new Size(16, 16), UI.FontScale);
		L_Counts.Font = L_FilterCount.Font = UI.Font(7.5F, FontStyle.Bold);
		DD_Sorting.Width = (int)(175 * UI.FontScale);
		TB_Search.Width = (int)(250 * UI.FontScale);

		B_ListView.Size = B_GridView.Size = B_CompactList.Size = UI.Scale(new Size(22, 22), UI.FontScale);

		var size = (int)(30 * UI.FontScale) - 6;

		TB_Search.MaximumSize = I_Refresh.MaximumSize = B_Filters.MaximumSize = I_SortOrder.MaximumSize = DD_Sorting.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = I_Refresh.MinimumSize = B_Filters.MinimumSize = I_SortOrder.MinimumSize = DD_Sorting.MinimumSize = new Size(0, size);
	}

	private void B_Filters_Click(object sender, MouseEventArgs? e)
	{
		if (e is null || e.Button is MouseButtons.Left or MouseButtons.None)
		{
			B_Filters.Text = P_FiltersContainer.Height == 0 ? "HideFilters" : "ShowFilters";
			AnimationHandler.Animate(P_FiltersContainer, P_FiltersContainer.Height == 0 ? new Size(0, P_FiltersContainer.Padding.Vertical + P_Filters.Height) : Size.Empty, 3, AnimationOption.IgnoreWidth);
			P_FiltersContainer.AutoSize = false;
		}
		else if (e?.Button == MouseButtons.Middle)
		{
			I_ClearFilters_Click(sender, e);
		}
	}

	private void CentralManager_ContentLoaded()
	{
		RefreshItems();
	}

	public void RefreshItems()
	{
		if (ListControl.Loading)
		{
			ListControl.Loading = false;
		}

		ListControl.SetItems(GetItems());

		this.TryInvoke(RefreshCounts);

		RefreshAuthorAndTags();
	}

	private void CentralManager_WorkshopInfoUpdated()
	{
		ListControl.Invalidate();
		I_Actions.Invalidate();

		this.TryInvoke(RefreshCounts);

		_delayedAuthorTagsRefresh.Run();
	}

	private void DD_Sorting_SelectedItemChanged(object sender, EventArgs e)
	{
		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.Sorting = (int)DD_Sorting.SelectedItem;
		_settings.UserSettings.Save();

		ListControl.SetSorting(DD_Sorting.SelectedItem, ListControl.SortDescending);
	}

	private void DelayedSearch()
	{
		UsageFilteredOut = 0;
		ListControl.DoFilterChanged();
		this.TryInvoke(RefreshCounts);
		I_Refresh.Loading = false;
	}

	private void FilterChanged(object sender, EventArgs e)
	{
		if (!clearingFilters)
		{
			I_Refresh.Loading = true;
			_delayedSearch.Run();

			if (sender == I_Refresh)
			{
				ListControl.SortingChanged();
			}
		}
	}

	private void I_ClearFilters_Click(object sender, EventArgs e)
	{
		clearingFilters = true;
		this.ClearForm();
		clearingFilters = false;
		FilterChanged(sender, e);
	}

	private bool IsFilteredOut(T item)
	{
		if (!ListControl.IsGenericPage && (_profileManager.CurrentPlayset?.DisableWorkshop ?? false))
		{
			if (item.GetPackage()?.IsLocal == true)
			{
				return true;
			}
		}

		if (ListControl.IsGenericPage && item.GetWorkshopInfo()?.IsInvalid == true)
		{
			return true;
		}

		if (_profileManager.CurrentPlayset?.Usage > 0)
		{
			if (!(_compatibilityManager.GetPackageInfo(item)?.Usage.HasFlag(_profileManager.CurrentPlayset.Usage) ?? true))
			{
				UsageFilteredOut++;
				return true;
			}
		}

		if (!firstFilterPassed)
		{
			return false;
		}

		if (OT_Workshop.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Workshop.SelectedValue == ThreeOptionToggle.Value.Option1 == !item.GetPackage()?.IsLocal)
			{
				return true;
			}
		}

		if (OT_Included.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Included.SelectedValue == ThreeOptionToggle.Value.Option2 == (item.IsIncluded(out var partiallyIncluded) || partiallyIncluded))
			{
				return true;
			}
		}

		if (OT_Enabled.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (item.GetPackage()?.IsCodeMod == false || OT_Enabled.SelectedValue == ThreeOptionToggle.Value.Option1 != (item.GetLocalPackage()?.IsEnabled()))
			{
				return true;
			}
		}

		if (OT_ModAsset.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_ModAsset.SelectedValue == ThreeOptionToggle.Value.Option2 == item.GetPackage()?.IsCodeMod)
			{
				return true;
			}
		}

		if (DD_PackageStatus.SelectedItem != DownloadStatusFilter.Any)
		{
			//if (DD_PackageStatus.SelectedItem == DownloadStatusFilter.None)
			//{
			//	if (item.Workshop)
			//	{
			//		return true;
			//	}
			//}
			//else
			if (DD_PackageStatus.SelectedItem == DownloadStatusFilter.AnyIssue)
			{
				if (item.GetPackage()?.IsLocal == true || _packageUtil.GetStatus(item.GetLocalPackageIdentity(), out _) <= DownloadStatus.OK)
				{
					return true;
				}
			}
			else
			{
				if (((int)DD_PackageStatus.SelectedItem - 1) != (int)_packageUtil.GetStatus(item.GetLocalPackageIdentity(), out _))
				{
					return true;
				}
			}
		}

		if (DD_ReportSeverity.SelectedItem != CompatibilityNotificationFilter.Any)
		{
			if (DD_ReportSeverity.SelectedItem == CompatibilityNotificationFilter.AnyIssue)
			{
				if (item.GetCompatibilityInfo().GetNotification() <= NotificationType.Info)
				{
					return true;
				}
			}
			else if (DD_ReportSeverity.SelectedItem == CompatibilityNotificationFilter.NoIssues)
			{
				return item.GetCompatibilityInfo().GetNotification() > NotificationType.Info;
			}
			else if ((int)item.GetCompatibilityInfo().GetNotification() != (int)DD_ReportSeverity.SelectedItem)
			{
				return true;
			}
		}

		if (DR_SubscribeTime.Set && !DR_SubscribeTime.Match(item.GetLocalPackage()?.LocalTime.ToLocalTime() ?? DateTime.MinValue))
		{
			return true;
		}

		if (DR_ServerTime.Set && !DR_ServerTime.Match(item.GetWorkshopInfo()?.ServerTime.ToLocalTime() ?? default))
		{
			return true;
		}

		if (DD_Author.SelectedItems.Any())
		{
			if (!DD_Author.SelectedItems.Any(x => item.GetWorkshopInfo()?.Author?.Equals(x) ?? false))
			{
				return true;
			}
		}

		if (DD_Tags.SelectedItems.Any())
		{
			if (!_tagUtil.HasAllTags(item.GetPackage(), DD_Tags.SelectedItems))
			{
				return true;
			}
		}

		if (DD_Profile.SelectedItem is not null && !DD_Profile.SelectedItem.Temporary)
		{
			return !_profileManager.IsPackageIncludedInPlayset(item.GetPackage(), DD_Profile.SelectedItem).Result;
		}

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

	private bool Search(string searchTerm, T item)
	{
		return searchTerm.SearchCheck(item.Name)
			|| searchTerm.SearchCheck(item.GetWorkshopInfo()?.Author?.Name)
			|| (!item.IsLocal() ? item.Id.ToString() : Path.GetFileName(item.GetLocalPackage()?.Folder) ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1;
	}

	private void LC_Items_CanDrawItem(object sender, CanDrawItemEventArgs<T> e)
	{
		e.DoNotDraw = IsFilteredOut(e.Item);
	}

	protected void RefreshCounts()
	{
		var countText = GetCountText();
		var format = ListControl.SelectedItemsCount == 0 ? (UsageFilteredOut == 0 ? Locale.ShowingCount : Locale.ShowingCountWarning) : (UsageFilteredOut == 0 ? Locale.ShowingSelectedCount : Locale.ShowingSelectedCountWarning);
		var filteredText = format.FormatPlural(
			ListControl.FilteredCount,
			GetItemText().FormatPlural(ListControl.FilteredCount).ToLower(),
			ListControl.SelectedItemsCount,
			Locale.ItemsHidden.FormatPlural(UsageFilteredOut, GetItemText().FormatPlural(ListControl.FilteredCount).ToLower()));

		if (L_Counts.Text != countText)
		{
			L_Counts.Text = countText;
		}

		if (L_FilterCount.Text != filteredText)
		{
			L_FilterCount.Visible = !string.IsNullOrEmpty(filteredText);
			L_FilterCount.Text = filteredText;
		}

		I_Actions.Invalidate();
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private void TB_Search_TextChanged(object sender, EventArgs e)
	{
		if (Regex.IsMatch(TB_Search.Text, @"filedetails/\?id=(\d+)"))
		{
			TB_Search.Text = Regex.Match(TB_Search.Text, @"filedetails/\?id=(\d+)").Groups[1].Value;
			return;
		}

		TB_Search.ImageName = (searchEmpty = string.IsNullOrWhiteSpace(TB_Search.Text)) ? "I_Search" : "I_ClearSearch";

		var searchText = TB_Search.Text.Trim();

		searchTermsAnd.Clear();
		searchTermsExclude.Clear();
		searchTermsOr.Clear();

		ListControl.IsTextSearchNotEmpty = !searchEmpty;

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

		FilterChanged(sender, e);
	}

	private void Icon_SizeChanged(object sender, EventArgs e)
	{
		(sender as Control)!.Width = (sender as Control)!.Height;
	}

	private void I_SortOrder_Click(object sender, EventArgs e)
	{
		ListControl.SetSorting(DD_Sorting.SelectedItem, !ListControl.SortDescending);

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.DescendingSort = ListControl.SortDescending;
		_settings.UserSettings.Save();

		I_SortOrder.ImageName = ListControl.SortDescending ? "I_SortDesc" : "I_SortAsc";
	}

	private void I_Actions_Click(object sender, EventArgs e)
	{
		var items = new SlickStripItem[]
		{
			  new (Locale.IncludeAll, "I_Check", action: () => IncludeAll(this, EventArgs.Empty))
			, new (Locale.ExcludeAll, "I_X", action: () =>ExcludeAll(this, EventArgs.Empty))
			, new (string.Empty)
			, new (Locale.EnableAll, "I_Enabled", _settings.UserSettings.AdvancedIncludeEnable, action:() => EnableAll(this, EventArgs.Empty))
			, new (Locale.DisableAll, "I_Disabled", _settings.UserSettings.AdvancedIncludeEnable, action: () => DisableAll(this, EventArgs.Empty))
			, new (string.Empty)
			, new (Locale.SelectAll, "I_DragDrop", ListControl.SelectedItemsCount < ListControl.FilteredItems.Count(), action: ListControl.SelectAll)
			, new (Locale.DeselectAll, "I_Select", ListControl.SelectedItemsCount > 0, action: ListControl.DeselectAll)
			, new (Locale.CopyAllIds, "I_Copy", action: () => Clipboard.SetText(ListControl.FilteredItems.ListStrings(x => x.IsLocal() ? $"Local: {x.Name}" : $"{x.Id}: {x.Name}", CrossIO.NewLine)))
#if CS2
			, new (Locale.SubscribeAll, "I_Paradox", this is PC_GenericPackageList, action: () => SubscribeAll(this, EventArgs.Empty))
#else
			, new (Locale.SubscribeAll, "I_Steam", this is PC_GenericPackageList, action: () => SubscribeAll(this, EventArgs.Empty))
			, new (Locale.DownloadAll, "I_Install", ListControl.FilteredItems.Any(x => x.GetLocalPackage() is null), action: () => DownloadAll(this, EventArgs.Empty))
			, new (Locale.ReDownloadAll, "I_ReDownload", ListControl.FilteredItems.Any(x => x.GetLocalPackage() is not null), action: () => ReDownloadAll(this, EventArgs.Empty))
#endif
			, new (string.Empty)
			, new (Locale.UnsubscribeAll, "I_RemoveSteam", action: () => UnsubscribeAll(this, EventArgs.Empty))
			, new (Locale.DeleteAll, "I_Disposable", action: () => DeleteAll(this, EventArgs.Empty))
		};

		this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, I_Actions.PointToScreen(new Point(I_Actions.Width + 5, 0)), items));
	}

	private void DisableAll(object sender, EventArgs e)
	{
		SetEnabled(ListControl.FilteredItems, false);
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private void EnableAll(object sender, EventArgs e)
	{
		SetEnabled(ListControl.FilteredItems, true);
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private void ExcludeAll(object sender, EventArgs e)
	{
		SetIncluded(ListControl.FilteredItems, false);
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private void IncludeAll(object sender, EventArgs e)
	{
		SetIncluded(ListControl.FilteredItems, true);
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

#if CS1
	private void DownloadAll(object sender, EventArgs e)
	{
		_downloadService.Download(ListControl.FilteredItems.Where(x => x.GetLocalPackage() is null).Select(x => (IPackageIdentity)x));
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private void ReDownloadAll(object sender, EventArgs e)
	{
		_downloadService.Download(ListControl.FilteredItems.Where(x => x.GetLocalPackage() is not null).Cast<IPackageIdentity>());
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}
#endif

	private void UnsubscribeAll(object sender, EventArgs e)
	{
		if (MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisUnsubscribesFrom.FormatPlural(ListControl.FilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		I_Actions.Loading = true;
		ServiceCenter.Get<ISubscriptionsManager>().UnSubscribe(ListControl.FilteredItems.Cast<IPackageIdentity>());
		I_Actions.Loading = false;
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private void SubscribeAll(object sender, EventArgs e)
	{
		var removeBadPackages = false;
		var steamIds = ListControl.SafeGetItems().AllWhere(x => x.Item.GetLocalPackage() == null && x.Item.Id != 0);

		foreach (var item in steamIds.ToList())
		{
			var report = item.Item.GetCompatibilityInfo();

			if (report.GetNotification() >= NotificationType.Unsubscribe)
			{
				if (!removeBadPackages && MessagePrompt.Show(Locale.ItemsShouldNotBeSubscribedInfo + "\r\n\r\n" + Locale.WouldYouLikeToSkipThose, PromptButtons.YesNo, PromptIcons.Hand, form: Program.MainForm) == DialogResult.No)
				{
					break;
				}
				else
				{
					removeBadPackages = true;
				}

				steamIds.Remove(item);
				ListControl.RemoveAll(x => x.Id == item.Item.Id);
			}
		}

		if (steamIds.Count == 0 || MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisSubscribesTo.FormatPlural(ListControl.FilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		ServiceCenter.Get<ISubscriptionsManager>().Subscribe(steamIds.Select(x => (IPackageIdentity)x.Item));
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private async void DeleteAll(object sender, EventArgs e)
	{
		if (MessagePrompt.Show(Locale.AreYouSure, PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		I_Actions.Loading = true;
		await Task.Run(() =>
		{
			var items = ListControl.FilteredItems.ToList();
			foreach (var item in items)
			{
				if (item.IsLocal() && item is IAsset asset)
				{
					CrossIO.DeleteFile(asset.FilePath);
				}
				else if (item is ILocalPackageData package)
				{
					ServiceCenter.Get<IPackageManager>().DeleteAll(package.Folder);
				}
			}
		});
		I_Actions.Loading = false;
	}

	private void B_ListView_Click(object sender, EventArgs e)
	{
		B_GridView.Selected = false;
		B_CompactList.Selected = false;
		B_ListView.Selected = true;
		ListControl.GridView = false;
		ListControl.CompactList = false;

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.GridView = ListControl.GridView;
		settings.Compact = ListControl.CompactList;
		_settings.UserSettings.Save();
	}

	private void B_GridView_Click(object sender, EventArgs e)
	{
		B_GridView.Selected = true;
		B_CompactList.Selected = false;
		B_ListView.Selected = false;
		ListControl.GridView = true;
		ListControl.CompactList = false;

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.GridView = ListControl.GridView;
		settings.Compact = ListControl.CompactList;
		_settings.UserSettings.Save();
	}

	private void B_CompactList_Click(object sender, EventArgs e)
	{
		B_GridView.Selected = false;
		B_ListView.Selected = false;
		B_CompactList.Selected = true;
		ListControl.GridView = false;
		ListControl.CompactList = true;

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.GridView = ListControl.GridView;
		settings.Compact = ListControl.CompactList;
		_settings.UserSettings.Save();
	}
}
