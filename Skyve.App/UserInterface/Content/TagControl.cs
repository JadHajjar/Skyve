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
			Text = TagInfo?.Value,
			Padding = Padding,
			AvailableSize = MultiLine ? availableSize : default
		};

		PrepareLayout(graphics, args);

		return args.Rectangle.Size + (TagInfo is null ? new Size((int)(20 * UI.FontScale), 0) : default);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		Draw(e.Graphics, new ButtonDrawArgs
		{
			Control = this,
			Font = Font,
			Icon = TagInfo is null ? ImageName : HoverState.HasFlag(HoverState.Hovered) ? Display ? "I_Link" : "I_Disposable" : TagInfo.Icon,
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
}
