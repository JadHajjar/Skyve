using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.StatusBubbles;
internal abstract class StatusBubbleBase : SlickImageControl
{
	public StatusBubbleBase()
	{
		Cursor = Cursors.Hand;
	}

	protected override void UIChanged()
	{
		Margin = UI.Scale(new Padding(10, 10, 5, 10), UI.FontScale);
		Padding = UI.Scale(new Padding(7), UI.FontScale);
		Width = (int)(180 * UI.FontScale);
	}

	public virtual Color? TintColor { get; set; }

	protected override void OnMouseMove(MouseEventArgs e)
	{ }

	protected sealed override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		SlickButton.GetColors(out var fore, out var back, HoverState);

		if (TintColor != null)
		{
			if (HoverState.HasFlag(HoverState.Pressed))
			{
				back = TintColor.Value;
			}
			else
			{
				back = back.MergeColor(TintColor.Value, 25);
			}

			fore = Color.FromArgb(220, back.GetTextColor());
		}

		if (!HoverState.HasFlag(HoverState.Pressed) && FormDesign.Design.Type == FormDesignType.Light)
		{
			back = back.Tint(Lum: 1.5F);
		}

		e.Graphics.FillRoundedRectangle(ClientRectangle.Gradient(back, 0.8F), ClientRectangle.Pad(2), Padding.Left);

		var targetHeight = Padding.Top;

		if (!string.IsNullOrEmpty(Text))
		{
			using var icon = ImageName?.Large;
			var titleHeight = Math.Max(icon?.Height ?? 0, (int)e.Graphics.Measure(Text, UI.Font(9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);
			var iconRectangle = new Rectangle(Padding.Left, Padding.Top + ((titleHeight - icon?.Height ?? 0) / 2), icon?.Width ?? 0, icon?.Height ?? 0);

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

			e.Graphics.DrawString(Text, UI.Font(9.75F, FontStyle.Bold), new SolidBrush(fore), new Rectangle((icon?.Height ?? 0) + (Padding.Left * 2), Padding.Top, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });

			targetHeight = titleHeight + Padding.Vertical;
		}

		CustomDraw(e, ref targetHeight);

		if (Height != targetHeight)
		{
			Height = targetHeight;
		}
	}

	protected abstract void CustomDraw(PaintEventArgs e, ref int targetHeight);

	protected void DrawValue(PaintEventArgs e, ref int targetHeight, string value, string descriptor, Color? foreColor = null, int x = 0)
	{
		SlickButton.GetColors(out var fore, out var back, HoverState);

		if (foreColor != null && !HoverState.HasFlag(HoverState.Pressed))
		{
			fore = fore.MergeColor(foreColor.Value, 10);
		}

		if (TintColor != null && foreColor == null)
		{
			if (HoverState.HasFlag(HoverState.Pressed))
			{
				back = TintColor.Value;
			}
			else
			{
				back = back.MergeColor(TintColor.Value, 25);
			}

			fore = Color.FromArgb(220, back.GetTextColor());
		}

		var valueSize = e.Graphics.Measure(value, UI.Font(8.25F, FontStyle.Bold), Width - Padding.Horizontal).ToSize();
		var descriptorSize = e.Graphics.Measure(descriptor, UI.Font(8.25F), Width - valueSize.Width - Padding.Right + 3).ToSize();

		e.Graphics.DrawString(value, UI.Font(8.25F, FontStyle.Bold), new SolidBrush(fore), new Rectangle(x + Padding.Left, targetHeight, valueSize.Width + 3, valueSize.Height));

		e.Graphics.DrawString(descriptor, UI.Font(8.25F), new SolidBrush(fore), new Rectangle(x + valueSize.Width + Padding.Left, targetHeight, Width - valueSize.Width - Padding.Right + 3, Height));

		targetHeight += Math.Max(valueSize.Height, descriptorSize.Height) + Padding.Bottom;
	}

	protected void DrawText(PaintEventArgs e, ref int targetHeight, string text, Color? foreColor = null)
	{
		SlickButton.GetColors(out var fore, out var back, HoverState);

		if (foreColor != null && !HoverState.HasFlag(HoverState.Pressed))
		{
			fore = fore.MergeColor(foreColor.Value, 10);
		}

		if (TintColor != null && foreColor == null)
		{
			if (HoverState.HasFlag(HoverState.Pressed))
			{
				back = TintColor.Value;
			}
			else
			{
				back = back.MergeColor(TintColor.Value, 25);
			}

			fore = Color.FromArgb(220, back.GetTextColor());
		}

		var valueSize = e.Graphics.Measure(text, UI.Font(8.25F), Width - Padding.Horizontal).ToSize();

		e.Graphics.DrawString(text, UI.Font(8.25F), new SolidBrush(fore), new Rectangle(Padding.Left, targetHeight, valueSize.Width + 3, valueSize.Height));

		targetHeight += valueSize.Height + Padding.Bottom;
	}
}
