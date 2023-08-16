namespace Skyve.App.UserInterface.Panels;
internal class PC_Assets : PC_ContentList<IAsset>
{
	private readonly IPlaysetManager _profileManager = ServiceCenter.Get<IPlaysetManager>();
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();
	private readonly IPackageManager _contentManager = ServiceCenter.Get<IPackageManager>();
	public PC_Assets()
	{
	}

	public override SkyvePage Page => SkyvePage.Assets;

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = $"{Locale.Asset.Plural} - {_profileManager.CurrentPlayset.Name}";
	}

	protected override IEnumerable<IAsset> GetItems()
	{
		if (_settings.UserSettings.LinkModAssets)
		{
			return _contentManager.Assets.Where(x => !(x.LocalParentPackage?.IsMod ?? false));
		}

		return _contentManager.Assets;
	}

	protected override string GetCountText()
	{
		var assetsIncluded = _contentManager.Assets.Count(x => x.IsIncluded());
		var total = LC_Items.ItemCount;
		var text = string.Empty;

		return string.Format(Locale.AssetIncludedTotal, assetsIncluded, total);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Asset;
	}
}
