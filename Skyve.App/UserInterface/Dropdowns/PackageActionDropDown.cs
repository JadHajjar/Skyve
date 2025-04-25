using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class PackageActionDropDown : PackageStatusTypeDropDown<StatusAction>
{
	public bool IsFlipped { get; set; }

    public PackageActionDropDown() : base(false)
    {
        
    }

	protected override LocaleHelper.Translation GetText(StatusAction item)
	{
		return LocaleCR.Get(!IsFlipped || item is StatusAction.NoAction ? item.ToString() : $"Flipped{item}");
	}
}
