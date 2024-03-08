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

	private void OnNewNotification()
	{
		OnResizeRequested();
	}

	protected override DrawingDelegate GetDrawingMethod(int width)
	{
		return Draw;
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle.ClipTo(sectionHeight), Locale.Notifications, "I_Notification", out _, ref preferredHeight);

		sectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += Margin.Top;

		var noNotifs = true;

		foreach (var item in _notificationsService.GetNotifications().OrderByDescending(x => x.Time))
		{
			noNotifs = false;
			Draw(e, applyDrawing, ref preferredHeight, item);

			preferredHeight += Margin.Top / 2;
		}

		if (noNotifs)
		{
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			using var brush = new SolidBrush(FormDesign.Design.InfoColor);

			e.Graphics.DrawString(Locale.NoNotifications, Font, brush, new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, (int)(25 * UI.FontScale)), format);

			preferredHeight += (int)(25 * UI.FontScale);
		}
		else
		{
			preferredHeight -= Margin.Top / 2;
		}
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, INotificationInfo notification)
	{
		using var icon = IconManager.GetIcon(notification.Icon);
		var maxWidth = e.ClipRectangle.Width - Margin.Horizontal - icon.Width - Margin.Left;
		using var titleFont = UI.Font(8.75F, FontStyle.Bold).FitToWidth(notification.Title, new Rectangle(0, 0, maxWidth, maxWidth), e.Graphics);
		using var smallFont = UI.Font(7F);
		var titleBounds = e.Graphics.Measure(notification.Title, titleFont, maxWidth + (Margin.Left / 2));
		var descBounds = notification.Description is null ? default : e.Graphics.Measure(notification.Description, smallFont, maxWidth);
		var rectangle = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, Math.Max(icon.Height, (int)titleBounds.Height) + (int)descBounds.Height).Pad(Margin.Left / 2);

		if (applyDrawing)
		{
			if (notification.HasAction && rectangle.Contains(CursorLocation))
			{
				using var backBrush = new SolidBrush(FormDesign.Design.AccentBackColor);
				e.Graphics.FillRoundedRectangle(backBrush, rectangle.Pad(0, -Margin.Top / 2, 0, -Margin.Top / 2), Margin.Left / 2);

				_buttonActions[rectangle.Pad(0, -Margin.Top / 2, 0, -Margin.Top / 2)] = notification.OnClick;
				_buttonRightClickActions[rectangle.Pad(0, -Margin.Top / 2, 0, -Margin.Top / 2)] = notification.OnRightClick;
			}

			using var activeBrush = new SolidBrush(notification.Color ?? FormDesign.Design.ActiveColor);

			e.Graphics.FillRoundedRectangle(activeBrush, rectangle.Align(new Size(Margin.Left / 2, rectangle.Height - Margin.Left / 2), ContentAlignment.MiddleLeft), Margin.Left / 4);

			e.Graphics.DrawImage(icon.Color(FormDesign.Design.ForeColor), new Rectangle(e.ClipRectangle.X + Margin.Left + Margin.Left / 2, preferredHeight, e.ClipRectangle.Width, Math.Max(icon.Height, (int)titleBounds.Height + (int)descBounds.Height)).Align(icon.Size, ContentAlignment.MiddleLeft));

			using var brush = new SolidBrush(FormDesign.Design.ForeColor);

			e.Graphics.DrawString(notification.Title, titleFont, brush, new Rectangle(e.ClipRectangle.X + icon.Width + Margin.Left / 2 + Margin.Horizontal - (Margin.Left / 2), preferredHeight, maxWidth + (Margin.Left / 2), e.ClipRectangle.Height));
		}

		if (notification.Description is not null)
		{
			preferredHeight += (int)titleBounds.Height;

			if (applyDrawing)
			{
				using var brush = new SolidBrush((FormDesign.Design.ForeColor).MergeColor(BackColor, 75));

				e.Graphics.DrawString(notification.Description, smallFont, brush, new Rectangle(e.ClipRectangle.X + Margin.Left / 2 + icon.Width + Margin.Horizontal, preferredHeight, maxWidth, e.ClipRectangle.Height));
			}

			preferredHeight += Math.Max(icon.Height * 3 / 4 - (int)titleBounds.Height, (int)descBounds.Height);
		}
		else
		{
			preferredHeight += Math.Max(icon.Height * 3 / 4, (int)titleBounds.Height);
		}

		preferredHeight += Margin.Top;
	}
}
