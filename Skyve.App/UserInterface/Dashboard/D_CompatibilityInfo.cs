using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;

internal class D_CompatibilityInfo : IDashboardItem
{
	public override int CalculateHeight(int width, PaintEventArgs e)
	{
		return (int)(300 * UI.FontScale);
	}

	public override void DrawItem(PaintEventArgs e)
	{
		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.Green), e.ClipRectangle, (int)(10 * UI.FontScale));
		e.Graphics.FillRectangle(Brushes.Black, ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight));
		e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, FormDesign.Design.AccentBackColor)), new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale)));
	}
}
