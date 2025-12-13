using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public partial class WorkshopTagsContainer : SlickControl
{
	public static List<IWorkshopTag> Tags => WorkshopTagsControl.Tags;
	public static List<string> SelectedTags => WorkshopTagsControl.SelectedTags;

	public event EventHandler SelectedTagChanged { add => tagControl.SelectedTagChanged += value; remove => tagControl.SelectedTagChanged -= value; }

	public void SetTags(IEnumerable<IWorkshopTag> tags)
	{
		tagControl.SetTags(tags);

		this.TryInvoke(SetControlState);
	}

	public WorkshopTagsContainer()
	{
		InitializeComponent();

		Dock = DockStyle.Right;
		Margin = default;
		tagControl.SelectedTagChanged += TagControl_SelectedTagChanged;
		title.Text = LocaleSlickUI.Tags.Plural.ToUpper();
		buttonClear.Text = LocaleSlickUI.Clear.ToUpper();
		TagControl_SelectedTagChanged(this, EventArgs.Empty);
		SetControlState();
	}

	private void SetControlState()
	{
		if (Tags.Count == 0)
		{
			tagControl.Loading = true;
			tagControl.Dock = DockStyle.Fill;
		}
		else
		{
			tagControl.Loading = false;
			tagControl.Dock = DockStyle.None;
			scroll.LinkedControl = tagControl;
		}
	}

	private void TagControl_SelectedTagChanged(object sender, EventArgs e)
	{
		title.Text = Locale.TagCount.FormatPlural(SelectedTags.Count).ToUpper();

		if (SelectedTags.Count > 0)
		{
			tableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.AutoSize;
			title.Anchor = AnchorStyles.Left;
		}
		else
		{
			tableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.Absolute;
			title.Anchor = AnchorStyles.None;
		}

		scroll_Scroll(sender, null);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(200);
		slickSpacer1.Width = UI.Scale(2);
		title.Font = UI.Font(6F, FontStyle.Bold);
		title.Margin = UI.Scale(new Padding(4));
		buttonClear.Font = UI.Font(6F);
		buttonClear.Padding = UI.Scale(new Padding(2, 2, 2, 1));
		buttonClear.Margin = UI.Scale(new Padding(2,2,4,2));
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = scroll.Percentage > 0 ? design.BackColor.Tint(Lum: design.IsDarkTheme ? 6 : -6) : design.AccentBackColor;
		scroll.BackColor = panel1.BackColor = design.AccentBackColor;
		title.ForeColor = design.LabelColor;
	}

	private void buttonClear_Click(object sender, EventArgs e)
	{
		tagControl.ClearTags();
	}

	private void scroll_Scroll(object sender, ScrollEventArgs? e)
	{
		var backColor = SelectedTags.Count > 0|| scroll.Percentage > 0 ? FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 6 : -6) : FormDesign.Design.AccentBackColor;

		if (BackColor != backColor)
		{
			BackColor = backColor;
		}
	}
}
