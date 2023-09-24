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
			preferredHeight -= Margin.Top;
		}
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, INotificationInfo notification)
	{
		using var titleFont = UI.Font(9F, FontStyle.Bold);
		using var smallFont = UI.Font(7F);
		using var icon = IconManager.GetIcon(notification.Icon, titleFont.Height + Margin.Top);
		var maxWidth = e.ClipRectangle.Width - Margin.Horizontal - icon.Width - Margin.Left;
		var titleBounds = e.Graphics.Measure(notification.Title, titleFont, maxWidth + (Margin.Left / 2));
		var descBounds = notification.Description is null ? default : e.Graphics.Measure(notification.Description, smallFont, maxWidth);
		var rectangle = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, Math.Max(icon.Height, (int)titleBounds.Height) + (int)descBounds.Height);

		if (applyDrawing)
		{
			if (notification.HasAction && rectangle.Contains(CursorLocation))
			{
				using var backBrush = new SolidBrush(FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, 6, -6)));
				e.Graphics.FillRoundedRectangle(backBrush, rectangle, Margin.Left);

				_buttonActions[rectangle] = notification.OnClick;
			}

			e.Graphics.DrawImage(icon.Color(notification.Color ?? FormDesign.Design.ForeColor), new Rectangle(e.ClipRectangle.X + Margin.Left, preferredHeight, e.ClipRectangle.Width, Math.Max(icon.Height, (int)titleBounds.Height + (int)descBounds.Height)).Align(icon.Size, ContentAlignment.MiddleLeft));

			using var brush = new SolidBrush(notification.Color ?? FormDesign.Design.ForeColor);

			e.Graphics.DrawString(notification.Title, titleFont, brush, new Rectangle(e.ClipRectangle.X + icon.Width + Margin.Horizontal - (Margin.Left / 2), preferredHeight, maxWidth + (Margin.Left / 2), e.ClipRectangle.Height));
		}

		preferredHeight += Math.Max(icon.Height, (int)titleBounds.Height);

		if (notification.Description is not null)
		{
			if (applyDrawing)
			{
				using var brush = new SolidBrush((notification.Color ?? FormDesign.Design.ForeColor).MergeColor(BackColor, 75));

				e.Graphics.DrawString(notification.Description, smallFont, brush, new Rectangle(e.ClipRectangle.X + icon.Width + Margin.Horizontal, preferredHeight, maxWidth, e.ClipRectangle.Height));
			}

			preferredHeight += (int)descBounds.Height;
		}

		preferredHeight += Margin.Top;
	}
}
