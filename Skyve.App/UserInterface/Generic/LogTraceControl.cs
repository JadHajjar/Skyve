using Skyve.App.Utilities;

using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class LogTraceControl : SlickListControl<ILogTrace>
{
	private ILogTrace? copyHovered;
	private ILogTrace? openHovered;
	private ILogTrace? folderHovered;

	public LogTraceControl()
	{
		CalculateItemSize += LogTraceControl_CalculateItemSize;
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(7), UI.FontScale);
		Margin = UI.Scale(new Padding(3), UI.FontScale);
	}

	private void LogTraceControl_CalculateItemSize(object sender, SizeSourceEventArgs<ILogTrace> e)
	{
		var rect = ClientRectangle.Pad(Padding);
		var y = rect.Top;

		using var smallFont = UI.Font("Consolas", 7F);
		using var font = UI.Font("Consolas", 8F);

		if (!string.IsNullOrWhiteSpace(e.Item.Type))
		{
			rect.Y += (int)(20 * UI.FontScale);
			y += (int)(20 * UI.FontScale);
		}

		y += (Padding.Top / 2) + (int)e.Graphics.Measure(e.Item.Title, font, rect.Width - Padding.Right).Height;

		foreach (var item in e.Item.Trace)
		{
			y += (int)e.Graphics.Measure(item, smallFont, Width - (2 * Padding.Horizontal)).Height + (int)(3 * UI.FontScale);
		}

		y += Padding.Bottom * 3 / 2;

		e.Size = y;
		e.Handled = true;
	}

	protected override void OnItemMouseClick(DrawableItem<ILogTrace> item, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && copyHovered == item.Item)
		{
			Clipboard.SetText(copyHovered.ToString());
		}

		if (e.Button == MouseButtons.Left && openHovered == item.Item)
		{
			ServiceCenter.Get<IIOUtil>().Execute(openHovered.SourceFile, string.Empty);
		}

		if (e.Button == MouseButtons.Left && folderHovered == item.Item)
		{
			PlatformUtil.OpenFolder(folderHovered.SourceFile);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		copyHovered = openHovered = folderHovered = null;
		base.OnPaint(e);

		Cursor = copyHovered is not null || openHovered is not null || folderHovered is not null ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnPaintItem(ItemPaintEventArgs<ILogTrace> e)
	{
		var rect = e.ClipRectangle.Pad(Padding);
		var y = rect.Top;

		using var smallFont = UI.Font("Consolas", 7F);

		var buttonRect = e.ClipRectangle.Pad(0, 0, Padding.Right, 0).Align(UI.Scale(new Size(20, 20), UI.FontScale), ContentAlignment.TopRight);
		var linkRect = new Rectangle(buttonRect.X - buttonRect.Width - Padding.Right, buttonRect.Y, buttonRect.Width, buttonRect.Height);
		var folderRect = new Rectangle(linkRect.X - linkRect.Width - Padding.Right, linkRect.Y, linkRect.Width, linkRect.Height);

		if (buttonRect.Contains(CursorLocation))
		{
			copyHovered = e.Item;
		}

		if (linkRect.Contains(CursorLocation))
		{
			openHovered = e.Item;
		}

		if (folderRect.Contains(CursorLocation))
		{
			folderHovered = e.Item;
		}

		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			Rectangle = buttonRect,
			Icon = "I_Copy",
			Font = Font,
			HoverState = HoverState,
			Cursor = CursorLocation
		});

		using var font = UI.Font("Consolas", 8F);
		using var titleBrush = new SolidBrush(ForeColor);
		using var textBrush = new SolidBrush(Color.FromArgb(175, ForeColor));

		if (!string.IsNullOrWhiteSpace(e.Item.Type))
		{
			var rect2 = rect;
			using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.ForeColor));
			using var titleFont = UI.Font("Consolas", 8.25F, FontStyle.Bold);
			using var typeBrush = new SolidBrush(e.Item.Type switch { "INFO" => ForeColor, "WARN" => FormDesign.Design.YellowColor, _ => FormDesign.Design.RedColor });

			e.Graphics.DrawString($"[", titleFont, textBrush, rect2);
			rect2.X += (int)e.Graphics.Measure($"[", titleFont).Width;

			e.Graphics.DrawString(e.Item.Type, titleFont, typeBrush, rect2);
			rect2.X += (int)e.Graphics.Measure(e.Item.Type, titleFont).Width;

			e.Graphics.DrawString("] - [", titleFont, textBrush, rect2);
			rect2.X += (int)e.Graphics.Measure($"] - [", titleFont).Width;

			e.Graphics.DrawString($"{e.Item.Timestamp:HH:mm:ss,fff}", titleFont, activeBrush, rect2);
			rect2.X += (int)e.Graphics.Measure($"{e.Item.Timestamp:HH:mm:ss,fff}", titleFont).Width;

			e.Graphics.DrawString($"] - (", titleFont, textBrush, rect2);
			rect2.X += (int)e.Graphics.Measure($"] - (", titleFont).Width;

			e.Graphics.DrawString(Path.GetFileName(e.Item.SourceFile), titleFont, titleBrush, rect2);
			rect2.X += (int)e.Graphics.Measure(Path.GetFileName(e.Item.SourceFile), titleFont).Width;

			e.Graphics.DrawString($")", titleFont, textBrush, rect2);
			rect2.X += (int)e.Graphics.Measure($")", titleFont).Width;

			rect.Y += (int)(20 * UI.FontScale);
			y += (int)(20 * UI.FontScale);

			SlickButton.Draw(e.Graphics, new ButtonDrawArgs
			{
				Rectangle = linkRect,
				Icon = "I_Link",
				Font = Font,
				HoverState = HoverState,
				Cursor = CursorLocation
			});

			SlickButton.Draw(e.Graphics, new ButtonDrawArgs
			{
				Rectangle = folderRect,
				Icon = "I_Folder",
				Font = Font,
				HoverState = HoverState,
				Cursor = CursorLocation
			});
		}

		e.Graphics.DrawString(e.Item.Title, font, titleBrush, rect.Pad(0, 0, Padding.Right, 0));

		y += (Padding.Top / 2) + (int)e.Graphics.Measure(e.Item.Title, font, rect.Width - Padding.Right).Height;

		foreach (var item in e.Item.Trace)
		{
			e.Graphics.DrawString(item, smallFont, textBrush, new Rectangle(Padding.Left + Padding.Horizontal, y, Width - (2 * Padding.Horizontal), Height));

			y += (int)e.Graphics.Measure(item, smallFont, Width - (2 * Padding.Horizontal)).Height + (int)(3 * UI.FontScale);
		}

		y += Padding.Bottom;

		using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1 * UI.FontScale));
		e.Graphics.DrawLine(pen, rect.Left, y - pen.Width, rect.Right, y - pen.Width);
	}
}
