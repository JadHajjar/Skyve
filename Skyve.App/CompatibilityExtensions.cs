using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;

namespace Skyve.App;

public static class CompatibilityExtensions
{
	private static readonly ICompatibilityManager _manager = ServiceCenter.Get<ICompatibilityManager>();

	public static DynamicIcon GetIcon(this ICustomPlayset profile)
	{
#if CS2
		return profile.Usage.GetIcon();
#else
		return profile.Temporary ? (DynamicIcon)"TempProfile" : profile.Usage.GetIcon();
#endif
	}

	public static DynamicIcon GetIcon(this PackageUsage usage)
	{
		return usage switch
		{
			PackageUsage.CityBuilding => "City",
			PackageUsage.AssetCreation => "Tools",
			PackageUsage.MapCreation => "Map",
#if CS1
			PackageUsage.ScenarioMaking => "ScenarioMaking",
			PackageUsage.ThemeMaking => "Paint",
#endif
			_ => "Playsets"
		};
	}

	public static ICompatibilityInfo GetCompatibilityInfo(this IPackage package, bool noCache = false, bool cacheOnly = false)
	{
		return _manager.GetCompatibilityInfo(package, noCache, cacheOnly);
	}

	public static DynamicIcon GetIcon(this LinkType link)
	{
		return link switch
		{
			LinkType.Website => "Globe",
			LinkType.Github => "Github",
			LinkType.Crowdin => "Translate",
			LinkType.Donation => "Donate",
			LinkType.Discord => "Discord",
			LinkType.YouTube => "Youtube",
			LinkType.Paypal => "Paypal",
			LinkType.Twitch => "Twitch",
			LinkType.Patreon => "Patreon",
			LinkType.X => "TwitterX",
			LinkType.BuyMeACoffee => "BuyMeACoffee",
			LinkType.Kofi => "Kofi",
			LinkType.Gitlabs => "Gitlabs",
			LinkType.Paradox => "Paradox",
			_ => "Share",
		};
	}

	public static DynamicIcon GetIcon(this PackageType type)
	{
		return type switch
		{
			PackageType.ContentPackage => "Assets",
			PackageType.SimulationMod => "Processor",
			PackageType.VisualMod => "Palette",
			PackageType.MusicPack => "Music",
			PackageType.NameList => "Translate",
			_ => "Cog",
		};
	}

	public static DynamicIcon GetIcon(this NotificationType notification, bool status)
	{
		return notification switch
		{
			NotificationType.Info => "Info",
			NotificationType.MissingDependency => "MissingMod",
			NotificationType.Caution => "Remarks",
			NotificationType.Warning => "Hazard",
			NotificationType.AttentionRequired => "MajorIssues",
			//NotificationType.Switch => "Switch",
			NotificationType.Broken => "Broken",
			NotificationType.Obsolete => "Obsolete",
			NotificationType.ActionRequired => "Hammer",
			//NotificationType.Exclude => "X",
			NotificationType.RequiredItem => "Important",
			NotificationType.None or _ => status ? "Ok" : "Info",
		};
	}

	public static Color GetColor(this NotificationType notification)
	{
		return notification switch
		{
			NotificationType.Info => FormDesign.Design.InfoColor,

			NotificationType.Caution => FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.GreenColor, 60),

			NotificationType.MissingDependency => FormDesign.Design.YellowColor,

			NotificationType.Warning => FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.RedColor, 60),
			NotificationType.AttentionRequired => FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.RedColor, 30),
			NotificationType.ActionRequired => FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.RedColor, 45),

			NotificationType.Broken => FormDesign.Design.RedColor,

			NotificationType.Obsolete => FormDesign.Design.RedColor.Tint(FormDesign.Design.RedColor.GetHue() - 10),

			//NotificationType.Switch => FormDesign.Design.RedColor.Tint(FormDesign.Design.RedColor.GetHue() - 10),

			_ => FormDesign.Design.GreenColor
		};
	}
}
