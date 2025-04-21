using Newtonsoft.Json;

using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Generic;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Systems.Compatibility.Domain;

using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_CompatibilityReport : PanelContent
{
	private bool customReportLoaded;
	private bool searchEmpty = true;
	private bool clearingFilters;
	private bool firstFilterPassed;
	private bool massSnoozeing;
	private readonly int UsageFilteredOut;
	private readonly DelayedAction _delayedSearch;
	private readonly List<string> searchTermsOr = [];
	private readonly List<string> searchTermsAnd = [];
	private readonly List<string> searchTermsExclude = [];
	private readonly IncludeAllButton I_Actions;

	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly ICompatibilityActionsHelper _compatibilityActions;
	private readonly IPackageManager _contentManager;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly ISettings _settings;
	private readonly IPlaysetManager _playsetManager;
	private readonly ITagsService _tagUtil;

	private static SkyvePage Page { get; } = SkyvePage.CompatibilityReport;

	public PC_CompatibilityReport() : base(ServiceCenter.Get<IUserService>().User.Manager && !ServiceCenter.Get<IUserService>().User.Malicious)
	{
		ServiceCenter.Get(out _subscriptionsManager, out _tagUtil, out _compatibilityActions, out _playsetManager, out _settings, out _packageUtil, out _compatibilityManager, out _contentManager, out _notifier, out IUserService userService);

		InitializeComponent();

		ListControl.CanDrawItem += LC_Items_CanDrawItem;
		ListControl.SelectedItemsChanged += (_, _) => RefreshCounts();

		notificationFilterControl.ListControl = ListControl;
		notificationFilterControl.Filter = x => !IsFilteredOut(x, false);
		notificationFilterControl.SelectedGroupChanged += (_, _) => ListControl.DoFilterChanged();

		I_Actions = new IncludeAllButton(() => ListControl.FilteredItems.Cast<IPackageIdentity>().ToList()!);
		I_Actions.ActionClicked += I_Actions_Click;
		I_Actions.IncludeAllClicked += IncludeAll;
		I_Actions.ExcludeAllClicked += ExcludeAll;
		I_Actions.EnableAllClicked += EnableAll;
		I_Actions.DisableAllClicked += DisableAll;
		I_Actions.SubscribeAllClicked += IncludeAll;

		TLP_MiddleBar.Controls.Add(I_Actions, 0, 0);

#if CS2
		OT_Workshop.Image2 = "Paradox";
		OT_Workshop.Visible = _playsetManager.CurrentPlayset is not null;
#else
		OT_Workshop.Image2 = "Steam";
		OT_Workshop.Visible = _playsetManager.CurrentPlayset is not null && !(_playsetManager.GetCustomPlayset(_playsetManager.CurrentPlayset)?.DisableWorkshop ?? false);
#endif

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			OT_Enabled.Hide();
			P_Filters.SetRow(OT_Workshop, 2);
			P_Filters.SetRow(OT_ModAsset, 3);
		}

		if (!_compatibilityManager.FirstLoadComplete)
		{
			ListControl.Loading = true;
		}
		else
		{
			CompatibilityManager_ReportProcessed();
		}

		I_SortOrder.ImageName = ListControl.SortDescending ? "SortDesc" : "SortAsc";

		_delayedSearch = new(350, DelayedSearch);

		_notifier.ContentLoaded += CompatibilityManager_ReportProcessed;
		_notifier.CompatibilityReportProcessed += CompatibilityManager_ReportProcessed;
		_notifier.SnoozeChanged += CompatibilityManager_ReportProcessed;

		new BackgroundAction("Getting tag list", RefreshAuthorAndTags).Run();
		
		if (_settings.UserSettings.FilterIncludedByDefault)
		{
			OT_Included.SelectedValue = ThreeOptionToggle.Value.Option1;
		}
	}

	protected void RefreshAuthorAndTags()
	{
		DD_Tags.Items = _tagUtil.GetDistinctTags().ToArray();
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
		Text = Locale.CompatibilityReport;

		DD_PackageStatus.Text = Locale.PackageStatus;
		DD_Tags.Text = LocaleSlickUI.Tags;
		DD_Playset.Text = Locale.PlaysetFilter;
		DR_SubscribeTime.Text = Locale.DateSubscribed;
		DR_ServerTime.Text = Locale.DateUpdated;
		DD_Author.Text = Locale.Author;
		OT_ModAsset.Option1 = Locale.Mod.Plural;
		OT_ModAsset.Option2 = Locale.Asset.Plural;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		I_SortOrder.Padding = I_Refresh.Padding = UI.Scale(new Padding(5));

		P_FiltersContainer.Padding = TB_Search.Margin = B_Filters.Padding
			= B_Filters.Margin = I_SortOrder.Margin = I_Refresh.Margin = DD_Sorting.Margin = UI.Scale(new Padding(5));

		B_Filters.Size = B_Filters.GetAutoSize(true);

		OT_Enabled.Margin = OT_Included.Margin = OT_Workshop.Margin = OT_ModAsset.Margin
			= DR_SubscribeTime.Margin = DR_ServerTime.Margin
			= DD_Author.Margin = DD_PackageStatus.Margin = DD_Playset.Margin = DD_Tags.Margin = UI.Scale(new Padding(4, 2, 4, 2));

		I_ClearFilters.Size = UI.Scale(new Size(16, 16));
		DD_Sorting.Width = UI.Scale(175);
		TB_Search.Width = UI.Scale(250);

		var size = UI.Scale(30) - 6;

		TB_Search.MaximumSize = B_Filters.MaximumSize = DD_Sorting.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = B_Filters.MinimumSize = DD_Sorting.MinimumSize = new Size(0, size);

		I_SortOrder.Size = I_Refresh.Size = new(size, size);
	}

	private void CompatibilityManager_ReportProcessed()
	{
		if (_compatibilityManager.FirstLoadComplete && !customReportLoaded && !massSnoozeing)
		{
			var packages = _contentManager.Packages.SelectWhereNotNull(x =>
			{
				var info = x.GetCompatibilityInfo(cacheOnly: true);

				return info.GetAction() is StatusAction.ExcludeThis or StatusAction.UnsubscribeThis or StatusAction.Switch && !_packageUtil.IsIncluded(x) ? null : info;
			}).ToList();

			this.TryInvoke(() =>
			{
				LoadReport(packages!);
			});
		}
	}

	private void LoadReport(List<ICompatibilityInfo> reports)
	{
		try
		{
			reports.RemoveAll(x => x.GetNotification() <= NotificationType.Info);

			ListControl.AllStatuses = true;
			ListControl.SetItems(reports);
			ListControl.Loading = false;

			DD_Author.RefreshItems();
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load compatibility report");
		}
	}

	private ExtensionClass.action? GetAction(ICompatibilityInfo report)
	{
		var message = GetMessage(report);

		return message is null || report.GetLocalPackage() is null
			? null
			: message.Status.Action switch
			{
				StatusAction.SubscribeToPackages => () =>
				{
					_packageUtil.SetIncluded(message.Packages, true);
					_packageUtil.SetEnabled(message.Packages, true);
					_compatibilityManager.QuickUpdate(message);
				}
				,
				StatusAction.RequiresConfiguration => () =>
				{
					_compatibilityManager.ToggleSnoozed(message);
					_compatibilityManager.QuickUpdate(message);
				}
				,
				StatusAction.UnsubscribeThis => () =>
				{
					_packageUtil.SetIncluded(report, true);
					_compatibilityManager.QuickUpdate(message);
				}
				,
				StatusAction.UnsubscribeOther => () =>
				{
					_packageUtil.SetIncluded(message.Packages!, false);
					_compatibilityManager.QuickUpdate(message);
				}
				,
				StatusAction.ExcludeThis => () =>
				{
					var pp = report.GetLocalPackage();
					if (pp is not null)
					{
						_packageUtil.SetIncluded(pp, false);
					}

					_compatibilityManager.QuickUpdate(message);
				}
				,
				StatusAction.ExcludeOther => () =>
				{
					foreach (var p in message.Packages!)
					{
						var pp = p.GetLocalPackage();
						if (pp is not null)
						{
							_packageUtil.SetIncluded(pp, false);
						}
					}

					_compatibilityManager.QuickUpdate(message);
				}
				,
				StatusAction.RequestReview => () =>
				{
					Program.MainForm.PushPanel(ServiceCenter.Get<IAppInterfaceService>().RequestReviewPanel(report));
				}
				,
				StatusAction.Switch => message.Packages.Count() == 1 ? () =>
				{
					var pp1 = report.GetLocalPackage();
					var pp2 = message.Packages.FirstOrDefault()?.GetLocalPackage();

					if (pp1 is not null && pp2 is not null)
					{
						_packageUtil.SetIncluded(pp1!, false);
						_packageUtil.SetEnabled(pp1!, false);
						_packageUtil.SetIncluded(pp2!, true);
						_packageUtil.SetEnabled(pp2!, true);
					}

					_compatibilityManager.QuickUpdate(message);
				}
				: null
			,
				_ => null,
			};
	}

	private ICompatibilityItem? GetMessage(ICompatibilityInfo report)
	{
		return report.ReportItems?.Any() == true ? report.ReportItems.OrderBy(x => _compatibilityManager.IsSnoozed(x) ? 0 : x.Status.Notification).LastOrDefault() : null;
	}

	private void LC_Items_CanDrawItem(object sender, CanDrawItemEventArgs<ICompatibilityInfo> e)
	{
		if (!e.DoNotDraw)
		{
			e.DoNotDraw = IsFilteredOut(e.Item, true);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == (Keys.Control | Keys.F))
		{
			TB_Search.Focus();
			TB_Search.SelectAll();
			return true;
		}

		if (keyData == (Keys.Control | Keys.Tab))
		{
			return true;
		}

		if (keyData == (Keys.Control | Keys.Shift | Keys.Tab))
		{
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	public void Import(string file)
	{
		if (Path.GetExtension(file).ToLower() is ".zip")
		{
			using var stream = File.OpenRead(file);
			using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read, false);

			var entry = zipArchive.GetEntry("Skyve\\CompatibilityReport.json") ?? zipArchive.GetEntry("Skyve/CompatibilityReport.json");

			if (entry is null)
			{
				return;
			}

			file = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");

			entry.ExtractToFile(file);
		}

		try
		{
			var items = JsonConvert.DeserializeObject<List<CompatibilityInfo>>(File.ReadAllText(file));

			customReportLoaded = false;

			this.TryInvoke(() =>
			{
				LoadReport(items.ToList(x => (ICompatibilityInfo)x));
			});

			customReportLoaded = true;
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load compatibility report");
		}
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

		TB_Search.ImageName = (searchEmpty = string.IsNullOrWhiteSpace(TB_Search.Text)) ? "Search" : "ClearSearch";

		var searchText = TB_Search.Text.Trim();

		searchTermsAnd.Clear();
		searchTermsExclude.Clear();
		searchTermsOr.Clear();

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

	private async Task ApplyAll()
	{
		var messages = ListControl.FilteredItems.SelectWhereNotNull(GetMessage).ToList();

		massSnoozeing = true;
		I_Actions.Loading = true;
		ListControl.Loading = true;
		ListControl.Enabled = false;
		_notifier.IsBulkUpdating = true;

		foreach (var message in messages)
		{
			var action = _compatibilityActions.GetRecommendedAction(message!);

			if (action is not null)
			{
				await action.Invoke(message!);
			}
		}

		_notifier.IsBulkUpdating = false;
		ListControl.Enabled = true;
		ListControl.Loading = false;
		I_Actions.Loading = false;
		massSnoozeing = false;

		CompatibilityManager_ReportProcessed();
	}

	private void SnoozeAll()
	{
		massSnoozeing = true;
		foreach (var item in ListControl.FilteredItems.ToList())
		{
			var Message = item.ReportItems.FirstOrDefault(x => x.Status.Notification == item.GetNotification() && !_compatibilityManager.IsSnoozed(x));
			_compatibilityManager.ToggleSnoozed(Message);
		}

		massSnoozeing = false;

		CompatibilityManager_ReportProcessed();
	}

	protected virtual async Task SetIncluded(IEnumerable<ICompatibilityInfo> filteredItems, bool included)
	{
		await ServiceCenter.Get<IPackageUtil>().SetIncluded(filteredItems, included);
	}

	protected virtual async Task SetEnabled(IEnumerable<ICompatibilityInfo> filteredItems, bool enabled)
	{
		await ServiceCenter.Get<IPackageUtil>().SetEnabled(filteredItems, enabled);
	}

	private void I_Actions_Click(object sender, EventArgs e)
	{
		var items = ListControl.SelectedOrFilteredItems.ToList();
		var isFiltered = items.Count != ListControl.ItemCount;
		var isSelected = ListControl.SelectedItemsCount > 0;
		var anyIncluded = items.Any(x => _packageUtil.IsIncluded(x));
		var anyExcluded = items.Any(x => !_packageUtil.IsIncluded(x));
		var anyEnabled = items.Any(x => _packageUtil.IsIncluded(x) && _packageUtil.IsEnabled(x));
		var anyDisabled = items.Any(x => _packageUtil.IsIncluded(x) && !_packageUtil.IsEnabled(x));
		var allLocal = items.Any(x => !x.IsLocal());
		var allWorkshop = items.Any(x => x.IsLocal());

		var stripItems = new SlickStripItem?[]
		{
			  new (LocaleCR.ApplyAllActions, "CompatibilityReport", async () => await ApplyAll(), !ListControl.FilteredItems.Any(x => GetMessage(x) is  ICompatibilityItem message && _compatibilityActions.HasRecommendedAction(message)))
			, new (isFiltered ? LocaleCR.SnoozeAllFiltered : LocaleCR.SnoozeAll, "Snooze", action: SnoozeAll)
			, new ()
			, anyDisabled ? new (isSelected ? Locale.EnableAllSelected : isFiltered ? Locale.EnableAllFiltered : Locale.EnableAll, "Ok", async () => await EnableAll()) : null
			, anyEnabled ? new (isSelected ? Locale.DisableAllSelected : isFiltered ? Locale.DisableAllFiltered : Locale.DisableAll, "Enabled",  async () => await DisableAll()) : null
			, new ()
			, anyExcluded ? new (isSelected ? Locale.IncludeAllSelected : isFiltered ? Locale.IncludeAllFiltered : Locale.IncludeAll, "Add",  async() => await IncludeAll()) : null
			, anyIncluded ? new (isSelected ? Locale.ExcludeAllSelected : isFiltered ? Locale.ExcludeAllFiltered : Locale.ExcludeAll, "X",  async() => await ExcludeAll()) : null
			, new ()
			, new (isSelected ? Locale.CopyAllIdsSelected : isFiltered ? Locale.CopyAllIdsFiltered : Locale.CopyAllIds, "Copy", () => Clipboard.SetText(items.ListStrings(x => x.IsLocal() ? $"Local: {x.Name}" : $"{x.Id}: {x.Name}", CrossIO.NewLine)))
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
		await SetEnabled(ListControl.FilteredItems, false);
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private async Task EnableAll()
	{
		await SetEnabled(ListControl.FilteredItems, true);
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private async Task ExcludeAll()
	{
		await SetIncluded(ListControl.FilteredItems, false);
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private async Task IncludeAll()
	{
		await SetIncluded(ListControl.FilteredItems, true);
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

	//private async Task UnsubscribeAll()
	//{
	//	if (MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisUnsubscribesFrom.FormatPlural(ListControl.FilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
	//	{
	//		return;
	//	}

	//	I_Actions.Loading = true;
	//	await ServiceCenter.Get<ISubscriptionsManager>().UnSubscribe(ListControl.FilteredItems.Cast<IPackageIdentity>());
	//	I_Actions.Loading = false;
	//	ListControl.Invalidate();
	//	I_Actions.Invalidate();
	//}

	//private async Task SubscribeAll()
	//{
	//	var removeBadPackages = false;
	//	var steamIds = ListControl.SafeGetItems().AllWhere(x => x.Item.GetLocalPackage() == null && x.Item.Id != 0);

	//	foreach (var item in steamIds.ToList())
	//	{
	//		var report = item.Item.GetCompatibilityInfo();

	//		if (report.GetNotification() >= NotificationType.Unsubscribe)
	//		{
	//			if (!removeBadPackages && MessagePrompt.Show(Locale.ItemsShouldNotBeSubscribedInfo + "\r\n\r\n" + Locale.WouldYouLikeToSkipThose, PromptButtons.YesNo, PromptIcons.Hand, form: Program.MainForm) == DialogResult.No)
	//			{
	//				break;
	//			}
	//			else
	//			{
	//				removeBadPackages = true;
	//			}

	//			steamIds.Remove(item);
	//			ListControl.RemoveAll(x => x.Id == item.Item.Id);
	//		}
	//	}

	//	if (steamIds.Count == 0 || MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisSubscribesTo.FormatPlural(ListControl.FilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
	//	{
	//		return;
	//	}

	//	await ServiceCenter.Get<ISubscriptionsManager>().Subscribe(steamIds.Select(x => (IPackageIdentity)x.Item));
	//	ListControl.Invalidate();
	//	I_Actions.Invalidate();
	//}

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

	private void I_Refresh_SizeChanged(object sender, EventArgs e)
	{
		(sender as Control)!.Width = (sender as Control)!.Height;
	}

	private void B_Filters_Click(object sender, MouseEventArgs e)
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

	private void RefreshCounts()
	{
		if (ListControl.ItemCount > 0)
		{
			var countText = GetCountText();
			var format = ListControl.SelectedItemsCount == 0 ? (UsageFilteredOut == 0 ? Locale.TotalCount : Locale.TotalCountWarning) : (UsageFilteredOut == 0 ? Locale.TotalSelectedCount : Locale.TotalSelectedCountWarning);
			var filteredText = format.FormatPlural(
				ListControl.ItemCount,
				Locale.Package.FormatPlural(ListControl.ItemCount).ToLower(),
				ListControl.SelectedItemsCount,
				Locale.ItemsHidden.FormatPlural(UsageFilteredOut, Locale.Package.FormatPlural(UsageFilteredOut).ToLower()));

			L_Counts.RightText = countText;
			L_Counts.LeftText = filteredText;
			I_Actions.Visible = true;
		}
		else
		{
			L_Counts.LeftText = L_Counts.RightText = string.Empty;
			I_Actions.Visible = false;
		}

		notificationFilterControl.Invalidate();
		L_Counts.Invalidate();
		I_Actions.Invalidate();
		I_Refresh.Loading = false;
	}

	protected string GetCountText()
	{
		var mods = ListControl.FilteredItems.ToList();
		var modsIncluded = mods.Count(x => _packageUtil.IsIncluded(x));
		var modsEnabled = mods.Count(x => _packageUtil.IsIncludedAndEnabled(x));
		var count = ListControl.FilteredCount;

#if CS1
		if (!ServiceCenter.Get<ISettings>().UserSettings.AdvancedIncludeEnable)
		{
			return string.Format(Locale.GenericIncludedTotal, GetItemText().Plural, modsIncluded, count);
		}
#endif

		if (modsIncluded == modsEnabled)
		{
			return string.Format(Locale.GenericIncludedAndEnabledTotal, Locale.Package.Plural, modsIncluded, count);
		}

		return string.Format(Locale.GenericIncludedEnabledTotal, Locale.Package.Plural, modsIncluded, modsEnabled, count);
	}

	private void DelayedSearch()
	{
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

	private bool IsFilteredOut(ICompatibilityInfo item, bool withGrouping)
	{
		if (!firstFilterPassed)
		{
			return false;
		}

		if (withGrouping && notificationFilterControl.CurrentGroup != NotificationType.None && item.GetNotification() != notificationFilterControl.CurrentGroup)
		{
			return true;
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
			if (OT_Included.SelectedValue == ThreeOptionToggle.Value.Option2 == (_packageUtil.IsIncluded(item, out var partiallyIncluded) || partiallyIncluded))
			{
				return item.GetNotification() is not NotificationType.RequiredItem;
			}
		}

		if (OT_Enabled.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Enabled.SelectedValue == ThreeOptionToggle.Value.Option1 != _packageUtil.IsEnabled(item))
			{
				return true;
			}
		}

		if (OT_ModAsset.SelectedValue != ThreeOptionToggle.Value.None)
		{
			var isCodeMod = item.IsCodeMod() && (item.GetPackageInfo()?.Type is null or PackageType.GenericPackage);

			if (OT_ModAsset.SelectedValue == ThreeOptionToggle.Value.Option2 == isCodeMod)
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

		if (DD_Tags.SelectedItems.Any())
		{
			if (!_tagUtil.HasAllTags(item, DD_Tags.SelectedItems))
			{
				return true;
			}
		}

#if CS1
		if (DD_Playset.SelectedItem is not null && !DD_Playset.SelectedItem.Temporary)
#else
		if (DD_Playset.SelectedItem is not null)
#endif
		{
			return !_packageUtil.IsIncluded(item, DD_Playset.SelectedItem.Id);
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
		return searchTerm.SearchCheck(item.ToString())
			|| searchTerm.SearchCheck(item.GetWorkshopInfo()?.Author?.Name)
			|| (!item.IsLocal() ? item.Id.ToString() : Path.GetFileName(item.GetLocalPackage()?.Folder) ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1;
	}

	private void I_Refresh_Click(object sender, EventArgs e)
	{
		CompatibilityManager_ReportProcessed();

		FilterChanged(sender, e);
	}

	private void DD_Sorting_SelectedItemChanged(object sender, EventArgs e)
	{
		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.Sorting = (int)DD_Sorting.SelectedItem;
		_settings.SessionSettings.Save();

		ListControl.SetSorting(DD_Sorting.SelectedItem, ListControl.SortDescending);
	}

	private void I_SortOrder_Click(object sender, EventArgs e)
	{
		ListControl.SetSorting(DD_Sorting.SelectedItem, !ListControl.SortDescending);

		var settings = _settings.UserSettings.PageSettings.GetOrAdd(Page);
		settings.DescendingSort = ListControl.SortDescending;
		_settings.SessionSettings.Save();

		I_SortOrder.ImageName = ListControl.SortDescending ? "SortDesc" : "SortAsc";
	}
}
