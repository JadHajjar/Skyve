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
			using var generic = IconManager.GetIcon(Collection ? "I_Package" : Package?.GetPackage()?.IsCodeMod ?? false ? "I_Mods" : "I_Assets", Height).Color(BackColor);
			var iconRect = ClientRectangle.CenterR(generic.Size);

			using var brush = new SolidBrush(FormDesign.Design.IconColor);
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle, (int)(10 * UI.FontScale));
		}
		else
		{
			if (Package?.IsLocal() ?? false)
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
