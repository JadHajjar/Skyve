using Newtonsoft.Json;

using Skyve.App.UserInterface.Generic;
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
	private readonly DelayedAction _delayedSearch;
	private readonly List<string> searchTermsOr = [];
	private readonly List<string> searchTermsAnd = [];
	private readonly List<string> searchTermsExclude = [];
	private readonly IncludeAllButton<ICompatibilityInfo> I_Actions;

	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageManager _contentManager;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly ISettings _settings;
	private readonly IDownloadService _downloadService;
	private readonly IPlaysetManager _playsetManager;
	private readonly ITagsService _tagUtil;

	private static SkyvePage Page { get; } = SkyvePage.CompatibilityReport;

	public PC_CompatibilityReport() : base(ServiceCenter.Get<IUserService>().User.Manager && !ServiceCenter.Get<IUserService>().User.Malicious)
	{
		ServiceCenter.Get(out _subscriptionsManager, out _tagUtil, out _playsetManager, out _downloadService, out _settings, out _packageUtil, out _compatibilityManager, out _contentManager, out _notifier, out IUserService userService);

		InitializeComponent();

		ListControl.Visible = false;
		ListControl.CanDrawItem += LC_Items_CanDrawItem;
		ListControl.SelectedItemsChanged += (_, _) => RefreshCounts();

		I_Actions = new IncludeAllButton<ICompatibilityInfo>(() => ListControl.FilteredItems.ToList()!);
		I_Actions.ActionClicked += I_Actions_Click;
		I_Actions.IncludeAllClicked += IncludeAll;
		I_Actions.ExcludeAllClicked += ExcludeAll;
		I_Actions.EnableAllClicked += EnableAll;
		I_Actions.DisableAllClicked += DisableAll;
		I_Actions.SubscribeAllClicked += SubscribeAll;

		TLP_MiddleBar.Controls.Add(I_Actions, 0, 0);

		OT_Workshop.Visible = _playsetManager.CurrentPlayset is not null && !_playsetManager.CurrentPlayset.DisableWorkshop;

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			OT_Enabled.Hide();
			P_Filters.SetRow(OT_Workshop, 2);
			P_Filters.SetRow(OT_ModAsset, 3);
		}

		if (!_compatibilityManager.FirstLoadComplete)
		{
			PB_Loader.Visible = true;
			PB_Loader.Loading = true;
		}
		else
		{
			CompatibilityManager_ReportProcessed();
		}

		_delayedSearch = new(350, DelayedSearch);

		_notifier.ContentLoaded += CompatibilityManager_ReportProcessed;
		_notifier.CompatibilityReportProcessed += CompatibilityManager_ReportProcessed;
		_compatibilityManager.SnoozeChanged += CompatibilityManager_ReportProcessed;
		new BackgroundAction("Getting tag list", RefreshAuthorAndTags).Run();
	}

	protected void RefreshAuthorAndTags()
	{
		DD_Tags.Items = _tagUtil.GetDistinctTags().ToArray();
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_MiddleBar.BackColor = design.AccentBackColor;
		P_Filters.BackColor = design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, -1, 1));
		ListControl.BackColor = design.BackColor;
		L_FilterCount.ForeColor = design.InfoColor;
	}

	protected override void LocaleChanged()
	{
		Text = Locale.CompatibilityReport;

		DD_PackageStatus.Text = Locale.PackageStatus;
		DD_Tags.Text = Locale.Tags;
		DD_Profile.Text = Locale.PlaysetFilter;
		DR_SubscribeTime.Text = Locale.DateSubscribed;
		DR_ServerTime.Text = Locale.DateUpdated;
		DD_Author.Text = Locale.Author;
		OT_ModAsset.Option1 = Locale.Mod.Plural;
		OT_ModAsset.Option2 = Locale.Asset.Plural;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		PB_Loader.Size = UI.Scale(new Size(32, 32), UI.FontScale);
		PB_Loader.Location = ClientRectangle.Center(PB_Loader.Size);

		P_FiltersContainer.Padding = TB_Search.Margin = I_Refresh.Padding = B_Filters.Padding
			= L_FilterCount.Margin = I_SortOrder.Padding
			= B_Filters.Margin = I_SortOrder.Margin = I_Refresh.Margin = DD_Sorting.Margin = UI.Scale(new Padding(5), UI.FontScale);

		B_Filters.Size = B_Filters.GetAutoSize(true);

		OT_Enabled.Margin = OT_Included.Margin = OT_Workshop.Margin = OT_ModAsset.Margin
			= DR_SubscribeTime.Margin = DR_ServerTime.Margin
			= DD_Author.Margin = DD_PackageStatus.Margin = DD_Profile.Margin = DD_Tags.Margin = UI.Scale(new Padding(4, 2, 4, 2), UI.FontScale);

		I_ClearFilters.Size = UI.Scale(new Size(16, 16), UI.FontScale);
		L_FilterCount.Font = UI.Font(7.5F, FontStyle.Bold);
		DD_Sorting.Width = (int)(175 * UI.FontScale);
		TB_Search.Width = (int)(250 * UI.FontScale);

		var size = (int)(30 * UI.FontScale) - 6;

		TB_Search.MaximumSize = I_Refresh.MaximumSize = B_Filters.MaximumSize = I_SortOrder.MaximumSize = DD_Sorting.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = I_Refresh.MinimumSize = B_Filters.MinimumSize = I_SortOrder.MinimumSize = DD_Sorting.MinimumSize = new Size(0, size);
	}

	private void CompatibilityManager_ReportProcessed()
	{
		if (_compatibilityManager.FirstLoadComplete && !customReportLoaded && !massSnoozeing)
		{
			var packages = _contentManager.Packages.SelectWhereNotNull(x =>
			{
				var info = x.GetCompatibilityInfo(cacheOnly: true);

				return info.GetNotification() is not NotificationType.Unsubscribe && !_packageUtil.IsIncluded(x) ? null : info;
			}).ToList();

			this.TryInvoke(() =>
			{
				LoadReport(packages!);
				PB_Loader.Hide();
			});
		}
	}

	private void B_Manage_Click(object sender, EventArgs e)
	{
		Form.PushPanel<PC_ManageCompatibilitySelection>();
	}

	private void LoadReport(List<ICompatibilityInfo> reports)
	{
		try
		{
			reports.RemoveAll(x => x.GetNotification() <= NotificationType.Info);

			if (reports.Count == 0)
			{
				label1.Location = ClientRectangle.Center(label1.Size);
				label1.Show();
			}
			else
			{
				label1.Hide();
			}

			ListControl.SetItems(reports);
			ListControl.Visible = reports.Count > 0;

			DD_Author.SetItems(reports);
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load compatibility report");
		}
	}

	private ExtensionClass.action? GetAction(ICompatibilityInfo report)
	{
		var message = report.ReportItems.FirstOrDefault(x => x.Status.Notification == ListControl.CurrentGroup && !_compatibilityManager.IsSnoozed(x));

		return message is null || report.GetLocalPackage() is null
			? null
			: message.Status.Action switch
			{
				StatusAction.SubscribeToPackages => () =>
				{
					_subscriptionsManager.Subscribe(message.Packages.Where(x => !x.IsInstalled()));
					_packageUtil.SetIncluded(message.Packages.SelectWhereNotNull(x => x.GetLocalPackage())!, true);
					_packageUtil.SetEnabled(message.Packages.SelectWhereNotNull(x => x.GetLocalPackage())!, true);
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
					_subscriptionsManager.UnSubscribe(new[] { report });
					_compatibilityManager.QuickUpdate(message);
				}
				,
				StatusAction.UnsubscribeOther => () =>
				{
					_subscriptionsManager.UnSubscribe(message.Packages!);
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
					Program.MainForm.PushPanel(null, new PC_RequestReview(report));
				}
				,
				StatusAction.Switch => message.Packages.Length == 1 ? () =>
				{
					var pp1 = report.GetLocalPackage();
					var pp2 = message.Packages[0]?.GetLocalPackage();

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

	private void LC_Items_CanDrawItem(object sender, CanDrawItemEventArgs<ICompatibilityInfo> e)
	{
		if (!e.DoNotDraw)
		{
			e.DoNotDraw = IsFilteredOut(e.Item);
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
			ListControl.Next();
			return true;
		}

		if (keyData == (Keys.Control | Keys.Shift | Keys.Tab))
		{

			ListControl.Previous();
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

		TB_Search.ImageName = (searchEmpty = string.IsNullOrWhiteSpace(TB_Search.Text)) ? "I_Search" : "I_ClearSearch";

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

	private async void ApplyAll()
	{
		await Task.Run(() => Parallelism.ForEach(ListControl.FilteredItems.SelectWhereNotNull(GetAction).ToList(), 4));
		ListControl.DoFilterChanged();
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
		var items = new SlickStripItem[]
		{
			  new (Locale.IncludeAll, "I_Check", action: async () => await IncludeAll())
			, new (Locale.ExcludeAll, "I_X", action: async () => await ExcludeAll())
			, new (string.Empty)
			, new (Locale.EnableAll, "I_Enabled", _settings.UserSettings.AdvancedIncludeEnable, action: async () => await EnableAll())
			, new (Locale.DisableAll, "I_Disabled", _settings.UserSettings.AdvancedIncludeEnable, action: async () => await DisableAll())
			, new (string.Empty)
			, new (LocaleCR.ApplyAllActions, "I_CompatibilityReport", ListControl.FilteredItems.Any(x => GetAction(x) is not null), action: ApplyAll)
			, new (LocaleCR.SnoozeAll, "I_Snooze", action: SnoozeAll)
			, new (string.Empty)
			, new (Locale.UnsubscribeAll, "I_RemoveSteam", action: async () => await UnsubscribeAll())
			, new (Locale.DeleteAll, "I_Disposable", action: () => DeleteAll(this, EventArgs.Empty))
		};

		this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, I_Actions.PointToScreen(new Point(I_Actions.Width + 5, 0)), items));
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

	private async Task UnsubscribeAll()
	{
		if (MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisUnsubscribesFrom.FormatPlural(ListControl.FilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		I_Actions.Loading = true;
		await ServiceCenter.Get<ISubscriptionsManager>().UnSubscribe(ListControl.FilteredItems.Cast<IPackageIdentity>());
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

		if (steamIds.Count == 0 || MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ThisSubscribesTo.FormatPlural(ListControl.FilteredItems.Count()), PromptButtons.YesNo, form: Program.MainForm) != DialogResult.Yes)
		{
			return;
		}

		await ServiceCenter.Get<ISubscriptionsManager>().Subscribe(steamIds.Select(x => (IPackageIdentity)x.Item));
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
		var format = ListControl.SelectedItemsCount == 0 ? Locale.ShowingCount : Locale.ShowingSelectedCount;
		var filteredText = format.FormatPlural(
			ListControl.FilteredCount,
			Locale.Package.FormatPlural(ListControl.FilteredCount).ToLower(),
			ListControl.SelectedItemsCount,
			string.Empty);

		if (L_FilterCount.Text != filteredText)
		{
			L_FilterCount.Visible = !string.IsNullOrEmpty(filteredText);
			L_FilterCount.Text = filteredText;
		}

		I_Actions.Invalidate();
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

	private bool IsFilteredOut(IPackageIdentity item)
	{
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
			if (!_tagUtil.HasAllTags(item, DD_Tags.SelectedItems))
			{
				return true;
			}
		}

		if (DD_Profile.SelectedItem is not null && !DD_Profile.SelectedItem.Temporary)
		{
			return !_packageUtil.IsIncluded(item, DD_Profile.SelectedItem.Id);
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

		I_SortOrder.ImageName = ListControl.SortDescending ? "I_SortDesc" : "I_SortAsc";
	}
}
