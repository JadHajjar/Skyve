using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Panels;

using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Skyve.App;

public partial class MainForm : BasePanelForm
{
	private readonly System.Timers.Timer _startTimeoutTimer = new(15000) { AutoReset = false };
	private bool isGameRunning;
	private bool? buttonStateRunning;

	private readonly ISubscriptionsManager _subscriptionsManager = ServiceCenter.Get<ISubscriptionsManager>();
	private readonly IPlaysetManager _profileManager = ServiceCenter.Get<IPlaysetManager>();
	private readonly ICitiesManager _citiesManager = ServiceCenter.Get<ICitiesManager>();
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();
	private readonly INotifier _notifier = ServiceCenter.Get<INotifier>();

	public MainForm()
	{
		InitializeComponent();

		base_PB_Icon.UserDraw = true;
		base_PB_Icon.Paint += Base_PB_Icon_Paint;

		SlickTip.SetTo(base_PB_Icon, string.Format(Locale.LaunchTooltip, "[F5]"));

		var currentVersion = Assembly.GetEntryAssembly().GetName().Version;

#if Stable
		L_Version.Text = "v" + currentVersion.GetString();
#else
		L_Version.Text = "v" + currentVersion.GetString() + " Beta";
#endif

		try
		{
			FormDesign.Initialize(this, DesignChanged);
		}
		catch { }

		try
		{

			if (!_settings.SessionSettings.FirstTimeSetupCompleted && string.IsNullOrEmpty(ConfigurationManager.AppSettings[nameof(ILocationManager.GamePath)]))
			{
				SetPanel<PC_Options>(PI_Options);
			}
			else
			{
				SetPanel<PC_MainPage>(PI_Dashboard);
			}
		}
		catch (Exception ex)
		{
			OnNextIdle(() => MessagePrompt.Show(ex, "Failed to load the dashboard", form: this));
		}

		new BackgroundAction("Loading content", ServiceCenter.Get<ICentralManager>().Start).Run();

		var timer = new System.Timers.Timer(1000);

		timer.Elapsed += Timer_Elapsed;

		timer.Start();

		var citiesManager = ServiceCenter.Get<ICitiesManager>();
		var playsetManager = ServiceCenter.Get<IPlaysetManager>();

		citiesManager.MonitorTick += CitiesManager_MonitorTick;

		isGameRunning = citiesManager.IsRunning();

		playsetManager.PromptMissingItems += PromptMissingItemsEvent;

		_startTimeoutTimer.Elapsed += StartTimeoutTimer_Elapsed;

		_notifier.RefreshUI += RefreshUI;
		_notifier.WorkshopInfoUpdated += RefreshUI;
		_notifier.WorkshopUsersInfoLoaded += RefreshUI;

		ConnectionHandler.ConnectionChanged += ConnectionHandler_ConnectionChanged;

		if (CrossIO.FileExists(CrossIO.Combine(Program.CurrentDirectory, "batch.bat")))
		{
			try
			{
				CrossIO.DeleteFile(CrossIO.Combine(Program.CurrentDirectory, "batch.bat"));
			}
			catch { }
		}

		base_PB_Icon.Loading = true;
	}

	public void RefreshUI()
	{
		this.TryInvoke(() => Invalidate(true));
	}

	private void PromptMissingItemsEvent(IPlaysetManager manager, IEnumerable<IPlaysetEntry> playsetEntries)
	{
		PC_MissingPackages.PromptMissingPackages(Program.MainForm, playsetEntries);
	}

	protected override void LocaleChanged()
	{
		PI_Packages.Text = Locale.Package.Plural;
		PI_Assets.Text = Locale.Asset.Plural;
		PI_Profiles.Text = Locale.Playset.Plural;
		PI_Mods.Text = Locale.Mod.Plural;
	}

	private void ConnectionHandler_ConnectionChanged(ConnectionState newState)
	{
		base_PB_Icon.Invalidate();
	}

	private void StartTimeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		buttonStateRunning = null;
		//base_PB_Icon.Loading = false;

		_citiesManager.SetLaunchingStatus(false);

