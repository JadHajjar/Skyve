using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
internal class ItemCountControl : Control
{
	public ItemCountControl()
	{
		ResizeRedraw = DoubleBuffered = true;
	}

	[DefaultValue(null)]
	public string? LeftText { get; set; }
	[DefaultValue(null)]
	public string? RightText { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		using var font = UI.Font(7.5F, FontStyle.Bold);
		using var brush = new SolidBrush(FormDesign.Design.InfoColor);

		var padding = UI.Scale(new Padding(5), UI.FontScale);
		var leftWidth = (int)e.Graphics.Measure(LeftText, font).Width;
		var rightWidth = (int)e.Graphics.Measure(RightText, font).Width;

		var leftRect = ClientRectangle.Align(new Size(Width * leftWidth / (leftWidth + rightWidth), Height), ContentAlignment.MiddleLeft).Pad(padding);
		var rightRect = ClientRectangle.Align(new Size(Width * rightWidth / (leftWidth + rightWidth), Height), ContentAlignment.MiddleRight).Pad(padding);

		using var leftFont = UI.Font(7.5F, FontStyle.Bold).FitTo(LeftText, leftRect, e.Graphics);
		using var rightFont = UI.Font(7.5F, FontStyle.Bold).FitTo(RightText, rightRect, e.Graphics);

		using var leftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
		using var rightFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(LeftText, leftFont, brush, leftRect, leftFormat);
		e.Graphics.DrawString(RightText, rightFont, brush, rightRect, rightFormat);
	}
}
