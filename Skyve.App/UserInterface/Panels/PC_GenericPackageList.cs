using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_GenericPackageList : PC_ContentList
{
	private readonly List<IPackageIdentity> _items = new();
	private readonly INotifier _notifier = ServiceCenter.Get<INotifier>();

	public override SkyvePage Page => SkyvePage.Generic;

	public PC_GenericPackageList(IEnumerable<IPackageIdentity> items, bool groupItems) : base(true, true)
	{
		LC_Items.IsGenericPage = true;

		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";

		var compatibilityManager = ServiceCenter.Get<ICompatibilityManager>();

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
					if (compatibilityManager.IsBlacklisted(packages.First()))
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

		_notifier.WorkshopInfoUpdated += _notifier_WorkshopPackagesInfoLoaded;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notifier.WorkshopInfoUpdated -= _notifier_WorkshopPackagesInfoLoaded;
		}

		base.Dispose(disposing);
	}

	private void _notifier_WorkshopPackagesInfoLoaded()
	{
		LC_Items.ListControl.Invalidate();
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems()
	{
		return await Task.FromResult(_items);
	}

	protected override string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in _items)
		{
			var package = item.GetLocalPackage();

			if (package is null)
			{
				continue;
			}

			if (package.IsIncluded())
			{
				packagesIncluded++;

				if (package.Package.IsCodeMod)
				{
					modsIncluded++;

					if (package.IsEnabled())
					{
						modsEnabled++;
					}
				}
			}
		}

		var total = LC_Items.ItemCount;

		if (!ServiceCenter.Get<ISettings>().UserSettings.AdvancedIncludeEnable)
		{
			return string.Format(Locale.PackageIncludedTotal, packagesIncluded, total);
		}

		return modsIncluded == modsEnabled
			? string.Format(Locale.PackageIncludedAndEnabledTotal, packagesIncluded, total)
			: string.Format(Locale.PackageIncludedEnabledTotal, packagesIncluded, modsIncluded, modsEnabled, total);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
