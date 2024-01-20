using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class LinkControl : SlickImageControl
{
	public LinkControl(ILink link, bool display)
	{
		Link = link;
		Display = display;
	}

	public ILink Link { get; set; }
	public bool Display { get; set; }

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			AutoSize = true;
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, Link.Url);
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (Display && e.Button == MouseButtons.Left)
		{
			PlatformUtil.OpenUrl(Link.Url);
		}
		else if (Display && e.Button == MouseButtons.Right)
		{
			SlickToolStrip.Show(Program.MainForm, PointToScreen(e.Location), new SlickStripItem(LocaleSlickUI.Copy, "I_Copy", action: () => Clipboard.SetText(Link.Url)));
		}
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		using var img = Link.Type.GetIcon().Default;
		return Size.Ceiling(FontMeasuring.Measure(Link.Title.IfEmpty(LocaleCR.Get(Link.Type.ToString())), Font)) + new Size(Padding.Horizontal + img.Width, Padding.Vertical);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		SlickButton.GetColors(out var fore, out var back, HoverState, !Display && ImageName is null ? ColorStyle.Red : ColorStyle.Active);

		using var brush = new SolidBrush(back);
		using var foreBrush = new SolidBrush(fore);

		e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1), Padding.Left);

		using (var img = (HoverState.HasFlag(HoverState.Hovered) ? Display ? "I_Link" : "I_Edit" : Link.Type.GetIcon()).Default)
		{
			e.Graphics.DrawImage(img.Color(FormDesign.Design.ButtonForeColor), ClientRectangle.Pad(Padding).Align(img.Size, ContentAlignment.MiddleLeft));

			e.Graphics.DrawString(Link.Title.IfEmpty(LocaleCR.Get(Link.Type.ToString())), Font, foreBrush, ClientRectangle.Pad(Padding.Horizontal + img.Width, 0, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		DrawFocus(e.Graphics, ClientRectangle.Pad(1), Padding.Left, !Display && ImageName is null ? FormDesign.Design.RedColor : null);
	}
}
