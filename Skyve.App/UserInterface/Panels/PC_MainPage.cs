using Skyve.App.UserInterface.Dashboard;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_MainPage : PanelContent
{
	private readonly ISettings _settings;
	private readonly SaveHandler _saveHandler;
	private readonly Control _control = new();
	private Dictionary<string, DashboardSetting> _dashItemSizes;
	private Point CursorStart;
	private Rectangle initialRect;
	private bool layoutInProgress;
	private IDashboardItem? MoveItem;
	private IDashboardItem? ResizeItem;
	private Rectangle savedRect;
	private Rectangle editRect;
	private Rectangle cancelRect;
	private Rectangle resetRect;

	public struct DashboardSetting
	{
		public Rectangle Rectangle { get; set; }
		public bool Hidden { get; set; }

		public DashboardSetting()
		{
		}

		public DashboardSetting(Rectangle rectangle, bool hidden)
		{
			Rectangle = rectangle;
			Hidden = hidden;
		}
	}

	public PC_MainPage()
	{
		ServiceCenter.Get(out _settings, out _saveHandler);

		InitializeComponent();

		_saveHandler.Load(out _dashItemSizes, "DashboardLayout.json");

		if (_dashItemSizes is null || _dashItemSizes.Count == 0)
		{
			_settings.SessionSettings.DashboardFirstTimeShown = false;
			_dashItemSizes = GetDefaultLayout();
		}

		P_Board.SuspendLayout();

		var assembly_base = Assembly.GetExecutingAssembly();
		var assembly = Assembly.GetEntryAssembly();

		LoadItems(assembly_base.GetTypes().Where(type => typeof(IDashboardItem).IsAssignableFrom(type) && !type.IsAbstract));
		LoadItems(assembly.GetTypes().Where(type => typeof(IDashboardItem).IsAssignableFrom(type) && !type.IsAbstract));
	}

	protected override void GlobalMouseMove(Point p)
	{
		if (MoveItem is not null)
		{
			p.Offset(-CursorStart.X, -CursorStart.Y);

			if (P_AvailableWidgets.ClientRectangle.Pad((int)(-12 * UI.FontScale)).Contains(P_AvailableWidgets.PointToClient(p)))
			{
				if (!MoveItem.DrawHeaderOnly)
				{
					MoveItem.DrawHeaderOnly = true;
					MoveItem.Parent = FLP_AvailableWidgets;
				}
			}
			else
			{
				if (MoveItem.DrawHeaderOnly)
				{
					MoveItem.DrawHeaderOnly = false;
					MoveItem.Parent = P_Board;
				}
			}

			savedRect.Location = MoveItem.Parent.PointToClient(p);

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

	protected override void LocaleChanged()
	{
		Text = Locale.Dashboard;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		Form.Deactivate += Form_Deactivate;
		Form.WindowStateChanged += Form_WindowStateChanged;
	}

	private void Form_WindowStateChanged(object sender, EventArgs e)
	{
		Form.OnNextIdle(P_Container.PerformLayout);
	}

	protected override void OnShown()
	{
		base.OnShown();

		P_Container.PerformLayout();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape && (MoveItem is not null || ResizeItem is not null))
		{
			Form_Deactivate(this, EventArgs.Empty);
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		P_Scroll.Width = UI.Scale(15);
		P_Board.Padding = new Padding(0, 0, 0, UI.Scale(48));
		P_AvailableWidgets.Padding = UI.Scale(new Padding(6));
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		FLP_AvailableWidgets.BackColor = design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 4 : -3);
	}

	public override Color GetTopBarColor()
	{
		return FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 2 : -5);
	}

	private void DashMouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		var control = (IDashboardItem)sender;

		if (control.MovementBlocked)
		{
			return;
		}

		if (!control.DrawHeaderOnly && control.ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight).Contains(e.Location))
		{
			ResizeItem = control;
			CursorStart = new(Cursor.Position.X, control.Width);
			initialRect = savedRect = control.Bounds;
			control.ResizeInProgress = true;
			if (!control.DrawHeaderOnly)
			{
				control.BringToFront();
			}

			P_Board.Invalidate();
			P_Container.Invalidate();
		}
		else if (control.MoveAreaContains(e.Location))
		{
			MoveItem = control;
			CursorStart = control.PointToClient(Cursor.Position);
			initialRect = savedRect = control.Bounds;
			control.MoveInProgress = true;
			if (!control.DrawHeaderOnly)
			{
				control.BringToFront();
			}

			P_Board.Invalidate();
			P_Container.Invalidate();
		}
	}

	private void DashMouseUp(object sender, MouseEventArgs e)
	{
		var dashItem = (IDashboardItem)sender;

		if (MoveItem is not null)
		{
			SaveDashItemBounds(MoveItem, GetSnappingRect(MoveItem, false, savedRect));
			MoveItem = null;

			if (!_settings.SessionSettings.DashboardFirstTimeShown)
			{
				_settings.SessionSettings.DashboardFirstTimeShown = true;
				_settings.SessionSettings.Save();
			}
		}

		if (ResizeItem is not null)
		{
			SaveDashItemBounds(ResizeItem, GetSnappingRect(ResizeItem, false, savedRect));
			ResizeItem = null;

			if (!_settings.SessionSettings.DashboardFirstTimeShown)
			{
				_settings.SessionSettings.DashboardFirstTimeShown = true;
				_settings.SessionSettings.Save();
			}
		}

		dashItem.MoveInProgress = false;
		dashItem.ResizeInProgress = false;

		P_Board.Invalidate();
		P_Container.Invalidate();
		P_Container.PerformLayout();
	}

	private void DashResizeRequested(object sender, EventArgs e)
	{
		this.TryInvoke(DoDashboardLayout);
	}

	private void Form_Deactivate(object sender, EventArgs e)
	{
		if (MoveItem is not null)
		{
			MoveItem.MoveInProgress = false;
			SaveDashItemBounds(MoveItem, initialRect);
			MoveItem = null;
		}
		else if (ResizeItem is not null)
		{
			ResizeItem.ResizeInProgress = false;
			SaveDashItemBounds(ResizeItem, initialRect);
			ResizeItem = null;
		}
		else
		{
			return;
		}

		P_Board.Invalidate(true);
		P_Container.Invalidate();
		P_Container.PerformLayout();
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

	private IEnumerable<int> GetGridSnapping()
	{
		if (!_settings.UserSettings.SnapDashToGrid)
		{
			yield break;
		}

		for (var i = 0; i < 12; i++)
		{
			yield return P_Container.Width * i / 12;
		}
	}

	private DashboardSetting GetNewRect(IDashboardItem dashItem)
	{
		return _dashItemSizes[dashItem.Key] = new(new(dashItem.Parent.Controls.GetChildIndex(dashItem) % 3 * 3_333, 0, 3_333, 1_000), true);
	}

	private Rectangle GetSnappingRect(IDashboardItem control, bool addPadding = true, Rectangle? savedRect = null)
	{
		var rect = savedRect ?? control.Bounds.InvertPad(control.Padding);

		rect.Width = Math.Max(rect.Width, control.MinimumWidth * P_Container.Width / 6 / 100);

		if (ModifierKeys.HasFlag(Keys.Shift))
		{
			rect.X -= rect.X % (P_Container.Width / 100);

			if (ResizeItem is not null)
				rect.Width -= rect.Width % (P_Container.Width / 100);
		}

		if (!ModifierKeys.HasFlag(Keys.Alt))
		{
			var margin = UI.Scale(32);
			var closestX = P_Board.Controls
				.Where(x => x != control && x is IDashboardItem)
				.Select(x => x.Left)
					.Concat(GetGridSnapping())
				.OrderBy(number => Math.Abs(number - rect.Left))
				.FirstOrDefault();

			if (Math.Abs(closestX - rect.Left) < margin)
			{
				rect.X = closestX;

				if (MoveItem is not null && rect.Right > P_Container.Width)
				{
					rect.X = Math.Min(rect.X, P_Container.Width - UI.Scale(32));
					rect.Width = P_Container.Width - rect.X;
				}
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

			closestX = P_Board.Controls
				.Where(x => x != control && x is IDashboardItem)
				.Select(x => x.Left)
					.Concat(GetGridSnapping())
				.OrderBy(number => Math.Abs(number - rect.Right - control.Padding.Horizontal))
				.FirstOrDefault();

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
		}
		else
		{
			rect.X = Math.Min(rect.X, P_Container.Width - UI.Scale(32));

			if (MoveItem is not null && rect.Right > P_Container.Width)
			{
				rect.Width = P_Container.Width - rect.X;
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

	private void LoadItems(IEnumerable<Type> types)
	{
		foreach (var type in types)
		{
			var control = (IDashboardItem)Activator.CreateInstance(type);

			control.Size = default;
			control.MouseDown += DashMouseDown;
			control.MouseUp += DashMouseUp;
			control.ResizeRequested += DashResizeRequested;
			control.ParentChanged += DashParentChanged;

			if (_dashItemSizes.TryGetValue(control.Key, out var settings) && !settings.Hidden)
			{
				control.MovementBlocked = true;
				P_Board.Controls.Add(control);
			}
			else
			{
				control.DrawHeaderOnly = true;
				FLP_AvailableWidgets.Controls.Add(control);
			}
		}
	}

	private void DashParentChanged(object sender, EventArgs e)
	{
		if (sender is IDashboardItem dash)
		{
			dash.Margin = dash.Parent == FLP_AvailableWidgets ? default : UI.Scale(new Padding(8));
		}
	}

	private void P_Board_Layout(object sender, LayoutEventArgs e)
	{
		DoDashboardLayout();
	}

	private void DoDashboardLayout()
	{
		if (layoutInProgress || !Live)
		{
			return;
		}

		layoutInProgress = true;
		using var g = _control.CreateGraphics();

		P_Board.SuspendLayout();

		var small = P_Container.Width < UI.Scale(500);
		var controls = P_Board.Controls.OfType<IDashboardItem>().ToList(x => new DashItemRect(x));

		foreach (var dashItem in controls)
		{
			var rect = (_dashItemSizes.ContainsKey(dashItem.Item.Key) ? _dashItemSizes[dashItem.Item.Key] : GetNewRect(dashItem.Item)).Rectangle;
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

			foreach (var dashItem in controls.OrderBy(x => _dashItemSizes[x.Item.Key].Rectangle.X).ThenBy(x => _dashItemSizes[x.Item.Key].Rectangle.Y))
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

		P_Board.Size = P_Board.GetPreferredSize(P_Board.Size);
		P_Board.ResumeLayout(false);
		layoutInProgress = false;
	}

	private void P_Container_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.SetUp();

		if (_settings.UserSettings.SnapDashToGrid && P_AvailableWidgets.Visible)
		{
			using var pen = new Pen(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor), 5F) { DashStyle = DashStyle.Dash, DashCap = DashCap.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };

			foreach (var x in GetGridSnapping())
			{
				if (x > 0)
				{
					e.Graphics.DrawLine(pen, x - 1, 0, x - 1, ((SlickControl)sender).Height);
				}
			}
		}

		if (P_Container == sender || P_Container.Height < P_Board.Height + P_Board.Padding.Bottom)
		{
			var control = (SlickControl)sender;
			var rectangle = (P_Container == sender || P_Container.Height > P_Board.Height ? P_Container : P_Board).ClientRectangle.Pad(0, 0, 0, !P_AvailableWidgets.Visible ? UI.Scale(8) : 0);

			rectangle = rectangle.Pad(0, rectangle.Bottom - P_Board.Padding.Bottom * 3 / 4, 0, 0);

			editRect = SlickButton.AlignAndDraw(e.Graphics, rectangle, _settings.SessionSettings.DashboardFirstTimeShown ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
			{
				Text = P_AvailableWidgets.Visible ? "SaveChanges" : "EditDashboard",
				Icon = P_AvailableWidgets.Visible ? "Ok" : "Edit",
				ButtonType = P_AvailableWidgets.Visible ? ButtonType.Active : ButtonType.Dimmed,
				BackgroundColor = P_AvailableWidgets.Visible ? default : BackColor,
				HoverState = control.HoverState & ~HoverState.Focused,
				Cursor = control.PointToClient(Cursor.Position)
			});

			if (P_AvailableWidgets.Visible)
			{
				cancelRect = SlickButton.AlignAndDraw(e.Graphics, rectangle.Pad(0, 0, editRect.Width + P_AvailableWidgets.Padding.Horizontal, 0), _settings.SessionSettings.DashboardFirstTimeShown ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
				{
					Text = "Cancel",
					Icon = "Cancel",
					ButtonType = ButtonType.Dimmed,
					BackgroundColor = BackColor,
					HoverState = control.HoverState & ~HoverState.Focused,
					Cursor = control.PointToClient(Cursor.Position)
				});

				resetRect = SlickButton.AlignAndDraw(e.Graphics, rectangle.Pad(P_AvailableWidgets.Padding.Horizontal, 0, 0, 0), _settings.SessionSettings.DashboardFirstTimeShown ? ContentAlignment.MiddleLeft : ContentAlignment.BottomLeft, new ButtonDrawArgs
				{
					Text = "ResetLayout",
					Icon = "Undo",
					ButtonType = ButtonType.Dimmed,
					ColorStyle = ColorStyle.Red,
					BackgroundColor = BackColor,
					HoverState = control.HoverState & ~HoverState.Focused,
					Cursor = control.PointToClient(Cursor.Position)
				});
			}
			else
			{
				resetRect = cancelRect = default;
			}

			if (!_settings.SessionSettings.DashboardFirstTimeShown)
			{
				var textRect = rectangle.Pad(resetRect.Width + UI.Scale(16), 0, rectangle.Right - (cancelRect == default ? editRect.X : cancelRect.X) + UI.Scale(8), 0);
				using var font = UI.Font(8.5F, FontStyle.Italic).FitTo(Locale.DashboardCustomizationInfo, textRect, e.Graphics);
				using var brush = new SolidBrush(FormDesign.Design.InfoColor);
				using var format = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

				e.Graphics.DrawString(Locale.DashboardCustomizationInfo, font, brush, textRect, format);
			}
		}
	}

	private void SaveDashItemBounds(IDashboardItem item, Rectangle? rectangle = null)
	{
		if (item.DrawHeaderOnly)
		{
			item.Size = new Size(UI.Scale(200), rectangle?.Height ?? UI.Scale(32));
			_dashItemSizes[item.Key] = new DashboardSetting(new(default, item.Size), true);
			return;
		}

		var size = P_Container.Size;
		var bounds = rectangle ?? item.Bounds;

		var rect = new Rectangle(
			bounds.X * 1_0000 / size.Width,
			bounds.Y * 1_0000 / size.Height,
			bounds.Width * 1_0000 / size.Width,
			bounds.Height * 1_0000 / size.Height);

		if (rectangle is null && _dashItemSizes.ContainsKey(item.Key))
		{
			rect.X = _dashItemSizes[item.Key].Rectangle.X;
			rect.Width = _dashItemSizes[item.Key].Rectangle.Width;
		}

		_dashItemSizes[item.Key] = new(rect, false);
	}

	private void SaveLayout()
	{
		_saveHandler.Save(_dashItemSizes, "DashboardLayout.json");
		_settings.SessionSettings.DashboardFirstTimeShown = true;
		_settings.SessionSettings.Save();
	}

	private void P_Board_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && editRect.Contains(e.Location))
		{
			P_AvailableWidgets.Visible = !P_AvailableWidgets.Visible;

			foreach (IDashboardItem control in P_Board.Controls)
			{
				control.MovementBlocked = !P_AvailableWidgets.Visible;
				control.Invalidate();
			}

			foreach (IDashboardItem control in FLP_AvailableWidgets.Controls)
			{
				SaveDashItemBounds(control, GetSnappingRect(control, false, savedRect));
				control.Invalidate();
			}

			SaveLayout();
		}

		if (e.Button == MouseButtons.Left && cancelRect.Contains(e.Location))
		{
			_saveHandler.Load(out _dashItemSizes, "DashboardLayout.json");

			var controls = P_Board.Controls.Cast<IDashboardItem>().Concat(FLP_AvailableWidgets.Controls.Cast<IDashboardItem>()).ToList();

			foreach (var control in controls)
			{
				control.DrawHeaderOnly = !(_dashItemSizes.TryGetValue(control.Key, out var settings) && !settings.Hidden);
				control.Parent = control.DrawHeaderOnly ? FLP_AvailableWidgets : P_Board;

				if (control.DrawHeaderOnly)
				{
					SaveDashItemBounds(control, GetSnappingRect(control, false, savedRect));
				}
			}

			DoDashboardLayout();

			P_AvailableWidgets.Visible = !P_AvailableWidgets.Visible;

			foreach (IDashboardItem control in P_Board.Controls)
			{
				control.MovementBlocked = !P_AvailableWidgets.Visible;
				control.Invalidate();
			}

			foreach (IDashboardItem control in FLP_AvailableWidgets.Controls)
			{
				SaveDashItemBounds(control, GetSnappingRect(control, false, savedRect));
				control.Invalidate();
			}
		}

		if (e.Button == MouseButtons.Left && resetRect.Contains(e.Location))
		{
			_dashItemSizes = GetDefaultLayout();

			var controls = P_Board.Controls.Cast<IDashboardItem>().Concat(FLP_AvailableWidgets.Controls.Cast<IDashboardItem>()).ToList();

			foreach (var control in controls)
			{
				control.DrawHeaderOnly = !(_dashItemSizes.TryGetValue(control.Key, out var settings) && !settings.Hidden);
				control.Parent = control.DrawHeaderOnly ? FLP_AvailableWidgets : P_Board;

				if (control.DrawHeaderOnly)
				{
					SaveDashItemBounds(control, GetSnappingRect(control, false, savedRect));
				}
			}

			DoDashboardLayout();
		}
	}

	private void P_Board_MouseMove(object sender, MouseEventArgs e)
	{
		P_Board.Cursor = P_Container.Cursor = editRect.Contains(e.Location) || cancelRect.Contains(e.Location) || resetRect.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
	}

	private void P_Container_Resize(object sender, EventArgs e)
	{
		P_Board.Invalidate();
	}

	private class DashItemRect(IDashboardItem item)
	{
		public IDashboardItem Item { get; set; } = item;
		public Rectangle Rectangle { get; set; }
	}
}