		//if (CurrentPanel is PC_MainPage mainPage)
		//{
		//	mainPage.B_StartStop.Loading = false;
		//}
	}

	private void CitiesManager_MonitorTick(bool isAvailable, bool isRunning)
	{
		isGameRunning = isRunning;

		if (buttonStateRunning is null || buttonStateRunning == isRunning)
		{
			if (_startTimeoutTimer.Enabled)
			{
				_startTimeoutTimer.Stop();
			}

			//if (base_PB_Icon.Loading != isRunning)
			//{
			//	base_PB_Icon.Loading = isRunning;
			//}

			base_PB_Icon.LoaderSpeed = 0.15;

			buttonStateRunning = null;

			_citiesManager.SetLaunchingStatus(false);
			//if (CurrentPanel is PC_MainPage mainPage)
			//{
			//	mainPage.B_StartStop.Loading = false;
			//}
		}
	}

	private void Base_PB_Icon_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.SetUp(base_PB_Icon.BackColor);

		var backBrightness = FormDesign.Design.MenuColor.GetBrightness();
		var foreBrightness = FormDesign.Design.ForeColor.GetBrightness();

		using var icon = new Bitmap(IconManager.GetIcons("I_AppIcon").FirstOrDefault(x => x.Key > base_PB_Icon.Width).Value).Color(base_PB_Icon.HoverState.HasFlag(HoverState.Hovered) && !base_PB_Icon.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.MenuForeColor : Math.Abs(backBrightness - foreBrightness) < 0.4F ? FormDesign.Design.BackColor : FormDesign.Design.ForeColor);

		var useGlow = !ConnectionHandler.IsConnected
			|| (buttonStateRunning is not null && buttonStateRunning != isGameRunning)
			|| isGameRunning
			|| _subscriptionsManager.SubscriptionsPending
			|| _profileManager.CurrentPlayset.UnsavedChanges
			|| base_PB_Icon.HoverState.HasFlag(HoverState.Pressed);

		e.Graphics.DrawImage(icon, base_PB_Icon.ClientRectangle);

		if (useGlow)
		{
			using var glowIcon = new Bitmap(IconManager.GetIcons("I_GlowAppIcon").FirstOrDefault(x => x.Key > base_PB_Icon.Width).Value);

			var color = FormDesign.Modern.ActiveColor;
			var minimum = 0;

			if (!ConnectionHandler.IsConnected)
			{
				minimum = 60;
				color = Color.FromArgb(194, 38, 33);
			}

			if (_profileManager.CurrentPlayset.UnsavedChanges)
			{
				minimum = 0;
				color = Color.FromArgb(122, 81, 207);
			}

			if (buttonStateRunning is null && isGameRunning)
			{
				minimum = 120;
				color = Color.FromArgb(15, 153, 212);
			}

			if (_subscriptionsManager.SubscriptionsPending)
			{
				minimum = 40;
				color = Color.FromArgb(50, 168, 82);
			}

			if (buttonStateRunning == false)
			{
				minimum = 0;
				color = Color.FromArgb(232, 157, 22);
			}

			glowIcon.Tint(Sat: color.GetSaturation(), Hue: color.GetHue());

			if (base_PB_Icon.Loading && !base_PB_Icon.HoverState.HasFlag(HoverState.Pressed))
			{
				var loops = 10;
				var target = 256;
				var perc = (-Math.Cos(base_PB_Icon.LoaderPercentage * loops * Math.PI / 200) * (target - minimum) / 2) + ((target + minimum) / 2);
				var alpha = (byte)perc;

				if (alpha == 0)
				{
					return;
				}

				glowIcon.Alpha(alpha);
			}

			e.Graphics.DrawImage(glowIcon, base_PB_Icon.ClientRectangle);
		}
	}

	private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		if (!CrossIO.FileExists(CrossIO.Combine(Program.CurrentDirectory, "Wake")))
		{
			return;
		}

		CrossIO.DeleteFile(CrossIO.Combine(Program.CurrentDirectory, "Wake"));

		if (isGameRunning)
		{
			SendKeys.SendWait("%{TAB}");
		}

		this.TryInvoke(this.ShowUp);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		MinimumSize = UI.Scale(new Size(600, 350), UI.FontScale);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.F5)
		{
			if (_citiesManager.IsAvailable())
			{
				LaunchStopCities();

				return true;
			}
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnAppIconClicked()
	{
		LaunchStopCities();
	}

	public void LaunchStopCities()
	{
		if (_citiesManager.IsAvailable())
		{
			if (CrossIO.CurrentPlatform is Platform.Windows)
			{
				_citiesManager.SetLaunchingStatus(true);
				//if (CurrentPanel is PC_MainPage mainPage)
				//{
				//	mainPage.B_StartStop.Loading = true;
				//}

				//base_PB_Icon.Loading = true;
				base_PB_Icon.LoaderSpeed = 1;
			}

			if (_citiesManager.IsRunning())
			{
				buttonStateRunning = false;
				new BackgroundAction("Stopping Cities: Skylines", _citiesManager.Kill).Run();
			}
			else
			{
				buttonStateRunning = true;
				new BackgroundAction("Starting Cities: Skylines", _citiesManager.Launch).Run();
			}

			_startTimeoutTimer.Stop();
			_startTimeoutTimer.Start();
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (_settings.SessionSettings.LastWindowsBounds != null)
		{
			if (!SystemInformation.VirtualScreen.Contains(_settings.SessionSettings.LastWindowsBounds.Value.Location))
			{
				return;
			}

			Bounds = _settings.SessionSettings.LastWindowsBounds.Value;

			LastUiScale = UI.UIScale;
		}

		if (_settings.SessionSettings.WindowWasMaximized)
		{
			WindowState = FormWindowState.Minimized;
			WindowState = FormWindowState.Maximized;
		}

		var assembly = Assembly.GetEntryAssembly();
		var currentVersion = assembly.GetName().Version;
		var date = File.GetLastWriteTime(assembly.Location);

		if (date > DateTime.Now.AddDays(-7))
		{
			ServiceCenter.Get<INotificationsService>().SendNotification(ServiceCenter.Get<IInterfaceService>().GetLastVersionNotification());
		}

		if (currentVersion.ToString() != _settings.SessionSettings.LastVersionNotification)
		{
			if (_settings.SessionSettings.FirstTimeSetupCompleted)
			{
				PushPanel(ServiceCenter.Get<IInterfaceService>().ChangelogPanel());
			}

			_settings.SessionSettings.LastVersionNotification = currentVersion.ToString();
			_settings.SessionSettings.Save();
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		base.OnFormClosing(e);

		if (!TopMost)
		{
			if (_settings.SessionSettings.WindowWasMaximized = WindowState == FormWindowState.Maximized)
			{
				if (SystemInformation.VirtualScreen.IntersectsWith(RestoreBounds))
				{
					_settings.SessionSettings.LastWindowsBounds = RestoreBounds;
				}
			}
			else
			{
				if (SystemInformation.VirtualScreen.IntersectsWith(Bounds))
				{
					_settings.SessionSettings.LastWindowsBounds = Bounds;
				}
			}

			_settings.SessionSettings.Save();
		}
	}

	private void PI_Dashboard_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_MainPage>(PI_Dashboard);
	}

	private void PI_Mods_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_Mods>(PI_Mods);
	}

	private void PI_Assets_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_Assets>(PI_Assets);
	}

	private void PI_Profiles_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_PlaysetList>(PI_Profiles);
	}

	private void PI_ModReview_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel(PI_ModUtilities, ServiceCenter.Get<IInterfaceService>().UtilitiesPanel());
	}

	private void PI_Packages_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_Packages>(PI_Packages);
	}

	private void PI_Options_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_Options>(PI_Options);
	}

	private void PI_Troubleshoot_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_HelpAndLogs>(PI_Troubleshoot);
	}

	private void PI_DLCs_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_DLCs>(PI_DLCs);
	}

	private void PI_Compatibility_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_CompatibilityReport>(PI_Compatibility);
	}
}
