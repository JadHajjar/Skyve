using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
public abstract class IDashboardItem : SlickImageControl
{
	private CancellationTokenSource? tokenSource;
	protected Rectangle headerRectangle;
	protected readonly Dictionary<Rectangle, ExtensionClass.action> _buttonActions = [];
	protected readonly Dictionary<Rectangle, ExtensionClass.action> _buttonRightClickActions = [];

	public event EventHandler? ResizeRequested;

	public string Key { get; }
	public bool MoveInProgress { get; internal set; }
	public bool ResizeInProgress { get; internal set; }
	public int MinimumWidth { get; protected set; } = 100;
	internal bool DrawHeaderOnly { get; set; }
	internal bool MovementBlocked { get; set; }
	protected int BorderRadius { get; set; }

	public IDashboardItem()
	{
		Key = GetType().Name;
	}

	protected delegate void DrawingDelegate(PaintEventArgs e, bool applyDrawing, ref int preferredHeight);
	protected abstract DrawingDelegate GetDrawingMethod(int width);
	protected abstract void DrawHeader(PaintEventArgs e, bool applyDrawing, ref int preferredHeight);

	public virtual bool MoveAreaContains(Point point)
	{
		if (headerRectangle != default)
		{
			return headerRectangle.Contains(point);
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

		headerRectangle = default;

		try
		{
			if (DrawHeaderOnly)
			{
				DrawHeader(pe, false, ref height);
			}
			else
			{
				GetDrawingMethod(pe.ClipRectangle.Width).Invoke(pe, false, ref height);
			}
		}
		catch { }

		return height;
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(10, 10, 4, 4));
		BorderRadius = UI.Scale(8);
	}

	protected void OnResizeRequested()
	{
		this.TryBeginInvoke(() =>
		{
			Invalidate();
			ResizeRequested?.Invoke(this, EventArgs.Empty);
		});
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (!MovementBlocked && !DrawHeaderOnly && (ResizeInProgress || ClientRectangle.Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight).Contains(e.Location)))
		{
			Cursor = Cursors.SizeWE;
		}
		else if (!MovementBlocked && (MoveInProgress || MoveAreaContains(e.Location)))
		{
			Cursor = Cursors.SizeAll;
		}
		else if (MovementBlocked && _buttonActions.Keys.Any(x => x.Contains(e.Location)))
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

		if (!MovementBlocked)
		{
			return;
		}

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

	public void LoadData()
	{
		tokenSource?.Cancel();
		tokenSource = new CancellationTokenSource();

		Task.Run(TriggerDataLoad);
	}

	private async void TriggerDataLoad()
	{
		var token = tokenSource!.Token;

		Loading = true;

		try
		{
			if (await ProcessDataLoad(token))
				Loading = false;
		}
		catch (Exception ex)
		{
			Loading = false;
			OnDataLoadError(ex);
		}
	}

	protected virtual Task<bool> ProcessDataLoad(CancellationToken token)
	{
		return Task.FromResult(true);
	}

	protected virtual void OnDataLoadError(Exception ex)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		_buttonActions.Clear();

		if (MoveInProgress || ResizeInProgress)
		{
			using var pe = new PaintEventArgs(e.Graphics, ClientRectangle.Pad(Padding));
			var height = pe.ClipRectangle.Y;

			e.Graphics.FillRoundedRectangleWithShadow(pe.ClipRectangle.Pad(UI.Scale(2)), BorderRadius, Padding.Right, FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 8 : -8), Color.FromArgb(20, FormDesign.Design.AccentColor), addOutline: true);

			DrawHeader(pe, true, ref height);

