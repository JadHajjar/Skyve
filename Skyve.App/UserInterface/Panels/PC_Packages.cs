namespace Skyve.App.UserInterface.Panels;
internal class PC_Packages : PC_ContentList<ILocalPackageWithContents>
{
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();
	private readonly IPackageManager _contentManager = ServiceCenter.Get<IPackageManager>();

	public PC_Packages()
	{
	}

	public override SkyvePage Page => SkyvePage.Packages;

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = $"{Locale.Package.Plural} - {ServiceCenter.Get<IPlaysetManager>().CurrentPlayset.Name}";
	}

	protected override IEnumerable<ILocalPackageWithContents> GetItems()
	{
		if (_settings.UserSettings.FilterOutPackagesWithOneAsset || _settings.UserSettings.FilterOutPackagesWithMods)
		{
			return _contentManager.Packages.Where(x =>
			{
				if (_settings.UserSettings.FilterOutPackagesWithOneAsset && (x.Assets?.Count() ?? 0) == 1)
				{
					return false;
				}

				if (_settings.UserSettings.FilterOutPackagesWithMods && x.Mod is not null)
				{
					return false;
				}

				return true;
			});
		}

		return _contentManager.Packages;
	}

	protected override string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in _contentManager.Packages)
		{
			if (item.IsIncluded())
			{
				packagesIncluded++;

				if (item.Mod is not null)
				{
					modsIncluded++;

					if (item.Mod.IsEnabled())
					{
						modsEnabled++;
					}
				}
			}
		}

		var total = LC_Items.ItemCount;

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			return string.Format(Locale.PackageIncludedTotal, packagesIncluded, total);
		}

		if (modsIncluded == modsEnabled)
		{
			return string.Format(Locale.PackageIncludedAndEnabledTotal, packagesIncluded, total);
		}

		return string.Format(Locale.PackageIncludedEnabledTotal, packagesIncluded, modsIncluded, modsEnabled, total);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
