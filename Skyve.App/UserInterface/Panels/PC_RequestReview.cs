using SkyveApp.Systems.Compatibility.Domain.Api;
using SkyveApp.Systems.CS1.Utilities;
using SkyveApp.UserInterface.CompatibilityReport;

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Panels;
public partial class PC_RequestReview : PanelContent
{
	private IPackageStatusControl<StatusType, PackageStatus>? statusControl;
	private IPackageStatusControl<InteractionType, PackageInteraction>? interactionControl;

	private readonly ICompatibilityManager _compatibilityManager = ServiceCenter.Get<ICompatibilityManager>();

	public PC_RequestReview(IPackage package)
	{
		CurrentPackage = package;

		InitializeComponent();

		PB_Icon.Package = CurrentPackage;
		PB_Icon.Image = null;
		PB_Icon.LoadImage(CurrentPackage.GetWorkshopInfo()?.ThumbnailUrl, ServiceCenter.Get<IImageService>().GetImage);
		P_Info.SetPackage(CurrentPackage, null);
	}

	public IPackage CurrentPackage { get; }

	protected override void LocaleChanged()
	{
		Text = LocaleCR.RequestReview;
		L_Disclaimer.Text = LocaleCR.RequestReviewDisclaimer;
		B_Apply.Text = Locale.SendReview + "*";
		L_English.Text = Locale.UseEnglishPlease;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		P_Main.Padding = UI.Scale(new Padding(7), UI.FontScale);
		PB_Icon.Width = TLP_Top.Height = (int)(128 * UI.FontScale);
		L_Disclaimer.Margin = B_Apply.Margin = B_Apply.Padding = TB_Note.Margin = UI.Scale(new Padding(5), UI.FontScale);
		foreach (Control item in TLP_MainInfo.Controls)
		{
			item.Margin = UI.Scale(new Padding(5), UI.FontScale);
		}

		B_AddInteraction.Padding = B_AddStatus.Padding = UI.Scale(new Padding(15), UI.FontScale);
		B_AddInteraction.Font = B_AddStatus.Font = UI.Font(9.75F);
		B_AddInteraction.Margin = B_AddStatus.Margin = UI.Scale(new Padding(50, 40, 0, 0), UI.UIScale);

		TB_Note2.MinimumSize = TB_Note.MinimumSize = UI.Scale(new Size(0, 100), UI.UIScale);
		L_Disclaimer.Font = UI.Font(7.5F, FontStyle.Bold | FontStyle.Italic);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		L_Disclaimer.ForeColor = design.InfoColor;
		P_Main.BackColor = design.AccentBackColor;
		L_English.ForeColor = design.YellowColor;
	}

	private void B_ReportIssue_Click(object sender, EventArgs e)
	{
		TLP_Actions.Hide();
		TLP_Button.Show();
		TLP_MainInfo.Show();
		P_Main.Show();

		var data = _compatibilityManager.GetPackageInfo(CurrentPackage);
		DD_Stability.SelectedItem = data?.Stability ?? PackageStability.NotReviewed;
		DD_PackageType.SelectedItem = data?.Type ?? PackageType.GenericPackage;
		DD_DLCs.SelectedItems = data is null ? Enumerable.Empty<IDlcInfo>() : ServiceCenter.Get<IDlcManager>().Dlcs.Where(x => data.RequiredDLCs?.Contains(x.Id) ?? false);
		DD_Usage.SelectedItems = Enum.GetValues(typeof(PackageUsage)).Cast<PackageUsage>().Where(x => data?.Usage.HasFlag(x) ?? true);
	}

	private void B_AddStatus_Click(object sender, EventArgs e)
	{
		tableLayoutPanel1.Controls.Add(statusControl = new IPackageStatusControl<StatusType, PackageStatus>(CurrentPackage) { Margin = TB_Note.Margin }, 0, 0);

		TLP_Button.Show();
		tableLayoutPanel1.Show();
		P_Main.Show();
		TLP_Actions.Hide();
	}

	private void B_AddInteraction_Click(object sender, EventArgs e)
	{
		tableLayoutPanel1.Controls.Add(interactionControl = new IPackageStatusControl<InteractionType, PackageInteraction>(CurrentPackage) { Margin = TB_Note.Margin }, 0, 0);

		TLP_Button.Show();
		tableLayoutPanel1.Show();
		P_Main.Show();
		TLP_Actions.Hide();
	}

	private async void B_Apply_Click(object sender, EventArgs e)
	{
		var tb = statusControl is null && interactionControl is null ? TB_Note2 : TB_Note;

		if (tb.Text.AsEnumerable().Count(x => !char.IsWhiteSpace(x) && x != '.') < 5)
		{
			ShowPrompt(Locale.AddMeaningfulDescription, PromptButtons.OK, PromptIcons.Hand);
			return;
		}

		if (B_Apply.Loading)
		{
			return;
		}

		B_Apply.Loading = true;

		var postPackage = new ReviewRequest
		{
			PackageId = CurrentPackage!.Id,
			PackageNote = tb.Text
		};

		if (statusControl is not null)
		{
			postPackage.IsStatus = true;
			postPackage.StatusNote = statusControl.PackageStatus.Note;
			postPackage.StatusAction = (int)statusControl.PackageStatus.Action;
			postPackage.StatusPackages = statusControl.PackageStatus.Packages!.ListStrings(",");
			postPackage.StatusType = (int)statusControl.PackageStatus.Type;
		}
		else if (interactionControl is not null)
		{
			postPackage.IsInteraction = true;
			postPackage.StatusNote = interactionControl.PackageStatus.Note;
			postPackage.StatusAction = (int)interactionControl.PackageStatus.Action;
			postPackage.StatusPackages = interactionControl.PackageStatus.Packages!.ListStrings(",");
			postPackage.StatusType = (int)interactionControl.PackageStatus.Type;
		}
		else
		{
			postPackage.PackageStability = (int)DD_Stability.SelectedItem;
			postPackage.PackageType = (int)DD_PackageType.SelectedItem;
			postPackage.PackageUsage = (int)DD_Usage.SelectedItems.Aggregate((prev, next) => prev | next);
			postPackage.RequiredDLCs = DD_DLCs.SelectedItems.Select(x => x.Id).ListStrings(",");
		}

		postPackage.LogFile = await Task.Run(() =>
		{
			using var stream = new MemoryStream();

			ServiceCenter.Get<ILogUtil>().CreateZipToStream(stream);

			return stream.ToArray();
		});

		var response = await ServiceCenter.Get<SkyveApiUtil>().SendReviewRequest(postPackage);

		if (response.Success)
		{
			PushBack();
			ShowPrompt(Locale.ReviewRequestSent.Format(CurrentPackage.CleanName()), PromptButtons.OK, PromptIcons.Info);
		}
		else
		{
			ShowPrompt(Locale.ReviewRequestFailed.Format(CurrentPackage.CleanName(), response.Message), PromptButtons.OK, PromptIcons.Info);
		}

		B_Apply.Loading = false;
	}
}
