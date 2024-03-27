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

	private List<IPackageIdentity> userItems = [];

	public IUser User { get; }

	public PC_UserPage(IUser user) : base(true)
	{
		ServiceCenter.Get(out _settings, out _workshopService);

		InitializeComponent();

		User = user;

		PB_Icon.LoadImage(User.AvatarUrl, ServiceCenter.Get<IImageService>().GetImage);
		P_Info.SetUser(User, this);

		T_Profiles.LinkedControl = L_Profiles = new(true)
		{
			GridView = true,
		};

		T_Packages.LinkedControl = LC_Items = new ContentList(SkyvePage.User, false, GetItems, GetItemText)
		{
			IsGenericPage = true
		};

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
		return await _workshopService.GetWorkshopItemsByUserAsync(User.Id!);
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

	protected override async Task<bool> LoadDataAsync()
	{
		try
		{
			//var profiles = await ServiceCenter.Get<ISkyveApiUtil>().GetUserPlaysets(User);

			//if (profiles?.Any() ?? false)
			//{
			//	L_Profiles.SetItems(profiles);

			//	this.TryInvoke(() =>
			//	{

			//		if (T_Profiles.Selected)
			//		{
			//			T_Profiles.Selected = true;
			//		}
			//	});
			// T_Profiles.Visible = true;
			//}
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
		if (L_Profiles.ItemCount > 0)
		{
			T_Profiles.Visible = true;
		}
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
