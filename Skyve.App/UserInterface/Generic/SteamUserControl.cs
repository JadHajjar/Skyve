using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
internal class SteamUserControl : SlickControl
{
	private readonly IUserService _userService = ServiceCenter.Get<IUserService>();
	private readonly IWorkshopService _workshopService = ServiceCenter.Get<IWorkshopService>();
	private readonly ulong _steamId;

	public SteamUserControl()
	{
		Visible = false;
		Enabled = false;
		Cursor = Cursors.Hand;
	}

	public SteamUserControl(ulong steamId)
	{
		Visible = false;
		Enabled = false;
		Cursor = Cursors.Hand;
		_steamId = steamId;
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(3), UI.FontScale);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left && User is not null)
		{
			PlatformUtil.OpenUrl(User.ProfileUrl);
		}
	}

	public IUser? User => _steamId == 0 ? _userService.User : _workshopService.GetUser(_steamId);
	public string? InfoText { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		if (User is null)
		{
			return;
		}

		e.Graphics.SetUp(BackColor);

		e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle, Padding.Horizontal);

		var size = Height - Padding.Vertical * 2;
		var image = ServiceCenter.Get<IImageService>().GetImage(User.AvatarUrl, true).Result;
		var textRectangle = ClientRectangle.Pad(size + Padding.Left * 2, 0, 0, 0);
		var avatarRect = ClientRectangle.Pad(Padding).Align(new Size(size, size), ContentAlignment.MiddleLeft);
		var infoSize = e.Graphics.Measure(InfoText.IfEmpty(Locale.LoggedInAs), UI.Font(6.75F, FontStyle.Bold));
		var nameSize = e.Graphics.Measure(User.Name, UI.Font(9F, FontStyle.Bold));

		if (image is not null)
		{
			e.Graphics.DrawRoundImage(image, avatarRect);
		}
		else
		{
			using var generic = Properties.Resources.I_GenericUser.Color(FormDesign.Design.IconColor);

			e.Graphics.DrawRoundImage(generic, avatarRect);
		}

		e.Graphics.DrawString(InfoText.IfEmpty(Locale.LoggedInAs), UI.Font(6.75F, FontStyle.Bold), new SolidBrush(FormDesign.Design.InfoColor), textRectangle);
		e.Graphics.DrawString(User.Name, UI.Font(9F, FontStyle.Bold), new SolidBrush(FormDesign.Design.ForeColor), textRectangle, new StringFormat { LineAlignment = StringAlignment.Far });

		var width = (int)Math.Max(infoSize.Width, nameSize.Width) + Padding.Left + Padding.Horizontal + size;
		var height = (int)(infoSize.Height + nameSize.Height);

		if (width != Width && Dock == DockStyle.None || height != Height)
		{
			Size = new(width, height);
		}

		if (ServiceCenter.Get<ICompatibilityManager>().IsUserVerified(User))
		{
			var checkRect = avatarRect.Align(new Size(avatarRect.Height / 3, avatarRect.Height / 3), ContentAlignment.BottomRight);

			e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect.Pad(-(int)(2 * UI.FontScale)));

			using var img = IconManager.GetIcon("I_Check", checkRect.Height);
			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}
	}
}
