using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
internal class D_Playsets : IDashboardItem
{
	private readonly IPlaysetManager _playsetManager;

	public D_Playsets()
	{
		ServiceCenter.Get(out  _playsetManager);
	}

	protected override DrawingDelegate GetDrawingMethod(int width)
	{
		return Draw;
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle, Locale.CurrentPlayset, _playsetManager.CurrentPlayset.GetIcon(), out var fore, ref preferredHeight, _playsetManager.CurrentPlayset.Color);
	}
}