			return;
		}

		var isHovered = HoverState.HasFlag(HoverState.Hovered);

		try
		{
			using var pe = new PaintEventArgs(e.Graphics, ClientRectangle.Pad(Padding));

			if (isHovered && !MovementBlocked && headerRectangle.Contains(CursorLocation))
			{
				DrawSectionHoverAndBackground(e, pe.ClipRectangle);
			}
			else
			{
				DrawBackground(e, pe.ClipRectangle);
			}

			e.Graphics.SetClip(pe.ClipRectangle);

			var height = pe.ClipRectangle.Y;

			if (DrawHeaderOnly)
			{
				DrawHeader(pe, true, ref height);
			}
			else
			{
				var hoverState = HoverState;

				if (!MovementBlocked)
				{
					HoverState = default;
				}

				GetDrawingMethod(pe.ClipRectangle.Width).Invoke(pe, true, ref height);

				if (!MovementBlocked)
				{
					HoverState = hoverState;
				}
			}
		}
		catch { }

		if (!MovementBlocked && !DrawHeaderOnly)
		{
			var dragRect = ClientRectangle.Pad(Padding).Align(UI.Scale(new Size(16, 16), UI.UIScale), ContentAlignment.BottomRight);

			using var dotBrush = new SolidBrush(isHovered && ClientRectangle.Pad(Padding.Left, Padding.Top, 0, 0).Contains(CursorLocation) ? Color.FromArgb(200, FormDesign.Design.ActiveColor) : Color.FromArgb(150, FormDesign.Design.AccentColor));

			for (var i = 3; i > 0; i--)
			{
				var y = dragRect.Y + ((3 - i) * dragRect.Height / 3.75F);

				for (var j = i; j > 0; j--)
				{
					var x = dragRect.X + ((j - 1) * dragRect.Width / 3.75F);

					e.Graphics.FillEllipse(dotBrush, x, y, dragRect.Width / 5F, dragRect.Width / 5F);
				}
			}
		}

		base.OnPaint(e);

		if (isHovered && !DrawHeaderOnly && !MovementBlocked && headerRectangle.Contains(CursorLocation))
		{
			using var grabberBrush = new SolidBrush(Color.FromArgb(35, FormDesign.Design.ActiveColor));
			e.Graphics.FillRoundedRectangle(grabberBrush, headerRectangle, BorderRadius, botLeft: DrawHeaderOnly, botRight: DrawHeaderOnly);
		}
	}

	private void DrawBackground(PaintEventArgs e, Rectangle clipRectangle)
	{
		e.Graphics.FillRoundedRectangleWithShadow(clipRectangle, BorderRadius, Padding.Right, addOutline: !MovementBlocked);
	}

	private void DrawSectionHoverAndBackground(PaintEventArgs e, Rectangle clipRectangle)
	{
		e.Graphics.FillRoundedRectangleWithShadow(clipRectangle, BorderRadius, Padding.Right, shadow: Color.FromArgb(12, FormDesign.Design.ActiveColor));

		using var penActive = new Pen(FormDesign.Design.ActiveColor, 1.5F);
		e.Graphics.DrawRoundedRectangle(penActive, clipRectangle, BorderRadius);
	}

	protected void DrawSection(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, string text, DynamicIcon? dynamicIcon, string? subText = null)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}

		var rectangle = e.ClipRectangle;
		using var icon = dynamicIcon?.Get(UI.Scale(20));
		var iconRectangle = new Rectangle(rectangle.Right - BorderRadius - icon?.Width ?? 0, rectangle.Y, icon?.Width ?? 0, icon?.Height ?? 0);
		var textRect = new Rectangle(rectangle.X + BorderRadius, rectangle.Y, rectangle.Right - (2 * BorderRadius) - BorderRadius - iconRectangle.Width, UI.Scale(26));
		using var font = UI.Font(8.5F, FontStyle.Bold).FitTo(text, textRect, e.Graphics);
		var titleHeight = (int)e.Graphics.Measure(text, font, rectangle.Right - (2 * BorderRadius) - iconRectangle.Right).Height + BorderRadius / 2;
		textRect.Height = titleHeight + (BorderRadius * 3 / 2);

		if (subText is not null)
		{
			textRect.Y += BorderRadius / 2;

			using var smallFont = UI.Font(6.75F).FitToWidth(subText, textRect, e.Graphics);
			var textHeight = (int)e.Graphics.Measure(subText, smallFont, rectangle.Right - (2 * BorderRadius) - iconRectangle.Right).Height;

			if (applyDrawing)
			{
				using var brush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.ForeColor));
				e.Graphics.DrawString(subText, smallFont, brush, textRect.Pad(UI.Scale(2), 0, 0, 0));
			}

			titleHeight += textHeight - (BorderRadius / 2);
			textRect.Y += textHeight - (BorderRadius / 2);
			headerRectangle = rectangle.ClipTo(textRect.Bottom - Padding.Top);
		}
		else
		{
			headerRectangle = rectangle.ClipTo(textRect.Height);
		}

		iconRectangle.Y = headerRectangle.Y + ((headerRectangle.Height - iconRectangle.Height) / 2);

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

		preferredHeight += titleHeight + (BorderRadius * 2);
	}

	protected void DrawLoadingSection(PaintEventArgs e, bool applyDrawing, ref int preferredHeight, string text, string? subText = null)
	{
		var iconSize = UI.Scale(18);
		var iconRectangle = new Rectangle(e.ClipRectangle.Right - BorderRadius - iconSize, preferredHeight, iconSize, 0);

		DrawSection(e, applyDrawing, ref preferredHeight, text, null, subText);

		iconRectangle.Height = preferredHeight - iconRectangle.Y;

		DrawLoader(e.Graphics, iconRectangle.CenterR(iconSize, iconSize));
	}

	protected void DrawDivider(PaintEventArgs e, Rectangle rect, bool applyDrawing, ref int preferredHeight, bool vertical = false)
	{
		if (!applyDrawing)
		{
			if (!vertical)
			{
				preferredHeight += BorderRadius;
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

		preferredHeight += BorderRadius;
	}

	protected void DrawValue(PaintEventArgs e, Rectangle rect, string label, string value, bool applyDrawing, ref int preferredHeight, DynamicIcon? dynamicIcon = null, Color? color = null, bool boldValue = true)
	{
		using var valueFont = UI.Font(8.75F, boldValue ? FontStyle.Bold : FontStyle.Regular);
		using var labelFont = UI.Font(7.75F);
		using var icon = dynamicIcon?.Get(valueFont.Height * 5 / 4);

		var valueSize = e.Graphics.Measure(value, valueFont).ToSize();
		var labelSize = e.Graphics.Measure(label, labelFont, rect.Width - valueSize.Width - BorderRadius - (icon is null ? 0 : ((BorderRadius / 2) + icon.Width))).ToSize();

		if (labelFont.Height * 4 < labelSize.Height)
		{
			labelSize = default;
		}

		rect = new Rectangle(rect.X, preferredHeight, rect.Width, Math.Max(valueSize.Height, labelSize.Height));
		preferredHeight += rect.Height + (BorderRadius / 2);

		if (!applyDrawing)
		{
			return;
		}

		var brush = new SolidBrush(color ?? FormDesign.Design.ForeColor);

		if (icon is not null)
		{
			e.Graphics.DrawImage(icon.Color(brush.Color), rect.Align(icon.Size, ContentAlignment.MiddleLeft));
		}

		if (labelSize != default)
		{
			using var format1 = new StringFormat { LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(label, labelFont, brush, rect.Pad(icon is null ? 0 : ((BorderRadius / 2) + icon.Width), 0, valueSize.Width + BorderRadius, 0), format1);
		}

		using var format2 = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
		e.Graphics.DrawString(value, valueFont, brush, rect, format2);

		using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale)) { DashStyle = DashStyle.Dot, DashCap = DashCap.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };
		e.Graphics.DrawLine(pen, rect.X + (icon is null ? 0 : ((BorderRadius / 2) + icon.Width)) + labelSize.Width, rect.Y + (rect.Height / 2), rect.Right - valueSize.Width - (BorderRadius / 2), rect.Y + (rect.Height / 2));
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
		buttonArgs.Padding = UI.Scale(new Padding(6, 3, 6, 3));
		buttonArgs.BorderRadius = UI.Scale(4);
		buttonArgs.Rectangle = new Rectangle(buttonArgs.Rectangle.X, preferredHeight, buttonArgs.Rectangle.Width, square ? buttonArgs.Rectangle.Height : buttonArgs.Size.Height.If(0, UI.Scale(26)));
		buttonArgs.HoverState = buttonArgs.Rectangle.Contains(CursorLocation) && buttonArgs.Enabled ? (HoverState & ~HoverState.Focused) : HoverState.Normal;

		if (buttonArgs.BackColor.A != 0 && buttonArgs.HoverState.HasFlag(HoverState.Hovered))
		{
			buttonArgs.BackColor = buttonArgs.BackColor.MergeColor(FormDesign.Design.ButtonColor, buttonArgs.HoverState.HasFlag(HoverState.Pressed) ? 25 : 65);
		}

		preferredHeight += buttonArgs.Rectangle.Height + BorderRadius;

		if (!applyDrawing)
		{
			return;
		}

		if (clickAction is not null && buttonArgs.Enabled)
		{
			_buttonActions.Add(buttonArgs.Rectangle, clickAction);
		}

		SlickButton.Draw(e, buttonArgs);
	}
}
