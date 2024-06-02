using Skyve.App.UserInterface.Panels;
using Skyve.App.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public partial class UserDescriptionControl : SlickControl
{
	public IUser User { get; }
	public PC_UserPage? UserPage { get; set; }

	private readonly IWorkshopService _workshopService;
	private readonly IPackageManager _packageManager;
	private readonly IUserService _userService;
	private readonly INotifier _notifier;

	public UserDescriptionControl(IUser user)
	{
		ServiceCenter.Get(out _workshopService, out _userService, out _packageManager, out _notifier);

		InitializeComponent();

		User = user;
		PB_Icon.User = User;

		SetInfo();

		_notifier.WorkshopUsersInfoLoaded += _notifier_WorkshopUsersInfoLoaded;

#if CS2
		I_More.ImageName = "Paradox";
#else
		I_More.ImageName = "Steam";
#endif
	}

	private void _notifier_WorkshopUsersInfoLoaded()
	{
		this.TryInvoke(SetInfo);
	}

	private void SetInfo()
	{
		var author = _workshopService.GetUser(User);

		L_Name.Text = (author ?? User).Name;

		var count = _packageManager.Packages.Count(x => User.Equals(x.GetWorkshopInfo()?.Author));
		L_Packages.Text = Locale.YouHavePackagesUser.FormatPlural(count, User.Name);

		I_Verified.Visible = _userService.IsUserVerified(User);

		if (author == null)
		{
			return;
		}

		L_Bio.Text = author.Bio;
		TLP_Bio.Visible = !string.IsNullOrEmpty(author.Bio);

		L_Followers.Text = Locale.FollowersCount.FormatPlural(author.FollowerCount, author.FollowerCount);
		I_Followers.Visible = L_Followers.Visible = author.FollowerCount > 1;

		var links = new List<ILink>();

		links.AddRange(author?.Links ?? []);
		links = links.DistinctList(x => x.Url);

		if (!links.ToList(x => x.Url).SequenceEqual(FLP_Package_Links.Controls.OfType<LinkControl>().Select(x => x.Link.Url)))
		{
			FLP_Package_Links.SuspendDrawing();
			FLP_Package_Links.Controls.Clear(true);
			FLP_Package_Links.Controls.AddRange(links.ToArray(x => new LinkControl(x, true)));
			FLP_Package_Links.ResumeDrawing();
		}

		TLP_Links.Visible = links.Count > 0;
	}

	protected override void LocaleChanged()
	{
		L_Links.Text = Locale.Links.One.ToUpper();
		L_BioLabel.Text = Locale.Bio.One.ToUpper();

		SlickTip.SetTo(I_Verified, Locale.VerifiedAuthor);
		SlickTip.SetTo(I_More, Locale.OpenAuthorPage);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = (int)(260 * UI.FontScale);

		L_Name.Font = UI.Font(11.25F, FontStyle.Bold);
		PB_Icon.Size = UI.Scale(new Size(72, 72), UI.FontScale);
		I_Verified.Size = I_Followers.Size = I_Packages.Size = UI.Scale(new Size(16, 16), UI.FontScale);
		I_More.MinimumSize = UI.Scale(new Size(24, 24), UI.FontScale);
		TLP_Side.Padding = UI.Scale(new Padding(8, 0, 0, 0), UI.FontScale);
		TLP_TopInfo.Padding = UI.Scale(new Padding(0, 0, 8, 0), UI.FontScale);
		TLP_TopInfo.Margin = base_slickSpacer.Margin = UI.Scale(new Padding(5), UI.FontScale);
		base_slickSpacer.Height = (int)UI.FontScale;
		L_Name.Margin = UI.Scale(new Padding(I_Verified.Visible ? 0 : 3, 5, 5, 5), UI.FontScale);
		L_Bio.Margin =  I_More.Padding = TLP_Bio.Padding = TLP_Bio.Margin = TLP_Links.Padding = TLP_Links.Margin = UI.Scale(new Padding(5), UI.FontScale);
		L_Followers.Font = L_Packages.Font = UI.Font(7F);
		L_BioLabel.Font = L_Links.Font = UI.Font(7F, FontStyle.Bold);
		I_Verified.Margin = I_Followers.Margin = I_Packages.Margin = UI.Scale(new Padding(5, 0, 1, 0), UI.FontScale);
		L_Followers.Margin = L_Packages.Margin = UI.Scale(new Padding(2, 3, 0, 3), UI.FontScale);
		L_BioLabel.Margin = L_Links.Margin = UI.Scale(new Padding(3), UI.FontScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		L_BioLabel.ForeColor = L_Links.ForeColor = design.LabelColor;
		TLP_Bio.BackColor = TLP_Links.BackColor = design.BackColor.Tint(Lum: design.IsDarkTheme ? 6 : -5);
	}

	private void I_More_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenUrl(User.ProfileUrl);
	}
}
