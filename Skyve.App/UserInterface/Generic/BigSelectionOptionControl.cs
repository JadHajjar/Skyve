using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;

[DefaultEvent("Click")]
public class BigSelectionOptionControl : SlickImageControl
{
	public BigSelectionOptionControl()
	{
		Cursor = Cursors.Hand;
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

	public bool FromScratch { get; set; }

	protected override void UIChanged()
	{
		if (Live)
		{
			Font = UI.Font(11.25F, FontStyle.Bold);
			Margin = UI.Scale(new Padding(Parent.Controls.IndexOf(this) % 2 * 100, 15, 100 - (Parent.Controls.IndexOf(this) % 2 * 100), 15), UI.FontScale);
			Padding = UI.Scale(new Padding(15), UI.FontScale);
			Size = UI.Scale(new Size(250, 90), UI.FontScale);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{ }

	protected sealed override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		SlickButton.GetColors(out var fore, out var back, HoverState, FromScratch ? ColorStyle.Green : ColorStyle.Active);

		if (!HoverState.HasFlag(HoverState.Pressed) && !FormDesign.Design.IsDarkTheme)
		{
			back = back.Tint(Lum: 1.5F);
		}

		e.Graphics.FillRoundedRectangle(ClientRectangle.Gradient(back, 0.8F), ClientRectangle, Padding.Left);

		if (Loading)
		{
			DrawLoader(e.Graphics, ClientRectangle.Pad(Padding).Align(UI.Scale(new Size(32, 32), UI.UIScale), ContentAlignment.MiddleLeft));
		}
		else
		{
			using var icon = ImageName.Get((int)(32 * UI.UIScale));

			if (icon != null)
			{
				e.Graphics.DrawImage(icon.Color(fore), ClientRectangle.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleLeft));
			}
		}

		var textRect = ClientRectangle.Pad(Padding).Pad(Padding.Left + (int)(32 * UI.UIScale), 0, 0, 0);

		e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), Font, new SolidBrush(fore), textRect, new StringFormat { LineAlignment = StringAlignment.Center });

		if (textRect.Height < e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), Font, textRect.Width).Height)
		{
			Height += (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), Font, textRect.Width).Height - textRect.Height + 1;
		}
	}
}
