namespace Skyve.App.UserInterface.Panels;
internal class PC_GenericPackageList : PC_ContentList<IPackage>
{
	private readonly List<IPackage> _items = new();
	private readonly INotifier _notifier = ServiceCenter.Get<INotifier>();

	public override SkyvePage Page => SkyvePage.Generic;

	public PC_GenericPackageList(IEnumerable<IPackageIdentity> items, bool groupItems) : base(true)
	{
		LC_Items.IsGenericPage = true;

		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";

		var compatibilityManager = ServiceCenter.Get<ICompatibilityManager>();

		if (!groupItems)
		{
			foreach (var item in items)
			{
				if (item is IPackage localPackage)
				{
					_items.Add(localPackage);
				}
				else
				{
					_items.Add(new GenericWorkshopPackage(item));
				}
			}
		}
		else
		{
			foreach (var package in items.GroupBy(x => x.Id))
			{
				if (package.Key != 0)
				{
					if (compatibilityManager.IsBlacklisted(package.First()))
					{
						continue;
					}

					var steamPackage = package.Last();

					if (steamPackage.GetWorkshopInfo()?.IsRemoved == true)
					{
						continue;
					}

					_items.Add(steamPackage is IPackage localPackage ? localPackage : new GenericWorkshopPackage(steamPackage));
				}
				else
				{
					foreach (var item in package)
					{
						if (item is IPackage localPackage)
						{
							_items.Add(localPackage);
						}
						else
						{
							_items.Add(new GenericWorkshopPackage(item));
						}
					}
				}
			}
		}

		_notifier.WorkshopInfoUpdated += _notifier_WorkshopPackagesInfoLoaded;

		LC_Items.RefreshItems();
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

	protected override IEnumerable<IPackage> GetItems()
	{
		return _items;
	}

	protected override string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in _items)
		{
			var package = item.LocalParentPackage;

			if (package is null)
			{
				continue;
			}

			if (package.IsIncluded())
			{
				packagesIncluded++;

				if (package.Mod is not null)
				{
					modsIncluded++;

					if (package.Mod.IsEnabled())
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
