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
		Margin = UI.Scale(new Padding(3), UI.FontScale);

		base.UIChanged();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (Display && TagInfo is not null && e.Button == MouseButtons.Left)
		{
			if (TagInfo.IsCustom)
			{
				var packagesPage = new PC_Packages();

				packagesPage.LC_Items.DD_Tags.SelectedItems = [TagInfo];

				Program.MainForm.PushPanel(packagesPage);
			}
			else
			{
				var workshopPanel = new PC_WorkshopList();

				workshopPanel.LC_Items.DD_Tags.SelectedItems = [TagInfo];

				Program.MainForm.PushPanel(workshopPanel);
			}
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
			Text = TagInfo?.Value ?? "X",
			Padding = Padding,
			AvailableSize = MultiLine ? availableSize : default
		};

		PrepareLayout(graphics, args);

		return args.Rectangle.Size + (TagInfo is null ? new Size((int)(20 * UI.FontScale), 0) : default);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		var arg = new ButtonDrawArgs
		{
			Control = this,
			Font = Font,
			Icon = TagInfo is null ? null : HoverState.HasFlag(HoverState.Hovered) ? Display ? "I_Link" : "I_Trash" : TagInfo.Icon,
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
		};

		Draw(e.Graphics, arg);

		if (TagInfo is not null)
		{
			return;
		}

		using var icon1 = IconManager.GetIcon("I_Edit", Font.Height * 5 / 4);
		using var icon2 = IconManager.GetIcon("I_Slash", Font.Height * 5 / 4);
		using var icon3 = IconManager.GetIcon("I_Add", Font.Height * 5 / 4);

		var rect2 = ClientRectangle.CenterR(icon2.Size);
		var rect1 = rect2;
		var rect3 = rect2;

		rect1.X -= rect1.Width;
		rect3.X += rect3.Width + Padding.Top;
		rect3.Y += 1;

		e.Graphics.DrawImage(icon1.Color(arg.ForeColor), rect1);
		e.Graphics.DrawImage(icon2.Color(arg.ForeColor, 100), rect2);
		e.Graphics.DrawImage(icon3.Color(arg.ForeColor), rect3);
	}
}
