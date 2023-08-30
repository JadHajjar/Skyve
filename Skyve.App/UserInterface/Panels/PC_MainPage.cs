using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Dashboard;

using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_MainPage : PanelContent
{
	private readonly bool buttonStateRunning;
	private readonly INotifier _notifier;
	private readonly ICitiesManager _citiesManager;
	private readonly IPlaysetManager _playsetManager;
	private readonly IModLogicManager _modLogicManager;
	private IDashboardItem? MoveItem;
	private IDashboardItem? ResizeItem;
	private Rectangle savedRect;
	private Point CursorStart;
	private bool layoutInProgress;
	private readonly Dictionary<string, Rectangle> _dashItemSizes = new();
	private readonly Control _control = new();

	public PC_MainPage()
	{
		ServiceCenter.Get(out _notifier, out _citiesManager, out _playsetManager, out _modLogicManager);

		InitializeComponent();

		P_Board.SuspendLayout();

		var assembly_base = Assembly.GetExecutingAssembly();
		var assembly = Assembly.GetEntryAssembly();

		LoadItems(assembly_base.GetTypes().Where(type => typeof(IDashboardItem).IsAssignableFrom(type) && !type.IsAbstract));
		LoadItems(assembly.GetTypes().Where(type => typeof(IDashboardItem).IsAssignableFrom(type) && !type.IsAbstract));

		//B_StartStop.Enabled = _notifier.IsContentLoaded && _citiesManager.IsAvailable();

		if (!_notifier.IsContentLoaded)
		{
			_notifier.ContentLoaded += SetButtonEnabledOnLoad;
		}

		_citiesManager.MonitorTick += CitiesManager_MonitorTick;

		RefreshButtonState(_citiesManager.IsRunning(), true);

		//SlickTip.SetTo(B_StartStop, string.Format(Locale.LaunchTooltip, "[F5]"));

		//label1.Text = Locale.MultipleLOM;

		_notifier.PlaysetUpdated += ProfileManager_ProfileUpdated;

		if (ServiceCenter.Get<INotifier>().PlaysetsLoaded)
		{
			ProfileManager_ProfileUpdated();
		}
	}

	protected override void OnShown()
	{
		base.OnShown();

		P_Container.PerformLayout();
	}

	private void LoadItems(IEnumerable<Type> types)
	{
		foreach (var type in types)
		{
			var control = (IDashboardItem)Activator.CreateInstance(type);

			control.Size = default;
			control.MouseDown += DashMouseDown;
			control.MouseMove += DashMouseMove;
			control.MouseUp += DashMouseUp;
			control.ResizeRequested += DashResizeRequested;

			P_Board.Controls.Add(control);
		}
	}

	private void DashResizeRequested(object sender, EventArgs e)
	{
		this.TryInvoke(() => P_Board_Layout(sender, null));
	}

	private void DashMouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		var control = (IDashboardItem)sender;

		if (control.ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight).Contains(e.Location))
		{
			ResizeItem = control;
			CursorStart = new(Cursor.Position.X, control.Width);
			savedRect = control.Bounds;
			control.ResizeInProgress = true;
			control.BringToFront();
		}
		else if (control.MoveAreaContains(e.Location))
		{
			MoveItem = control;
			CursorStart = control.PointToClient(Cursor.Position);
			savedRect = control.Bounds;
			control.MoveInProgress = true;
			control.BringToFront();
		}
	}

	private void DashMouseMove(object sender, MouseEventArgs e)
	{
		var dashItem = (IDashboardItem)sender;

		if (dashItem.ResizeInProgress || dashItem.ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight).Contains(e.Location))
		{
			dashItem.Cursor = Cursors.SizeWE;
		}
		else if (dashItem.MoveInProgress || dashItem.MoveAreaContains(e.Location))
		{
			dashItem.Cursor = Cursors.SizeAll;
		}
		else
		{
			dashItem.Cursor = Cursors.Default;
		}
	}

	private void DashMouseUp(object sender, MouseEventArgs e)
	{
		var dashItem = (IDashboardItem)sender;

		if (MoveItem is not null)
		{
			SaveDashItemBounds(MoveItem, GetSnappingRect(MoveItem, false, savedRect));
			MoveItem = null;
		}

		if (ResizeItem is not null)
		{
			SaveDashItemBounds(ResizeItem, GetSnappingRect(ResizeItem, false, savedRect));
			ResizeItem = null;
		}

		dashItem.MoveInProgress = false;
		dashItem.ResizeInProgress = false;

		P_Container.PerformLayout();
	}

	private void SaveDashItemBounds(IDashboardItem item, Rectangle? rectangle = null)
	{
		var size = P_Container.Size;
		var bounds = rectangle ?? item.Bounds;

		var rect = new Rectangle(
			bounds.X * 1_0000 / size.Width,
			bounds.Y * 1_0000 / size.Height,
			bounds.Width * 1_0000 / size.Width,
			bounds.Height * 1_0000 / size.Height);

		if (rectangle is null && _dashItemSizes.ContainsKey(item.Key))
		{
			rect.X = _dashItemSizes[item.Key].X;
			rect.Width = _dashItemSizes[item.Key].Width;
		}

		_dashItemSizes[item.Key] = rect;
	}

	private Rectangle GetSnappingRect(IDashboardItem control, bool addPadding = true, Rectangle? savedRect = null)
	{
		var rect = savedRect ?? control.Bounds.InvertPad(control.Padding);

		rect.Width = Math.Max(rect.Width, (int)(100 * UI.FontScale));

		var margin = (int)(32 * UI.FontScale);
		var closestX = P_Board.Controls.Where(x => x != control && x is IDashboardItem).Select(x => x.Left).OrderBy(number => Math.Abs(number - rect.Left)).FirstOrDefault();

		if (Math.Abs(closestX - rect.Left) < margin)
		{
			rect.X = closestX;
		}
		else
		{
			var closestRight = P_Board.Controls.Where(x => x != control && x is IDashboardItem).Select(x => x.Right).OrderBy(number => Math.Abs(number - rect.Left)).FirstOrDefault();

			if (Math.Abs(closestRight - rect.Left) < margin)
			{
				rect.X = closestRight;
			}
			else if (rect.Left < margin)
			{
				rect.X = 0;
			}
			else if (MoveItem is not null && rect.Right + margin > P_Container.Width)
			{
				rect.X = P_Container.Width - rect.Width;
			}
		}

		if (rect.Y < margin)
		{
			rect.Y = -margin;
		}

		closestX = P_Board.Controls.Where(x => x != control && x is IDashboardItem).Select(x => x.Left).OrderBy(number => Math.Abs(number - rect.Right - control.Padding.Horizontal)).FirstOrDefault();

		if (Math.Abs(closestX - rect.Right - control.Padding.Horizontal) < margin)
		{
			rect.Width = closestX - rect.X + (addPadding ? control.Padding.Horizontal : 0);
		}
		else
		{
			var closestRight = P_Board.Controls.Where(x => x != control && x is IDashboardItem).Select(x => x.Right).OrderBy(number => Math.Abs(number - rect.Right - control.Padding.Horizontal)).FirstOrDefault();

			if (Math.Abs(closestRight - rect.Right - control.Padding.Horizontal) < margin)
			{
				rect.Width = closestRight - rect.X + (addPadding ? control.Padding.Horizontal : 0);
			}
		}

		if (ResizeItem is not null && rect.Right > P_Container.Width)
		{
			rect.Width = P_Container.Width - rect.X;
		}

		using var g = _control.CreateGraphics();

		rect.Height = control.CalculateHeight(rect.Width, g);

		return rect;
	}

	protected override void GlobalMouseMove(Point p)
	{
		if (MoveItem is not null)
		{
			p.Offset(-CursorStart.X, -CursorStart.Y);

			savedRect.Location = P_Board.PointToClient(p);

			var rect = GetSnappingRect(MoveItem, false, savedRect);

			SaveDashItemBounds(MoveItem, rect);

			P_Container.PerformLayout();
		}

		if (ResizeItem is not null)
		{
			var width = CursorStart.Y + p.X - CursorStart.X;

			using var g = _control.CreateGraphics();

			savedRect.Size = new(width, ResizeItem.CalculateHeight(width, g));

			var rect = GetSnappingRect(ResizeItem, false, savedRect);

			SaveDashItemBounds(ResizeItem, rect);

			P_Container.PerformLayout();
		}
	}

	private void P_Board_Layout(object sender, LayoutEventArgs? e)
	{
		if (layoutInProgress || !Live)
		{
			return;
		}

		layoutInProgress = true;
		using var g = _control.CreateGraphics();

		P_Board.SuspendLayout();

		var small = P_Container.Width < (int)(500 * UI.FontScale);
		var controls = P_Board.Controls.OfType<IDashboardItem>().ToList(x => new DashItemRect(x));

		foreach (var dashItem in controls)
		{
			var rect = _dashItemSizes.ContainsKey(dashItem.Item.Key) ? _dashItemSizes[dashItem.Item.Key] : GetNewRect(dashItem.Item);
			var maxWidth = P_Container.Width;

			if (small)
			{
				rect.X = 0;
				rect.Width = maxWidth;
			}
			else
			{
				rect.X = (int)Math.Ceiling(rect.X * maxWidth / 1_0000D);
				rect.Width = (int)Math.Ceiling(rect.Width * maxWidth / 1_0000D);
			}

			rect.Y = (int)Math.Ceiling(rect.Y * P_Container.Height / 1_0000D);
			rect.Height = dashItem.Item.CalculateHeight(rect.Width, g);

			dashItem.Rectangle = rect;
		}

		if (small)
		{
			var y = 0;

			foreach (var dashItem in controls.OrderBy(x => _dashItemSizes[x.Item.Key].X).ThenBy(x => _dashItemSizes[x.Item.Key].Y))
			{
				dashItem.Item.Bounds = new Rectangle(dashItem.Rectangle.X, y, dashItem.Rectangle.Width, dashItem.Rectangle.Height);

				y += dashItem.Rectangle.Height;
			}
		}
		else
		{
			bool intersected, nudged;

			do
			{
				do
				{
					nudged = false;

					foreach (var dashItem in controls.OrderBy(x => x.Rectangle.Top))
					{
						var rect = new Rectangle(dashItem.Rectangle.X, 0, dashItem.Rectangle.Width, dashItem.Rectangle.Y);

						var intersecting = controls.Where(x => x.Item != dashItem.Item && x.Rectangle.IntersectsWith(rect.Pad(3)));

						if (intersecting.Any() && intersecting.Max(x => x.Rectangle.Bottom) != dashItem.Rectangle.Y)
						{
							nudged = true;
							intersected = true;

							dashItem.Rectangle = new Rectangle(dashItem.Rectangle.X, intersecting.Max(x => x.Rectangle.Bottom), dashItem.Rectangle.Width, dashItem.Rectangle.Height);
						}
					}
				}
				while (nudged);

				do
				{
					intersected = false;

					foreach (var dashItem in controls.OrderByDescending(x => x.Item.Top))
					{
						var intersecting = controls.Where(x => x.Item != dashItem.Item && x.Rectangle.IntersectsWith(dashItem.Rectangle.Pad(3)));

						if (intersecting.Any() && intersecting.Max(x => x.Rectangle.Bottom) != dashItem.Rectangle.Y)
						{
							intersected = true;
							nudged = true;

							dashItem.Rectangle = new Rectangle(dashItem.Rectangle.X, intersecting.Max(x => x.Rectangle.Bottom), dashItem.Rectangle.Width, dashItem.Rectangle.Height);
						}
					}
				}
				while (intersected);

				foreach (var dashItem in controls.Where(x => x.Rectangle.Y < 0))
				{
					nudged = true;
					dashItem.Rectangle = new(dashItem.Rectangle.X, 0, dashItem.Rectangle.Width, dashItem.Rectangle.Height);
				}
			}
			while (intersected || nudged);

			foreach (var item in controls)
			{
				item.Item.Bounds = item.Rectangle;

				if (MoveItem is null && ResizeItem is null)
				{
					SaveDashItemBounds(item.Item);
				}
			}
		}

		P_Board.ResumeLayout(true);
		layoutInProgress = false;
	}

	private class DashItemRect
	{
		public IDashboardItem Item { get; set; }
		public Rectangle Rectangle { get; set; }

		public DashItemRect(IDashboardItem item)
		{
			Item = item;
		}
	}

	private Rectangle GetNewRect(IDashboardItem dashItem)
	{
		return _dashItemSizes[dashItem.Key] = new(0, 0, 2_000, 1_000);
	}

	private void ProfileManager_ProfileUpdated()
	{
		//this.TryInvoke(() =>
		//{
		//	TLP_Profiles.Controls.Clear(true, x => x is FavoriteProfileBubble);
		//	TLP_Profiles.RowStyles.Clear();
		//	TLP_Profiles.RowStyles.Add(new());

		//	foreach (var item in _playsetManager.Playsets.Where(x => x.IsFavorite))
		//	{
		//		TLP_Profiles.RowStyles.Add(new());
		//		TLP_Profiles.Controls.Add(new FavoriteProfileBubble(item) { Dock = DockStyle.Top }, 0, TLP_Profiles.RowStyles.Count - 1);
		//	}
		//});
	}

	private void SetButtonEnabledOnLoad()
	{
		//this.TryInvoke(() =>
		//{
		//	B_StartStop.Enabled = _citiesManager.IsAvailable();

		//	label1.Visible = _modLogicManager.AreMultipleSkyvesPresent();
		//});
	}

	protected override void LocaleChanged()
	{
		Text = Locale.Dashboard;
	}

	private void CitiesManager_MonitorTick(bool isAvailable, bool isRunning)
	{
		//this.TryInvoke(() => B_StartStop.Enabled = isAvailable);

		RefreshButtonState(isRunning);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		P_Scroll.Width = (int)(15 * UI.FontScale);

		//B_StartStop.Font = UI.Font(9.75F, FontStyle.Bold);
		//label1.Font = UI.Font(10.5F, FontStyle.Bold);
		//label1.Margin = UI.Scale(new Padding(10), UI.FontScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = GetTopBarColor();

		//label1.ForeColor = design.RedColor;
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.BackColor;//.Tint(Lum: FormDesign.Design.Type == FormDesignType.Light ? -3 : 3, Sat: FormDesign.Design.Type == FormDesignType.Light ? -25 : 0);
	}

	private void ProfileBubble_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			Form.PushPanel(ServiceCenter.Get<IInterfaceService>().PlaysetSettingsPanel());
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
		//if (!running)
		//{
		//	if (buttonStateRunning || firstTime)
		//	{
		//		this.TryInvoke(() =>
		//		{
		//			B_StartStop.ImageName = "I_CS";
		//			B_StartStop.Text = LocaleHelper.GetGlobalText("StartCities");
		//			buttonStateRunning = false;
		//		});
		//	}

		//	return;
		//}

		//if (!buttonStateRunning || firstTime)
		//{
		//	this.TryInvoke(() =>
		//	{
		//		B_StartStop.ImageName = "I_Stop";
		//		B_StartStop.Text = LocaleHelper.GetGlobalText("StopCities");
		//		buttonStateRunning = true;
		//	});
		//}
	}
}
