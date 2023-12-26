using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_ViewSpecificPackages : PC_Packages
{
	private readonly List<ILocalPackageData> _packages;
	private readonly string _title;

	public PC_ViewSpecificPackages(List<ILocalPackageData> packages, string title)
    {
		_packages = packages;
		_title = title;

		LC_Items.RefreshItems();
	}

	protected override IEnumerable<ILocalPackageData> GetItems()
	{
		return _packages ?? new();
	}

	protected override void LocaleChanged()
	{
		Text = _title;
	}
}
