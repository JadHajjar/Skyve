using Skyve.App.UserInterface.Dashboard;

using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_MainPage : PanelContent
{
	private readonly Control _control = new();
	private readonly Dictionary<string, Rectangle> _dashItemSizes;
	private readonly ISettings _settings;
	private Point CursorStart;
	private Rectangle initialRect;
	private bool layoutInProgress;
	private IDashboardItem? MoveItem;
	private IDashboardItem? ResizeItem;
	private Rectangle savedRect;

	public PC_MainPage()
	{
		ServiceCenter.Get(out _settings);

		InitializeComponent();

		TLP_FirstTime.Visible = !_settings.SessionSettings.DashboardFirstTimeShown;

		ServiceCenter.Get<SaveHandler>().Load(out _dashItemSizes, "DashboardLayout.json");
		_dashItemSizes ??= GetDefaultLayout();

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

	protected override void LocaleChanged()
	{
		Text = Locale.Dashboard;
		L_Info.Text = Locale.DashboardCustomizationInfo;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		Form.Deactivate += Form_Deactivate;
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

		P_Scroll.Width = (int)(15 * UI.FontScale);

		TLP_FirstTime.Padding = UI.Scale(new Padding(12, 12, 12, 0), UI.FontScale);
	}

	private void B_Dismiss_Click(object sender, EventArgs e)
	{
		TLP_FirstTime.Hide();
		_settings.SessionSettings.DashboardFirstTimeShown = true;
		_settings.SessionSettings.Save();
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
			initialRect = savedRect = control.Bounds;
			control.ResizeInProgress = true;
			control.BringToFront();
			P_Board.Invalidate();
			P_Container.Invalidate();
		}
		else if (control.MoveAreaContains(e.Location))
		{
			MoveItem = control;
			CursorStart = control.PointToClient(Cursor.Position);
			initialRect = savedRect = control.Bounds;
			control.MoveInProgress = true;
			control.BringToFront();
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
		this.TryInvoke(() => P_Board_Layout(sender, null));
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

	private Dictionary<string, Rectangle> GetDefaultLayout()
	{
		return new()
		{
		  { "D_AssetsInfo", new Rectangle(2500, 100, 2500, 100) },
		  { "D_CompatibilityInfo", new Rectangle(0, 0, 5000, 100) },
		  { "D_ModsInfo", new Rectangle(0, 100, 2500, 100) },
		  { "D_Playsets", new Rectangle(5250, 0, 2250, 100) },
		  { "D_LaunchGame", new Rectangle(7750, 0, 2250, 100) },
		  { "D_NotificationCenter", new Rectangle(7750, 100, 2250, 100) },
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

	private Rectangle GetNewRect(IDashboardItem dashItem)
	{
		return _dashItemSizes[dashItem.Key] = new(dashItem.Parent.Controls.GetChildIndex(dashItem) % 3 * 3_333, 0, 3_333, 1_000);
	}

	private Rectangle GetSnappingRect(IDashboardItem control, bool addPadding = true, Rectangle? savedRect = null)
	{
		var rect = savedRect ?? control.Bounds.InvertPad(control.Padding);

		rect.Width = Math.Max(rect.Width, control.MinimumWidth * P_Container.Width / 6 / 100);

		var margin = (int)(32 * UI.FontScale);
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

			P_Board.Controls.Add(control);
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

			SaveLayout();
		}

		P_Board.Size = P_Board.GetPreferredSize(P_Board.Size);
		P_Board.ResumeLayout(false);
		layoutInProgress = false;
	}

	private void P_Container_Paint(object sender, PaintEventArgs e)
	{
		if (!_settings.UserSettings.SnapDashToGrid || (MoveItem == null && ResizeItem == null))
		{
			return;
		}

		using var pen = new Pen(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor), 5F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };

		foreach (var x in GetGridSnapping())
		{
			if (x > 0)
			{
				e.Graphics.DrawLine(pen, x - 1, e.Graphics.VisibleClipBounds.Y, x - 1, e.Graphics.VisibleClipBounds.Height);
			}
		}
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

	private void SaveLayout()
	{
		ServiceCenter.Get<SaveHandler>().Save(_dashItemSizes, "DashboardLayout.json");
	}
	private class DashItemRect
	{
		public DashItemRect(IDashboardItem item)
		{
			Item = item;
		}

		public IDashboardItem Item { get; set; }
		public Rectangle Rectangle { get; set; }
	}
}
