using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class TagControl : SlickButton
{
	public ITag? TagInfo { get; set; }
	public bool Display { get; set; }
	public bool ToAddPreview { get; set; }

	public TagControl()
	{
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);

		base.UIChanged();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (Display && TagInfo is not null && e.Button == MouseButtons.Left)
		{
			var workshopPanel = new PC_WorkshopList();

			workshopPanel.LC_Items.DD_Tags.SelectedItems = [TagInfo];

			Program.MainForm.PushPanel(workshopPanel);
		}
	}

	public override Size CalculateAutoSize(Size availableSize)
	{
		using var graphics = Graphics.FromHwnd(IntPtr.Zero);
		using var args = new ButtonDrawArgs
		{
			Control = this,
			Font = Font,
			Icon = TagInfo is null ? ImageName : HoverState.HasFlag(HoverState.Hovered) ? Display && !TagInfo.IsCustom ? "I_Copy" : "I_Disposable" : TagInfo.Icon,
			Image = image,
			Text = TagInfo?.Value,
			Padding = Padding,
			AvailableSize = MultiLine ? availableSize : default
		};

		PrepareLayout(graphics, args);

		return args.Rectangle.Size + (TagInfo is null ? new Size((int)(20*UI.FontScale),0): default);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		Draw(e.Graphics, new ButtonDrawArgs
		{
			Control = this,
			Font = Font,
			Icon = TagInfo is null ? ImageName : HoverState.HasFlag(HoverState.Hovered) ? Display && !TagInfo.IsCustom ? "I_Link" : "I_Disposable" : TagInfo.Icon,
			Image = image,
			Text = TagInfo?.Value,
			Padding = Padding,
			ButtonType = ButtonType,
			ColorShade = ColorShade,
			ColorStyle = ColorStyle,
			Enabled = Enabled,
			HoverState = HoverState,
			ColoredIcon = ColoredIcon,
			Rectangle = ClientRectangle,
			BackgroundColor = BackColor
		});
	}


	//protected override void OnPaint(PaintEventArgs e)
	//{
	//	e.Graphics.SetUp(BackColor);

	//	SlickButton.GetColors(out var fore, out var back, HoverState, !Display && ImageName is null ? ColorStyle.Red : ColorStyle.Active);

	//	using var brush = new SolidBrush(back);
	//	using var foreBrush = new SolidBrush(fore);

	//	e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1), Padding.Left);

	//	if (Live && ImageName is not null)
	//	{
	//		using var img = Image;
	//		e.Graphics.DrawImage(img.Color(FormDesign.Design.ButtonForeColor), ClientRectangle.CenterR(img.Size));
	//	}
	//	else if (TagInfo is not null)
	//	{
	//		using var img = IconManager.GetIcon(HoverState.HasFlag(HoverState.Hovered) ? Display && !TagInfo.IsCustom ? "I_Copy" : "I_Disposable" : TagInfo.Icon);
	//		e.Graphics.DrawImage(img.Color(FormDesign.Design.ButtonForeColor), ClientRectangle.Pad(Padding).Align(img.Size, ContentAlignment.MiddleLeft));

	//		e.Graphics.DrawString(TagInfo.Value, Font, foreBrush, ClientRectangle.Pad(Padding.Horizontal + img.Width, 0, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
	//	}

	//	DrawFocus(e.Graphics, ClientRectangle.Pad(1), Padding.Left, !Display && ImageName is null ? FormDesign.Design.RedColor : null);
	//}
}
