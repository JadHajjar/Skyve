using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;

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
		if (identity is IPackage localPackage && localPackage.LocalData is not null)
		{
			foreach (var item in localPackage.LocalData.Assets)
			{
				if (_assetUtil.IsIncluded(item))
				{
					return true;
				}
			}

			return _modUtil.IsIncluded(localPackage.LocalData);
		}

		if (identity is IAsset asset)
		{
			return _assetUtil.IsIncluded(asset);
		}

		var package = _packageManager.GetPackageById(identity);

		return package is not null && IsIncluded(package);
	}

	public bool IsIncluded(IPackage identity, out bool partiallyIncluded)
	{
		var included = false;
		var excluded = false;

		if (identity is IPackage localPackage && localPackage.LocalData is not null)
		{
			if (_modUtil.IsIncluded(localPackage.LocalData))
				included = true;
			else
				excluded = true;

			foreach (var item in localPackage.LocalData.Assets)
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

		partiallyIncluded = false;

		return package is not null && IsIncluded(package, out partiallyIncluded);
	}

	public bool IsEnabled(IPackage package)
	{
		return package.LocalData is null || _modUtil.IsEnabled(package.LocalData);
	}

	public bool IsIncludedAndEnabled(IPackage package)
	{
		return IsIncluded(package) && IsEnabled(package);
	}

	public void SetIncluded(IPackage localPackage, bool value)
	{
		if (localPackage is ILocalPackageData localPackageWithContents)
		{
			_bulkUtil.SetBulkIncluded(new[] { localPackage }, value);
		}

		if (localPackage is IMod mod)
		{
			_modUtil.SetIncluded(mod, value);

			if (_settings.UserSettings.LinkModAssets && mod.LocalParentPackage!.Assets.Any())
			{
				_bulkUtil.SetBulkIncluded(mod.LocalParentPackage!.Assets, value);
			}
		}

		if (localPackage is IAsset asset)
		{
			_assetUtil.SetIncluded(asset, value);
		}
	}

	public void SetEnabled(IPackage package, bool value)
	{
		if (package is IMod mod)
		{
			_modUtil.SetEnabled(mod, value);
		}

		if (package is ILocalPackageData packageWithContents && packageWithContents.Mod is not null)
		{
			_modUtil.SetEnabled(packageWithContents.Mod, value);
		}
	}

	public DownloadStatus GetStatus(IPackage mod, out string reason)
	{
		var workshopInfo = mod.GetWorkshopInfo();
		var localPackage = mod.LocalParentPackage;

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
			var localSize = localPackage.LocalSize;

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

	public IEnumerable<IPackage> GetPackagesThatReference(IPackage package, bool withExcluded = false)
	{
		var compatibilityUtil = ServiceCenter.Get<ICompatibilityManager>();
		var packages = withExcluded || ServiceCenter.Get<ISettings>().UserSettings.ShowAllReferencedPackages
			? _packageManager.Packages.ToList()
			: _packageManager.Packages.AllWhere(IsIncluded);

		foreach (var localPackage in packages)
		{
			foreach (var requirement in localPackage.Requirements)
			{
				if (compatibilityUtil.GetFinalSuccessor(requirement)?.Id == package.Id)
				{
					yield return localPackage;
				}
			}
		}
	}
}
