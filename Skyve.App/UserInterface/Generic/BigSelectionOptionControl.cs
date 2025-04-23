using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;

[DefaultEvent("Click")]
public class BigSelectionOptionControl : SlickImageControl
{
	private readonly Font? LargeFont;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text
	{
		get => base.Text;
		set => base.Text = value;
	}

	[DefaultValue(null), Category("Appearance")]
	public string? Title { get; set; }

	[DefaultValue(null), Category("Appearance")]
	public string? ButtonText { get; set; }

	[DefaultValue(ColorStyle.Text), Category("Appearance")]
	public ColorStyle ColorStyle { get; set; } = ColorStyle.Active;

	[DefaultValue(false), Category("Appearance")]
	public bool Highlighted { get; set; }

	protected override void UIChanged()
	{
		if (Live)
		{
			Margin = UI.Scale(new Padding(5));
			Padding = UI.Scale(new Padding(20));
			Size = UI.Scale(new Size(180, 320));
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			LargeFont?.Dispose();
		}

		base.Dispose(disposing);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var margin = Padding.Left / 2;
		var rectangle = ClientRectangle.Pad(margin);
		var buttonRect = ButtonText is null ? rectangle : rectangle.Pad(margin, rectangle.Height - margin - UI.Scale(32), margin, margin);

		Cursor = !Loading && buttonRect.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnClick(EventArgs e)
	{
		var margin = Padding.Left / 2;
		var rectangle = ClientRectangle.Pad(margin);
		var buttonRect = ButtonText is null ? rectangle : rectangle.Pad(margin, rectangle.Height - margin - UI.Scale(32), margin, margin);

		if (!Loading && buttonRect.Contains((e as MouseEventArgs)?.Location ?? default))
		{
			base.OnClick(e);
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		var margin = Padding.Left / 2;
		var rectangle = ClientRectangle.Pad(margin);
		var buttonRect = ButtonText is null ? rectangle : rectangle.Pad(margin, rectangle.Height - margin - UI.Scale(32), margin, margin);

		if (!Loading && buttonRect.Contains(e.Location))
		{
			base.OnMouseClick(e);
		}
	}

	protected sealed override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var margin = Padding.Left / 2;
		var activeColor = ColorStyle.GetColor();
		var rectangle = ClientRectangle.Pad(margin);
		var titleRect = rectangle.Pad(margin).ClipTo(UI.Scale(32));
		var iconRect = rectangle.Pad(margin, Title is not null ? 2 * margin + titleRect.Height : margin, margin, margin).Align(UI.Scale(new Size(40, 40)), ContentAlignment.TopCenter);
		var buttonRect = rectangle.Pad(margin, rectangle.Height - margin - UI.Scale(32), margin, margin);
		var textRect = Rectangle.FromLTRB(titleRect.Left, iconRect.Bottom + margin, titleRect.Right, buttonRect.Top - margin);

		e.Graphics.FillRoundedRectangleWithShadow(rectangle, margin, margin, FormDesign.Design.AccentBackColor, HoverState.HasFlag(HoverState.Hovered) ? Color.FromArgb(10, activeColor) : Highlighted ? Color.FromArgb(10, activeColor.MergeColor(FormDesign.Design.AccentColor)) : null, Highlighted);

		if (Title is not null)
		{
			var text = LocaleHelper.GetGlobalText(Title).ToUpper();

			using var format = new StringFormat { Alignment = StringAlignment.Center };
			using var font = UI.Font(11.25F, FontStyle.Bold).FitTo(text, titleRect, e.Graphics);
			using var brush = new SolidBrush(FormDesign.Design.ForeColor);

			e.Graphics.DrawHighResText(text, font, brush, titleRect, 2, format);
		}

		if (Loading)
		{
			DrawLoader(e.Graphics, iconRect);
		}
		else
		{
			using var icon = ImageName.Get(iconRect.Width);

			if (icon != null)
			{
				e.Graphics.DrawImage(icon.Color(FormDesign.Design.ForeColor), iconRect.CenterR(icon.Size));
			}
		}

		{
			var text = LocaleHelper.GetGlobalText(Text);

			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			using var font = UI.Font(9.75F).FitTo(text, textRect, e.Graphics);
			using var brush = new SolidBrush(FormDesign.Design.InfoColor.MergeColor(FormDesign.Design.ForeColor));

			e.Graphics.DrawString(text, font, brush, textRect, format);
		}

		if (ButtonText is not null)
		{
			using var buttonArgs = new ButtonDrawArgs
			{
				Text = ButtonText,
				ColorStyle = ColorStyle,
				Enabled = !Loading,
				Cursor = PointToClient(Cursor.Position),
				HoverState = HoverState,
				ButtonType = Highlighted ? ButtonType.Active : ButtonType.Normal,
				Rectangle = buttonRect
			};

			SlickButton.Draw(e, buttonArgs);
		}

		if (!Loading && HoverState.HasFlag(HoverState.Focused))
		{
			using var pen = new Pen(Color.FromArgb(100, Highlighted ? FormDesign.Design.ActiveForeColor : activeColor), UI.Scale(2.5F)) { DashStyle = DashStyle.Dash, Alignment = PenAlignment.Inset };

			e.Graphics.DrawRoundedRectangle(pen, ButtonText is not null ? buttonRect : rectangle, ButtonText is not null ? UI.Scale(4) : margin);
		}
	}
}
