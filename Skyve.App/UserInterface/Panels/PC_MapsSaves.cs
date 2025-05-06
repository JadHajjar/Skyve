using Skyve.Compatibility.Domain.Enums;

using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_MapsSaves : PC_ContentList
{
	private readonly IPlaysetManager _playsetManager;
	private readonly ISettings _settings;
	private readonly IPackageManager _contentManager;
	private readonly IModLogicManager _modLogicManager;

	public PC_MapsSaves()
	{
		ServiceCenter.Get(out _settings, out _playsetManager, out _contentManager, out _modLogicManager);

		if (_settings.UserSettings.FilterIncludedByDefault)
		{
			LC_Items.OT_Included.SelectedValue = Generic.ThreeOptionToggle.Value.Option1;
		}
	}

	public override SkyvePage Page => SkyvePage.Assets;

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = $"{LocaleCR.MapSavegame.Plural} - {_playsetManager.CurrentPlayset?.Name ?? Locale.NoActivePlayset}";
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		var assets = new List<IPackageIdentity>(_contentManager.SaveGames);

		foreach (var package in _contentManager.Packages)
		{
			if (package.LocalData is null || _modLogicManager.IsPseudoMod(package))
			{
				continue;
			}

			var hasAssets = false;

			foreach (var item in package.LocalData.Assets)
			{
				if (item.AssetType is AssetType.SaveGame or AssetType.Map)
				{
					hasAssets = true;

					assets.Add(item);
				}
			}

			if (!hasAssets && package.GetPackageInfo()?.Type is PackageType.MapSavegame)
			{
				assets.Add(package);
			}
		}

		return await Task.FromResult(assets);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Asset;
	}
}
