using Skyve.App.UserInterface.CompatibilityReport;
using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Forms;
using Skyve.App.UserInterface.Lists;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_PackagePage : PanelContent
{
	private readonly ItemListControl<IPackage>? LC_Items;
	private readonly ContentList<IPackage>? LC_References;
	private TagControl? addTagControl;

	private readonly INotifier _notifier;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageUtil _packageUtil;
	private readonly ISettings _settings;

	public IPackage Package { get; }

	public PC_PackagePage(IPackage package, bool compatibilityPage = false)
	{
		if (package is not ILocalPackage && package.LocalPackage is ILocalPackage localPackage)
		{
			package = localPackage;
		}

		ServiceCenter.Get(out _notifier, out _compatibilityManager, out _packageUtil, out _settings);

		InitializeComponent();

		Package = package;

		PB_Icon.Package = package;
		PB_Icon.LoadImage(package.GetWorkshopInfo()?.ThumbnailUrl, ServiceCenter.Get<IImageService>().GetImage);

		P_Info.SetPackage(package, this);

		T_CR.LinkedControl = new PackageCompatibilityReportControl(package);

		var tabs = slickTabControl1.Tabs.ToList();
		var crdata = _compatibilityManager.GetPackageInfo(Package);
		var crAvailable = crdata is not null;

		if (!crAvailable)
		{
			TLP_Info.ColumnStyles[1].Width = 0;
		}

		if (Package is ILocalPackageWithContents p && p.Assets is not null && p.Assets.Length > 0)
		{
			LC_Items = new ItemListControl<IPackage>(SkyvePage.SinglePackage)
			{
				IsPackagePage = true,
				Dock = DockStyle.Fill
			};

			LC_Items.AddRange(p.Assets);

			P_List.Controls.Add(LC_Items);
		}
		else if (crAvailable)
		{
			TLP_Info.ColumnStyles[0].Width = 0;
		}
		else
		{
			tabs.Remove(T_Info);
			T_CR.PreSelected = true;
		}

		if (compatibilityPage)
		{
			T_CR.PreSelected = true;
		}

		if (crAvailable)
		{
			foreach (var item in crdata?.Links ?? new())
			{
				FLP_Links.Controls.Add(new LinkControl(item, true));
			}

			label5.Visible = FLP_Links.Visible = FLP_Links.Controls.Count > 0;

			AddTags();
		}

		if (GetItems().Any())
		{
			LC_References = new ContentList<IPackage>(SkyvePage.SinglePackage, true, GetItems, SetIncluded, SetEnabled, GetItemText, GetCountText)
			{
				Dock = DockStyle.Fill
			};

			LC_References.TB_Search.Placeholder = "SearchGenericPackages";

			LC_References.RefreshItems();

			T_References.LinkedControl = LC_References;
		}
		else
		{
			tabs.Remove(T_References);
		}

		var requirements = package.Requirements.ToList();
		if (requirements.Count > 0)
		{
			foreach (var requirement in requirements)
			{
				var control = new MiniPackageControl(requirement.Id) { ReadOnly = true, Large = true };
				FLP_Requirements.Controls.Add(control);
				FLP_Requirements.SetFlowBreak(control, true);
			}
		}
		else
		{
			L_Requirements.Visible = false;
		}

		var pc = new OtherProfilePackage(package)
		{
			Dock = DockStyle.Fill
		};

		T_Profiles.FillTab = true;
		T_Profiles.LinkedControl = pc;

		slickTabControl1.Tabs = tabs.ToArray();

		_notifier.PackageInformationUpdated += CentralManager_PackageInformationUpdated;
	}

	protected IEnumerable<IPackage> GetItems()
	{
		return _packageUtil.GetPackagesThatReference(Package, _settings.UserSettings.ShowAllReferencedPackages);
	}

	protected void SetIncluded(IEnumerable<IPackage> filteredItems, bool included)
	{
		ServiceCenter.Get<IBulkUtil>().SetBulkIncluded(filteredItems.SelectWhereNotNull(x => x.LocalPackage)!, included);
	}

	protected void SetEnabled(IEnumerable<IPackage> filteredItems, bool enabled)
	{
		ServiceCenter.Get<IBulkUtil>().SetBulkEnabled(filteredItems.SelectWhereNotNull(x => x.LocalPackage)!, enabled);
	}

	protected LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}

	protected string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in LC_References!.Items.SelectWhereNotNull(x => x.LocalParentPackage))
		{
			if (item?.IsIncluded() == true)
			{
				packagesIncluded++;

				if (item.Mod is not null)
				{
					modsIncluded++;

					if (item.Mod.IsEnabled())
					{
						modsEnabled++;
					}
				}
			}
		}

		var total = LC_References!.ItemCount;

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			return string.Format(Locale.PackageIncludedTotal, packagesIncluded, total);
		}

		if (modsIncluded == modsEnabled)
		{
			return string.Format(Locale.PackageIncludedAndEnabledTotal, packagesIncluded, total);
		}

		return string.Format(Locale.PackageIncludedEnabledTotal, packagesIncluded, modsIncluded, modsEnabled, total);
	}

	private void AddTagControl_MouseClick(object sender, MouseEventArgs e)
	{
		if (Package.LocalPackage is null)
		{
			return;
		}

		var frm = EditTags(new[] { Package.LocalPackage });

		frm.FormClosed += (_, _) =>
		{
			if (frm.DialogResult == DialogResult.OK)
			{
				AddTags();
			}
		};
	}

	private void AddTags()
	{
		FLP_Tags.Controls.Clear(true);

		foreach (var item in Package.GetTags())
		{
			var control = new TagControl { TagInfo = item, Display = true };
			control.MouseClick += TagControl_Click;
			FLP_Tags.Controls.Add(control);
		}

		if (Package.LocalPackage is not null)
		{
			addTagControl = new TagControl { ImageName = "I_Add" };
			addTagControl.MouseClick += AddTagControl_MouseClick;
			FLP_Tags.Controls.Add(addTagControl);
		}
	}

	private void TagControl_Click(object sender, EventArgs e)
	{
		if (!(sender as TagControl)!.TagInfo!.IsCustom)
		{
			return;
		}

		(sender as TagControl)!.Dispose();

		ServiceCenter.Get<ITagsService>().SetTags(Package, FLP_Tags.Controls.OfType<TagControl>().Select(x => x.TagInfo!.IsCustom ? x.TagInfo.Value?.Replace(' ', '-') : null)!);
		Program.MainForm?.TryInvoke(() => Program.MainForm.Invalidate(true));
	}

	private void CentralManager_PackageInformationUpdated()
	{
		P_Info.Invalidate();
		LC_Items?.Invalidate();
	}

	protected override void LocaleChanged()
	{
		var cr = _compatibilityManager.GetPackageInfo(Package);

		if (cr is null)
		{
			return;
		}

		label1.Text = LocaleCR.Usage;
		label2.Text = cr.Usage.GetValues().If(x => x.Count() == Enum.GetValues(typeof(PackageUsage)).Length, x => Locale.AnyUsage.One, x => x.ListStrings(x => LocaleCR.Get(x.ToString()), ", "));
		label3.Text = LocaleCR.PackageType;
		label4.Text = cr.Type == PackageType.GenericPackage ? (Package.IsMod ? Locale.Mod : Locale.Asset) : LocaleCR.Get(cr.Type.ToString());
		label5.Text = LocaleCR.Links;
		label6.Text = LocaleSlickUI.Tags;
		L_Requirements.Text = LocaleHelper.GetGlobalText("CRT_RequiredPackages");
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		PB_Icon.Width = TLP_Top.Height = (int)(128 * UI.FontScale);
		TLP_About.Padding = UI.Scale(new Padding(5), UI.FontScale);
		label1.Margin = label3.Margin = label5.Margin = label6.Margin = L_Requirements.Margin = UI.Scale(new Padding(3, 4, 0, 0), UI.FontScale);
		label2.Margin = label4.Margin = FLP_Links.Margin = FLP_Tags.Margin = FLP_Requirements.Margin = UI.Scale(new Padding(3, 3, 0, 7), UI.FontScale);
		label1.Font = label3.Font = label5.Font = label6.Font = L_Requirements.Font = UI.Font(7.5F, FontStyle.Bold);
		FLP_Requirements.Font = UI.Font(9F);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = design.BackColor;
		label1.ForeColor = label3.ForeColor = label5.ForeColor = label6.ForeColor = L_Requirements.ForeColor = design.InfoColor.MergeColor(design.ActiveColor);
		panel1.BackColor = LC_Items is null ? design.AccentBackColor : design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, 5, -5));
	}

	internal static SlickStripItem[] GetRightClickMenuItems<T>(T item) where T : IPackage
	{
		return GetRightClickMenuItems(new[] { item });
	}

	internal static SlickStripItem[] GetRightClickMenuItems<T>(IEnumerable<T> items) where T : IPackage
	{
		var list = items.ToList();
		var isInstalled = list.Any(item => item.LocalParentPackage is not null);
		var isLocal = isInstalled && list.Any(item => item.IsLocal);

		var bulkUtil = ServiceCenter.Get<IBulkUtil>();
		var packageUtil = ServiceCenter.Get<IPackageUtil>();
		var packageManager = ServiceCenter.Get<IPackageManager>();
		var subscriptionManager = ServiceCenter.Get<ISubscriptionsManager>();
		var profileManager = ServiceCenter.Get<IPlaysetManager>();
		var compatibilityManager = ServiceCenter.Get<ICompatibilityManager>();
		var userService = ServiceCenter.Get<IUserService>();

		return new SlickStripItem[]
		{
			  new (Locale.IncludeAllItemsInThisPackage.FormatPlural(list.Count), "I_Ok", isInstalled && list.Any(item => !item.LocalPackage!.IsIncluded()), action: () => { bulkUtil.SetBulkIncluded(list.SelectWhereNotNull(x => list.Count == 1 ? x.LocalParentPackage : x.LocalPackage)!, true); })
			, new (Locale.ExcludeAllItemsInThisPackage.FormatPlural(list.Count), "I_Cancel", isInstalled && list.Any(item => item.LocalPackage!.IsIncluded()), action: () => { bulkUtil.SetBulkIncluded(list.SelectWhereNotNull(x => list.Count == 1 ? x.LocalParentPackage : x.LocalPackage)!, false); })
			, new ((isInstalled ? Locale.ReDownloadPackage : Locale.DownloadPackage).FormatPlural(list.Count), "I_Install", SteamUtil.IsSteamAvailable(), action: () => Redownload(list))
			, new (Locale.MovePackageToLocalFolder.FormatPlural(list.Count), "I_PC", isInstalled && !isLocal, action: () => list.SelectWhereNotNull(x => x.LocalParentPackage).Foreach(x => packageManager.MoveToLocalFolder(x!)))
			, new (string.Empty)
			, new ((isLocal && list[0] is IAsset ? Locale.DeleteAsset : Locale.DeletePackage).FormatPlural(list.Count), "I_Disposable", isInstalled, action: () => AskThenDelete(list))
			, new (Locale.UnsubscribePackage.FormatPlural(list.Count), "I_Steam", isInstalled && !isLocal, action: () => subscriptionManager.UnSubscribe(list.Cast<IPackageIdentity>()))
			, new (Locale.SubscribeToItem.FormatPlural(list.Count), "I_Steam", !isInstalled && !isLocal, action: () => subscriptionManager.Subscribe(list.Cast<IPackageIdentity>()))
			, new (string.Empty)
			, new (Locale.EditTagsOfPackage.FormatPlural(list.Count), "I_Tag", isInstalled, action: () => EditTags(list.SelectWhereNotNull(x => x.LocalParentPackage).SelectMany(x => x!.Assets)))
			, new (Locale.EditTags.FormatPlural(list.Count), "I_Tag", isInstalled, action: () => EditTags(list.SelectWhereNotNull(x => x.LocalPackage)!))
			, new (Locale.EditCompatibility.FormatPlural(list.Count), "I_CompatibilityReport", userService.User.Manager || list.Any(item => userService.User.Equals(item.GetWorkshopInfo()?.Author)), action: () => { Program.MainForm.PushPanel(null, new PC_CompatibilityManagement(items.Select(x => x.Id)));})
			, new (string.Empty)
			, new (Locale.OtherPlaysets, "I_ProfileSettings", fade: true)
			, new (Locale.IncludeThisItemInAllPlaysets.FormatPlural(list.Count), "I_Ok", tab: 1, action: () => { new BackgroundAction(() => list.SelectWhereNotNull(x => x.LocalPackage).Foreach(x => profileManager.SetIncludedForAll(x!, true))).Run(); bulkUtil.SetBulkIncluded(list.SelectWhereNotNull(x => x.LocalPackage)!, true); })
			, new (Locale.ExcludeThisItemInAllPlaysets.FormatPlural(list.Count), "I_Cancel", tab: 1, action: () => { new BackgroundAction(() => list.SelectWhereNotNull(x => x.LocalPackage).Foreach(x => profileManager.SetIncludedForAll(x!, false))).Run(); bulkUtil.SetBulkIncluded(list.SelectWhereNotNull(x => x.LocalPackage)!, false);})
			, new (Locale.Copy, "I_Copy", !isLocal, fade: true)
			, new (Locale.CopyPackageName.FormatPlural(list.Count), !isLocal ? null : "I_Copy", tab: !isLocal ? 1 : 0, action: () => Clipboard.SetText(list.ListStrings(CrossIO.NewLine)))
			, new (Locale.CopyWorkshopLink.FormatPlural(list.Count), null, !isLocal, tab: 1, action: () => Clipboard.SetText(list.ListStrings(x => x.Url, CrossIO.NewLine)))
			, new (Locale.CopyWorkshopId.FormatPlural(list.Count), null, !isLocal, tab: 1,  action: () => Clipboard.SetText(list.ListStrings(x => x.Id.ToString(), CrossIO.NewLine)))
			, new (string.Empty, show: !isLocal, tab: 1)
			, new (Locale.CopyAuthorName.FormatPlural(list.Count), null, !isLocal, tab: 1, action: () => Clipboard.SetText(list.ListStrings(x => x.GetWorkshopInfo()?.Author?.Name , CrossIO.NewLine)))
			, new (Locale.CopyAuthorLink.FormatPlural(list.Count), null, !isLocal, tab: 1, action: () => Clipboard.SetText(list.ListStrings(x => x.GetWorkshopInfo()?.Author?.ProfileUrl , CrossIO.NewLine)))
			, new (Locale.CopyAuthorSteamId.FormatPlural(list.Count), null, !isLocal, tab: 1,  action: () => Clipboard.SetText(list.ListStrings(x => x.GetWorkshopInfo()?.Author?.Id?.ToString() , CrossIO.NewLine)))
		};
	}

	private static EditTagsForm EditTags(IEnumerable<ILocalPackage> item)
	{
		var frm = new EditTagsForm(item);

		Program.MainForm.OnNextIdle(() =>
		{
			frm.Show(Program.MainForm);

			frm.ShowUp();
		});

		return frm;
	}

	private static void AskThenDelete<T>(IEnumerable<T> items) where T : IPackage
	{
		if (MessagePrompt.Show(Locale.AreYouSure + "\r\n\r\n" + Locale.ActionUnreversible.FormatPlural(items.Count()), PromptButtons.YesNo, form: Program.MainForm) == DialogResult.Yes)
		{
			foreach (var item in items.Where(x => !x.IsBuiltIn).SelectWhereNotNull(x => x.LocalPackage))
			{
				try
				{
					if (item!.IsLocal && item is IAsset asset)
					{
						CrossIO.DeleteFile(asset.FilePath);
					}
					else if (item.LocalParentPackage is not null)
					{
						ServiceCenter.Get<IPackageManager>().DeleteAll(item.LocalParentPackage.Folder);
					}
				}
				catch (Exception ex) { MessagePrompt.Show(ex, Locale.FailedToDeleteItem); }
			}
		}
	}

	private static void Redownload<T>(IEnumerable<T> item) where T : IPackageIdentity
	{
		ServiceCenter.Get<IDownloadService>().Download(item.Cast<IPackageIdentity>());
	}
}
