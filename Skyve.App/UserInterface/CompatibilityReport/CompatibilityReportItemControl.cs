using Skyve.App.UserInterface.Content;
using Skyve.App.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.CompatibilityReport;
public class CompatibilityReportItemControl : SlickControl
{
	private readonly IUserService _userService;
	private readonly IWorkshopService _workshopService;
	private readonly INotifier _notifier;
	private readonly ReviewRequest _request;
	private Rectangle UserRectangle;
	private Rectangle TextRectangle;
	private Rectangle ViewRectangle;

	private readonly Action<ReviewRequest, MouseEventArgs>? ItemMouseClick;

	public CompatibilityReportItemControl(ReviewRequest request, Action<ReviewRequest, MouseEventArgs>? onMouseClick)
	{
		ServiceCenter.Get(out _userService, out _notifier, out _workshopService);

		_request = request;

		_notifier.WorkshopUsersInfoLoaded += _notifier_WorkshopUsersInfoLoaded;

		ItemMouseClick = onMouseClick;
	}

	private void _notifier_WorkshopUsersInfoLoaded()
	{
		Invalidate();
	}

	protected override void UIChanged()
	{
		Margin = UI.Scale(new Padding(5));
		Padding = UI.Scale(new Padding(8));
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (TextRectangle.Contains(e.Location))
		{
			Clipboard.SetText(_request.PackageNote);
		}
		else if (UserRectangle.Contains(e.Location))
		{
			var user = _userService.TryGetUser(_request.UserId);

			if (user != null)
			{
				PlatformUtil.OpenUrl(user.ProfileUrl);
			}
		}
		else if (ViewRectangle.Contains(e.Location) || e.Button == MouseButtons.None)
		{
			ItemMouseClick?.Invoke(_request, e);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		Cursor = TextRectangle.Contains(e.Location) || UserRectangle.Contains(e.Location) || ViewRectangle.Contains(e.Location)
			? Cursors.Hand
			: Cursors.Default;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notifier.WorkshopUsersInfoLoaded -= _notifier_WorkshopUsersInfoLoaded;
		}

		base.Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		using var backBrush = new SolidBrush(FormDesign.Design.AccentBackColor);
		e.Graphics.FillRoundedRectangle(backBrush, ClientRectangle, Margin.Left);

		var mainRect = ClientRectangle.Pad(Padding);
		var user = _userService.TryGetUser(_request.UserId);
		var avatar = _workshopService.GetUser(user).GetThumbnail();
		var cursorLocation = PointToClient(Cursor.Position);

		using var font = UI.Font(10F);
		using var brush = new SolidBrush(UserIcon.GetUserColor(user.Id?.ToString() ?? string.Empty, true));
		using var icon = IconManager.GetIcon("User", font.Height * 5 / 4).Color(brush.Color);

		var nameSize = e.Graphics.Measure(user.Name, font);
		var nameHeight = Math.Max(icon.Height, (int)nameSize.Height);
		UserRectangle = new Rectangle(mainRect.X, mainRect.Y, mainRect.Width, nameHeight);

		if (avatar is null)
		{
			e.Graphics.DrawImage(icon, UserRectangle.Align(new Size(nameHeight, nameHeight), ContentAlignment.MiddleLeft).CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundImage(avatar, UserRectangle.Align(new Size(nameHeight, nameHeight), ContentAlignment.MiddleLeft).CenterR(icon.Size));
		}

		e.Graphics.DrawString(user.Name, font, brush, UserRectangle.Pad(nameHeight + Padding.Left, 0, 0, 0));

		using var smallFont = UI.Font(7.5F);
		using var timeBrush = new SolidBrush(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.ActiveColor));
		e.Graphics.DrawString(_request.Timestamp.ToLocalTime().ToRelatedString(true, false), smallFont, timeBrush, UserRectangle, new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });

		UserRectangle.Width = nameHeight + Padding.Horizontal + (int)nameSize.Width;

		if (UserRectangle.Contains(cursorLocation))
		{
			using var hoverBrush = new SolidBrush(Color.FromArgb(40, FormDesign.Design.ActiveColor));
			e.Graphics.FillRoundedRectangle(hoverBrush, UserRectangle.InvertPad(Margin), Margin.Left);
		}

		TextRectangle = SlickButton.AlignAndDraw(e.Graphics, mainRect.Pad(0, UserRectangle.Height + Padding.Top, 0, 0), ContentAlignment.TopRight, new ButtonDrawArgs
		{
			BackgroundColor = backBrush.Color,
			HoverState = HoverState & ~HoverState.Focused,
			Cursor = cursorLocation,
			Icon = "Copy"
		}).Rectangle;

		using var textFont = UI.Font(8.25F);
		var noteRect = Rectangle.FromLTRB(mainRect.X, TextRectangle.Y, TextRectangle.X, TextRectangle.Y);
		var noteSize = e.Graphics.Measure(_request.PackageNote, textFont, noteRect.Width);

		noteRect.Height = (int)noteSize.Height;

		using var fadedBrush = new SolidBrush(Color.FromArgb(200, FormDesign.Design.ForeColor));
		e.Graphics.DrawString(_request.PackageNote, textFont, fadedBrush, noteRect);

		ViewRectangle = new Rectangle(mainRect.X, noteRect.Bottom + Padding.Vertical, mainRect.Width, UI.Scale(28));

		using var buttonFont = UI.Font(9.5F);
		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			BackgroundColor = backBrush.Color,
			Font = buttonFont,
			HoverState = HoverState,
			Rectangle = ViewRectangle,
			Cursor = cursorLocation,
			Text = LocaleCR.ViewRequest,
			Icon = "Link"
		});

		Height = ViewRectangle.Bottom + Padding.Bottom;
	}
}
