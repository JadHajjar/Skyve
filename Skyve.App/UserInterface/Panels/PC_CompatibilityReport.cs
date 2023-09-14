using Newtonsoft.Json;

using Skyve.App.UserInterface.Dropdowns;
using Skyve.App.UserInterface.Generic;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;
using Skyve.Systems.Compatibility.Domain.Api;

using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_CompatibilityReport : PanelContent
{
	private ReviewRequest[]? reviewRequests;
	private bool customReportLoaded;
	private bool searchEmpty = true;
	private List<ExtensionClass.action>? recommendedActions;
	private bool clearingFilters;
	private bool firstFilterPassed;
	private readonly DelayedAction _delayedSearch;
	private readonly List<string> searchTermsOr = new();
	private readonly List<string> searchTermsAnd = new();
	private readonly List<string> searchTermsExclude = new();
	private readonly IncludeAllButton<ILocalPackage> I_Actions;

	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly IBulkUtil _bulkUtil;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageManager _contentManager;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IUserService _userService;
	private readonly ISettings _settings;
	private readonly IDownloadService _downloadService;
	private readonly IPlaysetManager _playsetManager;
	private readonly ITagsService _tagUtil;
	private readonly SkyveApiUtil _skyveApiUtil;

	public PC_CompatibilityReport() : base(ServiceCenter.Get<IUserService>().User.Manager && !ServiceCenter.Get<IUserService>().User.Malicious)
	{
		ServiceCenter.Get(out _subscriptionsManager, out _tagUtil, out _playsetManager, out _downloadService, out _settings, out _packageUtil, out _bulkUtil, out _compatibilityManager, out _contentManager, out _notifier, out _userService, out _skyveApiUtil);

		InitializeComponent();

		SetManagementButtons();

		ListControl.Visible = false;
		ListControl.CanDrawItem += LC_Items_CanDrawItem;
		ListControl.GroupChanged += LC_Items_GroupChanged;

		I_Actions = new IncludeAllButton<ILocalPackage>(() => ListControl.FilteredItems.SelectWhereNotNull(x => x.Package).ToList()!);
		I_Actions.ActionClicked += I_Actions_Click;
		I_Actions.IncludeAllClicked += IncludeAll;
		I_Actions.ExcludeAllClicked += ExcludeAll;
		I_Actions.EnableAllClicked += EnableAll;
		I_Actions.DisableAllClicked += DisableAll;
		I_Actions.SubscribeAllClicked += SubscribeAll;

		TLP_MiddleBar.Controls.Add(I_Actions, 0, 0);

		OT_Workshop.Visible = !_playsetManager.CurrentPlayset.DisableWorkshop;

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

	protected override async Task<bool> LoadDataAsync()
	{
		reviewRequests = await _skyveApiUtil.GetReviewRequests();

		return true;
	}

	protected override void OnDataLoad()
	{
		B_Requests.Text = LocaleCR.ReviewRequests.Format(reviewRequests is null ? string.Empty : $"({reviewRequests.Length})");
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

		PB_Loader.Size = UI.Scale(new System.Drawing.Size(32, 32), UI.FontScale);
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
		if (_compatibilityManager.FirstLoadComplete && !customReportLoaded)
		{
			var packages = _contentManager.Packages.SelectWhereNotNull(x =>
			{
				var info = x.GetCompatibilityInfo(cacheOnly: true);

				if (info.GetNotification() is not NotificationType.Unsubscribe && !_packageUtil.IsIncluded(x))
				{
					return null;
				}

				return info;
			}).ToList();

			this.TryInvoke(() => { LoadReport(packages!); PB_Loader.Hide(); });
		}

		this.TryInvoke(SetManagementButtons);
	}

	private void SetManagementButtons()
	{
		var hasPackages = _userService.User.Id is not null && _contentManager.Packages.Any(x => _userService.User.Equals(x.GetWorkshopInfo()?.Author));
		B_Manage.Visible = B_Requests.Visible = B_ManageSingle.Visible = _userService.User.Manager && !_userService.User.Malicious;
		B_YourPackages.Visible = hasPackages && !_userService.User.Manager && !_userService.User.Malicious;
		B_Requests.Text = LocaleCR.ReviewRequests.Format(reviewRequests is null ? string.Empty : $"({reviewRequests.Length})");
	}

	private void B_Manage_Click(object sender, EventArgs e)
	{
		Form.PushPanel(null, new PC_CompatibilityManagement());
	}

	private void B_ManageSingle_Click(object sender, EventArgs e)
	{
		var form = new PC_SelectPackage() { Text = LocaleHelper.GetGlobalText("Select a package") };

		form.PackageSelected += Form_PackageSelected;

		Program.MainForm.PushPanel(null, form);

	}

	private void Form_PackageSelected(IEnumerable<ulong> packages)
	{
		Form.PushPanel(null, new PC_CompatibilityManagement(packages));
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
			ListControl.Visible = true;

			DD_Author.SetItems(reports.Select(x => x.Package));
		}
		catch (Exception ex) { ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load compatibility report"); }
	}

	private ExtensionClass.action? GetAction(ICompatibilityInfo report)
	{
		var message = report.ReportItems.FirstOrDefault(x => x.Status.Notification == ListControl.CurrentGroup && !_compatibilityManager.IsSnoozed(x));

		if (message is null || report.Package is null)
		{
			return null;
		}

		return message.Status.Action switch
		{
			StatusAction.SubscribeToPackages => () =>
			{
				_subscriptionsManager.Subscribe(message.Packages.Where(x => x.GetLocalPackage() is null));
				_bulkUtil.SetBulkIncluded(message.Packages.SelectWhereNotNull(x => x.GetLocalPackage())!, true);
				_bulkUtil.SetBulkEnabled(message.Packages.SelectWhereNotNull(x => x.GetLocalPackage())!, true);
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
				_subscriptionsManager.UnSubscribe(new[] { report.Package! });
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
				var pp = report.Package?.GetLocalPackage();
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
				Program.MainForm.PushPanel(null, new PC_RequestReview(report.Package!));
			}
			,
			StatusAction.Switch => message.Packages.Length == 1 ? () =>
			{
				var pp1 = report.Package?.LocalParentPackage;
				var pp2 = message.Packages[0]?.LocalParentPackage;

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

	private void LC_Items_GroupChanged(object sender, EventArgs e)
	{
		recommendedActions = ListControl.Items.SelectWhereNotNull(GetAction).ToList()!;

		this.TryInvoke(() => B_ApplyAll.Enabled = recommendedActions.Count > 0);
	}

	private void LC_Items_CanDrawItem(object sender, CanDrawItemEventArgs<ICompatibilityInfo> e)
	{
		if (!e.DoNotDraw && e.Item.Package is not null)
		{
			e.DoNotDraw = IsFilteredOut(e.Item.Package);
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
				B_ApplyAll.Hide();
				LoadReport(items.ToList(x => (ICompatibilityInfo)x));
			});

			customReportLoaded = true;
		}
		catch (Exception ex) { ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load compatibility report"); }
	}

	private async void B_Requests_Click(object sender, EventArgs e)
	{
		if (reviewRequests == null)
		{
			B_Requests.Loading = true;
			reviewRequests = await _skyveApiUtil.GetReviewRequests();
		}

		B_Requests.Loading = false;
		if (reviewRequests != null)
		{
			Form.Invoke(() => Form.PushPanel(null, new PC_ReviewRequests(reviewRequests)));
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

	private async void B_ApplyAll_Click(object sender, EventArgs e)
	{
		if (!B_ApplyAll.Loading && recommendedActions is not null)
		{
			B_ApplyAll.Loading = true;
			await Task.Run(() => Parallelism.ForEach(recommendedActions, 4));
			ListControl.FilterChanged();
			B_ApplyAll.Loading = false;
		}
	}

	protected virtual void SetIncluded(IEnumerable<ICompatibilityInfo> filteredItems, bool included)
	{
		ServiceCenter.Get<IBulkUtil>().SetBulkIncluded(filteredItems.SelectWhereNotNull(x => x.Package)!, included);
	}

	protected virtual void SetEnabled(IEnumerable<ICompatibilityInfo> filteredItems, bool enabled)
	{
		ServiceCenter.Get<IBulkUtil>().SetBulkEnabled(filteredItems.SelectWhereNotNull(x => x.Package)!, enabled);
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
			, new (Locale.CopyAllIds, "I_Copy", action: () => Clipboard.SetText(ListControl.FilteredItems.ListStrings(x => x.Package?.IsLocal == true ? $"Local: {x.Package?.Name}" : $"{x.Package?.Id}: {x.Package?.Name}", CrossIO.NewLine)))
			, new (Locale.DownloadAll, "I_Install", ListControl.FilteredItems.Any(x => x.Package is null), action: () => DownloadAll(this, EventArgs.Empty))
			, new (Locale.ReDownloadAll, "I_ReDownload", ListControl.FilteredItems.Any(x => x.Package is not null), action: () => ReDownloadAll(this, EventArgs.Empty))
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

	private void DownloadAll(object sender, EventArgs e)
	{
		_downloadService.Download(ListControl.FilteredItems.Where(x => x.Package is null).Select(x => (IPackageIdentity)x));
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

	private void ReDownloadAll(object sender, EventArgs e)
	{
		_downloadService.Download(ListControl.FilteredItems.Where(x => x.Package is not null).Cast<IPackageIdentity>());
		ListControl.Invalidate();
		I_Actions.Invalidate();
	}

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
		var steamIds = ListControl.SafeGetItems().AllWhere(x => x.Item.Package == null && x.Item.Package?.Id != 0);

		foreach (var item in steamIds.ToList())
		{
			var report = item.Item;

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
				ListControl.RemoveAll(x => x.Package?.Id == item.Item.Package?.Id);
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
				if (item.Package?.IsLocal == true && item is IAsset asset)
				{
					CrossIO.DeleteFile(asset.FilePath);
				}
				else if (item is ILocalPackage package)
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

	private bool IsFilteredOut(ILocalPackage item)
	{
		if (!firstFilterPassed)
		{
			return false;
		}

		if (OT_Workshop.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Workshop.SelectedValue == ThreeOptionToggle.Value.Option1 == !item.IsLocal)
			{
				return true;
			}
		}

		if (OT_Included.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_Included.SelectedValue == ThreeOptionToggle.Value.Option2 == (item.LocalPackage is not null && (item.LocalPackage.IsIncluded(out var partiallyIncluded) || partiallyIncluded)))
			{
				return true;
			}
		}

		if (OT_Enabled.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (!item.IsMod || OT_Enabled.SelectedValue == ThreeOptionToggle.Value.Option1 != (item.LocalPackage?.IsEnabled()))
			{
				return true;
			}
		}

		if (OT_ModAsset.SelectedValue != ThreeOptionToggle.Value.None)
		{
			if (OT_ModAsset.SelectedValue == ThreeOptionToggle.Value.Option2 == item.IsMod)
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
				if (item.IsLocal || _packageUtil.GetStatus(item, out _) <= DownloadStatus.OK)
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

		if (DR_SubscribeTime.Set && !DR_SubscribeTime.Match(item.LocalPackage?.LocalTime.ToLocalTime() ?? DateTime.MinValue))
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
			return !_playsetManager.IsPackageIncludedInPlayset(item, DD_Profile.SelectedItem);
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

	private bool Search(string searchTerm, ILocalPackage item)
	{
		return searchTerm.SearchCheck(item.ToString())
			|| searchTerm.SearchCheck(item.GetWorkshopInfo()?.Author?.Name)
			|| (!item.IsLocal ? item.Id.ToString() : Path.GetFileName(item.LocalParentPackage?.Folder) ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1;
	}

	private void I_Refresh_Click(object sender, EventArgs e)
	{
		CompatibilityManager_ReportProcessed();

		FilterChanged(sender, e);
	}
}
