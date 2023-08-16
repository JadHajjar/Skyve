using Skyve.App.UserInterface.Content;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
internal class PC_ViewCollection : PC_GenericPackageList
{
	private readonly ulong _id;

	internal PC_ViewCollection(IPackage collection) : base(collection.Requirements, true)
	{
		_id = collection.Id;

		LC_Items.TLP_Main.SetColumn(LC_Items.P_FiltersContainer, 0);
		LC_Items.TLP_Main.SetColumnSpan(LC_Items.P_FiltersContainer, LC_Items.TLP_Main.ColumnCount);

		PB_Icon = new PackageIcon
		{
			Collection = true
		};
		PB_Icon.LoadImage(collection.GetWorkshopInfo()?.ThumbnailUrl, ServiceCenter.Get<IImageService>().GetImage);

		LC_Items.TLP_Main.Controls.Add(PB_Icon, 0, 0);
		LC_Items.TLP_Main.SetRowSpan(PB_Icon, 3);

		L_CollectionName = new Label
		{
			Text = collection.Name,
			AutoSize = true
		};
		LC_Items.TLP_Main.Controls.Add(L_CollectionName, 1, 0);
		LC_Items.TLP_Main.SetColumnSpan(L_CollectionName, LC_Items.TLP_Main.ColumnCount - 2);

		B_Steam = new SlickButton
		{
			ImageName = "I_Steam",
			AutoSize = true,
			Anchor = AnchorStyles.Right
		};
		B_Steam.Click += B_Steam_Click;
		LC_Items.TLP_Main.Controls.Add(B_Steam, LC_Items.TLP_Main.ColumnCount - 1, 0);
	}

	private void B_Steam_Click(object sender, EventArgs e)
	{
		PlatformUtil.OpenUrl($"https://steamcommunity.com/workshop/filedetails/?id={_id}");
	}

	public PackageIcon PB_Icon { get; }
	public Label L_CollectionName { get; }
	public SlickButton B_Steam { get; }

	protected override void UIChanged()
	{
		base.UIChanged();

		L_CollectionName.Font = UI.Font(12F, FontStyle.Bold);
		L_CollectionName.Margin = UI.Scale(new Padding(3, 3, 0, 5), UI.FontScale);
		PB_Icon.Size = UI.Scale(new Size(64, 64), UI.FontScale);
		PB_Icon.Margin = UI.Scale(new Padding(5, 0, 0, 5), UI.FontScale);
		B_Steam.Margin = UI.Scale(new Padding(0, 0, 5, 0), UI.FontScale);
	}

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = Locale.CollectionTitle;
	}
}
