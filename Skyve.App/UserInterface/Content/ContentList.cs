using Skyve.App.UserInterface.Dropdowns;
using Skyve.App.UserInterface.Generic;

using Skyve.App.UserInterface.Lists;
using Skyve.App.Utilities;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class ContentList : SlickControl
{
	public delegate Task<IEnumerable<IPackageIdentity>> GetAllItems(CancellationToken token);

	private CancellationTokenSource? tokenSource;
	private bool clearingFilters = true;
	private bool firstFilterPassed;
	private readonly DelayedAction _delayedSearch;
	private readonly DelayedAction _delayedAuthorTagsRefresh;
	private readonly IncludeAllButton I_Actions;
	private readonly GetAllItems GetItems;
	private readonly Func<LocaleHelper.Translation> GetItemText;
	private bool searchEmpty = true;
	private readonly List<string> searchTermsOr = [];
	private readonly List<string> searchTermsAnd = [];
	private readonly List<string> searchTermsExclude = [];
	public readonly ItemListControl ListControl;

	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPlaysetManager _playsetManager;
	private readonly ITagsService _tagUtil;
	private readonly IPackageUtil _packageUtil;
	private readonly IDownloadService _downloadService;
	private readonly IWorkshopService _workshopService;

	public SkyvePage Page { get; }
	public int ItemCount => ListControl.ItemCount;
	public int? SelectedPlayset { get => ListControl.SelectedPlayset; set => ListControl.SelectedPlayset = value; }
	public bool IsGenericPage { get => ListControl.IsGenericPage; set => ListControl.IsGenericPage = value; }
	public IEnumerable<IPackageIdentity> Items => ListControl.Items;

	public ContentList(SkyvePage page, bool loaded, GetAllItems getItems, Func<LocaleHelper.Translation> getItemText, IPackageUtil? customPackageUtil = null)
	{
		Page = page;
		GetItems = getItems;
		GetItemText = getItemText;

		ServiceCenter.Get(out _settings, out _notifier, out _compatibilityManager, out _playsetManager, out _tagUtil, out _packageUtil, out _downloadService, out _workshopService);

		if (customPackageUtil is not null)
		{
			_packageUtil = customPackageUtil;
		}

		ListControl = _settings.UserSettings.ComplexListUI
			? new ItemListControl.Complex(Page, customPackageUtil) { Dock = DockStyle.Fill, Margin = new() }
			: new ItemListControl.Simple(Page, customPackageUtil) { Dock = DockStyle.Fill, Margin = new() };

		InitializeComponent();

		DD_Sorting.SkyvePage = Page;

		I_Actions = new IncludeAllButton(() => ListControl.FilteredItems.ToList());
		I_Actions.ActionClicked += I_Actions_Click;
		I_Actions.IncludeAllClicked = IncludeAll;
		I_Actions.ExcludeAllClicked = ExcludeAll;
		I_Actions.EnableAllClicked = EnableAll;
		I_Actions.DisableAllClicked = DisableAll;
#if CS2
		I_Actions.SubscribeAllClicked = IncludeAll;
#else
		I_Actions.SubscribeAllClicked = SubscribeAll;
#endif

		TLP_Main.Controls.Add(ListControl, 0, TLP_Main.RowCount - 1);
		TLP_Main.SetColumnSpan(ListControl, TLP_Main.ColumnCount);
		TLP_MiddleBar.Controls.Add(I_Actions, 0, 0);

#if CS2
		OT_Workshop.Image2 = "Paradox";
#else
		OT_Workshop.Image2 = "Steam";
		OT_Workshop.Visible = _playsetManager.CurrentPlayset is not null && !(_playsetManager.GetCustomPlayset(_playsetManager.CurrentPlayset)?.DisableWorkshop ?? false);
#endif
		OT_ModAsset.Visible = Page is not SkyvePage.Assets and not SkyvePage.Mods;

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
		ListControl.SelectedItemsChanged += ListControl_SelectedItemsChanged;

		DD_Author.HideUsage = page is SkyvePage.Workshop or SkyvePage.Generic;
		DD_Tags.HideUsage = page is SkyvePage.Workshop or SkyvePage.Generic;

		_delayedSearch = new(350, DelayedSearch);
		_delayedAuthorTagsRefresh = new(350, RefreshAuthorAndTags);

		clearingFilters = false;

		I_SortOrder.ImageName = ListControl.SortDescending ? "SortDesc" : "SortAsc";
		C_ViewTypeControl.GridView = ListControl.GridView;
		C_ViewTypeControl.CompactList = ListControl.CompactList;

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
	}

	private void ListControl_SelectedItemsChanged(object sender, EventArgs e)
	{
		RefreshCounts();

		I_Actions.IsSelected = ListControl.SelectedItemsCount > 0;
	}



	protected async void RefreshAuthorAndTags()
	{
		var items = new List<IPackageIdentity>(ListControl.Items);

		DD_Author.SetItems(items);
		DD_Tags.Items = Page is SkyvePage.Workshop ? (await _workshopService.GetAvailableTags()).ToArray() : _tagUtil.GetDistinctTags().ToArray();
	}

	private void LC_Items_OpenWorkshopSearch()
	{
		var panel = new PC_WorkshopList();

		panel.LC_Items.TB_Search.Text = TB_Search.Text;
		panel.LC_Items.OT_ModAsset.SelectedValue = Page is SkyvePage.Mods ? ThreeOptionToggle.Value.Option1 : Page is SkyvePage.Assets ? ThreeOptionToggle.Value.Option2 : ThreeOptionToggle.Value.None;

		Program.MainForm.PushPanel(panel);
	}

	private void LC_Items_OpenWorkshopSearchInBrowser()
	{
#if CS2
		PlatformUtil.OpenUrl("https://mods.paradoxplaza.com/games/cities_skylines_2");
#else
		PlatformUtil.OpenUrl($"https://steamcommunity.com/workshop/browse/?appid=255710&searchtext={WebUtility.UrlEncode(TB_Search.Text)}&browsesort=trend&section=readytouseitems&actualsort=trend&p=1&days=365" + (Page is SkyvePage.Mods ? "&requiredtags%5B0%5D=Mod" : ""));
#endif
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
		DD_ReportSeverity.SelectedItem = (int)DD_ReportSeverity.SelectedItem == (int)obj ? CompatibilityNotificationFilter.Any : (CompatibilityNotificationFilter)obj;

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	private void LC_Items_DownloadStatusSelected(DownloadStatus obj)
	{
		DD_PackageStatus.SelectedItem = DD_PackageStatus.SelectedItem == (DownloadStatusFilter)(obj + 1) ? DownloadStatusFilter.Any : (DownloadStatusFilter)(obj + 1);

		if (P_FiltersContainer.Height == 0)
		{
			B_Filters_Click(this, null);
		}
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_MiddleBar.BackColor = design.AccentBackColor;
		P_Filters.BackColor = design.BackColor.Tint(Lum: design.IsDarkTheme ? -1 : 1);
		ListControl.BackColor = design.BackColor;
	}

	protected override void LocaleChanged()
	{
		DD_PackageUsage.Text = Locale.PackageUsage;
		DD_PackageStatus.Text = Locale.PackageStatus;
		DD_ReportSeverity.Text = Locale.CompatibilityStatus;
		DD_Tags.Text = LocaleSlickUI.Tags;
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

		Program.MainForm.HandleKeyPress += MainForm_HandleKeyPress;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		P_FiltersContainer.Padding = TB_Search.Margin = I_Refresh.Padding = B_Filters.Padding
			= I_SortOrder.Padding = B_Filters.Margin = I_SortOrder.Margin = I_Refresh.Margin = DD_Sorting.Margin = UI.Scale(new Padding(5));

		B_Filters.Size = B_Filters.GetAutoSize(true);

		OT_Enabled.Margin = OT_Included.Margin = OT_Workshop.Margin = OT_ModAsset.Margin
			= DD_ReportSeverity.Margin = DR_SubscribeTime.Margin = DR_ServerTime.Margin
			= DD_Author.Margin = DD_PackageStatus.Margin = DD_PackageUsage.Margin = DD_Profile.Margin = DD_Tags.Margin = UI.Scale(new Padding(4, 2, 4, 2));

		TLP_MiddleBar.Padding = UI.Scale(new Padding(3, 0, 3, 0));

		I_ClearFilters.Size = UI.Scale(new Size(16, 16));
		DD_Sorting.Width = UI.Scale(175);
		TB_Search.Width = UI.Scale(250);

		var size = UI.Scale(30) - 6;

		TB_Search.MaximumSize = I_Refresh.MaximumSize = B_Filters.MaximumSize = I_SortOrder.MaximumSize = DD_Sorting.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = I_Refresh.MinimumSize = B_Filters.MinimumSize = I_SortOrder.MinimumSize = DD_Sorting.MinimumSize = new Size(0, size);
	}

	private void B_Filters_Click(object sender, MouseEventArgs? e)
	{
		if (e is null || e.Button is MouseButtons.Left or MouseButtons.None)
		{
			B_Filters.Text = P_FiltersContainer.Height == 0 ? "HideFilters" : "ShowFilters";
			//P_FiltersContainer.Size = P_FiltersContainer.Height == 0 ? new Size(0, P_FiltersContainer.Padding.Vertical + P_Filters.Height) : Size.Empty;
			AnimationHandler.Animate(P_FiltersContainer, P_FiltersContainer.Height == 0 ? new Size(0, P_FiltersContainer.Padding.Vertical + P_Filters.Height) : Size.Empty, 4, AnimationOption.IgnoreWidth, ListControl.Invalidate);
			P_FiltersContainer.AutoSize = false;
		}
		else if (e?.Button == MouseButtons.Middle)
		{
			I_ClearFilters_Click(sender, e);
		}
	}

	private async void CentralManager_ContentLoaded()
	{
		await RefreshItems();
	}

	public async Task RefreshItems()
	{
		if (ListControl.ItemCount == 0)
		{
			ListControl.Loading = true;
		}

		tokenSource?.Cancel();
		tokenSource = new();

		var token = tokenSource.Token;

		I_Refresh.Loading = true;

		var items = await GetItems(token);

		if (token.IsCancellationRequested)
		{
			return;
		}

		if (ListControl.ItemCount == 0)
		{
			ListControl.Loading = false;
		}

		ListControl.SetItems(items);

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

	private async void DD_Sorting_SelectedItemChanged(object sender, EventArgs e)
	{
		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.Sorting = (int)DD_Sorting.SelectedItem;
		_settings.UserSettings.Save();

		if (Page is SkyvePage.Workshop)
		{
			await RefreshItems();
		}
		else
		{
			ListControl.SetSorting(DD_Sorting.SelectedItem, ListControl.SortDescending);
		}
	}

	private void DelayedSearch()
	{
		ListControl.DoFilterChanged();
		this.TryInvoke(RefreshCounts);
	}

	private async void FilterChanged(object sender, EventArgs e)
	{
		if (!clearingFilters && Live)
		{
			I_Refresh.Loading = true;

			if (sender == I_Refresh)
			{
				await Task.Delay(150);

				await RefreshItems();
			}
			else if (Page is SkyvePage.Workshop && sender == DD_Tags)
			{
				await RefreshItems();
			}
			else
			{
				_delayedSearch.Run();
			}
		}
	}

	private async void I_ClearFilters_Click(object sender, EventArgs e)
	{
		clearingFilters = true;
		this.ClearForm();
		clearingFilters = false;

		if (Page is SkyvePage.Workshop)
		{
			await RefreshItems();
		}
		else
		{
			FilterChanged(sender, e);
		}
	}

	private bool IsFilteredOut(IPackageIdentity item)
	{
#if CS2
		if (!ListControl.IsGenericPage && _playsetManager.CurrentPlayset is null)
#else
		if (!ListControl.IsGenericPage && (_playsetManager.CurrentCustomPlayset?.DisableWorkshop ?? false))
#endif
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

		if (!firstFilterPassed)
		{
			return false;
		}

		if (OT_Workshop.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Workshop.SelectedValue == ThreeOptionToggle.Value.Option1 == !item.IsLocal())
			{
				return true;
			}
		}

		if (OT_Included.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Included.SelectedValue == ThreeOptionToggle.Value.Option2 == (_packageUtil.IsIncluded(item, out var partiallyIncluded, SelectedPlayset) || partiallyIncluded))
			{
				return true;
			}
		}

		if (OT_Enabled.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Enabled.SelectedValue == ThreeOptionToggle.Value.Option1 != _packageUtil.IsEnabled(item, SelectedPlayset))
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
				if (item.GetPackage()?.IsLocal == true || _packageUtil.GetStatus(item, out _) <= DownloadStatus.OK)
				{
					return true;
				}
			}
			else
			{
				if (((int)DD_PackageStatus.SelectedItem - 1) != (int)_packageUtil.GetStatus(item, out _))
				{
					return true;
				}
			}
		}

		if (DD_PackageUsage.SelectedItems.Count() != DD_PackageUsage.Items.Length)
		{
			var usage = item.GetPackageInfo()?.Usage ?? (PackageUsage)(-1);

			if (!DD_PackageUsage.SelectedItems.Any(x => usage.HasFlag(x)))
			{
				return true;
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

		if (DR_SubscribeTime.Set && !DR_SubscribeTime.Match(item.GetLocalPackageIdentity()?.LocalTime.ToLocalTime() ?? DateTime.MinValue))
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

		if (Page is not SkyvePage.Workshop && DD_Tags.SelectedItems.Any())
		{
			if (!_tagUtil.HasAllTags(item, DD_Tags.SelectedItems))
			{
				return true;
			}
		}

		if (DD_Profile.SelectedItem is not null && !DD_Profile.SelectedItem.Temporary)
		{
			return !_packageUtil.IsIncluded(item, DD_Profile.SelectedItem.Id);
		}

		if (!searchEmpty && Page is not SkyvePage.Workshop)
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

	private bool Search(string searchTerm, IPackageIdentity item)
	{
		return searchTerm.SearchCheck(item.Name)
			|| searchTerm.SearchCheck(item.GetWorkshopInfo()?.Author?.Name)
			|| (!item.IsLocal() ? item.Id.ToString() : Path.GetFileName(item.GetLocalPackageIdentity()?.Folder) ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1;
	}

	private void LC_Items_CanDrawItem(object sender, CanDrawItemEventArgs<IPackageIdentity> e)
	{
		e.DoNotDraw = IsFilteredOut(e.Item);
	}

	protected void RefreshCounts()
	{
		if (ListControl.ItemCount > 0)
		{
			var countText = GetCountText();
			var format = ListControl.SelectedItemsCount == 0 ? (ListControl.UsageFilteredOut == 0 ? Locale.ShowingCount : Locale.ShowingCountWarning) : (ListControl.UsageFilteredOut == 0 ? Locale.ShowingSelectedCount : Locale.ShowingSelectedCountWarning);
			var filteredText = format.FormatPlural(
				ListControl.FilteredCount,
				GetItemText().FormatPlural(ListControl.FilteredCount).ToLower(),
				ListControl.SelectedItemsCount,
				Locale.ItemsHidden.FormatPlural(ListControl.UsageFilteredOut, GetItemText().FormatPlural(ListControl.FilteredCount).ToLower()));

			L_Counts.RightText = countText;
			L_Counts.LeftText = filteredText;
			I_Actions.Visible = true;
		}
		else
		{
			L_Counts.LeftText = L_Counts.RightText = string.Empty;
			I_Actions.Visible = false;
		}

		L_Counts.Invalidate();
		I_Actions.Invalidate();
		I_Refresh.Loading = false;
	}

	protected string GetCountText()
	{
		var mods = ListControl.Items.ToList();
		var modsIncluded = mods.Count(x => _packageUtil.IsIncluded(x, SelectedPlayset));
		var modsEnabled = mods.Count(x => _packageUtil.IsIncludedAndEnabled(x, SelectedPlayset));
		var count = ListControl.ItemCount;

#if CS1
		if (!ServiceCenter.Get<ISettings>().UserSettings.AdvancedIncludeEnable)
		{
			return string.Format(Locale.GenericIncludedTotal, GetItemText().Plural, modsIncluded, count);
		}
#endif

		if (modsIncluded == modsEnabled)
		{
			return string.Format(Locale.GenericIncludedAndEnabledTotal, GetItemText().Plural, modsIncluded, count);
		}

		return string.Format(Locale.GenericIncludedEnabledTotal, GetItemText().Plural, modsIncluded, modsEnabled, count);
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private void TB_Search_TextChanged(object sender, EventArgs e)
	{
#if CS2
		if (Regex.IsMatch(TB_Search.Text, @"/mods/(\d+)"))
		{
			TB_Search.Text = Regex.Match(TB_Search.Text, @"/mods/(\d+)").Groups[1].Value;
			return;
		}
#else
		if (Regex.IsMatch(TB_Search.Text, @"filedetails/\?id=(\d+)"))
		{
			TB_Search.Text = Regex.Match(TB_Search.Text, @"filedetails/\?id=(\d+)").Groups[1].Value;
			return;
		}
#endif

		TB_Search.ImageName = (searchEmpty = string.IsNullOrWhiteSpace(TB_Search.Text)) ? "Search" : "ClearSearch";

		OnSearch();
	}

	protected virtual void OnSearch()
	{
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

		FilterChanged(this, EventArgs.Empty);
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

		I_SortOrder.ImageName = ListControl.SortDescending ? "SortDesc" : "SortAsc";
	}

	private void I_Actions_Click(object sender, EventArgs e)
	{
		var items = ListControl.SelectedOrFilteredItems.ToList();
		var isFiltered = items.Count != ItemCount;
		var isSelected = ListControl.SelectedItemsCount > 0;
		var anyIncluded = items.Any(x => _packageUtil.IsIncluded(x));
		var anyExcluded = items.Any(x => !_packageUtil.IsIncluded(x));
		var anyEnabled = items.Any(x => _packageUtil.IsIncluded(x) && _packageUtil.IsEnabled(x));
		var anyDisabled = items.Any(x => _packageUtil.IsIncluded(x) && !_packageUtil.IsEnabled(x));
		var allLocal = items.Any(x => !x.IsLocal());
		var allWorkshop = items.Any(x => x.IsLocal());

		var stripItems = new SlickStripItem?[]
		{
			  anyDisabled ? new (isSelected ? Locale.EnableAllSelected : isFiltered ? Locale.EnableAllFiltered : Locale.EnableAll, "Ok", async () => await EnableAll()) : null
			, anyEnabled ? new (isSelected ? Locale.DisableAllSelected : isFiltered ? Locale.DisableAllFiltered : Locale.DisableAll, "Enabled",  async () => await DisableAll()) : null
			, new ()
			, anyExcluded ? new (isSelected ? Locale.IncludeAllSelected : isFiltered ? Locale.IncludeAllFiltered : Locale.IncludeAll, "Add",  async() => await IncludeAll()) : null
			, anyIncluded ? new (isSelected ? Locale.ExcludeAllSelected : isFiltered ? Locale.ExcludeAllFiltered : Locale.ExcludeAll, "X",  async() => await ExcludeAll()) : null
			, anyDisabled ? new (isSelected ? Locale.ExcludeAllDisabledSelected : isFiltered ? Locale.ExcludeAllDisabledFiltered : Locale.ExcludeAllDisabled, "Cancel",  async() => await ExcludeAllDisabled()) : null
			, new ()
			, ListControl.SelectedItemsCount < ListControl.FilteredItems.Count() ? new (Locale.SelectAll, "DragDrop",  ListControl.SelectAll) : null
			, ListControl.SelectedItemsCount > 0 ? new (Locale.DeselectAll, "Select", ListControl.DeselectAll) : null
			, new (isSelected ? Locale.CopyAllIdsSelected : isFiltered ? Locale.CopyAllIdsFiltered : Locale.CopyAllIds, "Copy", () => Clipboard.SetText(items.ListStrings(x => x.IsLocal() ? $"Local: {x.Name} {x.GetLocalPackage()?.Version}" : $"{x.Id}: {x.Name} {x.GetLocalPackage()?.Version}", CrossIO.NewLine)))
#if CS1
			, new (Locale.SubscribeAll, "Steam", this is PC_GenericPackageList, action: () => SubscribeAll(this, EventArgs.Empty))
			, new (Locale.DownloadAll, "Install", ListControl.FilteredItems.Any(x => x.GetLocalPackage() is null), action: () => DownloadAll(this, EventArgs.Empty))
			, new (Locale.ReDownloadAll, "ReDownload", ListControl.FilteredItems.Any(x => x.GetLocalPackage() is not null), action: () => ReDownloadAll(this, EventArgs.Empty))
			, new (string.Empty)
			, new (Locale.UnsubscribeAll, "RemoveSteam", action: async () => await UnsubscribeAll())
			, new (Locale.DeleteAll, "Disposable", action: () => DeleteAll(this, EventArgs.Empty))
#endif
		};

		this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, I_Actions.PointToScreen(new Point(I_Actions.Width + 5, 0)), stripItems));
	}

	private async Task DisableAll()
	{
		I_Actions.Loading = true;
		await SetEnabled(ListControl.SelectedOrFilteredItems.ToList(), false);
		ListControl.Invalidate();
		I_Actions.Loading = false;
	}

	private async Task EnableAll()
	{
		I_Actions.Loading = true;
		await SetEnabled(ListControl.SelectedOrFilteredItems.ToList(), true);
		ListControl.Invalidate();
		I_Actions.Loading = false;
	}

	private async Task ExcludeAll()
	{
		var items = ListControl.SelectedOrFilteredItems.ToList();

		if (items.Count > 10 && MessagePrompt.Show(Locale.AreYouSure, PromptButtons.YesNo, PromptIcons.Question, Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		I_Actions.Loading = true;
		await SetIncluded(items, false);
		ListControl.Invalidate();
		I_Actions.Loading = false;
	}

	private async Task ExcludeAllDisabled()
	{
		await SetIncluded(ListControl.SelectedOrFilteredItems.AllWhere(x => !_packageUtil.IsEnabled(x)), false);
		ListControl.Invalidate();
		I_Actions.Loading = false;
	}

	private async Task IncludeAll()
	{
		var count = ListControl.SortedAndFilteredItems.Count();

		if (count > 20)
		{
			if (MessagePrompt.Show(Locale.MassSubscribeWarningInfo.Format(count), Locale.MassSubscribeWarningTitle, PromptButtons.YesNo, PromptIcons.Hand, Program.MainForm) != DialogResult.Yes)
			{
				return;
			}
		}

		I_Actions.Loading = true;
		await SetIncluded(ListControl.SelectedOrFilteredItems.ToList(), true);
		ListControl.Invalidate();
		I_Actions.Loading = false;
	}

	protected async Task SetIncluded(IEnumerable<IPackageIdentity> filteredItems, bool included)
	{
		await _packageUtil.SetIncluded(filteredItems, included, SelectedPlayset);
	}

	protected async Task SetEnabled(IEnumerable<IPackageIdentity> filteredItems, bool enabled)
	{
		await _packageUtil.SetEnabled(filteredItems, enabled, SelectedPlayset);
	}

#if CS1
	private void DownloadAll(object sender, EventArgs e)
	{
		_downloadService.Download(ListControl.SelectedOrFilteredItems.Where(x => x.GetLocalPackage() is null).Select(x => (IPackageIdentity)x));
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private void ReDownloadAll(object sender, EventArgs e)
	{
		_downloadService.Download(ListControl.SelectedOrFilteredItems.Where(x => x.GetLocalPackage() is not null).Cast<IPackageIdentity>());
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private async Task UnsubscribeAll()
	{
		if (MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisUnsubscribesFrom.FormatPlural(ListControl.SelectedOrFilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		I_Actions.Loading = true;
		await ServiceCenter.Get<ISubscriptionsManager>().UnSubscribe(ListControl.SelectedOrFilteredItems.ToList());
		I_Actions.Loading = false;
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private async Task SubscribeAll()
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

		if (steamIds.Count == 0 || MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisSubscribesTo.FormatPlural(ListControl.SelectedOrFilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		await ServiceCenter.Get<ISubscriptionsManager>().Subscribe(steamIds.Select<DrawableItem<Domain.IPackageIdentity, ItemListControl.Rectangles>, Domain.IPackageIdentity>(x => x.Item));
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}
#endif

	private async void DeleteAll(object sender, EventArgs e)
	{
		if (MessagePrompt.Show(Locale.AreYouSure, PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		I_Actions.Loading = true;
		await Task.Run(() =>
		{
			var items = ListControl.SelectedOrFilteredItems.ToList();
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
		C_ViewTypeControl.GridView = ListControl.GridView = false;
		C_ViewTypeControl.CompactList = ListControl.CompactList = false;

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.GridView = ListControl.GridView;
		settings.Compact = ListControl.CompactList;
		_settings.UserSettings.Save();
	}

	private void B_GridView_Click(object sender, EventArgs e)
	{
		C_ViewTypeControl.GridView = ListControl.GridView = true;
		C_ViewTypeControl.CompactList = ListControl.CompactList = false;

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.GridView = ListControl.GridView;
		settings.Compact = ListControl.CompactList;
		_settings.UserSettings.Save();
	}

	private void B_CompactList_Click(object sender, EventArgs e)
	{
		C_ViewTypeControl.GridView = ListControl.GridView = false;
		C_ViewTypeControl.CompactList = ListControl.CompactList = true;

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.GridView = ListControl.GridView;
		settings.Compact = ListControl.CompactList;
		_settings.UserSettings.Save();
	}

	private bool MainForm_HandleKeyPress(Message arg1, Keys arg2)
	{
		if (arg2 == (Keys.Control | Keys.Z))
		{
			ServiceCenter.Get<IModUtil>().UndoChanges();
			return true;
		}

		if (arg2 == (Keys.Control | Keys.Y))
		{
			ServiceCenter.Get<IModUtil>().RedoChanges();
			return true;
		}

		return false;
	}
}
