using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
internal class D_NotificationCenter : IDashboardItem
{
	private readonly INotificationsService _notificationsService;
	private int sectionHeight;

	public D_NotificationCenter()
	{
		ServiceCenter.Get(out _notificationsService);

		_notificationsService.OnNewNotification += OnNewNotification;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notificationsService.OnNewNotification -= OnNewNotification;
		}

		base.Dispose(disposing);
	}

	private void OnNewNotification()
	{
		OnResizeRequested();
	}

	protected override DrawingDelegate GetDrawingMethod(int width)
	{
		return Draw;
	}

	protected override void DrawHeader(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, ref preferredHeight, Locale.Notifications, "Notification");
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, ref preferredHeight, Locale.Notifications, "Notification");

		sectionHeight = preferredHeight - e.ClipRectangle.Y;

		var notifications = _notificationsService.GetNotifications().OrderByDescending(x => x.Time).ToList();

		foreach (var item in notifications)
		{
			Draw(e, applyDrawing, ref preferredHeight, item);

			preferredHeight += BorderRadius / 2;
		}

		if (notifications.Count == 0)
		{
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			using var brush = new SolidBrush(FormDesign.Design.InfoColor);

			e.Graphics.DrawString(Locale.NoNotifications, Font, brush, new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, UI.Scale(25)), format);

			preferredHeight += UI.Scale(25) + BorderRadius;
		}
		else
		{
			preferredHeight += BorderRadius / 2;

			if (notifications.Any(x => x.CanBeRead))
			{
				using var font = UI.Font(7.5F);

				DrawButton(e, applyDrawing, ref preferredHeight, MarkAllAsRead, new ButtonDrawArgs
				{
					Font = font,
					Icon = "Check",
					ButtonType = ButtonType.Dimmed,
					Size = new Size(0, UI.Scale(20)),
					Text = Locale.MarkAllAsRead,
					Rectangle = e.ClipRectangle.Pad(BorderRadius)
				});

				preferredHeight += BorderRadius / 2;
			}
		}
	}
	
	private void MarkAllAsRead()
	{
		foreach (var item in _notificationsService.GetNotifications())
		{
			if (item.CanBeRead)
			{
				item.OnRead();
			}
		}
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, INotificationInfo notification)
	{
		using var icon = IconManager.GetIcon(notification.Icon);
		var maxWidth = e.ClipRectangle.Width - Margin.Horizontal - icon.Width - Margin.Left;
		using var titleFont = UI.Font(8.75F, FontStyle.Bold).FitToWidth(notification.Title, new Rectangle(0, 0, maxWidth, maxWidth), e.Graphics);
		using var smallFont = UI.Font(7F);
		var titleBounds = e.Graphics.Measure(notification.Title, titleFont, maxWidth + (BorderRadius / 2));
		var descBounds = notification.Description is null ? default : e.Graphics.Measure(notification.Description, smallFont, maxWidth);
		var rectangle = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, Math.Max(icon.Height, (int)titleBounds.Height + (int)descBounds.Height) + BorderRadius).Pad(BorderRadius / 2);

		if (applyDrawing)
		{
			if (notification.HasAction && rectangle.Contains(CursorLocation))
			{
				using var backBrush = new SolidBrush((HoverState.HasFlag(HoverState.Pressed) ? BackColor : FormDesign.Design.AccentBackColor).Tint(Lum: FormDesign.Design.IsDarkTheme ? 3 : -3));
				e.Graphics.FillRoundedRectangle(backBrush, rectangle.Pad(0, -Margin.Top / 2, 0, -Margin.Top / 2), Margin.Left / 2);

				_buttonActions[rectangle.Pad(0, -Margin.Top / 2, 0, -Margin.Top / 2)] = notification.OnClick;
				_buttonRightClickActions[rectangle.Pad(0, -Margin.Top / 2, 0, -Margin.Top / 2)] = notification.OnRightClick;
			}

			using var activeBrush = Gradient(notification.Color ?? FormDesign.Design.ActiveColor, 3f);

			e.Graphics.FillRoundedRectangle(activeBrush, rectangle.Align(new Size(Margin.Left / 2, rectangle.Height - (Margin.Left / 2)), ContentAlignment.MiddleLeft), Margin.Left / 4);

			e.Graphics.DrawImage(icon.Color(FormDesign.Design.ForeColor), rectangle.Pad(Margin.Left, -Margin.Top / 2, 0, -Margin.Top / 2).Align(icon.Size, ContentAlignment.MiddleLeft));

			using var brush = new SolidBrush(FormDesign.Design.ForeColor);

			e.Graphics.DrawString(notification.Title, titleFont, brush, new Rectangle(e.ClipRectangle.X + icon.Width + (Margin.Left / 2) + Margin.Horizontal - (Margin.Left / 2), preferredHeight, maxWidth + (Margin.Left / 2), e.ClipRectangle.Height));
		}

		if (notification.Description is not null)
		{
			if (applyDrawing)
			{
				using var brush = new SolidBrush(FormDesign.Design.ForeColor.MergeColor(BackColor, 75));
				using var format = new StringFormat { LineAlignment = string.IsNullOrEmpty(notification.Title) ? StringAlignment.Center : StringAlignment.Near };

				e.Graphics.DrawString(notification.Description, smallFont, brush, new Rectangle(e.ClipRectangle.X + (Margin.Left / 2) + icon.Width + Margin.Horizontal, rectangle.Y + (int)titleBounds.Height, maxWidth, rectangle.Height), format);
			}
		}

		preferredHeight += rectangle.Height + BorderRadius;
	}
}
