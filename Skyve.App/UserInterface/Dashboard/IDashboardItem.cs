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

	public virtual Rectangle GetMoveArea()
	{
		return new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale));
	}

	public int CalculateHeight(int width, Graphics graphics)
	{
		var clip = ClientRectangle;
		var height = clip.Y;

		clip.Width = width;
		clip = /*MoveInProgress || ResizeInProgress ? clip :*/ clip.Pad(Padding);

		using var pe = new PaintEventArgs(graphics, clip);

		try
		{
			GetDrawingMethod(clip.Width).Invoke(pe, false, ref height);
		}
		catch { }

		//if (!MoveInProgress && !ResizeInProgress)
		//	height+=Padding.Vertical;

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

		var clip = MoveInProgress || ResizeInProgress ? ClientRectangle : ClientRectangle.Pad(Padding);

		e.Graphics.SetClip(clip);

		try
		{
			using var pe = new PaintEventArgs(e.Graphics, clip);
			var height = clip.Y;

			GetDrawingMethod(clip.Width).Invoke(pe, true, ref height);
		}
		catch { }

		//if (MoveInProgress || ResizeInProgress)
		//{
		//	using var brush = new SolidBrush(Color.FromArgb(100, FormDesign.Design.AccentBackColor));

		//	e.Graphics.FillRectangle(brush, ClientRectangle);
		//}

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
			var titleHeight = Math.Max(icon?.Height ?? 0, (int)e.Graphics.Measure(text, UI.Font(9.75F, FontStyle.Bold), rectangle.Width - Margin.Horizontal).Height);
			var iconRectangle = new Rectangle(Margin.Left + rectangle.X, Margin.Top + ((titleHeight - icon?.Height ?? 0) / 2) + rectangle.Y, icon?.Width ?? 0, icon?.Height ?? 0);

			if (applyDrawing)
			{
				if (Loading)
				{
					DrawLoader(e.Graphics, iconRectangle);
				}
				else if (icon is not null)
				{
					try
					{
						e.Graphics.DrawImage(icon.Color(fore), iconRectangle);
					}
					catch { }
				}

				e.Graphics.DrawString(text, UI.Font(9.75F, FontStyle.Bold), new SolidBrush(fore), new Rectangle((icon?.Height ?? 0) + (Margin.Left * 2) + rectangle.X, Margin.Top + rectangle.Y, rectangle.Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });
			}

			preferredHeight += titleHeight + (Margin.Top * 2);
		}
	}

	protected void OnResizeRequested()
	{
		ResizeRequested?.Invoke(this, EventArgs.Empty);
	}
}
