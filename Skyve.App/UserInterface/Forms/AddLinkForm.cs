

using Skyve.Compatibility.Domain;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Forms;
public partial class AddLinkForm : BaseForm
{
	public event Action<IEnumerable<PackageLink>>? LinksReturned;

	public AddLinkForm(List<ILink> links)
	{
		InitializeComponent();

		Text = LocaleHelper.GetGlobalText("Links");

		foreach (var link in links)
		{
			AddLink(link);
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		B_AddLink.Margin = UI.Scale(new Padding(10), UI.FontScale);
		B_Apply.Margin = UI.Scale(new Padding(0, 0, 10, 10), UI.FontScale);
	}

	private void B_AddLink_Click(object sender, EventArgs e)
	{
		AddLink(new PackageLink());
	}

	private void AddLink(ILink packageLink)
	{
		TLP.SuspendDrawing();

		TLP.RowStyles.Add(new());

		TLP.Controls.Add(new LinkControl(packageLink), 0, TLP.RowStyles.Count - 1);

		TLP.ResumeDrawing();
	}

	private IEnumerable<PackageLink> GetLinks()
	{
		return TLP.Controls.OfType<LinkControl>().Select(x => x.Link);
	}

	private void B_Apply_Click(object sender, EventArgs e)
	{
		LinksReturned?.Invoke(GetLinks());
		Close();
	}

	private class LinkControl : SmartTablePanel
	{
		private readonly SlickIcon icon;
		private readonly SlickTextBox tbLink;
		private readonly SlickTextBox tbName;
		private readonly SlickIcon deleteButton;
		private readonly ILink link;

		public LinkControl(ILink packageLink)
		{
			Dock = DockStyle.Top;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;

			link = packageLink;
			icon = new SlickIcon();
			tbLink = new SlickTextBox();
			tbName = new SlickTextBox();
			deleteButton = new SlickIcon();

			icon.ImageName = packageLink.Type.GetIcon();
			icon.Size = UI.Scale(new Size(24, 24), UI.FontScale);
			icon.Padding = UI.Scale(new Padding(5), UI.FontScale);
			icon.Margin = UI.Scale(new Padding(10, 3, 3, 3), UI.FontScale);
			icon.Click += Icon_Click;

			tbLink.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			tbLink.ShowLabel = false;
			tbLink.Placeholder = "Link URL";
			tbLink.Text = packageLink.Url;
			tbLink.TextChanged += TbLink_TextChanged;

			tbName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			tbName.ShowLabel = false;
			tbName.Placeholder = "Link Name";
			tbName.Text = packageLink.Title;

			deleteButton.Size = UI.Scale(new Size(24, 24), UI.FontScale);
			deleteButton.Padding = UI.Scale(new Padding(5), UI.FontScale);
			deleteButton.ImageName = "I_Trash";
			deleteButton.ColorStyle = ColorStyle.Red;
			deleteButton.Margin = UI.Scale(new Padding(3, 3, 10, 3), UI.FontScale);
			deleteButton.Click += (s, e) => Dispose();

			ColumnStyles.Add(new());
			ColumnStyles.Add(new(SizeType.Percent, 60));
			ColumnStyles.Add(new(SizeType.Percent, 40));
			ColumnStyles.Add(new());
			RowStyles.Add(new());
			Controls.Add(icon, 0, 0);
			Controls.Add(tbLink, 1, 0);
			Controls.Add(deleteButton, 3, 0);

			if (link.Type == LinkType.Other)
			{
				SetColumnSpan(tbLink, 1);
				Controls.Add(tbName, 2, 0);
			}
			else
			{
				Controls.Remove(tbName);
				SetColumnSpan(tbLink, 2);
			}
		}

		private void TbLink_TextChanged(object sender, EventArgs e)
		{
			if (tbLink.Text.StartsWith("https://steamcommunity.com/linkfilter/?url="))
			{
				tbLink.Text = tbLink.Text.Remove(0, "https://steamcommunity.com/linkfilter/?url=".Length);
			}
		}

		public PackageLink Link => new()
		{
			Type = link.Type,
			Title = tbName.Text,
			Url = tbLink.Text,
		};

		private void Icon_Click(object sender, EventArgs e)
		{
			var items = Enum.GetValues(typeof(LinkType)).Cast<LinkType>().Select(x => new SlickStripItem(x.ToString(), x.GetIcon(), action: () =>
			{
				link.Type = x;
				icon.ImageName = link.Type.GetIcon();

				if (link.Type == LinkType.Other)
				{
					SetColumnSpan(tbLink, 1);
					Controls.Add(tbName, 2, 0);
				}
				else
				{
					Controls.Remove(tbName);
					SetColumnSpan(tbLink, 2);
				}
			})).ToArray();

			SlickToolStrip.Show(FindForm() as AddLinkForm, icon.PointToScreen(new Point(icon.Width + 5, 0)), items);
		}
	}
}
