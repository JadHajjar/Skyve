using Skyve.Compatibility.Domain.Enums;

namespace Skyve.App.UserInterface.Dropdowns;

public class SavegameEffectDropDown : PackageStatusTypeDropDown<SavegameEffect>
{
	public SavegameEffectDropDown() : base(false)
	{
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(200);
	}
}
