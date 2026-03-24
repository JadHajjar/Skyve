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
		  { "D_AssetsInfo", new (new(2500, 100, 2500, 100),false) },
		  { "D_CompatibilityInfo", new (new(0, 0, 5000, 100),false) },
		  { "D_ModsInfo", new (new(0, 100, 2500, 100) ,false)},
		  { "D_Playsets", new (new(5250, 0, 2250, 100),false) },
		  { "D_NotificationCenter", new (new(7750, 0, 2250, 100),false) },
		  { "D_DiskInfo", new (new(7750, 100, 2250, 100),false) },
		};
	}
}