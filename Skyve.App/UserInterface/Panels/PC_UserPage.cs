using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Lists;

using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_UserPage : PanelContent
{
	private readonly ContentList LC_Items;
	private readonly PlaysetListControl L_Profiles;

	private readonly ISettings _settings;
	private readonly IWorkshopService _workshopService;

	private readonly List<IPackageIdentity> userItems = [];
	private UserDescriptionControl P_Info;

	public IUser User { get; }

	public PC_UserPage(IUser user) : base(true)
	{
		ServiceCenter.Get(out _settings, out _workshopService);

		InitializeComponent();

		User = user;
		Text = user.Name;
		P_Info = new UserDescriptionControl(user) { Dock = System.Windows.Forms.DockStyle.Right };
		Controls.Add(P_Info);

		T_Profiles.LinkedControl = L_Profiles = new(true)
		{
			GridView = true,
		};

		T_Packages.LinkedControl = LC_Items = new ContentList(SkyvePage.User, false, GetItems, GetItemText);

		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";

		LC_Items.ListControl.Loading = true;
	}

	protected override async void OnCreateControl()
	{
		base.OnCreateControl();

		await LC_Items.RefreshItems();
	}

	protected override void LocaleChanged()
	{
		T_Profiles.Text = Locale.Playset.Plural;
		T_Packages.Text = Locale.Package.Plural;
	}

	protected async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		return (await _workshopService.GetWorkshopItemsByUserAsync(User.Id!)).Mods;
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

	protected override void OnDataLoad()
	{
		if (L_Profiles.ItemCount > 0)
		{
			T_Profiles.Visible = true;
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);
	}
}
