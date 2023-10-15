using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Content;
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
	private readonly SubscriptionInfoControl subscriptionInfoControl;
	private readonly DownloadsInfoControl downloadsInfoControl;
	private readonly TroubleshootInfoControl TroubleshootInfoControl;

	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly IPlaysetManager _playsetManager;
	private readonly IPackageManager _packageManager;
	private readonly ICitiesManager _citiesManager;
	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IUserService _userService;
	private readonly SkyveApiUtil _skyveApiUtil;

	public MainForm()
	{
		ServiceCenter.Get(out _skyveApiUtil, out _packageManager, out _subscriptionsManager, out _playsetManager, out _citiesManager, out _settings, out _notifier, out _userService);

		InitializeComponent();

		_userService.UserInfoUpdated += _userService_UserInfoUpdated;

		subscriptionInfoControl = new() { Dock = DockStyle.Top };
		downloadsInfoControl = new() { Dock = DockStyle.Top };
		TroubleshootInfoControl = new() { Dock = DockStyle.Top };

		TLP_SideBarTools.Controls.Add(downloadsInfoControl, 0, 0);
		TLP_SideBarTools.Controls.Add(subscriptionInfoControl, 0, 1);
		TLP_SideBarTools.Controls.Add(TroubleshootInfoControl, 0, 2);

		TLP_SideBarTools.SetColumnSpan(subscriptionInfoControl, 2);
		TLP_SideBarTools.SetColumnSpan(downloadsInfoControl, 2);
		TLP_SideBarTools.SetColumnSpan(TroubleshootInfoControl, 2);

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
		_notifier.ContentLoaded += _userService_UserInfoUpdated;

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
		PI_Compatibility.Loading = true;

		_notifier.CompatibilityReportProcessed += _notifier_CompatibilityReportProcessed;
	}

	private void _notifier_CompatibilityReportProcessed()
	{
		_notifier.CompatibilityReportProcessed -= _notifier_CompatibilityReportProcessed;

		PI_Compatibility.Loading = false;
	}

	private void _userService_UserInfoUpdated()
	{
		var hasPackages = _userService.User.Id is not null && _packageManager.Packages.Any(x => _userService.User.Equals(x.GetWorkshopInfo()?.Author));
		PI_CompatibilityManagement.Hidden = !((hasPackages || _userService.User.Manager) && !_userService.User.Malicious);
		PI_ManageAllCompatibility.Hidden = PI_ReviewRequests.Hidden = PI_ManageSinglePackage.Hidden = !(_userService.User.Manager && !_userService.User.Malicious);
		PI_ManageYourPackages.Hidden = !(hasPackages && !_userService.User.Malicious);

		base_P_Tabs.FilterChanged();
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
		PI_Playsets.Text = Locale.Playset.Plural;
		PI_Mods.Text = Locale.Mod.Plural;
		PI_ReviewRequests.Text = LocaleCR.ReviewRequests.Format(string.Empty).Trim();
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
			|| _playsetManager.CurrentPlayset.UnsavedChanges
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

			if (_playsetManager.CurrentPlayset.UnsavedChanges)
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
			ServiceCenter.Get<INotificationsService>().SendNotification(ServiceCenter.Get<IAppInterfaceService>().GetLastVersionNotification());
		}

		if (currentVersion.ToString() != _settings.SessionSettings.LastVersionNotification)
		{
			if (_settings.SessionSettings.FirstTimeSetupCompleted)
			{
				PushPanel(ServiceCenter.Get<IAppInterfaceService>().ChangelogPanel());
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

	private void PI_ModReview_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel(PI_ModUtilities, ServiceCenter.Get<IAppInterfaceService>().UtilitiesPanel());
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

	private void PI_ViewPlaysets_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_PlaysetList>(PI_ViewPlaysets);
	}

	private void PI_AddPlayset_OnClick(object sender, MouseEventArgs e)
	{
		SetPanel<PC_PlaysetAdd>(PI_AddPlayset);
	}

	private void PI_CurrentPlayset_OnClick(object sender, MouseEventArgs e)
	{
		PushPanel(PI_CurrentPlayset, ServiceCenter.Get<IAppInterfaceService>().PlaysetSettingsPanel());
	}

	private async void PI_ManageYourPackages_OnClick(object sender, MouseEventArgs e)
	{
		if (PI_ManageYourPackages.Loading)
		{
			return;
		}

		PI_ManageYourPackages.Loading = true;

		try
		{
			var results = await ServiceCenter.Get<IWorkshopService>().GetWorkshopItemsByUserAsync(_userService.User.Id ?? 0);

			if (results != null)
			{
				Invoke(() => PushPanel(PI_ManageYourPackages, new PC_CompatibilityManagement(results.Select(x => x.Id))));
			}
		}
		catch (Exception ex)
		{
			MessagePrompt.Show(ex, "Failed to load your packages", form: this);
		}

		PI_ManageYourPackages.Loading = false;

	}

	private void PI_ManageSinglePackage_OnClick(object sender, MouseEventArgs e)
	{
		var panel = new PC_SelectPackage() { Text = LocaleHelper.GetGlobalText("Select a package") };

		panel.PackageSelected += Form_PackageSelected;

		Program.MainForm.PushPanel(PI_ManageSinglePackage, panel);
	}

	private void Form_PackageSelected(IEnumerable<ulong> packages)
	{
		PushPanel(PI_ManageSinglePackage, new PC_CompatibilityManagement(packages));
	}

	private async void PI_ReviewRequests_OnClick(object sender, MouseEventArgs e)
	{
		if (PI_ReviewRequests.Loading)
		{
			return;
		}

		PI_ReviewRequests.Loading = true;

		try
		{
			var reviewRequests = await _skyveApiUtil.GetReviewRequests();

			if (reviewRequests is not null)
			{
				Invoke(() => PushPanel(PI_ReviewRequests, new PC_ReviewRequests(reviewRequests)));
			}
		}
		catch (Exception ex)
		{
			MessagePrompt.Show(ex, "Failed to load your packages", form: this);
		}

		PI_ReviewRequests.Loading = false;
	}

	private void PI_ManageAllCompatibility_OnClick(object sender, MouseEventArgs e)
	{
		PushPanel<PC_CompatibilityManagement>(PI_ManageAllCompatibility);
	}
}
