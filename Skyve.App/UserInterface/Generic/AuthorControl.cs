using Skyve.App.UserInterface.Content;

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
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (Live)
		{
			_notifier.WorkshopUsersInfoLoaded += Notifier_WorkshopUsersInfoLoaded;
		}
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
		var authorColor = UserIcon.GetUserColor(Author.Id?.ToString() ?? string.Empty, true);
		var args = new ButtonDrawArgs
		{
			Text = user?.Name ?? Author.Name,
			Icon = "Author",
			AvailableSize = GetAvailableSize(),
			HoverState = base.HoverState & ~HoverState.Focused,
			BorderRadius = UI.Scale(5),
			Padding = UI.Scale(new Padding(thumbnail is not null ? 7 : 4, 3, 4, 3)),
			Cursor = PointToClient(Cursor.Position),
			ButtonType = ButtonType.Hidden,
			ActiveColor = authorColor.MergeColor(base.BackColor, 75),
			ForeColor = authorColor.MergeColor(base.ForeColor, 75)
		};

		SlickButton.Draw(e.Graphics, args);

		if (!HoverState.HasFlag(HoverState.Pressed))
		{
			DrawFocus(e.Graphics, args.Rectangle, args.BorderRadius.Value, authorColor);
		}

		if (thumbnail is not null)
		{
			e.Graphics.DrawRoundImage(thumbnail, args.IconRectangle.Pad((int)(-3 * UI.FontScale)));
		}

		if (_userService.IsUserVerified(Author))
		{
			var checkRect = args.IconRectangle.Align(new Size(args.IconRectangle.Height / 3, args.IconRectangle.Height / 3), ContentAlignment.BottomRight);

			e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect.Pad(-UI.Scale(2)));

			using var img = IconManager.GetIcon("Check", checkRect.Height);
			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}

		Size = args.Rectangle.Size;
	}
}
