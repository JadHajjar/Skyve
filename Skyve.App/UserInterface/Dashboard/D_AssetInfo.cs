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
	}
}
