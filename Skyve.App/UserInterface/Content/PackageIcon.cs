using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class PackageIcon : SlickImageControl
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IPackageIdentity? Package { get; set; }
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Collection { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var thumbnail = Package?.GetThumbnail();

		Loading = thumbnail is null && !(Package?.IsLocal() ?? false) && ConnectionHandler.IsConnected;

		if (Loading)
		{
			using var brush = new SolidBrush(FormDesign.Design.AccentBackColor);
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1), (int)(5 * UI.FontScale));

			DrawLoader(e.Graphics, ClientRectangle.CenterR(UI.Scale(new Size(32, 32), UI.UIScale)));
			return;
		}

		e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

		if (thumbnail == null)
		{
			using var generic = IconManager.GetIcon(Package is IAsset ? "Assets" : Package?.IsLocal() == true ? "Package" : "Paradox", Height * 8 / 10).Color(BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1), (int)(5 * UI.FontScale));
			e.Graphics.DrawImage(generic, ClientRectangle.CenterR(generic.Size));

			return;
		}

		if (Package?.IsLocal() ?? false)
		{
			using var unsatImg = new Bitmap(thumbnail, Size).Tint(Sat: 0);
			e.Graphics.DrawRoundedImage(unsatImg, ClientRectangle.Pad(1), (int)(5 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}
		else
		{
			e.Graphics.DrawRoundedImage(thumbnail, ClientRectangle.Pad(1), (int)(5 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}
	}
}
