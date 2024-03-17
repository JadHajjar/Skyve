using Skyve.App.UserInterface.Content;

using SlickControls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_WorkshopPackageSelection : PC_WorkshopList
{
	public event Action<IEnumerable<ulong>>? PackageSelected;

	public PC_WorkshopPackageSelection(IEnumerable<ulong>? selectedItems = null)
	{
		InitializeComponent();

		P_Packages.ControlAdded += P_Packages_ControlChanged;
		P_Packages.ControlRemoved += P_Packages_ControlChanged;

		LC_Items.OT_ModAsset.SelectedValue = Generic.ThreeOptionToggle.Value.Option1;
		LC_Items.ListControl.IsSelection = true;
		LC_Items.ListControl.PackageSelected += ListControl_PackageSelected;
		LC_Items.ListControl.PackageUnSelected += ListControl_PackageUnSelected;

		if (selectedItems != null)
		{
			LC_Items.ListControl ._selectionList.AddRange(selectedItems);

			foreach (var item in selectedItems)
			{
				ListControl_PackageSelected(new GenericPackageIdentity(item));
			}
		}
	}

	private void P_Packages_ControlChanged(object sender, ControlEventArgs e)
	{
		L_SelectedPackages.Text = Locale.PackagesSelected.Format(P_Packages.Controls.Count);

		L_Info.Visible = P_Packages.Controls.Count == 0;
		base_TLP_Side.RowStyles[1].Height = P_Packages.Controls.Count == 0 ? 100F : 0F;
		base_TLP_Side.RowStyles[2].Height = P_Packages.Controls.Count == 0 ? 0F : 100F;
	}

	private void ListControl_PackageUnSelected(IPackageIdentity obj)
	{
		P_Packages.Controls.Clear(true, x => (x as MiniPackageControl)?.Id == obj.Id);
	}

	private void ListControl_PackageSelected(IPackageIdentity obj)
	{
		P_Packages.Controls.Add(new MiniPackageControl(obj.Id)
		{
			Dock = DockStyle.Top,
			Large = true
		});
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		base_TLP_Side.BackColor = design.MenuColor;
		base_TLP_Side.ForeColor = design.MenuForeColor;

		L_Info.ForeColor = design.InfoColor;
	}

	protected override void UIChanged()
	{
		base_P_Side.Width = (int)(200 * UI.FontScale);
		base_TLP_Side.Padding = UI.Scale(new Padding(5), UI.FontScale);
		base_P_Side.Padding = UI.Scale(new Padding(0, 5, 5, 5), UI.FontScale);
		LC_Items.Padding = new Padding(0, (int)(30 * UI.FontScale), 0, 0);
		CustomTitleBounds = new Point( (int)(200 * UI.FontScale), 0);

		base.UIChanged();

		B_Confirm.Margin = UI.Scale(new Padding(5), UI.FontScale);
		B_Confirm.Font = UI.Font(9F);
		L_Info.Font = UI.Font(8F, FontStyle.Italic);
		L_SelectedPackages.Font = UI.Font(9F, FontStyle.Bold);
		L_Info.Margin = UI.Scale(new Padding(5), UI.FontScale);
		L_SelectedPackages.Margin = UI.Scale(new Padding(5), UI.FontScale);
	}

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = LocaleCR.AddPackages;
		L_SelectedPackages.Text = Locale.PackagesSelected.Format(P_Packages.Controls.Count);
		L_Info.Text = Locale.PackagesSelectionInfo;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (Form != null)
		{
			if (base_P_Side.Visible)
			{
				Form.base_TLP_Side.TopRight = true;
				Form.base_TLP_Side.BotRight = true;
				Form.base_TLP_Side.Invalidate();
			}

			LC_Items.MouseDown += (s, e) => Form.ForceWindowMove(e);
		}

		base_P_Side.SendToBack();
	}

	private void B_Confirm_Click(object sender, EventArgs e)
	{
		PushBack();
		PackageSelected?.Invoke(P_Packages.Controls.OfType<MiniPackageControl>().Select(x => x.Id));
	}
}
