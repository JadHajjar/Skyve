using Newtonsoft.Json;

using SkyveApp.Systems.Compatibility.Domain;
using SkyveApp.Systems.Compatibility.Domain.Api;
using SkyveApp.Systems.CS1.Utilities;

using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Panels;
public partial class PC_CompatibilityReport : PanelContent
{
	private ReviewRequest[]? reviewRequests;
	private NotificationType CurrentKey;
	private bool customReportLoaded;
	private bool searchEmpty = true;
	private List<ExtensionClass.action>? recommendedActions;
	private readonly List<string> searchTermsOr = new();
	private readonly List<string> searchTermsAnd = new();
	private readonly List<string> searchTermsExclude = new();

	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly IBulkUtil _bulkUtil;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageManager _contentManager;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IUserService _userService;
	private readonly SkyveApiUtil _skyveApiUtil;

	public PC_CompatibilityReport() : base(ServiceCenter.Get<IUserService>().User.Manager && !ServiceCenter.Get<IUserService>().User.Malicious)
	{
		ServiceCenter.Get(out _subscriptionsManager, out _packageUtil, out _bulkUtil, out _compatibilityManager, out _contentManager, out _notifier, out _userService, out _skyveApiUtil);

		InitializeComponent();

		SetManagementButtons();

		LC_Items.Visible = false;
		LC_Items.CanDrawItem += LC_Items_CanDrawItem;

		if (!_compatibilityManager.FirstLoadComplete)
		{
			PB_Loader.Visible = true;
			PB_Loader.Loading = true;
		}
		else
		{
			CompatibilityManager_ReportProcessed();
		}

		_notifier.CompatibilityReportProcessed += CompatibilityManager_ReportProcessed;
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

	protected override void LocaleChanged()
	{
		Text = Locale.CompatibilityReport;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		PB_Loader.Size = UI.Scale(new System.Drawing.Size(32, 32), UI.FontScale);
		PB_Loader.Location = ClientRectangle.Center(PB_Loader.Size);

		TB_Search.Margin = UI.Scale(new Padding(5), UI.FontScale);
		TB_Search.Width = (int)(250 * UI.FontScale);

		var size = (int)(30 * UI.FontScale) - 6;

		TB_Search.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = new Size(0, size);
	}

	private void CompatibilityManager_ReportProcessed()
	{
		if (_compatibilityManager.FirstLoadComplete && !customReportLoaded)
		{
			var packages = _contentManager.Packages.ToList(x => x.GetCompatibilityInfo());

			packages.RemoveAll(x => x.GetNotification() < NotificationType.Unsubscribe && !(x.Package?.IsIncluded() == true));

			this.TryInvoke(() => { LoadReport(packages); PB_Loader.Dispose(); });
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
			var notifs = reports.GroupBy(x => x.GetNotification()).Where(x => x.Key > NotificationType.Info).OrderByDescending(x => x.Key).ToList();

			if (tabHeader.Tabs.Select(x => (NotificationType)x.Tag).SequenceEqual(notifs.Select(x => x.Key)))
			{
				LC_Items.SetItems(reports);

				recommendedActions = LC_Items.Items.SelectWhereNotNull(GetAction).ToList()!;

				this.TryInvoke(() =>
				{
					B_ApplyAll.Enabled = recommendedActions.Count > 0;
					foreach (var item in tabHeader.Tabs)
					{
						item.Text = LocaleCR.Get(item.Tag.ToString()) + $" ({(notifs.FirstOrDefault(x => x.Key == (NotificationType)item.Tag)?.Count() ?? 0)})";
					}
				});

				return;
			}

			var tabs = new List<SlickTab>();
			foreach (var report in notifs)
			{
				var tab = new SlickTab()
				{
					Tag = report.Key,
					Text = LocaleCR.Get(report.Key.ToString()) + $" ({report.Count()})",
					Tint = report.Key.GetColor(),
					IconName = report.Key.GetIcon(true)
				};

				tab.TabSelected += Tab_TabSelected;

				tabs.Add(tab);
			}

			tabHeader.Tabs = tabs.ToArray();
			LC_Items.SetItems(reports);
			LC_Items.Visible = true;
		}
		catch (Exception ex) { ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load compatibility report"); }
	}

	private ExtensionClass.action? GetAction(ICompatibilityInfo report)
	{
		var message = report.ReportItems.FirstOrDefault(x => x.Status.Notification == CurrentKey && !_compatibilityManager.IsSnoozed(x));

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
			},
			StatusAction.RequiresConfiguration => () =>
			{
				_compatibilityManager.ToggleSnoozed(message);

			},
			StatusAction.UnsubscribeThis => () =>
			{
				_subscriptionsManager.UnSubscribe(new[] { report.Package! });
			},
			StatusAction.UnsubscribeOther => () =>
			{
				_subscriptionsManager.UnSubscribe(message.Packages!);

			},
			StatusAction.ExcludeThis => () =>
			{
				var pp = report.Package?.GetLocalPackage();
				if (pp is not null)
				{
					_packageUtil.SetIncluded(pp, false);
				}
			},
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
			},
			StatusAction.RequestReview => () =>
			{
				Program.MainForm.PushPanel(null, new PC_RequestReview(report.Package!));
			},
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
			} : null
			,
			_ => null,
		};
	}

	private void Tab_TabSelected(object sender, EventArgs e)
	{
		CurrentKey = (NotificationType)(sender as SlickTab)!.Tag;

		recommendedActions = LC_Items.Items.SelectWhereNotNull(GetAction).ToList()!;

		LC_Items.FilterChanged();

		this.TryInvoke(() => B_ApplyAll.Enabled = recommendedActions.Count > 0);
	}

	private void LC_Items_CanDrawItem(object sender, CanDrawItemEventArgs<ICompatibilityInfo> e)
	{
		e.DoNotDraw = e.Item.GetNotification() != CurrentKey;

		if (!searchEmpty && !e.DoNotDraw && e.Item.Package is not null)
		{
			for (var i = 0; i < searchTermsExclude.Count; i++)
			{
				if (Search(searchTermsExclude[i], e.Item.Package))
				{
					e.DoNotDraw = true;
					return;
				}
			}

			var orMatched = searchTermsOr.Count == 0;

			for (var i = 0; i < searchTermsOr.Count; i++)
			{
				if (Search(searchTermsOr[i], e.Item.Package))
				{
					orMatched = true;
					break;
				}
			}

			if (!orMatched)
			{
				e.DoNotDraw = true;
				return;
			}

			for (var i = 0; i < searchTermsAnd.Count; i++)
			{
				if (!Search(searchTermsAnd[i], e.Item.Package))
				{
					e.DoNotDraw = true;
					return;
				}
			}
		}
	}

	private bool Search(string searchTerm, IPackage item)
	{
		return searchTerm.SearchCheck(item.ToString())
			|| searchTerm.SearchCheck(item.GetWorkshopInfo()?.Author?.Name)
			|| (!item.IsLocal ? item.Id.ToString() : Path.GetFileName(item.LocalParentPackage?.Folder) ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1;
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
			tabHeader.Next();
			return true;
		}

		if (keyData == (Keys.Control | Keys.Shift | Keys.Tab))
		{

			tabHeader.Previous();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	internal void Import(string file)
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

		LC_Items.FilterChanged();
	}

	private async void B_ApplyAll_Click(object sender, EventArgs e)
	{
		if (!B_ApplyAll.Loading && recommendedActions is not null)
		{
			B_ApplyAll.Loading = true;
			await Task.Run(() => Parallelism.ForEach(recommendedActions, 4));
			LC_Items.FilterChanged();
			B_ApplyAll.Loading = false;
		}
	}
}
