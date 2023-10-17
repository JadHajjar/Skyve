using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class DownloadsInfoControl : SlickControl
{
	private readonly Timer refreshTimer;
	private Rectangle buttonRect;
	private Rectangle cancelRect;

	private readonly ISubscriptionsManager _subscriptionsManager;

	public DownloadsInfoControl()
	{
		_subscriptionsManager = ServiceCenter.Get<ISubscriptionsManager>();

		Visible = false;
		refreshTimer = new Timer() { Interval = 1000 };
		refreshTimer.Tick += RefreshTimer_Tick;
		refreshTimer.Enabled = true;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (Live)
		{
			refreshTimer.Start();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);


		if (cancelRect.Contains(e.Location))
		{
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, "CancelDownloads");
		}
		else if (buttonRect.Contains(e.Location))
		{
			var c = (_subscriptionsManager.PendingSubscribingTo.Count > 0 ? 1 : 0) + (_subscriptionsManager.PendingUnsubscribingFrom.Count > 0 ? 2 : 0);
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, c switch { 3 => "ApplyAllActions", 2 => "ApplyRemoval", _ => "ApplyDownloads" });
		}
		else
		{
			Cursor = Cursors.Default;
			SlickTip.SetTo(this, "DownloadsPendingInfo");
		}
	}

	private void RefreshTimer_Tick(object sender, EventArgs e)
	{
		if (Visible != (!_subscriptionsManager.SubscriptionsPending && (_subscriptionsManager.PendingSubscribingTo.Any() || _subscriptionsManager.PendingUnsubscribingFrom.Any())))
		{
			Visible = !Visible;
			Loading = false;
		}

		if (Visible)
		{
			Invalidate();
		}
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(5), UI.FontScale);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!Live)
		{
			return;
		}

		e.Graphics.SetUp(BackColor);

		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(100, FormDesign.Design.YellowColor)), ClientRectangle, Padding.Left);

		var y = Padding.Top;

		if (_subscriptionsManager.PendingSubscribingTo.Count > 0)
		{
			e.Graphics.DrawStringItem(Locale.PendingDownloads.FormatPlural(_subscriptionsManager.PendingSubscribingTo.Count), Font, FormDesign.Design.MenuForeColor, Width - Padding.Horizontal, 0, ref y);
		}

		if (_subscriptionsManager.PendingUnsubscribingFrom.Count > 0)
		{
			e.Graphics.DrawStringItem(Locale.PendingDeletions.FormatPlural(_subscriptionsManager.PendingUnsubscribingFrom.Count), Font, FormDesign.Design.MenuForeColor, Width - Padding.Horizontal, 0, ref y);
		}

		var c = (_subscriptionsManager.PendingSubscribingTo.Count > 0 ? 1 : 0) + (_subscriptionsManager.PendingUnsubscribingFrom.Count > 0 ? 2 : 0);

		using var buttonIcon = IconManager.GetSmallIcon(c switch { 3 => "I_AppIcon", 2 => "I_Disposable", _ => "I_Install" });
		using var font = UI.Font(6.75F);
		var buttonSize = SlickButton.GetSize(e.Graphics, buttonIcon, c switch { 3 => LocaleSlickUI.Apply, 2 => LocaleSlickUI.Remove, _ => LocaleSlickUI.Download }, font, new(4, 2, 2, 2));
		buttonRect = ClientRectangle.Pad(Padding).Align(buttonSize, ContentAlignment.BottomRight);

		SlickButton.Draw(e, new ButtonDrawArgs
		{
			Rectangle = buttonRect,
			Text = c switch { 3 => LocaleSlickUI.Apply, 2 => LocaleSlickUI.Remove, _ => LocaleSlickUI.Download },
			Font = font,
			Image = buttonIcon,
			Padding = new Padding(4, 2, 2, 2),
			HoverState = buttonRect.Contains(PointToClient(Cursor.Position)) ? base.HoverState & ~HoverState.Focused : HoverState.Normal,
			ColorStyle = ColorStyle.Yellow,
			Control = this
		});

		using var cancelButtonIcon = IconManager.GetSmallIcon("I_Cancel");
		buttonSize = SlickButton.GetSize(e.Graphics, cancelButtonIcon, LocaleSlickUI.Cancel, font, new(4, 2, 2, 2));
		cancelRect = ClientRectangle.Pad(Padding).Align(buttonSize, ContentAlignment.BottomLeft);

		SlickLabel.DrawLabel(e, cancelRect, LocaleSlickUI.Cancel, font, cancelButtonIcon, new Padding(4, 2, 2, 2), FormDesign.Design.MenuForeColor, cancelRect.Contains(PointToClient(Cursor.Position)) ? base.HoverState & ~HoverState.Focused : HoverState.Normal, ColorStyle.Red);

		Height = y + buttonSize.Height + Padding.Bottom;
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.None || (e.Button == MouseButtons.Left && buttonRect.Contains(e.Location)))
		{
			Loading = true;

			if (_subscriptionsManager.PendingSubscribingTo.Count > 0)
			{
				ServiceCenter.Get<IDownloadService>().Download(_subscriptionsManager.PendingSubscribingTo.Select(x => (IPackageIdentity)new GenericPackageIdentity(x)));
			}

			if (_subscriptionsManager.PendingUnsubscribingFrom.Count > 0)
			{
				ServiceCenter.Get<IPackageManager>().DeleteAll(_subscriptionsManager.PendingUnsubscribingFrom);
			}
		}
		else if (e.Button == MouseButtons.Left && cancelRect.Contains(e.Location))
		{
			_subscriptionsManager.PendingSubscribingTo.Clear();
			_subscriptionsManager.PendingUnsubscribingFrom.Clear();
			Hide();
		}
	}
}
