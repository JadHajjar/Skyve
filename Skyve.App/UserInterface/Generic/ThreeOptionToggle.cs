using SkyveApp.Systems.CS1.Utilities;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Generic;
public class ThreeOptionToggle : SlickControl, ISupportsReset
{
	private Value _selectedValue;

	public enum Value
	{
		None = 0,
		Option1 = 1,
		Option2 = 2
	}

	public event EventHandler? SelectedValueChanged;

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Value SelectedValue { get => _selectedValue; set { _selectedValue = value; Invalidate(); SelectedValueChanged?.Invoke(this, EventArgs.Empty); } }
	[Category("Appearance"), DefaultValue("")]
	public string Option1 { get; set; } = string.Empty;
	[Category("Appearance"), DefaultValue("")]
	public string Option2 { get; set; } = string.Empty;
	[Category("Appearance"), DefaultValue(ColorStyle.Red)]
	public ColorStyle OptionStyle2 { get; set; } = ColorStyle.Red;
	[Category("Appearance"), DefaultValue(ColorStyle.Green)]
	public ColorStyle OptionStyle1 { get; set; } = ColorStyle.Green;
	[Category("Appearance"), DefaultValue(null)]
	public string? Image1 { get; set; }
	[Category("Appearance"), DefaultValue(null)]
	public string? Image2 { get; set; }

	public ThreeOptionToggle()
	{
		Cursor = Cursors.Hand;
	}

	protected override void UIChanged()
	{
		if (Live)
		{
			Font = UI.Font(8.25F);
			Padding = UI.Scale(new Padding(5), UI.FontScale);
			if (!Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom))
			{
				Height = (int)(24 * UI.UIScale);
			}
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);

		if (Live && !Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom))
		{
			Height = (int)(24 * UI.UIScale);
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		var centerWidth = Math.Max(Width / 5, (int)(40 * UI.FontScale));
		var option1Hovered = e.Location.X < (Width - centerWidth) / 2;
		var option2Hovered = e.Location.X > (Width + centerWidth) / 2;

		if (option1Hovered && SelectedValue != Value.Option1)
		{
			SelectedValue = Value.Option1;
		}
		else if (option2Hovered && SelectedValue != Value.Option2)
		{
			SelectedValue = Value.Option2;
		}
		else
		{
			SelectedValue = Value.None;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var centerWidth = Math.Max(Width / 5, (int)(40 * UI.FontScale));
		var option1Hovered = e.Location.X < (Width - centerWidth) / 2;
		var option2Hovered = e.Location.X > (Width + centerWidth) / 2;

		if (option1Hovered)
		{
			SlickTip.SetTo(this, Option1);
		}
		else if (option2Hovered)
		{
			SlickTip.SetTo(this, Option2, offset: new Point((Width + centerWidth) / 2, 0));
		}
		else
		{
			SlickTip.SetTo(this, Locale.AnyStatus, offset: new Point((Width - centerWidth) / 2, 0));
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var iconOnly = Width < 200 * UI.FontScale;
		var centerWidth = Math.Max(Width / 5, (int)(40 * UI.FontScale));
		var cursorLocation = PointToClient(Cursor.Position);
		var rectangle1 = new Rectangle(0, 0, (Width - centerWidth) / 2, Height - 1);
		var rectangle2 = new Rectangle((Width + centerWidth) / 2, 0, (Width - centerWidth) / 2, Height - 1);
		var rectangleNone = new Rectangle((Width - centerWidth) / 2, 0, centerWidth, Height - 1);
		var option1Hovered = rectangle1.Contains(cursorLocation) && HoverState.HasFlag(HoverState.Hovered);
		var option2Hovered = rectangle2.Contains(cursorLocation) && HoverState.HasFlag(HoverState.Hovered);
		var noneHovered = rectangleNone.Contains(cursorLocation) && HoverState.HasFlag(HoverState.Hovered);
		var textColor1 = SelectedValue == Value.Option1 || (option1Hovered && HoverState.HasFlag(HoverState.Pressed)) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor;
		var textColor2 = SelectedValue == Value.Option2 || (option2Hovered && HoverState.HasFlag(HoverState.Pressed)) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor;
		var textColorNone = SelectedValue == Value.None || (noneHovered && HoverState.HasFlag(HoverState.Pressed)) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor;

		e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, 4, -4))), ClientRectangle, Padding.Left);

		// Option 1
		if (option1Hovered || SelectedValue == Value.Option1)
		{
			e.Graphics.FillRoundedRectangle(rectangle1.Gradient(Color.FromArgb(HoverState.HasFlag(HoverState.Pressed) || SelectedValue == Value.Option1 ? 255 : 100, OptionStyle1.GetColor()), 0.5F), rectangle1, Padding.Left, topRight: false, botRight: false);
		}

		if (Image1 != null)
		{
			using var img1 = IconManager.GetIcon(Image1, Height - Padding.Vertical)?.Color(textColor1);

			if (img1 != null)
			{
				e.Graphics.DrawImage(img1, iconOnly ? rectangle1.CenterR(img1.Size) : new Rectangle(Padding.Left, (Height - img1.Width) / 2, img1.Width, img1.Height));

				if (!iconOnly)
				{
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Option1), Font, new SolidBrush(textColor1), rectangle1.Pad(Padding).Pad(Padding.Left + img1.Width, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
				}
			}
		}

		// Option 2
		if (option2Hovered || SelectedValue == Value.Option2)
		{
			e.Graphics.FillRoundedRectangle(rectangle2.Gradient(Color.FromArgb(HoverState.HasFlag(HoverState.Pressed) || SelectedValue == Value.Option2 ? 255 : 100, OptionStyle2.GetColor()), 0.5F), rectangle2, Padding.Left, topLeft: false, botLeft: false);
		}

		if (Image2 != null)
		{
			using var img2 = IconManager.GetIcon(Image2, Height - Padding.Vertical)?.Color(textColor2);

			if (img2 != null)
			{
				e.Graphics.DrawImage(img2, iconOnly ? rectangle2.CenterR(img2.Size) : new Rectangle(Width - img2.Width - Padding.Right, (Height - img2.Height) / 2, img2.Width, img2.Height));

				if (!iconOnly)
				{
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Option2), Font, new SolidBrush(textColor2), rectangle2.Pad(Padding).Pad(0, 0, Padding.Right + img2.Width, 0).AlignToFontSize(Font, ContentAlignment.MiddleRight), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far });
				}
			}
		}

		// Center
		if (noneHovered || SelectedValue == Value.None)
		{
			e.Graphics.FillRectangle(rectangleNone.Gradient(Color.FromArgb(HoverState.HasFlag(HoverState.Pressed) || SelectedValue == Value.None ? 255 : 100, FormDesign.Design.ActiveColor), 0.5F), rectangleNone);
		}

		using var slash = IconManager.GetIcon("I_Slash", Height - Padding.Vertical).Color(textColorNone);
		e.Graphics.DrawImage(slash, ClientRectangle.CenterR(slash.Size));

		if (!Enabled)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, BackColor)), ClientRectangle);
		}
	}

	public void ResetValue()
	{
		SelectedValue = Value.None;
	}
}
