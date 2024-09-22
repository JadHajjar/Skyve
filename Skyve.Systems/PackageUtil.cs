using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Linq;
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

	public bool IsIncluded(IPackageIdentity identity, int? playsetId = null)
	{
		if (identity is IAsset asset)
		{
			return _assetUtil.IsIncluded(asset, playsetId);
		}

		return _modUtil.IsIncluded(identity, playsetId);
	}

	public bool IsIncluded(IPackageIdentity identity, out bool partiallyIncluded, int? playsetId = null)
	{
		if (identity is IAsset asset)
		{
			partiallyIncluded = false;
			return _assetUtil.IsIncluded(asset, playsetId);
		}

		bool included, excluded = false;

		if (_modUtil.IsIncluded(identity, playsetId))
		{
			included = true;
		}
		else
		{
			partiallyIncluded = false;

			return false;
		}

		partiallyIncluded = false;

		if (identity.GetLocalPackage() is ILocalPackageData localPackage)
		{
			foreach (var item in localPackage.Assets)
			{
				if (_assetUtil.IsIncluded(item, playsetId))
				{
					included = true;
				}
				else
				{
					excluded = true;
				}

				if (included && excluded)
				{
					partiallyIncluded = true;

					return true;
				}
			}
		}

		return included;
	}

	public bool IsEnabled(IPackageIdentity package, int? playsetId = null)
	{
		return _modUtil.IsEnabled(package, playsetId);
	}

	public bool IsIncludedAndEnabled(IPackageIdentity package, int? playsetId = null)
	{
		return IsIncluded(package, playsetId) && IsEnabled(package, playsetId);
	}

	public async Task SetIncluded(IEnumerable<IPackageIdentity> packages, bool value, int? playsetId = null)
	{
		var packageList = packages.ToList();

		if (packageList.Count == 0)
		{
			return;
		}

		_notifier.IsBulkUpdating = true;

		var assets = packageList.SelectMany(x => x.GetLocalPackage()?.Assets ?? []);

		foreach (var asset in assets)
		{
			await _assetUtil.SetIncluded(asset, value, playsetId);
		}

		await _modUtil.SetIncluded(packages, value, playsetId);

		_notifier.IsBulkUpdating = false;

		if (_notifier.IsContentLoaded)
		{
			_notifier.OnInclusionUpdated();
			_assetUtil.SaveChanges();
			_notifier.OnRefreshUI(true);
		}
	}

	public async Task SetEnabled(IEnumerable<IPackageIdentity> packages, bool value, int? playsetId = null)
	{
		await _modUtil.SetEnabled(packages, value, playsetId);
	}

	public async Task SetVersion(IPackageIdentity package, string version, int? playsetId = null)
	{
		await _modUtil.SetVersion(package, version, playsetId);
	}

	public async Task SetIncluded(IPackageIdentity localPackage, bool value, int? playsetId = null)
	{
		//if (localPackage is ILocalPackageData localPackageData && localPackageData.Assets.Length > 0)
		//{
		//	_bulkUtil.SetIncluded(new[] { localPackage }, value);
		//}
		//else
		if (localPackage is IAsset asset)
		{
			await _assetUtil.SetIncluded(asset, value, playsetId);
		}
		else
		{
			await _modUtil.SetIncluded(localPackage, value, playsetId);
		}
	}

	public async Task SetEnabled(IPackageIdentity package, bool value, int? playsetId = null)
	{
		await _modUtil.SetEnabled(package, value, playsetId);
	}

	public string? GetSelectedVersion(IPackageIdentity package, int? playsetId = null)
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

		var latestVersion = workshopInfo.VersionId.SmartParse();
		var currentVersion = _modUtil.GetSelectedVersion(mod!).SmartParse();

		if (latestVersion > currentVersion && currentVersion != 0)
		{
			reason = _locale.Get("PackageIsOutOfDateVersion").Format(_packageUtil.CleanName(mod));
			return DownloadStatus.OutOfDate;
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
