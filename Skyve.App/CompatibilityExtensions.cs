using System.Drawing;

namespace Skyve.App;

public static class CompatibilityExtensions
{
	public static DynamicIcon GetIcon(this IPlayset profile)
	{
		return profile.Temporary ? (DynamicIcon)"I_TempProfile" : profile.Usage.GetIcon();
	}

	public static DynamicIcon GetIcon(this PackageUsage usage)
	{
		return usage switch
		{
			PackageUsage.CityBuilding => "I_City",
			PackageUsage.AssetCreation => "I_Tools",
			PackageUsage.MapCreation => "I_Map",
			PackageUsage.ScenarioMaking => "I_ScenarioMaking",
			PackageUsage.ThemeMaking => "I_Paint",
			_ => "I_ProfileSettings"
		};
	}

	public static ICompatibilityInfo GetCompatibilityInfo(this IPackage package, bool noCache = false, bool cacheOnly = false)
	{
		var manager = ServiceCenter.Get<ICompatibilityManager>();

		return manager.GetCompatibilityInfo(package, noCache, cacheOnly);
	}

	public static DynamicIcon GetIcon(this LinkType link)
	{
		return link switch
		{
			LinkType.Website => "I_Globe",
			LinkType.Github => "I_Github",
			LinkType.Crowdin => "I_Translate",
			LinkType.Donation => "I_Donate",
			LinkType.Discord => "I_Discord",
			_ => "I_Share",
		};
	}

	public static DynamicIcon GetIcon(this NotificationType notification, bool status)
	{
		return notification switch
		{
			NotificationType.Info => "I_Info",
			NotificationType.MissingDependency => "I_MissingMod",
			NotificationType.Caution => "I_Remarks",
			NotificationType.Warning => "I_MinorIssues",
			NotificationType.AttentionRequired => "I_MajorIssues",
			NotificationType.Switch => "I_Switch",
			NotificationType.Unsubscribe => "I_Broken",
			NotificationType.Exclude => "I_X",
			NotificationType.RequiredItem => "I_Important",
			NotificationType.None or _ => status ? "I_Ok" : "I_Info",
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

			NotificationType.Exclude or
			NotificationType.Unsubscribe => FormDesign.Design.RedColor,

			NotificationType.Switch => FormDesign.Design.RedColor.Tint(FormDesign.Design.RedColor.GetHue() - 10),

			_ => FormDesign.Design.GreenColor
		};
	}
}
