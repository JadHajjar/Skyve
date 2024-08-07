﻿using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_Assets : PC_ContentList
{
	private readonly IPlaysetManager _profileManager = ServiceCenter.Get<IPlaysetManager>();
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();
	private readonly IPackageManager _contentManager = ServiceCenter.Get<IPackageManager>();

	public PC_Assets()
	{
		if (_settings.UserSettings.FilterIncludedByDefault)
		{
			LC_Items.OT_Included.SelectedValue = Generic.ThreeOptionToggle.Value.Option1;
		}
	}

	public override SkyvePage Page => SkyvePage.Assets;

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = $"{Locale.Asset.Plural} - {_profileManager.CurrentPlayset?.Name ?? Locale.NoActivePlayset}";
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		if (_settings.UserSettings.LinkModAssets)
		{
			return _contentManager.Assets.Where(x => !(x.GetLocalPackage()?.IsCodeMod ?? false));
		}

		return await Task.FromResult(_contentManager.Assets);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Asset;
	}
}
