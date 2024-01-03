using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Skyve.Systems;
public class PackageUtil : IPackageUtil
{
	private readonly IModUtil _modUtil;
	private readonly IAssetUtil _assetUtil;
	private readonly IBulkUtil _bulkUtil;
	private readonly ILocale _locale;
	private readonly IPackageManager  _packageManager;
	private readonly IPackageNameUtil _packageUtil;
	private readonly ISettings _settings;

	public PackageUtil(IModUtil modUtil, IAssetUtil assetUtil, IBulkUtil bulkUtil, ILocale locale, IPackageNameUtil packageUtil, IPackageManager packageManager, ISettings settings)
	{
		_modUtil = modUtil;
		_assetUtil = assetUtil;
		_bulkUtil = bulkUtil;
		_locale = locale;
		_packageUtil = packageUtil;
		_packageManager = packageManager;
		_settings = settings;
	}

	public bool IsIncluded(IPackageIdentity identity)
	{
		if (identity is ILocalPackageData localPackage)
		{
			foreach (var item in localPackage.Assets)
			{
				if (_assetUtil.IsIncluded(item))
				{
					return true;
				}
			}

			return _modUtil.IsIncluded(identity);
		}

		if (identity is IAsset asset)
		{
			return _assetUtil.IsIncluded(asset);
		}

		var package = _packageManager.GetPackageById(identity);

		if (package?.LocalData is not null)

			return IsIncluded(package.LocalData);

		return _modUtil.IsIncluded(identity);
	}

	public bool IsIncluded(IPackageIdentity identity, out bool partiallyIncluded)
	{
		var included = false;
		var excluded = false;

		if (identity is ILocalPackageData localPackage)
		{
			if (_modUtil.IsIncluded(localPackage))
				included = true;
			else
				excluded = true;

			foreach (var item in localPackage.Assets)
			{
				if (_assetUtil.IsIncluded(item))
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

			partiallyIncluded = false;

			return included;
		}

		if (identity is IAsset asset)
		{
			partiallyIncluded = false;
			return _assetUtil.IsIncluded(asset);
		}

		var package = _packageManager.GetPackageById(identity);

		if (package?.LocalData is not null)

			return IsIncluded(package.LocalData, out partiallyIncluded);

		partiallyIncluded = false;

		return _modUtil.IsIncluded(identity);
	}

	public bool IsEnabled(IPackageIdentity package)
	{
		return _modUtil.IsEnabled(package);
	}

	public bool IsIncludedAndEnabled(IPackageIdentity package)
	{
		return IsIncluded(package) && IsEnabled(package);
	}

	public void SetIncluded(IPackageIdentity localPackage, bool value)
	{
		//if (localPackage is ILocalPackageData localPackageData && localPackageData.Assets.Length > 0)
		//{
		//	_bulkUtil.SetBulkIncluded(new[] { localPackage }, value);
		//}
		//else
		if (localPackage is IAsset asset)
		{
			_assetUtil.SetIncluded(asset, value);
		}
		else
		{
			_modUtil.SetIncluded(localPackage, value);
		}
	}

	public void SetEnabled(IPackageIdentity package, bool value)
	{
		_modUtil.SetEnabled(package, value);
	}

	public DownloadStatus GetStatus(IPackageIdentity? mod, out string reason)
	{
		var workshopInfo = mod?.GetWorkshopInfo();
		var localPackage = mod?.GetLocalPackage();

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

#if CS2
		reason = string.Empty;
		return DownloadStatus.OK;
#endif

		if (workshopInfo.ServerTime == default)
		{
			reason = _locale.Get("PackageIsUnknown").Format(_packageUtil.CleanName(mod));
			return DownloadStatus.Unknown;
		}

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
	}

	public IEnumerable<IPackage> GetPackagesThatReference(IPackageIdentity package, bool withExcluded = false)
	{
		var compatibilityUtil = ServiceCenter.Get<ICompatibilityManager>();
		var packages = withExcluded || ServiceCenter.Get<ISettings>().UserSettings.ShowAllReferencedPackages
			? _packageManager.Packages.ToList()
			: _packageManager.Packages.AllWhere(x => x.IsIncluded());

		foreach (var localPackage in packages)
		{
			foreach (var requirement in localPackage.GetWorkshopInfo()?.Requirements ?? [])
			{
				if (compatibilityUtil.GetFinalSuccessor(requirement)?.Id == package.Id)
				{
					yield return localPackage;
				}
			}
		}
	}
}
