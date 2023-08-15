using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.StatusBubbles;
internal class UserBubble : StatusBubbleBase
{
	private readonly IUserService _userService = ServiceCenter.Get<IUserService>();
	public UserBubble()
	{
		Visible = false;
		Enabled = false;
	}


	protected override void CustomDraw(PaintEventArgs e, ref int targetHeight)
	{
		var user = _userService.User;

		if (user is null)
		{
			return;
		}

		var size = UI.FontScale >= 4 ? 96 : UI.FontScale >= 2 ? 48 : 24;
		var icon = ServiceCenter.Get<IImageService>().GetImage(user.AvatarUrl, true).Result;
		var titleHeight = Math.Max(size, (int)e.Graphics.Measure(user.Name, UI.Font(9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);
		var iconRectangle = new Rectangle(Padding.Left, Padding.Top + ((titleHeight - size) / 2), size, size);

		e.Graphics.DrawRoundImage(icon, iconRectangle);

		e.Graphics.DrawString(user.Name, UI.Font(9.75F, FontStyle.Bold), new SolidBrush(FormDesign.Design.ForeColor), new Rectangle(size + (Padding.Left * 2), Padding.Top, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });

		targetHeight = titleHeight + Padding.Vertical;
	}
}
