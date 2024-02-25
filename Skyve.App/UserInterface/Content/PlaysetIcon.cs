using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class PlaysetIcon : SlickImageControl
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IPlayset? Playset { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		if (Playset is null)
		{
			return;
		}

		var customPlayset = Playset.GetCustomPlayset();
		var banner = customPlayset.GetThumbnail();

		if (banner is null)
		{
			using var brush = new SolidBrush(customPlayset.Color ?? FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, ClientRectangle, (int)(5 * UI.FontScale));

			using var icon = customPlayset.Usage.GetIcon().Get(ClientRectangle.Width * 3 / 4).Color(brush.Color.GetTextColor());

			e.Graphics.DrawImage(icon, ClientRectangle.CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(banner, ClientRectangle, (int)(5 * UI.FontScale));
		}
	}
}
