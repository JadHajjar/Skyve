using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
internal class D_AssetInfo : IDashboardItem
{
	protected override DrawingDelegate GetDrawingMethod(int width)
	{
		return Drawitem;
	}

	private void Drawitem(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		preferredHeight= (int)(150 * UI.FontScale);
		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.Red), e.ClipRectangle, (int)(10 * UI.FontScale));
		e.Graphics.FillRectangle(Brushes.Black, ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight));
		e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, FormDesign.Design.AccentBackColor)), new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale)));
	}
}
