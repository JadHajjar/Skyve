﻿using Skyve.App.Utilities;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class LogTraceControl : SlickStackedListControl<ILogTrace, LogTraceControl.Rectangles>
{
	public bool OrderAsc { get; set; }
	public string LogLevel { get; set; } = string.Empty;
	public string SearchText { get; set; } = string.Empty;

	public LogTraceControl()
	{
		SeparateWithLines = true;
		DynamicSizing = true;
		ItemHeight = 75;
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(7, 10, 7, 5));

		base.UIChanged();
	}

	protected override IEnumerable<IDrawableItem<ILogTrace>> OrderItems(IEnumerable<IDrawableItem<ILogTrace>> items)
	{
		return OrderAsc
			? items.OrderBy(x => x.Item.Timestamp)
			: items.OrderByDescending(x => x.Item.Timestamp);
	}

	protected override void CanDrawItemInternal(CanDrawItemEventArgs<ILogTrace> args)
	{
		if (!Compare(args.Item.Type, LogLevel))
		{
			args.DoNotDraw = true;
			base.CanDrawItemInternal(args);
			return;
		}

		if (!string.IsNullOrWhiteSpace(SearchText) && !SearchLog(args.Item))
		{
			args.DoNotDraw = true;
			base.CanDrawItemInternal(args);
			return;
		}

		base.CanDrawItemInternal(args);
	}

	private bool Compare(string type, string level)
	{
		if (type == level)
		{
			return true;
		}

		return level switch
		{
			"ERROR" => type is "FATAL" or "CRITICAL" or "EMERGENCY" or "EXCEPTION",
			"WARN"  => type is "FATAL" or "CRITICAL" or "EMERGENCY" or "EXCEPTION" or "ERROR",
			"DEBUG" => type is "FATAL" or "CRITICAL" or "EMERGENCY" or "EXCEPTION" or "ERROR" or "WARN",
			"INFO"  => type is "FATAL" or "CRITICAL" or "EMERGENCY" or "EXCEPTION" or "ERROR" or "WARN" or "DEBUG",
			"TRACE" => type is "FATAL" or "CRITICAL" or "EMERGENCY" or "EXCEPTION" or "ERROR" or "WARN" or "DEBUG" or "INFO",
			_ => true,
		};
	}

	private bool SearchLog(ILogTrace trace)
	{
		if (CheckText(trace.Type)
			|| CheckText(Path.GetFileName(trace.SourceFile))
			|| CheckText(trace.Title))
		{
			return true;
		}

		for (var i = 0; i < trace.Trace.Count; i++)
		{
			if (CheckText(trace.Trace[i]))
			{
				return true;
			}
		}

		return false;
	}

	private bool CheckText(in string text)
	{
		return text.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
	}

	protected override void OnItemMouseClick(DrawableItem<ILogTrace, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		if (item.Rectangles.ButtonRect.Contains(e.Location))
		{
			Clipboard.SetText(item.Item.ToString());
		}

		if (item.Rectangles.LinkRect.Contains(e.Location))
		{
			ServiceCenter.Get<IIOUtil>().Execute(item.Item.SourceFile, string.Empty);
		}

		if (item.Rectangles.FolderRect.Contains(e.Location))
		{
			PlatformUtil.OpenFolder(item.Item.SourceFile);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		if (ItemCount == 0)
		{
			using var font = UI.Font(9.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.InfoColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };

			e.Graphics.DrawString(Locale.DefaultLogViewInfo, font, brush, ClientRectangle.Pad(Math.Min(Height, Width) / 3), format);
		}
		else if (FilteredCount == 0)
		{
			using var font = UI.Font(9.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.InfoColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };

			e.Graphics.DrawString(Locale.NoLogsMatchFilters, font, brush, ClientRectangle.Pad(Math.Min(Height, Width) / 3), format);
		}
	}

	protected override IDrawableItemRectangles<ILogTrace> GenerateRectangles(ILogTrace item, Rectangle rectangle, IDrawableItemRectangles<ILogTrace> current)
	{
		var rects = new Rectangles(item)
		{
			ButtonRect = rectangle.Pad(0, 0, Padding.Right, 0).Align(UI.Scale(new Size(20, 20)), ContentAlignment.TopRight)
		};
		rects.LinkRect = new Rectangle(rects.ButtonRect.X - rects.ButtonRect.Width - Padding.Right, rects.ButtonRect.Y, rects.ButtonRect.Width, rects.ButtonRect.Height);
		rects.FolderRect = new Rectangle(rects.LinkRect.X - rects.LinkRect.Width - Padding.Right, rects.LinkRect.Y, rects.LinkRect.Width, rects.LinkRect.Height);

		return rects;
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<ILogTrace, Rectangles> e)
	{
		base.OnPaintItemList(e);

		var y = e.ClipRectangle.Y;

		using var smallFont = UI.Font("Consolas", 7F);

		SlickButton.Draw(e.Graphics, new ButtonDrawArgs
		{
			Rectangle = e.Rects.ButtonRect,
			Icon = "Copy",
			Font = Font,
			HoverState = HoverState,
			Cursor = CursorLocation
		});

		using var font = UI.Font("Consolas", 8F);
		using var titleBrush = new SolidBrush(ForeColor);
		using var textBrush = new SolidBrush(Color.FromArgb(175, ForeColor));

		if (!string.IsNullOrWhiteSpace(e.Item.Type))
		{
			var rect2 = e.ClipRectangle;
			using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.ForeColor));
			using var titleFont = UI.Font("Consolas", 8.25F, FontStyle.Bold);
			using var typeBrush = new SolidBrush(GetColor(e.Item.Type));

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

			y += UI.Scale(20);

			SlickButton.Draw(e.Graphics, new ButtonDrawArgs
			{
				Rectangle = e.Rects.LinkRect,
				Icon = "Link",
				Font = Font,
				HoverState = HoverState,
				Cursor = CursorLocation
			});

			SlickButton.Draw(e.Graphics, new ButtonDrawArgs
			{
				Rectangle = e.Rects.FolderRect,
				Icon = "Folder",
				Font = Font,
				HoverState = HoverState,
				Cursor = CursorLocation
			});
		}

		e.Graphics.DrawString(e.Item.Title.Length < 32000 ? e.Item.Title : e.Item.Title.Substring(0, 32000), font, titleBrush, e.ClipRectangle.Pad(0, UI.Scale(20), Padding.Right, 0));

		y += Padding.Top / 2 + (int)e.Graphics.Measure(e.Item.Title, font, e.ClipRectangle.Width - Padding.Right).Height;

		foreach (var item in e.Item.Trace)
		{
			e.Graphics.DrawString(item.Length < 32000 ? item : item.Substring(0, 32000), smallFont, textBrush, new Rectangle(Padding.Left + Padding.Horizontal, y, e.ClipRectangle.Width - 2 * Padding.Horizontal, Height));

			y += (int)e.Graphics.Measure(item, smallFont, e.ClipRectangle.Width - 2 * Padding.Horizontal).Height + UI.Scale(3);
		}

		e.DrawableItem.CachedHeight = y - e.ClipRectangle.Top + Padding.Vertical;
	}

	private Color GetColor(string type)
	{
		return type switch
		{
			"FATAL" => Color.FromArgb(61, 4, 22),
			"CRITICAL" or "EMERGENCY" => Color.FromArgb(79, 3, 18),
			"EXCEPTION" => FormDesign.Design.RedColor.Tint(Lum: -2, Sat: 3),
			"ERROR" => FormDesign.Design.RedColor,
			"WARN" => FormDesign.Design.OrangeColor,
			"DEBUG" => Color.FromArgb(15, 109, 186),
			_ => ForeColor
		};
	}

	public class Rectangles : IDrawableItemRectangles<ILogTrace>
	{
		public Rectangles(ILogTrace item)
		{
			Item = item;
		}

		public ILogTrace Item { get; set; }
		public Rectangle ButtonRect { get; set; }
		public Rectangle LinkRect { get; set; }
		public Rectangle FolderRect { get; set; }

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			text = string.Empty;
			point = default;
			return false;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return ButtonRect.Contains(location) || LinkRect.Contains(location) || FolderRect.Contains(location);
		}
	}
}
