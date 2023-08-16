using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class LogTraceControl : SlickListControl<ILogTrace>
{
	private ILogTrace? itemHovered;

	public LogTraceControl(IEnumerable<ILogTrace> logs)
	{
		Dock = DockStyle.Top;
		CalculateItemSize += LogTraceControl_CalculateItemSize;
		AddRange(logs);
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(6), UI.FontScale);
		Margin = UI.Scale(new Padding(3), UI.FontScale);
	}

	private void LogTraceControl_CalculateItemSize(object sender, SizeSourceEventArgs<ILogTrace> e)
	{
		var rect = ClientRectangle.Pad(Padding);
		var y = rect.Top;

		using var smallFont = UI.Font(7.5F);

		var timeStampSize = e.Graphics.Measure(e.Item.Timestamp, smallFont);

		timeStampSize.Width += Padding.Left;
		timeStampSize.Height += Padding.Left;

		using var titleFont = UI.Font(8.25F, FontStyle.Bold);

		y += Padding.Top + (int)e.Graphics.Measure(e.Item.Title, e.Item.Trace.Count == 0 ? smallFont : titleFont, rect.Width - (int)timeStampSize.Width * 2 - Padding.Right).Height;

		foreach (var item in e.Item.Trace)
		{
			var trace = item.StartsWith("at");

			y += (int)e.Graphics.Measure(item, smallFont, Width - (trace ? 2 : 1) * Padding.Horizontal).Height;
		}

		y += Padding.Bottom;

		e.Size = y;
		e.Handled = true;
	}

	protected override void OnItemMouseClick(DrawableItem<ILogTrace> item, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && itemHovered == item.Item)
		{
			Clipboard.SetText($"{item.Item.Title}\r\n{item.Item.Trace.ListStrings("\r\n")}");
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		itemHovered = null;
		base.OnPaint(e);

		Cursor = itemHovered is not null ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnPaintItem(ItemPaintEventArgs<ILogTrace> e)
	{
		var rect = e.ClipRectangle.Pad(Padding);
		var y = rect.Top;

		using var smallFont = UI.Font(7.5F);

		var timeStampSize = e.Graphics.Measure(e.Item.Timestamp, smallFont);

		timeStampSize.Width += Padding.Left;
		timeStampSize.Height += Padding.Left;

		var buttonRect = rect.Align(new Size((int)timeStampSize.Height, (int)timeStampSize.Height), ContentAlignment.TopRight);

		if (buttonRect.Contains(CursorLocation))
		{
			itemHovered = e.Item;
		}

		SlickButton.DrawButton(e, buttonRect, null, Font, IconManager.GetIcon("I_Copy"), null, buttonRect.Contains(CursorLocation) ? HoverState : HoverState.Normal);

		using var yellowBrush = new SolidBrush(FormDesign.Design.MenuColor);
		using var activeBrush = new SolidBrush(FormDesign.Design.MenuForeColor);
		e.Graphics.FillRoundedRectangle(yellowBrush, rect.Pad(0, 0, (int)timeStampSize.Height + Padding.Right, 0).Align(timeStampSize.ToSize(), ContentAlignment.TopRight), Padding.Left);
		e.Graphics.DrawString(e.Item.Timestamp, smallFont, activeBrush, rect.Pad(0, 0, (int)timeStampSize.Height + Padding.Right, 0).Align(timeStampSize.ToSize(), ContentAlignment.TopRight), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

		using var titleFont = UI.Font(8.25F, FontStyle.Bold);
		using var titleBrush = new SolidBrush(ForeColor);
		e.Graphics.DrawString(e.Item.Title, titleFont, titleBrush, rect.Pad(0, 0, (int)timeStampSize.Width * 2 + Padding.Right, 0));

		y += Padding.Top + (int)e.Graphics.Measure(e.Item.Title, e.Item.Trace.Count == 0 ? smallFont : titleFont, rect.Width - (int)timeStampSize.Width * 2 - Padding.Right).Height;

		using var textBrush = new SolidBrush(Color.FromArgb(215, ForeColor));
		foreach (var item in e.Item.Trace)
		{
			var trace = item.StartsWith("at");

			e.Graphics.DrawString(item, smallFont, trace ? textBrush : titleBrush, new Rectangle(Padding.Left + (trace ? Padding.Horizontal : 0), y, Width - (trace ? 2 : 1) * Padding.Horizontal, Height));

			y += (int)e.Graphics.Measure(item, smallFont, Width - (trace ? 2 : 1) * Padding.Horizontal).Height;
		}

		y += Padding.Bottom;

		using var pen = new Pen(FormDesign.Design.AccentColor, (float)(2 * UI.FontScale));
		e.Graphics.DrawLine(pen, rect.Left, y - pen.Width, rect.Right, y - pen.Width);
	}
}
