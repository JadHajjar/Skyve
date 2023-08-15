using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Content;
internal class PackageIcon : SlickImageControl
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IPackage? Package { get; set; }
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Collection { get; set; }
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
			using var image = (Collection ? Properties.Resources.I_CollectionIcon : Package?.IsMod ?? false ? Properties.Resources.I_ModIcon : Properties.Resources.I_AssetIcon).Color(FormDesign.Design.IconColor);
			var iconRect = ClientRectangle.CenterR(image.Size);

			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.IconColor), ClientRectangle, (int)(10 * UI.FontScale));
			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.BackColor), iconRect.Pad(1), (int)(10 * UI.FontScale));

			e.Graphics.DrawRoundedImage(image, iconRect, (int)(10 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}
		else
		{
			if (Package?.IsLocal ?? false)
			{
				using var unsatImg = new Bitmap(Image, Size).Tint(Sat: 0);
				e.Graphics.DrawRoundedImage(unsatImg, ClientRectangle, (int)(10 * UI.FontScale), FormDesign.Design.AccentBackColor);
			}
			else
			{
				e.Graphics.DrawRoundedImage(Image, ClientRectangle, (int)(10 * UI.FontScale), FormDesign.Design.AccentBackColor);
			}
		}
	}
}
