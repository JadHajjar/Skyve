using System.Drawing;

using static Skyve.App.UserInterface.Panels.DashboardPanelControl;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_MainPage : PanelContent
{
	private readonly DashboardPanelControl dashboardPanel;

	public PC_MainPage()
	{
		InitializeComponent();

		Controls.Add(dashboardPanel = new DashboardPanelControl(null, GetDefaultLayout));
	}

	protected override void OnShown()
	{
		base.OnShown();

		dashboardPanel.PerformLayout();
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 2 : -5);
	}

	private Dictionary<string, DashboardSetting> GetDefaultLayout()
	{
		return new()
		{
		  { "D_CompatibilityInfo", new(new(0, 0, 4500, 100), false) },
		  { "D_ContentInfo",  new(new(0, 100, 2250, 100), false) },
		  { "D_DiskInfo",  new(new(2250, 100, 2250, 100), false) },
		  { "D_PdxModsNew",  new(new(4500, 0, 3500, 100), false) },
		  { "D_PdxUser",  new(new(8000, 0, 2000, 100), false) },
		  { "D_Playsets",  new(new(8000, 100, 2000, 100), false) },
		  { "D_NotificationCenter",  new(new(8000, 200, 2000, 100), false) },
		};
	}
}
