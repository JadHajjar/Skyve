using Skyve.App.UserInterface.CompatibilityReport;
using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Generic;
using Skyve.Systems.Compatibility.Domain.Api;

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_ReviewSingleRequest : PanelContent
{
	private ReviewRequest _request;
	private readonly SlickControl logControl;

	private readonly IWorkshopService _workshopService;
	private readonly IDlcManager _dlcManager;
	private readonly SkyveApiUtil _skyveApiUtil;

	public PC_ReviewSingleRequest(ReviewRequest request) : base(true)
	{
		ServiceCenter.Get(out _dlcManager, out _skyveApiUtil, out _workshopService);

		InitializeComponent();

		_request = request;

		TLP_Info.Controls.Add(new MiniPackageControl(request.PackageId) { Dock = DockStyle.Top, ReadOnly = true }, 0, 0);
		TLP_Info.Controls.Add(new SteamUserControl(request.UserId) { InfoText = "Requested by", Dock = DockStyle.Top, Margin = UI.Scale(new Padding(5), UI.FontScale) }, 0, 1);

		logControl = new SlickControl
		{
			Cursor = Cursors.Hand,
			Text = $"RequestBy_{_workshopService.GetUser(_request.UserId)?.Name}_{DateTime.Now:yy-MM-dd_HH-mm}",
			Dock = DockStyle.Top,
			Height = (int)(60 * UI.UIScale),
			Margin = UI.Scale(new Padding(5), UI.FontScale)
		};

		logControl.Paint += PC_ReviewSingleRequest_Paint;
		logControl.Click += LogControl_Click;

		TLP_Info.Controls.Add(logControl, 0, 2);

		label2.Text = request.PackageNote.IfEmpty("No description given");

		if (request.IsInteraction)
		{
			tableLayoutPanel1.Controls.Add(new IPackageStatusControl<InteractionType, PackageInteraction>(_workshopService.GetPackage(new GenericPackageIdentity(request.PackageId)), new PackageInteraction
			{
				Action = (StatusAction)request.StatusAction,
				IntType = request.StatusType,
				Note = request.StatusNote,
				Packages = request.StatusPackages?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray(),
			})
			{ BackColor = FormDesign.Design.AccentBackColor }, 0, 4);
		}
		else if (request.IsStatus)
		{
			tableLayoutPanel1.Controls.Add(new IPackageStatusControl<StatusType, PackageStatus>(_workshopService.GetPackage(new GenericPackageIdentity(request.PackageId)), new PackageStatus
			{
				Action = (StatusAction)request.StatusAction,
				IntType = request.StatusType,
				Note = request.StatusNote,
				Packages = request.StatusPackages?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray(),
			})
			{ BackColor = FormDesign.Design.AccentBackColor }, 0, 4);
		}
		else
		{
			DD_Stability.SelectedItem = (PackageStability)_request.PackageStability;
			DD_PackageType.SelectedItem = (PackageType)_request.PackageType;
			DD_DLCs.SelectedItems = _dlcManager.Dlcs.Where(x => _request.RequiredDLCs?.Contains(x.Id.ToString()) ?? false);
			DD_Usage.SelectedItems = Enum.GetValues(typeof(PackageUsage)).Cast<PackageUsage>().Where(x => ((PackageUsage)_request.PackageUsage).HasFlag(x));

			tableLayoutPanel3.Visible = true;
		}
	}

	private void LogControl_Click(object sender, EventArgs e)
	{
		if (_request.LogFile != null)
		{
			var fileName = CrossIO.Combine(ServiceCenter.Get<ILocationManager>().SkyveSettingsPath, "Support Logs", logControl.Text + ".zip");

			Directory.CreateDirectory(Path.GetDirectoryName(fileName));

			File.WriteAllBytes(fileName, _request.LogFile);

			PlatformUtil.OpenFolder(fileName);
		}
	}

	protected override async Task<bool> LoadDataAsync()
	{
		logControl.Loading = true;

		var request = await _skyveApiUtil.GetReviewRequest(_request.UserId, _request.PackageId);

		if (request != null)
		{
			_request = request;

			return true;
		}

		return false;
	}

	protected override void OnDataLoad()
	{
		logControl.Loading = false;
	}

	protected override void OnLoadFail()
	{
		logControl.Dispose();
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		tableLayoutPanel2.Width = (int)(200 * UI.FontScale);
		label3.Font = label1.Font = UI.Font(7.5F, FontStyle.Bold);
		tableLayoutPanel1.Padding = UI.Scale(new Padding(5), UI.FontScale);
		slickIcon1.Size = UI.Scale(new Size(24, 24), UI.FontScale);
		slickIcon1.Padding = UI.Scale(new Padding(5), UI.FontScale);

		foreach (Control item in roundedGroupTableLayoutPanel1.Controls)
		{
			item.Margin = UI.Scale(new Padding(3, 7, 3, 3), UI.FontScale);
		}

		foreach (Control item in tableLayoutPanel1.Controls)
		{
			item.Margin = UI.Scale(new Padding(5), UI.FontScale);
		}
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		tableLayoutPanel1.BackColor = design.AccentBackColor;
	}

	private void PC_ReviewSingleRequest_Paint(object sender, PaintEventArgs e)
	{
		var ctrl = (sender as SlickControl)!;

		e.Graphics.SetUp(ctrl.BackColor);

		if (ctrl.Loading)
		{
			ctrl.DrawLoader(e.Graphics, ctrl.ClientRectangle.CenterR(UI.Scale(new Size(24, 24), UI.UIScale)));
			return;
		}

		using var fileIcon = IconManager.GetLargeIcon("I_File").Color(FormDesign.Design.MenuForeColor);

		var Padding = ctrl.Margin;
		var textSize = e.Graphics.Measure(ctrl.Text, new Font(Font, FontStyle.Bold), ctrl.Width - Padding.Left);
		var fileHeight = (int)textSize.Height + 3 + fileIcon.Height + Padding.Top;
		var fileRect = ctrl.ClientRectangle;
		var fileContentRect = fileRect.CenterR(Math.Max((int)textSize.Width + 3, fileIcon.Width), fileHeight);

		e.Graphics.FillRoundedRectangle(new SolidBrush(ctrl.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.MenuColor.MergeColor(FormDesign.Design.ActiveColor) : FormDesign.Design.MenuColor), fileRect, Padding.Left);
		e.Graphics.DrawImage(fileIcon, fileContentRect.Align(fileIcon.Size, ContentAlignment.TopCenter));
		e.Graphics.DrawString(ctrl.Text, new Font(Font, FontStyle.Bold), new SolidBrush(FormDesign.Design.MenuForeColor), fileContentRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
	}

	private void B_ManagePackage_Click(object sender, EventArgs e)
	{
		Form.PushPanel(new PC_CompatibilityManagement(new[] { _request.PackageId }));
	}

	private void B_ApplyChanges_Click(object sender, EventArgs e)
	{
		Form.PushPanel(new PC_CompatibilityManagement(_request));
	}

	private async void B_DeleteRequest_Click(object sender, EventArgs e)
	{
		B_DeleteRequest.Loading = true;

		var response = await _skyveApiUtil.ProcessReviewRequest(_request);

		if (response.Success)
		{
			PushBack();
		}
		else
		{
			ShowPrompt("Failed to process the request: " + response.Message, PromptButtons.OK, PromptIcons.Info);
		}

		B_DeleteRequest.Loading = false;
	}

	private void slickIcon1_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(label2.Text);
	}
}
