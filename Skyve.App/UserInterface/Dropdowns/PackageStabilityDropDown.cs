using Skyve.Compatibility.Domain.Enums;

namespace Skyve.App.UserInterface.Dropdowns;

public class PackageStabilityDropDown : PackageStatusTypeDropDown<PackageStability>
{
	public PackageStabilityDropDown() : base(false)
	{
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(200);
	}
}
