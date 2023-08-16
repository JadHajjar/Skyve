using Skyve.Systems.Compatibility.Domain.Api;

using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_ReviewRequests : PanelContent
{
	private List<ReviewRequest> _reviewRequests;

	internal ulong CurrentPackage;

	private readonly IWorkshopService _workshopService = ServiceCenter.Get<IWorkshopService>();

	public PC_ReviewRequests(ReviewRequest[] reviewRequests)
	{
		InitializeComponent();

		_reviewRequests = reviewRequests.ToList();

		packageCrList.SetItems(reviewRequests.Select(x => x.PackageId).Distinct());
		packageCrList.CanDrawItem += PackageCrList_CanDrawItem;

		TB_Search.Placeholder = $"{LocaleSlickUI.Search}..";
		Text = LocaleCR.ReviewRequests.Format($"({reviewRequests?.Length})");
	}

	protected override async void OnShown()
	{
		base.OnShown();

		_reviewRequests = (await ServiceCenter.Get<SkyveApiUtil>().GetReviewRequests())?.ToList() ?? _reviewRequests;

		packageCrList.SetItems(_reviewRequests.Select(x => x.PackageId).Distinct());

		Text = LocaleCR.ReviewRequests.Format($"({_reviewRequests.Count})");

		SetPackage(_reviewRequests.Any(x => x.PackageId == CurrentPackage) ? CurrentPackage : 0);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		TLP_List.Padding = UI.Scale(new Padding(5), UI.FontScale);
		TLP_List.Width = (int)(210 * UI.FontScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);
	}

	private void packageCrList_ItemMouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			SetPackage((ulong)sender);
		}

		if (e.Button == MouseButtons.Right)
		{
			SlickToolStrip.Show(Form, packageCrList.PointToClient(e.Location), PC_PackagePage.GetRightClickMenuItems(_workshopService.GetPackage(new GenericPackageIdentity((ulong)sender)!)!));
		}
	}

	private void SetPackage(ulong sender)
	{
		CurrentPackage = sender;

		packageCrList.Invalidate();
		reviewRequestList1.SetItems(_reviewRequests.Where(x => x.PackageId == sender));

		B_DeleteRequests.Visible = sender != 0;
	}

	private void PackageCrList_CanDrawItem(object sender, CanDrawItemEventArgs<ulong> e)
	{
		var package = _workshopService.GetInfo(new GenericPackageIdentity(e.Item));

		if (package is null)
		{
			return;
		}

		e.DoNotDraw = !(TB_Search.Text.SearchCheck(package.ToString())
			|| TB_Search.Text.SearchCheck(package.Author?.Name)
			|| package.Id.ToString().IndexOf(TB_Search.Text, StringComparison.OrdinalIgnoreCase) != -1);
	}

	private void reviewRequestList1_ItemMouseClick(object sender, MouseEventArgs e)
	{
		Form.Invoke(() => Form.PushPanel(new PC_ReviewSingleRequest((ReviewRequest)sender)));
	}

	private void TB_Search_TextChanged(object sender, EventArgs e)
	{
		TB_Search.ImageName = string.IsNullOrWhiteSpace(TB_Search.Text) ? "I_Search" : "I_ClearSearch";

		packageCrList.FilterChanged();
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private async void B_DeleteRequests_Click(object sender, EventArgs e)
	{
		B_DeleteRequests.Loading = true;

		foreach (var request in _reviewRequests.Where(x => x.PackageId == CurrentPackage))
		{
			await ServiceCenter.Get<SkyveApiUtil>().ProcessReviewRequest(request);
		}

		OnShown();

		B_DeleteRequests.Loading = false;
	}
}
