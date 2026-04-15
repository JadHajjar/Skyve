using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;

public class PC_GenericPackageList : PC_ContentList
{
	private readonly List<IPackageIdentity> _items = [];

	public override SkyvePage Page => SkyvePage.Generic;

	public PC_GenericPackageList(IEnumerable<IPackageIdentity> items, bool groupItems) : base(true, true)
	{
		LC_Items.IsGenericPage = true;
		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";

		var skyveDataManager = ServiceCenter.Get<ISkyveDataManager>();

		if (!groupItems)
		{
			_items.AddRange(items);
		}
		else
		{
			foreach (var packages in items.GroupBy(x => x.GetKey()))
			{
				var package = packages.Last();

				if (package.Source == Defaults.WORKSHOP_SOURCE)
				{
					if (skyveDataManager.IsBlacklisted(package))
					{
						continue;
					}

					if (package.GetWorkshopInfo()?.IsRemoved == true)
					{
						continue;
					}
				}

				_items.Add(package);
			}
		}
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		return await Task.FromResult(_items);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
