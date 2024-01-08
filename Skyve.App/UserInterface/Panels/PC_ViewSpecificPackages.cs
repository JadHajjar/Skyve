namespace Skyve.App.UserInterface.Panels;
public class PC_ViewSpecificPackages : PC_ContentList
{
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();
	private readonly IPackageManager _contentManager = ServiceCenter.Get<IPackageManager>();
	private readonly List<IPackageIdentity> _packages;
	private readonly string _title;

	public override SkyvePage Page => SkyvePage.Packages;

	public PC_ViewSpecificPackages(List<IPackageIdentity> packages, string title)
	{
		_packages = packages;
		_title = title;

		LC_Items.RefreshItems();
	}

	protected override IEnumerable<IPackageIdentity> GetItems()
	{
		return _packages ?? new();
	}

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = _title;
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}

	protected override string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in _contentManager.Packages)
		{
			if (item.IsIncluded())
			{
				packagesIncluded++;

				if (item.IsCodeMod)
				{
					modsIncluded++;

					if (item.IsEnabled())
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
}
