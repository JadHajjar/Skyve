using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;

internal class D_CompatibilityInfo : IDashboardItem
{
	protected override DrawingDelegate GetDrawingMethod(int width)
	{
		return Drawitem;
	}

	private void Drawitem(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		preferredHeight= (int)(300 * UI.FontScale);

		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.Green), e.ClipRectangle, (int)(10 * UI.FontScale));
		e.Graphics.FillRectangle(Brushes.Black, ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight));
		e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, FormDesign.Design.AccentBackColor)), new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale)));
	}
}
