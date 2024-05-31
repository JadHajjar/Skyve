using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class AuthorControl : SlickControl
{
	private readonly IUserService _userService;
	private readonly IWorkshopService _workshopService;
	private readonly INotifier _notifier;

	public IUser? Author { get; set; }

	public AuthorControl()
	{
		Cursor = Cursors.Hand;

		ServiceCenter.Get(out _userService, out _notifier, out _workshopService);

		_notifier.WorkshopUsersInfoLoaded += Notifier_WorkshopUsersInfoLoaded;
	}

	private void Notifier_WorkshopUsersInfoLoaded()
	{
		Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		_notifier.WorkshopUsersInfoLoaded -= Notifier_WorkshopUsersInfoLoaded;

		base.Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (Author is null)
		{
			Height = 1;
			return;
		}

		e.Graphics.SetUp(BackColor);

		var user = _workshopService.GetUser(Author);
		var thumbnail = (user ?? Author).GetThumbnail();
		var args = new ButtonDrawArgs
		{
			Text = user?.Name ?? Author.Name,
			Icon = "Author",
			AvailableSize = GetAvailableSize(),
			HoverState = HoverState,
			BorderRadius = (int)(5 * UI.FontScale),
			Padding = UI.Scale(new Padding(thumbnail is not null ? 7 : 4, 4, 4, 4), UI.FontScale),
			Cursor = PointToClient(Cursor.Position),
		};

		SlickButton.Draw(e.Graphics, args);

		if (thumbnail is not null)
		{
			e.Graphics.DrawRoundImage(thumbnail, args.IconRectangle.Pad((int)(-3 * UI.FontScale)));
		}

		if (_userService.IsUserVerified(Author))
		{
			var checkRect = args.IconRectangle.Align(new Size(args.IconRectangle.Height / 3, args.IconRectangle.Height / 3), ContentAlignment.BottomRight);

			e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect.Pad(-(int)(2 * UI.FontScale)));

			using var img = IconManager.GetIcon("Check", checkRect.Height);
			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}

		Size = args.Rectangle.Size;
	}
}
