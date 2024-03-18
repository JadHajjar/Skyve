using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_GenericPackageList : PC_ContentList
{
	private readonly List<IPackageIdentity> _items = [];
	private readonly INotifier _notifier = ServiceCenter.Get<INotifier>();

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
			foreach (var packages in items.GroupBy(x => x.Id))
			{
				if (packages.Key != 0)
				{
					if (skyveDataManager.IsBlacklisted(packages.First()))
					{
						continue;
					}

					var package = packages.Last();

					if (package.GetWorkshopInfo()?.IsRemoved == true)
					{
						continue;
					}

					_items.Add(package);
				}
				else
				{
					foreach (var item in packages)
					{
						_items.Add(item);
					}
				}
			}
		}
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems()
	{
		return await Task.FromResult(_items);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
