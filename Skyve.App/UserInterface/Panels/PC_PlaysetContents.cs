using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_PlaysetContents : PC_ContentList
{
	private readonly IPlaysetManager _playsetManager = ServiceCenter.Get<IPlaysetManager>();

	public override SkyvePage Page => SkyvePage.Generic;

	public IPlayset Playset { get; }

	public PC_PlaysetContents(IPlayset playset) : base(true, true)
	{
		Playset = playset;

		Text = Playset.Name;
		LC_Items.IsGenericPage = true;
		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems()
	{
		return await _playsetManager.GetPlaysetContents(Playset);
	}

	protected override string GetCountText()
	{
		int packagesIncluded = 0, modsIncluded = 0, modsEnabled = 0;

		foreach (var item in LC_Items.Items)
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
