using System.Windows.Forms;

namespace Skyve.App.UserInterface.Forms;
public partial class EditTagsForm : BaseForm
{
	private readonly ITagsService _tagsService = ServiceCenter.Get<ITagsService>();
	private readonly List<ITag> _tags;

	public List<IPackageIdentity> Packages { get; }

	public EditTagsForm(IEnumerable<IPackageIdentity> packages)
	{
		InitializeComponent();

		Packages = packages.ToList();

		if (L_MultipleWarning.Visible = Packages.Count > 1)
		{
			Text = Locale.TagsTitle.FormatPlural(Packages.Count);

			L_MultipleWarning.Text = Locale.EditingMultipleTags;
		}
		else
		{
			Text = Locale.TagsTitle.Format(Packages[0].ToString());
		}

		_tags = new(Packages.SelectMany(x => x.GetTags().Where(x => x.IsCustom)).Distinct(x => x.Value));

		TLC.Tags.AddRange(_tags);
		TLC.AllTags.AddRange(_tagsService.GetDistinctTags().OrderByDescending(x => _tags.Any(t => t.Value == x.Value)).ThenByDescending(x => x.IsCustom).ThenByDescending(_tagsService.GetTagUsage));
		TLC.Height = 1;
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		L_MultipleWarning.ForeColor = design.YellowColor.MergeColor(design.RedColor);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		TB_NewTag.Margin = UI.Scale(new Padding(7), UI.FontScale);
		L_MultipleWarning.Margin = UI.Scale(new Padding(4), UI.FontScale);
		L_MultipleWarning.Font = UI.Font(7.5F);
		B_Apply.Margin = UI.Scale(new Padding(0, 0, 10, 10), UI.FontScale);
	}

	private void B_Apply_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;

		foreach (var package in Packages)
		{
			ServiceCenter.Get<ITagsService>().SetTags(package, TLC.Tags.Select(x => x.Value).Distinct());
		}

		Close();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			Close();
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void TB_NewTag_IconClicked(object sender, EventArgs e)
	{
		var current = TLC.AllTags.FirstOrDefault(x => x.Value.Equals(TB_NewTag.Text, StringComparison.OrdinalIgnoreCase));

		if (current is not null)
		{
			TLC.Tags.Insert(0, current);
		}
		else
		{
			current = _tagsService.CreateCustomTag(TB_NewTag.Text);

			TLC.Tags.Insert(0, current);
			TLC.AllTags.Insert(0, current);
		}

		TB_NewTag.Text = string.Empty;
		TLC.Invalidate();
	}

	private void TB_NewTag_TextChanged(object sender, EventArgs e)
	{
		TLC.CurrentSearch = TB_NewTag.Text;
		TLC.Invalidate();
	}
}
