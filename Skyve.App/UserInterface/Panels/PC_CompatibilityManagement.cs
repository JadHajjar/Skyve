using Skyve.App.Interfaces;
using Skyve.App.UserInterface.CompatibilityReport;
using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Forms;
using Skyve.Systems.Compatibility;
using Skyve.Systems.Compatibility.Domain.Api;

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_CompatibilityManagement : PanelContent
{
	private readonly Dictionary<ulong, IPackage?> _packages = new();
	private int currentPage;
	private PostPackage? postPackage;
	private PostPackage? lastPackageData;
	private bool valuesChanged;
	private readonly ReviewRequest? _request;

	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IWorkshopService _workshopService;
	private readonly IUserService _userService;
	private readonly ITagsService _tagsService;

	public IPackage? CurrentPackage { get; private set; }

	private PC_CompatibilityManagement(bool load) : base(load)
	{
		ServiceCenter.Get(out _workshopService, out _compatibilityManager, out _userService, out _tagsService);

		InitializeComponent();

		SlickTip.SetTo(B_Skip, "Skip");
		SlickTip.SetTo(B_Previous, "Previous");
		SlickTip.SetTo(P_Tags, "GlobalTagsInfo");
		SlickTip.SetTo(P_Tags, "GlobalTagsInfo");
		SlickTip.SetTo(B_ReuseData, "ReuseData_Tip");
		TB_Search.Placeholder = $"{LocaleSlickUI.Search}..";
		T_Statuses.Text = LocaleCR.StatusesCount.Format(0);
		T_Interactions.Text = LocaleCR.InteractionCount.Format(0);

		packageCrList.CanDrawItem += PackageCrList_CanDrawItem;

		DD_Stability.Enabled = _userService.User.Manager;
		TB_Note.Enabled = _userService.User.Manager;
	}

	public PC_CompatibilityManagement() : this(true)
	{
		PB_Loading.BringToFront();
	}

	public PC_CompatibilityManagement(IEnumerable<ulong> packages) : this(false)
	{
		foreach (var package in packages)
		{
			_packages[package] = _workshopService.GetPackage(new GenericPackageIdentity(package));
		}

		packageCrList.SetItems(_packages.Keys);

		SetPackage(0);

		if (_packages.Count == 1)
		{
			I_List.Visible = false;
		}
	}

	public PC_CompatibilityManagement(ReviewRequest request) : this(new[] { request.PackageId })
	{
		_request = request;
	}

	private List<ulong> PackageList => packageCrList.SafeGetItems().ToList(x => x.Item);

	protected override void UIChanged()
	{
		base.UIChanged();

		TLP_Main.Padding = UI.Scale(new Padding(5, 0, 5, 5), UI.FontScale);
		TLP_List.Padding = UI.Scale(new Padding(5), UI.FontScale);
		I_List.Margin = B_Skip.Margin = B_Previous.Margin = B_Apply.Margin = L_Page.Margin = B_ReuseData.Margin = UI.Scale(new Padding(1, 5, 1, 0), UI.FontScale);
		P_Main.Padding = UI.Scale(new Padding(7), UI.FontScale);
		PB_Icon.Width = TLP_Top.Height = (int)(128 * UI.FontScale);
		TLP_List.Width = (int)(210 * UI.FontScale);
		I_List.Padding = UI.Scale(new Padding(4), UI.FontScale);
		I_List.Margin = UI.Scale(new Padding(1, 5, 6, 0), UI.FontScale);
		foreach (Control control in TLP_MainInfo.Controls)
		{
			control.Margin = UI.Scale(new Padding(5), UI.FontScale);
		}

		B_AddInteraction.Padding = B_AddStatus.Padding = UI.Scale(new Padding(15), UI.FontScale);
		B_AddInteraction.Font = B_AddStatus.Font = UI.Font(9.75F);
		B_AddInteraction.Margin = B_AddStatus.Margin = UI.Scale(new Padding(50, 40, 0, 0), UI.UIScale);

		TB_Note.MinimumSize = UI.Scale(new Size(0, 100), UI.UIScale);
		P_Tags.MinimumSize = P_Links.MinimumSize = UI.Scale(new Size(300, 00), UI.UIScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		P_Main.BackColor = design.AccentBackColor;
		P_Tags.BackColor = P_Links.BackColor = design.BackColor;
	}

	public override bool CanExit(bool toBeDisposed)
	{
		var canExit = !toBeDisposed
			|| currentPage <= 0
			|| currentPage >= _packages.Count - 1
			|| ShowPrompt(LocaleCR.ConfirmEndSession, PromptButtons.YesNo, PromptIcons.Question) == DialogResult.Yes;

		if (toBeDisposed && canExit)
		{
			RefreshData();
		}

		return canExit;
	}

	private async void RefreshData()
	{
		await Task.Run(() =>
		{
			_compatibilityManager.DownloadData();
			_compatibilityManager.CacheReport();
		});
	}

	protected override async Task<bool> LoadDataAsync()
	{
		PB_Loading.Loading = true;

		var mods = _userService.User.Manager ?
			await _workshopService.QueryFilesAsync(PackageSorting.UpdateTime, requiredTags: new[] { "Mod" }, all: true) :
			await _workshopService.GetWorkshopItemsByUserAsync(_userService.User.Id ?? 0);

		foreach (var mod in mods)
		{
			_packages.Add(mod.Id, _workshopService.GetPackage(mod));
		}

		packageCrList.SetItems(_packages.Keys);


		return true;
	}

	protected override void OnDataLoad()
	{
		CB_ShowUpToDate.Checked = false;
		if (PackageList.Count == 0)
		{
			CB_ShowUpToDate.Checked = true;
		}

		SetPackage(0);
		PB_Loading.Dispose();
	}

	protected override void OnLoadFail()
	{
		ShowPrompt(LocaleCR.CrDataLoadFailed, PromptButtons.OK, PromptIcons.Error);
		PushBack();
	}

	private async void SetPackage(int page)
	{
		if (valuesChanged)
		{
			switch (ShowPrompt(LocaleCR.ApplyChangedBeforeExit, PromptButtons.YesNoCancel, PromptIcons.Hand))
			{
				case DialogResult.Yes:
					if (!await Apply())
					{
						return;
					}

					break;
				case DialogResult.Cancel:
					return;
			}
		}

		var packages = PackageList;

		if (packages.Count == 0)
		{
			L_Page.Text = $"0 / 0";
			return;
		}

		if (page < 0 || page >= packages.Count)
		{
			PushBack();
			return;
		}

		L_Page.Text = $"{page + 1} / {packages.Count}";

		PB_LoadingPackage.BringToFront();
		PB_LoadingPackage.Loading = true;

		currentPage = page;
		CurrentPackage = _packages[packages[page]];

		try
		{
			CurrentPackage ??= await _workshopService.GetPackageAsync(new GenericPackageIdentity(packages[page]));

			if (!_userService.User.Manager && !_userService.User.Equals(CurrentPackage.GetWorkshopInfo()?.Author))
			{
				packageCrList.Remove(CurrentPackage.Id);
				SetPackage(page);
				return;
			}

			var catalogue = await ServiceCenter.Get<SkyveApiUtil>().Catalogue(CurrentPackage!.Id);

			postPackage = catalogue?.Packages.FirstOrDefault()?.CloneTo<CompatibilityPackageData, PostPackage>();

			if (postPackage is null)
			{
				postPackage = (_compatibilityManager as CompatibilityManager)!.GetAutomatedReport(CurrentPackage).CloneTo<CompatibilityPackageData, PostPackage>();
			}
			else
			{
				var automatedPackage = (_compatibilityManager as CompatibilityManager)!.GetAutomatedReport(CurrentPackage).CloneTo<CompatibilityPackageData, PostPackage>();

				if (automatedPackage.Stability is PackageStability.Broken)
				{
					postPackage.Stability = PackageStability.Broken;
				}

				foreach (var item in automatedPackage.Statuses ?? new())
				{
					if (!postPackage.Statuses.Any(x => x.Type == item.Type))
					{
						postPackage.Statuses!.Add(item);
					}
				}
			}

			postPackage.BlackListId = catalogue?.BlackListedIds?.Contains(postPackage.SteamId) ?? false;
			postPackage.BlackListName = catalogue?.BlackListedNames?.Contains(postPackage.Name ?? string.Empty) ?? false;

			SetData(postPackage);

			PB_Icon.Package = CurrentPackage;
			PB_Icon.Image = null;
			PB_Icon.LoadImage(CurrentPackage.GetWorkshopInfo()?.ThumbnailUrl, ServiceCenter.Get<IImageService>().GetImage);
			P_Info.SetPackage(CurrentPackage, null);

			B_Previous.Enabled = currentPage > 0;
			B_Skip.Enabled = currentPage != _packages.Count - 1;

			PB_LoadingPackage.SendToBack();
			PB_LoadingPackage.Loading = false;

			packageCrList.Invalidate();
			valuesChanged = false;
		}
		catch { OnLoadFail(); }
	}

	private void SetData(PostPackage postPackage)
	{
		if (!IsHandleCreated)
		{
			CreateHandle();
		}

		CB_BlackListName.Checked = postPackage.BlackListName;
		CB_BlackListId.Checked = postPackage.BlackListId;

		if (_request is not null && !_request.IsStatus && !_request.IsInteraction)
		{
			DD_Stability.SelectedItem = (PackageStability)_request.PackageStability;
			DD_PackageType.SelectedItem = (PackageType)_request.PackageType;
			DD_DLCs.SelectedItems = ServiceCenter.Get<IDlcManager>().Dlcs.Where(x => _request.RequiredDLCs?.Contains(x.Id.ToString()) ?? false);
			DD_Usage.SelectedItems = Enum.GetValues(typeof(PackageUsage)).Cast<PackageUsage>().Where(x => ((PackageUsage)_request.PackageUsage).HasFlag(x));
		}
		else
		{
			DD_Stability.SelectedItem = postPackage.Stability;
			DD_PackageType.SelectedItem = postPackage.Type;
			DD_DLCs.SelectedItems = ServiceCenter.Get<IDlcManager>().Dlcs.Where(x => postPackage.RequiredDLCs?.Contains(x.Id) ?? false);
			DD_Usage.SelectedItems = Enum.GetValues(typeof(PackageUsage)).Cast<PackageUsage>().Where(x => postPackage.Usage.HasFlag(x));
		}

		TB_Note.Text = postPackage.Note;

		P_Tags.Controls.Clear(true, x => !string.IsNullOrEmpty(x.Text));
		FLP_Statuses.Controls.Clear(true, x => x is IPackageStatusControl<StatusType, PackageStatus>);
		FLP_Interactions.Controls.Clear(true, x => x is IPackageStatusControl<InteractionType, PackageInteraction>);

		foreach (var item in postPackage.Tags ?? new())
		{
			var control = new TagControl { TagInfo = _tagsService.CreateCustomTag(item) };
			control.Click += TagControl_Click;
			P_Tags.Controls.Add(control);
			T_NewTag.SendToBack();
		}

		SetLinks(postPackage.Links ?? new());

		postPackage.Statuses ??= new();
		postPackage.Interactions ??= new();

		if (_request?.IsInteraction ?? false)
		{
			postPackage.Interactions.Add(new()
			{
				Action = (StatusAction)_request.StatusAction,
				IntType = _request.StatusType,
				Note = _request.StatusNote,
				Packages = _request.StatusPackages?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray(),
			});
		}

		if (_request?.IsStatus ?? false)
		{
			postPackage.Statuses.Add(new()
			{
				Action = (StatusAction)_request.StatusAction,
				IntType = _request.StatusType,
				Note = _request.StatusNote,
				Packages = _request.StatusPackages?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray(),
			});
		}

		foreach (var item in postPackage.Statuses)
		{
			var control = new IPackageStatusControl<StatusType, PackageStatus>(CurrentPackage, item, !_userService.User.Manager)
			{
				Enabled = _userService.User.Manager || CRNAttribute.GetAttribute(item.Type).AllowedChange == CRNAttribute.ChangeType.Allow
			};

			control.ValuesChanged += ControlValueChanged;

			FLP_Statuses.Controls.Add(control);
			B_AddStatus.SendToBack();
		}

		foreach (var item in postPackage.Interactions)
		{
			var control = new IPackageStatusControl<InteractionType, PackageInteraction>(CurrentPackage, item, !_userService.User.Manager)
			{
				Enabled = _userService.User.Manager || CRNAttribute.GetAttribute(item.Type).AllowedChange == CRNAttribute.ChangeType.Allow
			};

			control.ValuesChanged += ControlValueChanged;

			FLP_Interactions.Controls.Add(control);
			B_AddInteraction.SendToBack();
		}
	}

	private void B_Skip_Click(object sender, EventArgs e)
	{
		SetPackage(ModifierKeys.HasFlag(Keys.Shift) ? (_packages.Count - 1) : (currentPage + 1));
	}

	private void B_Previous_Click(object sender, EventArgs e)
	{
		SetPackage(ModifierKeys.HasFlag(Keys.Shift) ? 0 : (currentPage - 1));
	}

	private void T_NewTag_Click(object sender, EventArgs e)
	{
		var prompt = ShowInputPrompt(LocaleCR.AddGlobalTag);

		if (prompt.DialogResult != DialogResult.OK)
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(prompt.Input) || P_Tags.Controls.Any(x => x.Text.Equals(prompt.Input, StringComparison.CurrentCultureIgnoreCase)))
		{
			return;
		}

		var control = new TagControl { TagInfo = _tagsService.CreateCustomTag(prompt.Input) };
		control.Click += TagControl_Click;
		P_Tags.Controls.Add(control);
		T_NewTag.SendToBack();
		ControlValueChanged(sender, e);
	}

	private void TagControl_Click(object sender, EventArgs e)
	{
		(sender as Control)?.Dispose();

		ControlValueChanged(sender, e);
	}

	private void T_NewLink_Click(object sender, EventArgs e)
	{
		var form = new AddLinkForm(P_Links.Controls.OfType<LinkControl>().ToList(x => x.Link));

		form.Show(Form);

		form.LinksReturned += SetLinks;
	}

	private void SetLinks(IEnumerable<PackageLink> links)
	{
		P_Links.Controls.Clear(true, x => x is LinkControl);

		foreach (var item in links.OrderBy(x => x.Type))
		{
			var control = new LinkControl(item, false);
			control.Click += T_NewLink_Click;
			P_Links.Controls.Add(control);
		}

		T_NewLink.SendToBack();

		ControlValueChanged(this, EventArgs.Empty);
	}

	private void B_AddStatus_Click(object sender, EventArgs e)
	{
		var control = new IPackageStatusControl<StatusType, PackageStatus>(CurrentPackage, default, !_userService.User.Manager);

		control.ValuesChanged += ControlValueChanged;

		FLP_Statuses.Controls.Add(control);
		B_AddStatus.SendToBack();

		ControlValueChanged(sender, e);
	}

	private void B_AddInteraction_Click(object sender, EventArgs e)
	{
		var control = new IPackageStatusControl<InteractionType, PackageInteraction>(CurrentPackage, default, !_userService.User.Manager);

		control.ValuesChanged += ControlValueChanged;

		FLP_Interactions.Controls.Add(control);
		B_AddInteraction.SendToBack();

		ControlValueChanged(sender, e);
	}

	private async void B_Apply_Click(object sender, EventArgs e)
	{
		if (await Apply())
		{
			SetPackage(currentPage + 1);
		}
	}

	private async Task<bool> Apply()
	{
		if (B_Apply.Loading)
		{
			return false;
		}

		if (DD_Usage.SelectedItems.Count() == 0)
		{
			ShowPrompt(LocaleCR.PleaseReviewPackageUsage, PromptButtons.OK, PromptIcons.Hand);
			return false;
		}

		postPackage!.SteamId = CurrentPackage!.Id;
		postPackage.FileName = Path.GetFileName(CurrentPackage.LocalParentPackage?.Mod?.FilePath ?? string.Empty).IfEmpty(postPackage.FileName);
		postPackage.Name = CurrentPackage.Name;
		postPackage.ReviewDate = DateTime.UtcNow;
		postPackage.AuthorId = (ulong)(CurrentPackage.GetWorkshopInfo()?.Author?.Id ?? 0);
		postPackage.Author = new Author
		{
			SteamId = postPackage.AuthorId,
			Name = CurrentPackage.GetWorkshopInfo()?.Author?.Name,
		};

		postPackage.BlackListId = CB_BlackListId.Checked;
		postPackage.BlackListName = CB_BlackListName.Checked;
		postPackage.Stability = DD_Stability.SelectedItem;
		postPackage.Type = DD_PackageType.SelectedItem;
		postPackage.Usage = DD_Usage.SelectedItems.Aggregate((prev, next) => prev | next);
		postPackage.RequiredDLCs = DD_DLCs.SelectedItems.Select(x => x.Id).ToArray();
		postPackage.Note = TB_Note.Text;
		postPackage.Tags = P_Tags.Controls.OfType<TagControl>().Where(x => !string.IsNullOrEmpty(x.TagInfo?.Value)).ToList(x => x.TagInfo!.Value);
		postPackage.Links = P_Links.Controls.OfType<LinkControl>().ToList(x => (PackageLink)x.Link);
		postPackage.Statuses = FLP_Statuses.Controls.OfType<IPackageStatusControl<StatusType, PackageStatus>>().ToList(x => x.PackageStatus);
		postPackage.Interactions = FLP_Interactions.Controls.OfType<IPackageStatusControl<InteractionType, PackageInteraction>>().ToList(x => x.PackageStatus);

		if (!CRNAttribute.GetAttribute(postPackage.Stability).Browsable)
		{
			ShowPrompt(LocaleCR.PleaseReviewTheStability, PromptButtons.OK, PromptIcons.Hand);
			return false;
		}

		if (postPackage.Statuses.Any(x => !CRNAttribute.GetAttribute(x.Type).Browsable))
		{
			ShowPrompt(LocaleCR.PleaseReviewPackageStatuses, PromptButtons.OK, PromptIcons.Hand);
			return false;
		}

		if (postPackage.Interactions.Any(x => !CRNAttribute.GetAttribute(x.Type).Browsable || !(x.Packages?.Any() ?? false)))
		{
			ShowPrompt(LocaleCR.PleaseReviewPackageInteractions, PromptButtons.OK, PromptIcons.Hand);
			return false;
		}

		B_Apply.Loading = true;

		var response = await ServiceCenter.Get<SkyveApiUtil>().SaveEntry(postPackage);

		B_Apply.Loading = false;

		if (!response.Success)
		{
			ShowPrompt(response.Message, PromptButtons.OK, PromptIcons.Error);
			return false;
		}

		lastPackageData = postPackage;
		B_ReuseData.Visible = true;

		valuesChanged = false;

		return true;
	}

	private void FLP_Statuses_ControlAdded(object sender, ControlEventArgs e)
	{
		T_Statuses.Text = LocaleCR.StatusesCount.Format(FLP_Statuses.Controls.Count - 1);
		T_Interactions.Text = LocaleCR.InteractionCount.Format(FLP_Interactions.Controls.Count - 1);
	}

	private void B_ReuseData_Click(object sender, EventArgs e)
	{
		if (lastPackageData is not null)
		{
			SetData(lastPackageData);
		}

		if (ModifierKeys.HasFlag(Keys.Shift))
		{
			B_Apply_Click(sender, e);
		}
	}

	private void I_List_Click(object sender, EventArgs e)
	{
		CB_ShowUpToDate.Visible = TLP_List.Visible = I_List.Selected = !I_List.Selected;
	}

	private void I_List_SizeChanged(object sender, EventArgs e)
	{
		I_List.Width = I_List.Height;
	}

	private void packageCrList1_ItemMouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			SetPackage(PackageList.IndexOf((ulong)sender));
		}

		if (e.Button == MouseButtons.Right)
		{
			SlickToolStrip.Show(Form, packageCrList.PointToClient(e.Location), ServiceCenter.Get<ICustomPackageService>().GetRightClickMenuItems(_workshopService.GetPackage(new GenericPackageIdentity((ulong)sender))!));
		}
	}

	private void TB_Search_TextChanged(object sender, EventArgs e)
	{
		TB_Search.ImageName = string.IsNullOrWhiteSpace(TB_Search.Text) ? "I_Search" : "I_ClearSearch";

		packageCrList.FilterChanged();

		if (sender == CB_ShowUpToDate)
		{
			SetPackage(0);
		}
	}

	private void PackageCrList_CanDrawItem(object sender, CanDrawItemEventArgs<ulong> e)
	{
		var package = _workshopService.GetPackage(new GenericPackageIdentity(e.Item));

		if (package is null)
		{
			return;
		}

		if (!CB_ShowUpToDate.Checked)
		{
			var cr = _compatibilityManager.GetPackageInfo(package);

			if (cr is null || cr.ReviewDate > package.GetWorkshopInfo()?.ServerTime)
			{
				e.DoNotDraw = true;
				return;
			}
		}

		e.DoNotDraw = !(TB_Search.Text.SearchCheck(package.ToString())
			|| TB_Search.Text.SearchCheck(package.GetWorkshopInfo()?.Author?.Name)
			|| package.Id.ToString().IndexOf(TB_Search.Text, StringComparison.OrdinalIgnoreCase) != -1);
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private void ControlValueChanged(object sender, EventArgs e)
	{
		valuesChanged = true;
	}
}
