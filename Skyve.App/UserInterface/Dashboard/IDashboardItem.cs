using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
public abstract class IDashboardItem : SlickImageControl
{
	protected readonly List<(Rectangle rectangle, int height)> _sections = [];
	protected readonly Dictionary<Rectangle, ExtensionClass.action> _buttonActions = [];
	protected readonly Dictionary<Rectangle, ExtensionClass.action> _buttonRightClickActions = [];

	public event EventHandler? ResizeRequested;

	public string Key { get; }
	public bool MoveInProgress { get; internal set; }
	public bool ResizeInProgress { get; internal set; }
	public int MinimumWidth { get; protected set; } = 100;

	public IDashboardItem()
	{
		Key = GetType().Name;
	}

	protected delegate void DrawingDelegate(PaintEventArgs e, bool applyDrawing, ref int preferredHeight);
	protected abstract DrawingDelegate GetDrawingMethod(int width);

	public virtual bool MoveAreaContains(Point point)
	{
		if (_sections.Count > 0)
		{
			return _sections.Any(x => x.rectangle.ClipTo(x.height).Contains(point));
		}

		return point.Y < Padding.Top * 3 / 2;
	}

	public int CalculateHeight(int width, Graphics graphics)
	{
		var clip = ClientRectangle;

		clip.Width = width;

		using var pe = new PaintEventArgs(graphics, clip.Pad(Padding));

		graphics.SetClip(pe.ClipRectangle);

		var height = pe.ClipRectangle.Y;

		_sections.Clear();

		try
		{
			GetDrawingMethod(pe.ClipRectangle.Width).Invoke(pe, false, ref height);
		}
		catch { }

		return height;
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(10, 10, 4, 4), UI.FontScale);
		Margin = UI.Scale(new Padding(8), UI.FontScale);
	}

	protected void OnResizeRequested()
	{
		this.TryInvoke(() =>
		{
			Invalidate();
			ResizeRequested?.Invoke(this, EventArgs.Empty);
		});
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (ResizeInProgress || ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight).Contains(e.Location))
		{
			Cursor = Cursors.SizeWE;
		}
		else if (MoveInProgress || MoveAreaContains(e.Location))
		{
			Cursor = Cursors.SizeAll;
		}
		else if (_buttonActions.Keys.Any(x => x.Contains(e.Location)))
		{
			Cursor = Cursors.Hand;
		}
		else
		{
			Cursor = Cursors.Default;
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			foreach (var item in _buttonActions)
			{
				if (item.Key.Contains(e.Location))
				{
					item.Value();
					return;
				}
			}
		}

		if (e.Button == MouseButtons.Right)
		{
			foreach (var item in _buttonRightClickActions)
			{
				if (item.Key.Contains(e.Location))
				{
					item.Value();
					return;
				}
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		_buttonActions.Clear();

		if (MoveInProgress || ResizeInProgress)
		{
			var border = Margin.Left;
			var rect = ClientRectangle.Pad((int)(1.5 * UI.FontScale)).Pad(Padding);

			using var brush = new SolidBrush(FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 8 : -8));
			e.Graphics.FillRoundedRectangle(brush, rect, border);

			using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale));
			e.Graphics.DrawRoundedRectangle(pen, rect, border);

			return;
		}

		try
		{
			using var pe = new PaintEventArgs(e.Graphics, ClientRectangle.Pad(Padding));

			if (HoverState.HasFlag(HoverState.Hovered) && _sections.Any(x => x.rectangle.ClipTo(x.height).Contains(CursorLocation)))
			{
				DrawSectionHoverAndBackground(e, pe.ClipRectangle);
			}
			else
			{
				DrawBackground(e, pe.ClipRectangle);
			}

			e.Graphics.SetClip(pe.ClipRectangle);

			var height = pe.ClipRectangle.Y;

			GetDrawingMethod(pe.ClipRectangle.Width).Invoke(pe, true, ref height);
		}
		catch { }

		var dragRect = ClientRectangle.Pad(Padding).Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight);

		using var dotBrush = new SolidBrush(HoverState.HasFlag(HoverState.Hovered) && ClientRectangle.Pad(Padding.Left, Padding.Top, 0, 0).Contains(CursorLocation) ? Color.FromArgb(150, FormDesign.Design.ActiveColor) : Color.FromArgb(50, FormDesign.Design.AccentColor));

		for (var i = 3; i > 0; i--)
		{
			var y = dragRect.Y + ((3 - i) * dragRect.Height / 3.75F);

			for (var j = i; j > 0; j--)
			{
				var x = dragRect.X + ((j - 1) * dragRect.Width / 3.75F);

				e.Graphics.FillEllipse(dotBrush, x, y, dragRect.Width / 5F, dragRect.Width / 5F);
			}
		}

		base.OnPaint(e);

		if (HoverState.HasFlag(HoverState.Hovered))
		{
			using var grabberBrush = new SolidBrush(Color.FromArgb(35, FormDesign.Design.ActiveColor));

			foreach (var rectangle in _sections)
			{
				if (rectangle.rectangle.ClipTo(rectangle.height).Contains(CursorLocation))
				{
					e.Graphics.FillRoundedRectangle(grabberBrush, rectangle.rectangle.ClipTo(rectangle.height), Margin.Left, botLeft: false, botRight: false);
				}
			}
		}
	}

	private void DrawBackground(PaintEventArgs e, Rectangle clipRectangle)
	{
		using var brush = new SolidBrush(FormDesign.Design.IsDarkTheme ? Color.FromArgb(1, 255, 255, 255) : Color.FromArgb(10, FormDesign.Design.AccentColor));
		for (var i = Padding.Right; i > 0; i--)
		{
			e.Graphics.FillRoundedRectangle(brush, clipRectangle.Pad(-i), Margin.Left + i);
		}

		using var brushBack = new SolidBrush(FormDesign.Design.BackColor);
		e.Graphics.FillRoundedRectangle(brushBack, clipRectangle, Margin.Left);
	}

	private void DrawSectionHoverAndBackground(PaintEventArgs e, Rectangle clipRectangle)
	{
		using var brush = new SolidBrush(Color.FromArgb(12, FormDesign.Design.ActiveColor));
		for (var i = Padding.Right; i > 0; i--)
		{
			e.Graphics.FillRoundedRectangle(brush, clipRectangle.Pad(-i), Margin.Left + i);
		}

		using var brushBack = new SolidBrush(FormDesign.Design.BackColor);
		e.Graphics.FillRoundedRectangle(brushBack, clipRectangle, Margin.Left);

		using var penActive = new Pen(FormDesign.Design.ActiveColor, 1.5F);
		e.Graphics.DrawRoundedRectangle(penActive, clipRectangle, Margin.Left);
	}

	protected void DrawSection(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, string text, DynamicIcon? dynamicIcon, string? subText = null)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}

		var rectangle = e.ClipRectangle;
		using var icon = dynamicIcon?.Get((int)(20 * UI.FontScale));
		var iconRectangle = new Rectangle(rectangle.Right - Margin.Right - icon?.Width ?? 0, rectangle.Y, icon?.Width ?? 0, icon?.Height ?? 0);
		var textRect = new Rectangle(rectangle.X + Margin.Left, rectangle.Y, rectangle.Right - Margin.Horizontal - Margin.Left - iconRectangle.Width, (int)(26 * UI.FontScale));
		using var font = UI.Font(8.5F, FontStyle.Bold).FitTo(text, textRect, e.Graphics);
		var titleHeight = Math.Max(icon?.Height ?? 0, (int)e.Graphics.Measure(text, font, rectangle.Right - Margin.Horizontal - iconRectangle.Right).Height);
		textRect.Height = titleHeight + Margin.Top;

		if (subText is not null)
		{
			textRect.Y += (Margin.Top / 2);

			using var smallFont = UI.Font(6.75F).FitToWidth(subText, textRect, e.Graphics);
			var textHeight = (int)e.Graphics.Measure(subText, smallFont, rectangle.Right - Margin.Horizontal - iconRectangle.Right).Height;

			if (applyDrawing)
			{
				using var brush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.ForeColor));
				e.Graphics.DrawString(subText, smallFont, brush, textRect.Pad((int)(2 * UI.FontScale), 0, 0, 0));
			}

			titleHeight += textHeight - (Margin.Top / 2);
			textRect.Y += textHeight - (Margin.Top / 2);
		}
		else
		{
			textRect.Y += Margin.Top + ((titleHeight - textRect.Height) / 2);
		}

		iconRectangle.Y += Margin.Top + ((titleHeight - icon?.Height ?? 0) / 2);

		if (applyDrawing)
		{
			if (icon is not null)
			{
				try
				{
					e.Graphics.DrawImage(icon.Color(FormDesign.Design.ForeColor), iconRectangle);
				}
				catch { }
			}

			using var brush = new SolidBrush(FormDesign.Design.ForeColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(text, font, brush, textRect, format);
		}
		else
		{
			_sections.Add((rectangle, titleHeight + (Margin.Top * 2)));
		}

		preferredHeight += titleHeight + (Margin.Top * 2);
	}

	protected void DrawLoadingSection(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, string text, string? subText = null)
	{
		var iconSize = (int)(18 * UI.FontScale);
		var iconRectangle = new Rectangle(e.ClipRectangle.Right - Margin.Right - iconSize, preferredHeight, iconSize, 0);

		DrawSection(e, applyDrawing, ref preferredHeight, text, null, subText);

		iconRectangle.Height = preferredHeight - iconRectangle.Y;

		DrawLoader(e.Graphics, iconRectangle.CenterR(iconSize, iconSize));
	}

	protected void DrawSquareButton(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, ExtensionClass.action? clickAction, ButtonDrawArgs buttonArgs)
	{
		buttonArgs.BorderRadius = Padding.Left;

		DrawButtonInternal(e, applyDrawing, ref preferredHeight, true, buttonArgs, clickAction);
	}

	protected void DrawButton(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, ExtensionClass.action? clickAction, ButtonDrawArgs buttonArgs)
	{
		DrawButtonInternal(e, applyDrawing, ref preferredHeight, false, buttonArgs, clickAction);
	}

	private void DrawButtonInternal(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, bool square, ButtonDrawArgs buttonArgs, ExtensionClass.action? clickAction)
	{
		buttonArgs.Padding = UI.Scale(new Padding(8, 4, 8, 4), UI.FontScale);
		buttonArgs.BorderRadius = (int)(4 * UI.FontScale);
		buttonArgs.Rectangle = new Rectangle(buttonArgs.Rectangle.X + (Margin.Left / 2), preferredHeight, buttonArgs.Rectangle.Width - Margin.Left, square ? buttonArgs.Rectangle.Height : buttonArgs.Size.Height.If(0, (int)(28 * UI.FontScale)));
		buttonArgs.HoverState = buttonArgs.Rectangle.Contains(CursorLocation) ? (HoverState & ~HoverState.Focused) : HoverState.Normal;

		if (buttonArgs.BackColor.A != 0 && buttonArgs.HoverState.HasFlag(HoverState.Hovered))
		{
			buttonArgs.BackColor = buttonArgs.BackColor.MergeColor(FormDesign.Design.ButtonColor, buttonArgs.HoverState.HasFlag(HoverState.Pressed) ? 25 : 65);
		}

		preferredHeight += buttonArgs.Rectangle.Height + Margin.Bottom;

		if (!applyDrawing)
		{
			return;
		}

		if (clickAction is not null)
		{
			_buttonActions.Add(buttonArgs.Rectangle, clickAction);
		}

		SlickButton.Draw(e, buttonArgs);
	}

	protected void DrawDivider(PaintEventArgs e, Rectangle rect, bool applyDrawing, ref int preferredHeight, bool vertical = false)
	{
		if (!applyDrawing)
		{
			if (!vertical)
			{
				preferredHeight += Margin.Top;
			}

			return;
		}

		using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale));

		if (vertical)
		{
			e.Graphics.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
			return;
		}

		e.Graphics.DrawLine(pen, rect.X, preferredHeight, rect.Right, preferredHeight);

		preferredHeight += Margin.Top;
	}
}
