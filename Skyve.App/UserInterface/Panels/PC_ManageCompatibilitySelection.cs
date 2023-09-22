using Skyve.Systems.Compatibility.Domain.Api;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_ManageCompatibilitySelection : PanelContent
{
	private readonly IUserService _userService;
	private readonly IPackageManager _contentManager;
	private readonly SkyveApiUtil _skyveApiUtil;

	private ReviewRequest[]? reviewRequests;

	public PC_ManageCompatibilitySelection() : base(true)
	{
		ServiceCenter.Get(out _userService, out _contentManager, out _skyveApiUtil);

		InitializeComponent();

		SetManagementButtons();
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		B_Cancel.Font = UI.Font(9.75F);
	}

	protected override async Task<bool> LoadDataAsync()
	{
		reviewRequests = await _skyveApiUtil.GetReviewRequests();

		return true;
	}

	protected override void OnDataLoad()
	{
		SetManagementButtons();
	}

	private void B_Cancel_Click(object sender, EventArgs e)
	{
		PushBack();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			PushBack();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void SetManagementButtons()
	{
		var hasPackages = _userService.User.Id is not null && _contentManager.Packages.Any(x => _userService.User.Equals(x.GetWorkshopInfo()?.Author));
		B_Manage.Visible = B_Requests.Visible = B_ManageSingle.Visible = _userService.User.Manager && !_userService.User.Malicious;
		B_YourPackages.Visible = hasPackages && !_userService.User.Manager && !_userService.User.Malicious;
		B_Requests.Text = LocaleCR.ReviewRequests.Format(reviewRequests is null ? string.Empty : $"({reviewRequests.Length})");
		B_Requests.Enabled = reviewRequests is null || reviewRequests.Length > 0;
		B_Requests.Invalidate();
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

	private async void B_Requests_Click(object sender, EventArgs e)
	{
		if (B_Requests.Loading)
		{
			return;
		}

		B_Requests.Loading = reviewRequests == null;

		try
		{
			while (DataLoading)
			{
				await Task.Delay(250);
			}

			if (reviewRequests == null)
			{
				reviewRequests = await _skyveApiUtil.GetReviewRequests();
			}

			if (reviewRequests != null)
			{
				Form.Invoke(() => Form.PushPanel(null, new PC_ReviewRequests(reviewRequests)));
			}
		}
		catch (Exception ex)
		{
			ShowPrompt(ex, "Failed to load your packages");
		}

		B_Requests.Loading = false;
	}

	private async void B_YourPackages_Click(object sender, EventArgs e)
	{
		if (B_YourPackages.Loading)
		{
			return;
		}

		B_YourPackages.Loading = true;

		try
		{
			var results = await ServiceCenter.Get<IWorkshopService>().GetWorkshopItemsByUserAsync(_userService.User.Id ?? 0);

			if (results != null)
			{
				Form.Invoke(() => Form.PushPanel(null, new PC_CompatibilityManagement(results.Select(x => x.Id))));
			}
		}
		catch (Exception ex)
		{
			ShowPrompt(ex, "Failed to load your packages");
		}

		B_YourPackages.Loading = false;
	}
}
