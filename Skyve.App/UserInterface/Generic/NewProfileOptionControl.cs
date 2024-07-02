using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class NewProfileOptionControl : SlickImageControl
{
	public NewProfileOptionControl()
	{
		Cursor = Cursors.Hand;
	}

	public bool FromScratch { get; set; }
	public bool FromLink { get; set; }

	protected override void UIChanged()
	{
		if (Live)
		{
			Font = UI.Font(11.25F, FontStyle.Bold);
			Margin = UI.Scale(new Padding(100 - (Parent.Controls.IndexOf(this) % 2 * 100), 15, Parent.Controls.IndexOf(this) % 2 * 100, 15));
			Padding = UI.Scale(new Padding(15));
			Size = UI.Scale(new Size(250, 75));
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{ }

	protected sealed override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		SlickButton.GetColors(out var fore, out var back, HoverState, FromScratch ? ColorStyle.Green : ColorStyle.Active);

		if (!HoverState.HasFlag(HoverState.Pressed) && !FormDesign.Design.IsDarkTheme)
		{
			back = back.Tint(Lum: 1.5F);
		}

		e.Graphics.FillRoundedRectangle(ClientRectangle.Gradient(back, 0.8F), ClientRectangle, Padding.Left);

		using var icon = IconManager.GetIcon(FromLink ? "LinkChain" : FromScratch ? "New" : "Copy", 48);

		if (Loading)
		{
			DrawLoader(e.Graphics, ClientRectangle.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleLeft));
		}
		else
		{
			e.Graphics.DrawImage(icon.Color(fore), ClientRectangle.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleLeft));
		}

		e.Graphics.DrawString(FromLink ? Locale.ImportFromLink : FromScratch ? Locale.StartScratch : Locale.ContinueFromCurrent, Font, new SolidBrush(fore), ClientRectangle.Pad(Padding).Pad(Padding.Left + icon.Width, 0, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center });
	}
}
