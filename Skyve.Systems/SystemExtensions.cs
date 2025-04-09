using Extensions;

using Microsoft.Extensions.DependencyInjection;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Skyve.Systems;
public static class SystemExtensions
{
	private static IServiceProvider? serviceProvider;
	private static IWorkshopService? _workshopService;
	private static ISkyveDataManager? _skyveDataManager;
	private static ICompatibilityManager? _compatibilityManager;
	private static IImageService? _imageService;
	private static IPackageNameUtil? _packageNameUtil;
	private static IPackageManager? _packageManager;
	private static IPackageUtil? _packageUtil;
	private static ITagsService? _tagService;
	private static IPlaysetManager? _playsetManager;

	private static IWorkshopService WorkshopService => _workshopService ??= serviceProvider!.GetService<IWorkshopService>()!;
	private static ICompatibilityManager CompatibilityManager => _compatibilityManager ??= serviceProvider!.GetService<ICompatibilityManager>()!;
	private static ISkyveDataManager SkyveDataManager => _skyveDataManager ??= serviceProvider!.GetService<ISkyveDataManager>()!;
	private static IImageService ImageService => _imageService ??= serviceProvider!.GetService<IImageService>()!;
	private static IPackageNameUtil PackageNameUtil => _packageNameUtil ??= serviceProvider!.GetService<IPackageNameUtil>()!;
	private static IPackageManager PackageManager => _packageManager ??= serviceProvider!.GetService<IPackageManager>()!;
	private static IPackageUtil PackageUtil => _packageUtil ??= serviceProvider!.GetService<IPackageUtil>()!;
	private static ITagsService TagsService => _tagService ??= serviceProvider!.GetService<ITagsService>()!;
	private static IPlaysetManager PlaysetManager => _playsetManager ??= serviceProvider!.GetService<IPlaysetManager>()!;

	public static void Initialize(IServiceProvider provider)
	{
		serviceProvider = provider;
	}

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

	public static bool IsCodeMod(this IPackageIdentity identity)
	{
		if (identity.Id > 0 && identity.GetWorkshopInfo() is IWorkshopInfo workshopInfo)
		{
			return workshopInfo.IsCodeMod;
		}

		return identity.GetLocalPackage()?.IsCodeMod ?? false;
	}

	public static bool IsIncluded(this IPackageIdentity package, bool withVersion = true)
	{
		return PackageUtil.IsIncluded(package, withVersion: withVersion);
	}

	public static bool IsIncluded(this IPackageIdentity package, out bool partiallyIncluded, bool withVersion = true)
	{
		return PackageUtil.IsIncluded(package, out partiallyIncluded, withVersion: withVersion);
	}

	public static bool IsEnabled(this IPackageIdentity package, bool withVersion = true)
	{
		return PackageUtil.IsEnabled(package, withVersion: withVersion);
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

		return PackageManager.GetPackageById(identity)?.Package;
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

		return PackageManager.GetPackageById(identity);
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

		return PackageManager.GetPackageById(identity);
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

	public static ICustomPlayset GetCustomPlayset(this IPlayset playset)
	{
		return PlaysetManager.GetCustomPlayset(playset);
	}

	public static LocaleHelper.Translation GetTypeTranslation(this IBackupMetaData meta)
	{
		return LocaleHelper.GetGlobalText("Backup_" + meta.Type);
	}

	public static string GetIcon(this IBackupMetaData meta)
	{
		return meta.Type switch
		{
			"SaveGames" => "City",
			"ActivePlayset" => "Playsets",
			"SettingsFiles" => "UserOptions",
			"ModsSettingsFiles" => "FileSettings",
			"LocalMods" => "Mods",
			"Maps" => "Map",
			_ => string.Empty,
		};
	}
}
