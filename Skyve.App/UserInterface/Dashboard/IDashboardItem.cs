using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
public abstract class IDashboardItem : SlickImageControl
{
	public event EventHandler? ResizeRequested;

	public string Key { get; }
	public bool MoveInProgress { get; internal set; }
	public bool ResizeInProgress { get; internal set; }

	public IDashboardItem()
	{
		Key = GetType().Name;
	}

	protected delegate void DrawingDelegate(PaintEventArgs e, bool applyDrawing, ref int preferredHeight);
	protected abstract DrawingDelegate GetDrawingMethod(int width);

	public virtual bool MoveAreaContains(Point point)
	{
		return new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale)).Contains(point);
	}

	public int CalculateHeight(int width, Graphics graphics)
	{
		var clip = ClientRectangle;

		clip.Width = width;

		using var pe = new PaintEventArgs(graphics, clip.Pad(Padding));

		graphics.SetClip(pe.ClipRectangle);

		var height = pe.ClipRectangle.Y;

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

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		if (MoveInProgress || ResizeInProgress)
		{
			var border = (int)(10 * UI.FontScale);
			var color = FormDesign.Design.ForeColor;

			using var brush = new SolidBrush(Color.FromArgb(25, color));
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad((int)(1.5 * UI.FontScale)).Pad(Padding), border);

			using var pen = new Pen(Color.FromArgb(100, color), (float)(1.5 * UI.FontScale)) { DashStyle = DashStyle.Dash };
			e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad((int)(1.5 * UI.FontScale)).Pad(Padding), border);

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

		base.OnPaint(e);

		e.Graphics.ResetClip();
	}

	protected void DrawSection(PaintEventArgs e, bool applyDrawing, Rectangle rectangle, string text, DynamicIcon dynamicIcon, out Color fore, ref int preferredHeight, Color? tintColor = null)
	{
		var hoverState = rectangle.Contains(CursorLocation) ? (HoverState & ~HoverState.Focused) : HoverState.Normal;

		SlickButton.GetColors(out fore, out var back, hoverState);

		if (applyDrawing)
		{
			if (tintColor != null)
			{
				if (hoverState.HasFlag(HoverState.Pressed))
				{
					back = tintColor.Value;
				}
				else
				{
					back = back.MergeColor(tintColor.Value, 25);
				}

				fore = Color.FromArgb(220, back.GetTextColor());
			}

			if (!hoverState.HasFlag(HoverState.Pressed) && FormDesign.Design.Type == FormDesignType.Light)
			{
				back = back.Tint(Lum: 1.5F);
			}

			e.Graphics.FillRoundedRectangle(rectangle.Gradient(back, 0.8F), rectangle.Pad(2), Padding.Left);
		}

		if (!string.IsNullOrEmpty(text))
		{
			using var icon = dynamicIcon?.Large;
			var iconRectangle = new Rectangle(Margin.Left + rectangle.X, rectangle.Y, icon?.Width ?? 0, icon?.Height ?? 0);
			var titleHeight = Math.Max(icon?.Height ?? 0, (int)e.Graphics.Measure(text, UI.Font(9.75F, FontStyle.Bold), rectangle.Right - Margin.Horizontal - iconRectangle.Right).Height);

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
				e.Graphics.DrawString(text, UI.Font(9.75F, FontStyle.Bold), brush, new Rectangle(iconRectangle.Right + Margin.Left, Margin.Top + rectangle.Y, rectangle.Right - Margin.Horizontal - iconRectangle.Right, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });
			}

			preferredHeight += titleHeight + (Margin.Top * 2);
		}
	}

	protected void OnResizeRequested()
	{
		ResizeRequested?.Invoke(this, EventArgs.Empty);
	}

	protected void DrawLoadingSection(PaintEventArgs e, bool applyDrawing, string text, DynamicIcon icon, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle, text, icon, out _, ref preferredHeight);

		DrawLoader(e.Graphics, e.ClipRectangle.Pad(Margin).Align(UI.Scale(new Size(32, 32), UI.FontScale), ContentAlignment.BottomCenter));

		preferredHeight += (int)(32 * UI.FontScale) + Margin.Vertical ;
	}
}
