using SlickControls;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class IconTopButton : SlickImageControl
{
	[Category("Appearance"), DisplayName("Match Background Color"), DefaultValue(true)]
	public bool MatchBackgroundColor { get; set; } = true;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text
	{
		get => base.Text; set
		{
			base.Text = value;
			UIChanged();
		}
	}

	public IconTopButton()
	{
		Cursor = Cursors.Hand;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(5), UI.FontScale);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var client = ClientRectangle.Pad(1);
		var active = FormDesign.Design.ActiveColor;
		var backColor = HoverState.HasFlag(HoverState.Pressed) ? active
			: HoverState.HasFlag(HoverState.Hovered) ? (MatchBackgroundColor ? BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 12 : -10) : FormDesign.Design.ButtonColor.Tint(Lum: !FormDesign.Design.IsDarkTheme ? -7 : 7))
			: (MatchBackgroundColor ? BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 6 : -5) : FormDesign.Design.ButtonColor);
		var activeColor = backColor.GetTextColor();

		using var activeBrush = new SolidBrush(activeColor);

		using var backBrush = new SolidBrush(backColor);
		e.Graphics.FillRoundedRectangle(backBrush, client, Padding.Left);

		DrawFocus(e.Graphics, client, Padding.Left, active);

		var text = LocaleHelper.GetGlobalText(Text).One.ToUpper();
		using var font = UI.Font(7F, FontStyle.Bold).FitToWidth(text, client.Pad(Padding), e.Graphics);
		var textHeight = (int)e.Graphics.Measure(text, font).Height;
		using var img = ImageName?.Get(Height / 2)?.Color(activeColor);

		if (img == null)
		{
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(text, font, activeBrush, client, format);
		}
		else
		{
			var rect = client.CenterR(client.Width, textHeight + img.Height + (int)(2.5 * UI.FontScale));

			e.Graphics.DrawImage(img, rect.Align(img.Size, ContentAlignment.TopCenter));

			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
			e.Graphics.DrawString(text, font, activeBrush, rect, format);
		}
	}
}
