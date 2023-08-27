using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
internal class D_LaunchGame : IDashboardItem
{
	public override int CalculateHeight(int width, PaintEventArgs e)
	{
		return (int)(50 * UI.FontScale);
	}

	public override void DrawItem(PaintEventArgs e)
	{
		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.Blue), e.ClipRectangle, (int)(10 * UI.FontScale));
		e.Graphics.FillRectangle(Brushes.Black, ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight));
		e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, FormDesign.Design.AccentBackColor)), new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale)));
	}
}

internal class D_LaunchGame1 : IDashboardItem
{
	public override int CalculateHeight(int width, PaintEventArgs e)
	{
		return (int)(100 * UI.FontScale);
	}

	public override void DrawItem(PaintEventArgs e)
	{
		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.Orange), e.ClipRectangle, (int)(10 * UI.FontScale));
		e.Graphics.FillRectangle(Brushes.Black, ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight));
		e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, FormDesign.Design.AccentBackColor)), new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale)));
	}
}

internal class D_LaunchGame2 : IDashboardItem
{
	public override int CalculateHeight(int width, PaintEventArgs e)
	{
		return (int)(150 * UI.FontScale);
	}

	public override void DrawItem(PaintEventArgs e)
	{
		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.Purple), e.ClipRectangle, (int)(10 * UI.FontScale));
		e.Graphics.FillRectangle(Brushes.Black, ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight));
		e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, FormDesign.Design.AccentBackColor)), new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale)));
	}
}
