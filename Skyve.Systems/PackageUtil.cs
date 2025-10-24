using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Systems;
public class PackageUtil : IPackageUtil
{
	private readonly IModUtil _modUtil;
	private readonly IAssetUtil _assetUtil;
	private readonly ILocale _locale;
	private readonly IPackageManager _packageManager;
	private readonly IPackageNameUtil _packageUtil;
	private readonly ISettings _settings;
	private readonly INotifier _notifier;

	public PackageUtil(IModUtil modUtil, IAssetUtil assetUtil, ILocale locale, IPackageNameUtil packageUtil, IPackageManager packageManager, ISettings settings, INotifier notifier)
	{
		_modUtil = modUtil;
		_assetUtil = assetUtil;
		_locale = locale;
		_packageUtil = packageUtil;
		_packageManager = packageManager;
		_settings = settings;
		_notifier = notifier;
	}

	public bool IsIncluded(IPackageIdentity identity, string? playsetId = null, bool withVersion = true)
	{
		//if (identity is IAsset asset)
		//{
		//	return _assetUtil.IsIncluded(asset, playsetId);
		//}

		return _modUtil.IsIncluded(identity, playsetId, withVersion);
	}

	public bool IsIncluded(IPackageIdentity identity, out bool partiallyIncluded, string? playsetId = null, bool withVersion = true)
	{
		//if (identity is IAsset asset)
		//{
		//	partiallyIncluded = false;
		//	return _assetUtil.IsIncluded(asset, playsetId);
		//}

		bool included;

		if (_modUtil.IsIncluded(identity, playsetId, withVersion))
		{
			included = true;
		}
		else
		{
			partiallyIncluded = false;

			return false;
		}

		partiallyIncluded = false;

		//if (identity.GetLocalPackage() is ILocalPackageData localPackage)
		//{
		//	foreach (var item in localPackage.Assets)
		//	{
		//		if (_assetUtil.IsIncluded(item, playsetId))
		//		{
		//			included = true;
		//		}
		//		else
		//		{
		//			excluded = true;
		//		}

		//		if (included && excluded)
		//		{
		//			partiallyIncluded = true;

		//			return true;
		//		}
		//	}
		//}

		return included;
	}

	public bool IsEnabled(IPackageIdentity package, string? playsetId = null, bool withVersion = true)
	{
		return _modUtil.IsEnabled(package, playsetId, withVersion);
	}

	public bool IsIncludedAndEnabled(IPackageIdentity package, string? playsetId = null, bool withVersion = true)
	{
		return IsEnabled(package, playsetId, withVersion);
	}

	public async Task SetIncluded(IEnumerable<IPackageIdentity> packages, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true)
	{
		await _modUtil.SetIncluded(packages, value, playsetId, withVersion, promptForDependencies);
	}

	public async Task SetEnabled(IEnumerable<IPackageIdentity> packages, bool value, string? playsetId = null)
	{
		await _modUtil.SetEnabled(packages, value, playsetId);
	}

	public async Task SetIncluded(IPackageIdentity package, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true)
	{
		await _modUtil.SetIncluded(package, value, playsetId, withVersion, promptForDependencies);
	}

	public async Task SetEnabled(IPackageIdentity package, bool value, string? playsetId = null)
	{
		await _modUtil.SetEnabled(package, value, playsetId);
	}

	public string? GetSelectedVersion(IPackageIdentity package, string? playsetId = null)
	{
		return _modUtil.GetSelectedVersion(package, playsetId);
	}

	public DownloadStatus GetStatus(IPackageIdentity? mod, out string reason)
	{
		var workshopInfo = mod?.GetWorkshopInfo();

		if (workshopInfo is null)
		{
			reason = string.Empty;
			return DownloadStatus.None;
		}

		if (workshopInfo.IsRemoved)
		{
			reason = _locale.Get("PackageIsRemoved").Format(_packageUtil.CleanName(mod));
			return DownloadStatus.Removed;
		}

		var latestVersion = workshopInfo.Version.SmartParse();
		var currentVersion = _modUtil.GetSelectedVersion(mod!).SmartParse();

		if (latestVersion > currentVersion && currentVersion != 0)
		{
			reason = _locale.Get("PackageIsOutOfDateVersion").Format(_packageUtil.CleanName(mod));
			return DownloadStatus.OutOfDate;
		}

		if (mod is IPlaysetPackage playsetPackage && playsetPackage.IsVersionLocked)
		{
			reason = _locale.Get("VersionLockedInfo").Format(_packageUtil.CleanName(mod), playsetPackage.VersionName);
			return DownloadStatus.VersionLocked;
		}

#if CS2
		reason = string.Empty;
		return DownloadStatus.OK;
#else

		if (workshopInfo.ServerTime == default)
		{
			reason = _locale.Get("PackageIsUnknown").Format(_packageUtil.CleanName(mod));
			return DownloadStatus.Unknown;
		}

		var localPackage = mod?.GetLocalPackage();

		if (localPackage is not null)
		{
			var updatedServer = workshopInfo.ServerTime;
			var updatedLocal = localPackage.LocalTime;
			var sizeServer = workshopInfo.ServerSize;
			var localSize = localPackage.FileSize;

			if (updatedLocal < updatedServer)
			{
				var certain = updatedLocal < updatedServer.AddHours(-24);

				reason = certain
					? _locale.Get("PackageIsOutOfDate").Format(_packageUtil.CleanName(mod), (updatedServer - updatedLocal).ToReadableString(true))
					: _locale.Get("PackageIsMaybeOutOfDate").Format(_packageUtil.CleanName(mod), updatedServer.ToLocalTime().ToRelatedString(true));
				return DownloadStatus.OutOfDate;
			}

			if (localSize < sizeServer && sizeServer > 0)
			{
				reason = _locale.Get("PackageIsIncomplete").Format(_packageUtil.CleanName(mod), localSize.SizeString(), sizeServer.SizeString());
				return DownloadStatus.PartiallyDownloaded;
			}
		}

		reason = string.Empty;
		return DownloadStatus.OK;
#endif
	}
}
