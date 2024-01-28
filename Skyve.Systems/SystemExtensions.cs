using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Drawing;

namespace Skyve.Systems;
public static class SystemExtensions
{
	private static IWorkshopService? _workshopService;
	private static ISkyveDataManager? _skyveDataManager;
	private static ICompatibilityManager? _compatibilityManager;
	private static IImageService? _imageService;
	private static IPackageNameUtil? _packageNameUtil;
	private static IPackageManager? _packageManager;
	private static IPackageUtil? _packageUtil;
	private static ITagsService? _tagService;

	private static IWorkshopService WorkshopService => _workshopService ??= ServiceCenter.Get<IWorkshopService>();
	private static ICompatibilityManager CompatibilityManager => _compatibilityManager ??= ServiceCenter.Get<ICompatibilityManager>();
	private static ISkyveDataManager SkyveDataManager => _skyveDataManager ??= ServiceCenter.Get<ISkyveDataManager>();
	private static IImageService ImageService => _imageService ??= ServiceCenter.Get<IImageService>();
	private static IPackageNameUtil PackageNameUtil => _packageNameUtil ??= ServiceCenter.Get<IPackageNameUtil>();
	private static IPackageManager PackageManager => _packageManager ??= ServiceCenter.Get<IPackageManager>();
	private static IPackageUtil PackageUtil => _packageUtil ??= ServiceCenter.Get<IPackageUtil>();
	private static ITagsService TagsService => _tagService ??= ServiceCenter.Get<ITagsService>();

	public static string CleanName(this IPackageIdentity? package, bool keepTags = false)
	{
		return PackageNameUtil.CleanName(package, keepTags);
	}

	public static string CleanName(this IPackageIdentity? package, out List<(Color Color, string Text)> tags, bool keepTags = false)
	{
		return PackageNameUtil.CleanName(package, out tags, keepTags);
	}

	public static bool IsLocal(this IPackageIdentity identity)
	{
		return identity.Id <= 0;
	}

	public static bool IsIncluded(this IPackageIdentity package)
	{
		return PackageUtil.IsIncluded(package);
	}

	public static bool IsIncluded(this IPackageIdentity package, out bool partiallyIncluded)
	{
		return PackageUtil.IsIncluded(package, out partiallyIncluded);
	}

	public static bool IsEnabled(this IPackageIdentity package)
	{
		return PackageUtil.IsEnabled(package);
	}

	public static IPackage? GetPackage(this IPackageIdentity identity)
	{
		if (identity is IPackage package)
		{
			return package;
		}

		if (identity is ILocalPackageData packageData && packageData.Package is not null)
		{
			return packageData.Package;
		}

		return PackageManager.GetPackageById(identity);
	}

	public static bool IsInstalled(this IPackageIdentity identity)
	{
		return PackageManager.GetPackageById(identity) != null;
	}

	public static ILocalPackageData? GetLocalPackage(this IPackageIdentity identity)
	{
		if (identity is ILocalPackageData packageData)
		{
			return packageData;
		}

		if (identity is IPackage package && package.LocalData is not null)
		{
			return package.LocalData;
		}

		return PackageManager.GetPackageById(identity)?.LocalData;
	}

	public static ILocalPackageIdentity? GetLocalPackageIdentity(this IPackageIdentity identity)
	{
		if (identity is ILocalPackageIdentity packageData)
		{
			return packageData;
		}

		if (identity is IPackage package && package.LocalData is not null)
		{
			return package.LocalData;
		}

		return PackageManager.GetPackageById(identity)?.LocalData;
	}

	public static Bitmap? GetThumbnail<T>(this T? identity) where T : IPackageIdentity
	{
		if (identity is IThumbnailObject thumbnailObject)
		{
			return GetThumbnail(thumbnailObject);
		}

		return GetThumbnail((IThumbnailObject?)identity?.GetWorkshopInfo());
	}

	public static Bitmap? GetThumbnail(this IThumbnailObject? thumbnailObject)
	{
		if (thumbnailObject is null)
		{
			return null;
		}

		if (thumbnailObject.GetThumbnail(ImageService, out var thumbnail, out var thumbnailUrl))
		{
			return thumbnail;
		}

		if (thumbnailUrl is null or "")
		{
			return null;
		}

		return ImageService.GetImage(thumbnailUrl, true).Result;
	}

	public static Bitmap? GetUserAvatar(this IPackageIdentity? package)
	{
		return (package?.GetWorkshopInfo()?.Author).GetUserAvatar();
	}

	public static Bitmap? GetUserAvatar(this IWorkshopInfo? workshopInfo)
	{
		return (workshopInfo?.Author).GetUserAvatar();
	}

	public static Bitmap? GetUserAvatar(this IUser? user)
	{
		var url = user?.AvatarUrl;

		return url is null or "" ? null : ImageService.GetImage(url, true).Result;
	}

	public static Bitmap? GetThumbnail(this IDlcInfo? dlc)
	{
		return dlc?.ThumbnailUrl is null or "" ? null : ImageService.GetImage(dlc.ThumbnailUrl, true, $"{dlc.Id}.png", false).Result;
	}

	public static IEnumerable<ITag> GetTags(this IPackageIdentity package, bool ignoreParent = false)
	{
		return TagsService.GetTags(package, ignoreParent);
	}

	public static IWorkshopInfo? GetWorkshopInfo(this IPackageIdentity identity)
	{
		return identity is IWorkshopInfo workshopInfo ? WorkshopService.GetInfo(identity) ?? workshopInfo : WorkshopService.GetInfo(identity);
	}

	public static IPackage? GetWorkshopPackage(this IPackageIdentity identity)
	{
		return identity is IWorkshopInfo and IPackage package ? package : WorkshopService.GetPackage(identity);
	}

	public static ICompatibilityInfo GetCompatibilityInfo(this IPackageIdentity package, bool noCache = false, bool cacheOnly = false)
	{
		return CompatibilityManager.GetCompatibilityInfo(package, noCache, cacheOnly);
	}

	public static IPackageCompatibilityInfo? GetPackageInfo(this IPackageIdentity package)
	{
		return SkyveDataManager.GetPackageCompatibilityInfo(package);
	}

	public static NotificationType GetNotification(this ICompatibilityInfo compatibilityInfo)
	{
		return CompatibilityManager.GetNotification(compatibilityInfo);
	}
}
