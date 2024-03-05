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
		Padding = UI.Scale(new Padding(12, 12, 0, 0), UI.FontScale);
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
			var border = (int)(8 * UI.FontScale);
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

			e.Graphics.SetClip(pe.ClipRectangle);

			var height = pe.ClipRectangle.Y;

			GetDrawingMethod(pe.ClipRectangle.Width).Invoke(pe, true, ref height);
		}
		catch { }

		var dragRect = ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight);

		using var dotBrush = new SolidBrush(HoverState.HasFlag(HoverState.Hovered) ? Color.FromArgb(150, FormDesign.Design.ActiveColor) : Color.FromArgb(50, FormDesign.Design.AccentColor));

		for (var i = 3; i > 0; i--)
		{
			var y = dragRect.Y + ((3 - i) * dragRect.Height / 3.75F);

			for (var j = i; j > 0; j--)
			{
				var x = dragRect.X + ((j - 1) * dragRect.Width / 3.75F);

				e.Graphics.FillEllipse(dotBrush, x, y, dragRect.Width / 5F, dragRect.Width / 5F);
			}
		}

		if (HoverState.HasFlag(HoverState.Hovered))
		{
			using var grabberBrush = new SolidBrush(Color.FromArgb(25, FormDesign.Design.MenuForeColor));

			foreach (var rectangle in _sections)
			{
				if (rectangle.rectangle.Contains(CursorLocation))
				{
					e.Graphics.FillRoundedRectangle(grabberBrush, rectangle.rectangle.ClipTo(rectangle.height).Pad(2), (int)(8 * UI.FontScale));
				}
			}
		}

		base.OnPaint(e);
	}

	protected void DrawSection(PaintEventArgs e, bool applyDrawing, Rectangle rectangle, string text, DynamicIcon dynamicIcon, out Color fore, ref int preferredHeight, Color? tintColor = null, string? subText = null, bool drawBackground = true)
	{
		var hoverState = rectangle.Contains(CursorLocation) ? (HoverState & ~HoverState.Focused) : HoverState.Normal;

		Color back;

		//if (hoverState.HasFlag(HoverState.Pressed))
		//{
		//	fore = ColorStyle.Active.GetBackColor().Tint(tintColor?.GetHue());
		//	back = tintColor == null ? ColorStyle.Active.GetColor() : ColorStyle.Active.GetColor().Tint(tintColor.Value.GetHue()).MergeColor(tintColor.Value);
		//}
		//else if (hoverState.HasFlag(HoverState.Hovered))
		//{
		//	fore = FormDesign.Design.MenuForeColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Light ? -3 : 3);
		//	back = FormDesign.Design.MenuColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Light ? -3 : 3);
		//}
		//else
		{
			fore = FormDesign.Design.MenuForeColor;
			back = FormDesign.Design.MenuColor;
		}

		if (applyDrawing && drawBackground)
		{
			if (tintColor != null)
			{
				//	if (hoverState.HasFlag(HoverState.Pressed))
				//	{
				//		back = tintColor.Value;
				//	}
				//	else
				//	{
				back = back.MergeColor(tintColor.Value, 25);
				//}

				fore = Color.FromArgb(220, back.GetTextColor());
			}

			//if (!hoverState.HasFlag(HoverState.Pressed) && FormDesign.Design.Type == FormDesignType.Light)
			//{
			//	back = back.Tint(Lum: 1.5F);
			//}

			using var gradient = rectangle.Gradient(back, /*hoverState.HasFlag(HoverState.Pressed) ? 1F :*/ 0.5F);
			e.Graphics.FillRoundedRectangle(gradient, rectangle.Pad(2), (int)(8 * UI.FontScale));
		}

		if (!string.IsNullOrEmpty(text))
		{
			using var icon = dynamicIcon?.Get((int)(25 * UI.FontScale));
			var iconRectangle = new Rectangle(Margin.Left + rectangle.X, rectangle.Y, icon?.Width ?? 0, icon?.Height ?? 0);
			var textRect = new Rectangle(iconRectangle.Right + Margin.Left / 2, Margin.Top + rectangle.Y, rectangle.Right - Margin.Horizontal - iconRectangle.Right, 0);
			using var font = UI.Font(9.75F, FontStyle.Bold).FitToWidth(text, textRect, e.Graphics);
			var titleHeight = Math.Max(icon?.Height ?? 0, (int)e.Graphics.Measure(text, font, rectangle.Right - Margin.Horizontal - iconRectangle.Right).Height);
			textRect.Height = titleHeight;

			if (subText is not null)
			{
				using var smallFont = UI.Font(7F).FitToWidth(subText, textRect, e.Graphics);
				var textHeight = (int)e.Graphics.Measure(subText, smallFont, rectangle.Right - Margin.Horizontal - iconRectangle.Right).Height;

				if (applyDrawing)
				{
					using var brush = new SolidBrush(Color.FromArgb(150, fore));
					e.Graphics.DrawString(subText, smallFont, brush, textRect.Pad((int)(2 * UI.FontScale), Margin.Top / -2, 0, 0));
				}

				titleHeight += textHeight - (Margin.Top / 2);
				textRect.Y += textHeight - (Margin.Top / 2);
			}

			iconRectangle.Y += Margin.Top + ((titleHeight - icon?.Height ?? 0) / 2);

			if (applyDrawing)
			{
				if (icon is not null)
				{
					try
					{
						e.Graphics.DrawImage(icon.Color(fore), iconRectangle);
					}
					catch { }
				}

				using var brush = new SolidBrush(fore);
				e.Graphics.DrawString(text, font, brush, textRect, new StringFormat { LineAlignment = StringAlignment.Center });
			}
			else
			{
				_sections.Add((rectangle, titleHeight + (Margin.Top * 2)));
			}

			preferredHeight += titleHeight + (Margin.Top * 2);
		}
	}

	protected void DrawLoadingSection(PaintEventArgs e, bool applyDrawing, string text, DynamicIcon icon, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle, text, icon, out _, ref preferredHeight);

		DrawLoader(e.Graphics, e.ClipRectangle.Pad(Margin).Align(UI.Scale(new Size(32, 32), UI.FontScale), ContentAlignment.BottomCenter));

		preferredHeight += (int)(32 * UI.FontScale) + Margin.Vertical;
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
		buttonArgs.Padding = Margin;
		buttonArgs.BorderRadius = (int)(4 * UI.FontScale);
		buttonArgs.Rectangle = new Rectangle(buttonArgs.Rectangle.X, preferredHeight, buttonArgs.Rectangle.Width, square ? buttonArgs.Rectangle.Height : SlickButton.GetSize(e.Graphics, buttonArgs.Image, buttonArgs.Text, buttonArgs.Font, buttonArgs.Padding).Height);
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
