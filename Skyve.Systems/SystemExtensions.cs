﻿using SkyveApp.Domain;
using SkyveApp.Domain.Enums;
using SkyveApp.Domain.Systems;

using System.Collections.Generic;
using System.Drawing;

namespace SkyveApp.Systems;
public static class SystemExtensions
{
	private static IWorkshopService? _workshopService;
	private static ICompatibilityManager? _compatibilityManager;
	private static IImageService? _imageService;
	private static IPackageNameUtil? _packageNameUtil;
	private static IPackageManager? _packageManager;
	private static IPackageUtil? _packageUtil;
	private static ITagsService? _tagService;

	private static IWorkshopService WorkshopService => _workshopService ??= ServiceCenter.Get<IWorkshopService>();
	private static ICompatibilityManager CompatibilityManager => _compatibilityManager ??= ServiceCenter.Get<ICompatibilityManager>();
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

	public static bool IsIncluded(this ILocalPackage package)
	{
		return PackageUtil.IsIncluded(package);
	}

	public static bool IsIncluded(this ILocalPackage package, out bool partiallyIncluded)
	{
		return PackageUtil.IsIncluded(package, out partiallyIncluded);
	}

	public static bool IsEnabled(this ILocalPackage package)
	{
		return PackageUtil.IsEnabled(package);
	}

	public static ILocalPackage? GetLocalPackage(this IPackageIdentity package)
	{
		if (package is ILocalPackage local)
		{
			return local;
		}

		return PackageManager.GetPackageById(package);
	}

	public static Bitmap? GetThumbnail(this IPackageIdentity? package)
	{
		return (package?.GetWorkshopInfo()).GetThumbnail();
	}

	public static Bitmap? GetThumbnail(this IWorkshopInfo? workshopInfo)
	{
		var url = workshopInfo?.ThumbnailUrl;

		return url is null or "" ? null : ImageService.GetImage(url, true).Result;
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

	public static IEnumerable<ITag> GetTags(this IPackage package, bool ignoreParent = false)
	{
		return TagsService.GetTags(package, ignoreParent);
	}

	public static IWorkshopInfo? GetWorkshopInfo(this IPackageIdentity identity)
	{
		return WorkshopService.GetInfo(identity);
	}

	public static IPackage? GetWorkshopPackage(this IPackageIdentity identity)
	{
		return WorkshopService.GetPackage(identity);
	}

	public static ICompatibilityInfo GetCompatibilityInfo(this IPackage package)
	{
		return CompatibilityManager.GetCompatibilityInfo(package);
	}

	public static IPackageCompatibilityInfo? GetPackageInfo(this IPackage package)
	{
		return CompatibilityManager.GetPackageInfo(package);
	}

	public static NotificationType GetNotification(this ICompatibilityInfo compatibilityInfo)
	{
		return CompatibilityManager.GetNotification(compatibilityInfo);
	}
}
