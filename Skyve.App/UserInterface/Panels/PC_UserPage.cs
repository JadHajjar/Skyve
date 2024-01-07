using Skyve.App.UserInterface.Lists;

using System.Drawing;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_UserPage : PanelContent
{
	private readonly ContentList<IPackage> LC_Items;
	private readonly PlaysetListControl L_Profiles;

	private readonly ISettings _settings;
	private readonly IWorkshopService _workshopService;

	private List<IPackage> userItems = new();

	public IUser User { get; }

	public PC_UserPage(IUser user) : base(true)
	{
		ServiceCenter.Get(out _settings, out _workshopService);

		InitializeComponent();

		User = user;

		PB_Icon.LoadImage(User.AvatarUrl, ServiceCenter.Get<IImageService>().GetImage);
		P_Info.SetUser(User, this);

		L_Profiles = new(true)
		{
			GridView = true,
		};

		LC_Items = new ContentList<IPackage>(SkyvePage.User, false, GetItems, SetIncluded, SetEnabled, GetItemText, GetCountText)
		{
			IsGenericPage = true
		};

		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";

		LC_Items.ListControl.Loading = true;
	}

	protected IEnumerable<IPackage> GetItems()
	{
		return userItems;
	}

	protected async Task SetIncluded(IEnumerable<IPackage> filteredItems, bool included)
	{
		await ServiceCenter.Get<IPackageUtil>().SetIncluded(filteredItems, included);
	}

	protected async Task SetEnabled(IEnumerable<IPackage> filteredItems, bool enabled)
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

		foreach (var item in userItems.SelectWhereNotNull(x => x.GetLocalPackage()))
		{
			if (item?.IsIncluded() == true)
			{
				packagesIncluded++;

				if (item.Package.IsCodeMod)
				{
					modsIncluded++;

					if (item.IsEnabled())
					{
						modsEnabled++;
					}
				}
			}
		}

		var total = LC_Items.ItemCount;

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

	protected override async Task<bool> LoadDataAsync()
	{
		try
		{
			var profiles = await ServiceCenter.Get<SkyveApiUtil>().GetUserProfiles(User.Id!);

			if (profiles?.Any() ?? false)
			{
				L_Profiles.SetItems(profiles);

				this.TryInvoke(() =>
				{
					T_Profiles.LinkedControl = L_Profiles;

					if (T_Profiles.Selected)
					{
						T_Profiles.Selected = true;
					}
				});
			}
			else
			{
				this.TryInvoke(() => tabControl.Tabs = tabControl.Tabs.Where(x => x != T_Profiles).ToArray());
			}

			var results = await _workshopService.GetWorkshopItemsByUserAsync(User.Id!);

			userItems = results.ToList(_workshopService.GetPackage);

			LC_Items.RefreshItems();
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load user data");
			throw;
		}

		return true;
	}

	protected override void OnDataLoad()
	{
		if (LC_Items.ItemCount == 0)
		{
			OnLoadFail();
			return;
		}

		T_Packages.LinkedControl = LC_Items;

		if (T_Packages.Selected)
		{
			T_Packages.Selected = true;
		}
	}

	protected override void OnLoadFail()
	{
		tabControl.Tabs = tabControl.Tabs.Where(x => x != T_Packages).ToArray();
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		PB_Icon.Width = TLP_Top.Height = (int)(128 / 2 * UI.FontScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = design.AccentBackColor;
		P_Content.BackColor = P_Back.BackColor = design.BackColor;
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.AccentBackColor;
	}
}
