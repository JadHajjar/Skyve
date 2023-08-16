using Skyve.App.UserInterface.Bubbles;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_MainPage : PanelContent
{
	private bool buttonStateRunning;
	private readonly INotifier _notifier;
	private readonly ICitiesManager _citiesManager;
	private readonly IPlaysetManager _playsetManager;
	private readonly IModLogicManager _modLogicManager;

	public PC_MainPage()
	{
		ServiceCenter.Get(out _notifier, out _citiesManager, out _playsetManager, out _modLogicManager);

		InitializeComponent();

		B_StartStop.Enabled = _notifier.IsContentLoaded && _citiesManager.IsAvailable();

		if (!_notifier.IsContentLoaded)
		{
			_notifier.ContentLoaded += SetButtonEnabledOnLoad;
		}

		_citiesManager.MonitorTick += CitiesManager_MonitorTick;

		RefreshButtonState(_citiesManager.IsRunning(), true);

		SlickTip.SetTo(B_StartStop, string.Format(Locale.LaunchTooltip, "[F5]"));

		label1.Text = Locale.MultipleLOM;

		_notifier.PlaysetUpdated += ProfileManager_ProfileUpdated;

		if (ServiceCenter.Get<INotifier>().PlaysetsLoaded)
		{
			ProfileManager_ProfileUpdated();
		}
	}

	private void ProfileManager_ProfileUpdated()
	{
		this.TryInvoke(() =>
		{
			TLP_Profiles.Controls.Clear(true, x => x is FavoriteProfileBubble);
			TLP_Profiles.RowStyles.Clear();
			TLP_Profiles.RowStyles.Add(new());

			foreach (var item in _playsetManager.Playsets.Where(x => x.IsFavorite))
			{
				TLP_Profiles.RowStyles.Add(new());
				TLP_Profiles.Controls.Add(new FavoriteProfileBubble(item) { Dock = DockStyle.Top }, 0, TLP_Profiles.RowStyles.Count - 1);
			}
		});
	}

	private void SetButtonEnabledOnLoad()
	{
		this.TryInvoke(() =>
		{
			B_StartStop.Enabled = _citiesManager.IsAvailable();

			label1.Visible = _modLogicManager.AreMultipleSkyvesPresent();
		});
	}

	protected override void LocaleChanged()
	{
		Text = Locale.Dashboard;
	}

	private void CitiesManager_MonitorTick(bool isAvailable, bool isRunning)
	{
		this.TryInvoke(() => B_StartStop.Enabled = isAvailable);

		RefreshButtonState(isRunning);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		B_StartStop.Font = UI.Font(9.75F, FontStyle.Bold);
		label1.Font = UI.Font(10.5F, FontStyle.Bold);
		label1.Margin = UI.Scale(new Padding(10), UI.FontScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		label1.ForeColor = design.RedColor;
	}

	private void ProfileBubble_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			Form.PushPanel<PC_PlaysetSettings>();
		}
	}

	private void ModsBubble_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			Form.PushPanel<PC_Mods>((Form as MainForm)?.PI_Mods);
		}
	}

	private void AssetsBubble_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			Form.PushPanel<PC_Assets>((Form as MainForm)?.PI_Assets);
		}
	}

	private void B_StartStop_Click(object sender, EventArgs e)
	{
		Program.MainForm.LaunchStopCities();
	}

	private void RefreshButtonState(bool running, bool firstTime = false)
	{
		if (!running)
		{
			if (buttonStateRunning || firstTime)
			{
				this.TryInvoke(() =>
				{
					B_StartStop.ImageName = "I_CS";
					B_StartStop.Text = LocaleHelper.GetGlobalText("StartCities");
					buttonStateRunning = false;
				});
			}

			return;
		}

		if (!buttonStateRunning || firstTime)
		{
			this.TryInvoke(() =>
			{
				B_StartStop.ImageName = "I_Stop";
				B_StartStop.Text = LocaleHelper.GetGlobalText("StopCities");
				buttonStateRunning = true;
			});
		}
	}
}
