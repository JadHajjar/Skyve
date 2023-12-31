﻿using Skyve.App.UserInterface.CompatibilityReport;
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

	private static EditTagsForm EditTags(IEnumerable<ILocalPackage> item)
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
}
