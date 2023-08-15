﻿using SkyveApp.Systems.CS1.Utilities;

namespace SkyveApp.UserInterface.Panels;
internal class PC_Mods : PC_ContentList<IMod>
{
	public PC_Mods()
	{

	}

	public override SkyvePage Page => SkyvePage.Mods;

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = $"{Locale.Mod.Plural} - {ServiceCenter.Get<IPlaysetManager>().CurrentPlayset.Name}";
	}

	protected override IEnumerable<IMod> GetItems()
	{
		return ServiceCenter.Get<IPackageManager>().Mods;
	}

	protected override string GetCountText()
	{
		var mods = LC_Items.Items;
		var modsIncluded = mods.Count(x => x.IsIncluded());
		var modsEnabled = mods.Count(x => x.IsEnabled() && x.IsIncluded());
		var total = LC_Items.ItemCount;

		if (!ServiceCenter.Get<ISettings>().UserSettings.AdvancedIncludeEnable)
		{
			return string.Format(Locale.ModIncludedTotal, modsIncluded, total);
		}

		if (modsIncluded == modsEnabled)
		{
			return string.Format(Locale.ModIncludedAndEnabledTotal, modsIncluded, total);
		}

		return string.Format(Locale.ModIncludedEnabledTotal, modsIncluded, modsEnabled, total);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Mod;
	}
}
