using Skyve.App.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class LinkControl : SlickImageControl
{
	public LinkControl(ILink link, bool display)
	{
		Link = link;
		Display = display;
		Cursor = Cursors.Hand;
		SlickTip.SetTo(this, Link.Url);
	}

	public ILink Link { get; set; }
	public bool Display { get; set; }

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(5), UI.FontScale);
		Margin = UI.Scale(new Padding(3, 3, 6, 6), UI.FontScale);
		Size = UI.Scale(new Size(55, 55), UI.FontScale);
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
			SlickToolStrip.Show(Program.MainForm, PointToScreen(e.Location), new SlickStripItem(LocaleSlickUI.Copy, "Copy", action: () => Clipboard.SetText(Link.Url)));
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var client = ClientRectangle.Pad(1);
		var active = !Display && ImageName is null ? FormDesign.Design.RedColor : FormDesign.Design.ActiveColor;
		var backColor = HoverState.HasFlag(HoverState.Pressed) ? active
				: HoverState.HasFlag(HoverState.Hovered) ? BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 10 : -8)
				: BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? -6 : 6);
		var activeColor = backColor.GetTextColor();

		using var activeBrush = new SolidBrush(activeColor);

		using var backBrush = new SolidBrush(backColor);
		e.Graphics.FillRoundedRectangle(backBrush, client, Padding.Left);

		DrawFocus(e.Graphics, client, Padding.Left, active);

		var text = Link.Title.IfEmpty(LocaleCR.Get(Link.Type.ToString())).ToUpper();
		using var font = UI.Font(7F, FontStyle.Bold).FitToWidth(text, client.Pad(Padding), e.Graphics);
		var textHeight = (int)e.Graphics.Measure(text, font).Height;
		using var img = (HoverState.HasFlag(HoverState.Hovered) ? Display ? "Link" : "Edit" : Link.Type.GetIcon()).Get(Height / 2)?.Color(activeColor);

		if (img == null)
		{
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(text, font, activeBrush, client, format);
		}
		else
		{
			var rect = client.CenterR(client.Width, textHeight + img.Height + (int)(2.5 * UI.FontScale));

			e.Graphics.DrawImage(img, rect.Align(img.Size, ContentAlignment.TopCenter));

			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
			e.Graphics.DrawString(text, font, activeBrush, rect, format);
		}
	}
}
