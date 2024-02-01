using Skyve.App.UserInterface.CompatibilityReport;
using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Forms;
using Skyve.App.UserInterface.Lists;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_PackagePage : PanelContent
{
	private readonly ItemListControl? LC_Items;
	private readonly ContentList? LC_References;
	private TagControl? addTagControl;

	private readonly INotifier _notifier;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageUtil _packageUtil;
	private readonly ISettings _settings;

	public IPackageIdentity Package { get; }

	public PC_PackagePage(IPackageIdentity package, bool compatibilityPage = false)
	{
		ServiceCenter.Get(out _notifier, out _compatibilityManager, out _packageUtil, out _settings, out IImageService imageService);

		InitializeComponent();

		Package = package;

		PB_Icon.Package = package;

		if (package.GetWorkshopInfo() is IWorkshopInfo workshopInfo)
		{
			if (workshopInfo.GetThumbnail(imageService, out var thumbnail, out var thumbnailUrl))
			{
				PB_Icon.Image = new Bitmap(thumbnail);
			}
			else
			{
				PB_Icon.LoadImage(thumbnailUrl, imageService.GetImage);
			}
		}

		P_Info.SetPackage(package, this);

		T_CR.LinkedControl = new PackageCompatibilityReportControl(package);

		var tabs = slickTabControl1.Tabs.ToList();
		var crdata = Package.GetPackageInfo();
		var crAvailable = crdata is not null;

		if (!crAvailable)
		{
			TLP_Info.ColumnStyles[1].Width = 0;
		}

		if (Package is ILocalPackageData p && p.Assets is not null && p.Assets.Length > 0)
		{
			if (_settings.UserSettings.ComplexListUI)
			{
				LC_Items = new ItemListControl.Complex(SkyvePage.SinglePackage) { Dock = DockStyle.Fill, IsPackagePage = true };
			}
			else
			{
				LC_Items = new ItemListControl.Simple(SkyvePage.SinglePackage) { Dock = DockStyle.Fill, IsPackagePage = true };
			}

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
			foreach (var item in crdata?.Links ?? [])
			{
				FLP_Links.Controls.Add(new LinkControl(item, true));
			}

			label5.Visible = FLP_Links.Visible = FLP_Links.Controls.Count > 0;

			AddTags();
		}

		//if (GetItems().Any())
		//{
		//	LC_References = new ContentList(SkyvePage.SinglePackage, true, GetItems, SetIncluded, SetEnabled, GetItemText, GetCountText)
		//	{
		//		Dock = DockStyle.Fill
		//	};

		//	LC_References.TB_Search.Placeholder = "SearchGenericPackages";

		//	LC_References.RefreshItems();

		//	T_References.LinkedControl = LC_References;
		//}
		//else
		{
			tabs.Remove(T_References);
		}

		var requirements = package.GetWorkshopInfo()?.Requirements.ToList() ?? [];
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

		var pc = new OtherPlaysetPackage(package)
		{
			Dock = DockStyle.Fill
		};

		T_Profiles.FillTab = true;
		T_Profiles.LinkedControl = pc;

		slickTabControl1.Tabs = tabs.ToArray();

		_notifier.PackageInformationUpdated += CentralManager_PackageInformationUpdated;
	}

	protected async Task<IEnumerable<IPackage>> GetItems()
	{
		return await Task.FromResult(_packageUtil.GetPackagesThatReference(Package, _settings.UserSettings.ShowAllReferencedPackages));
	}

	protected async Task SetIncluded(IEnumerable<IPackageIdentity> filteredItems, bool included)
	{
		await ServiceCenter.Get<IPackageUtil>().SetIncluded(filteredItems, included);
	}

	protected async Task SetEnabled(IEnumerable<IPackageIdentity> filteredItems, bool enabled)
	{
		await ServiceCenter.Get<IPackageUtil>().SetEnabled(filteredItems, enabled);
	}

	protected LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}

	protected string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in LC_References!.Items)
		{
			if (item?.IsIncluded() == true)
			{
				packagesIncluded++;

				if (item.GetPackage()?.IsCodeMod == true)
				{
					modsIncluded++;

					if (item.IsEnabled())
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
		var frm = EditTags(new[] { Package });

		frm.FormClosed += (_, _) =>
		{
			if (frm.DialogResult == DialogResult.OK)
			{
				AddTags();
			}
		};
	}

	private static EditTagsForm EditTags(IEnumerable<IPackageIdentity> item)
	{
		var frm = new EditTagsForm(item);

		App.Program.MainForm.OnNextIdle(() =>
		{
			frm.Show(App.Program.MainForm);

			frm.ShowUp();
		});

		return frm;
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

		//if (Package.LocalPackage is not null)
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
		var cr = Package.GetPackageInfo();

		if (cr is null)
		{
			return;
		}

		label1.Text = LocaleCR.Usage;
		label2.Text = cr.Usage.GetValues().If(x => x.Count() == Enum.GetValues(typeof(PackageUsage)).Length, x => Locale.AnyUsage.One, x => x.ListStrings(x => LocaleCR.Get(x.ToString()), ", "));
		label3.Text = LocaleCR.PackageType;
		label4.Text = cr.Type == PackageType.GenericPackage ? (Package.GetPackage()?.IsCodeMod == true ? Locale.Mod : Locale.Asset) : LocaleCR.Get(cr.Type.ToString());
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
		panel1.BackColor = LC_Items is null ? design.AccentBackColor : design.BackColor.Tint(Lum: design.IsDarkTheme? 5: -5);
	}
}
