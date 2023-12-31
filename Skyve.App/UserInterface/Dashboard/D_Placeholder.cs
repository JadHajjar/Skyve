﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
internal class D_Placeholder : SlickControl
{
	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var padding = (int)(12 * UI.FontScale);
		var border = (int)(10 * UI.FontScale);
		var color = FormDesign.Design.ForeColor;

		using var brush = new SolidBrush(Color.FromArgb(25, color));
		e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad((int)(1.5 * UI.FontScale) + padding), border);

		using var pen = new Pen(Color.FromArgb(100, color), (float)(1.5 * UI.FontScale)) { DashStyle = DashStyle.Dash };
		e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad((int)(1.5 * UI.FontScale) + padding), border);
	}
}
