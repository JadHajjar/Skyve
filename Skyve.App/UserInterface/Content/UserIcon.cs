using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;

public class UserIcon : SlickImageControl
{
	[Category("Appearance"), DefaultValue(true)]
	public bool HalfColor { get; set; } = true;

	protected override void OnPaint(PaintEventArgs e)
	{
		if (HalfColor)
		{
			e.Graphics.Clear(FormDesign.Design.BackColor);

			e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), new Rectangle(0, 0, Width, Height / 2));
		}
		else
		{
			e.Graphics.Clear(BackColor);
		}

		if (Loading)
		{
			DrawLoader(e.Graphics, ClientRectangle.CenterR(UI.Scale(new Size(32, 32), UI.UIScale)));
			return;
		}

		e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

		if (Image == null)
		{
			using var image = IconManager.GetIcon("I_User", ClientRectangle.Height).Color(FormDesign.Design.AccentColor.GetTextColor());

			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentColor), ClientRectangle.Pad(1), (int)(10 * UI.FontScale));

			e.Graphics.DrawImage(image, ClientRectangle.CenterR(image.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(Image, ClientRectangle, (int)(10 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}
	}
}
