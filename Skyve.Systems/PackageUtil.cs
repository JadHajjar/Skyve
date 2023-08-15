using Extensions;

using SkyveApp.Domain;
using SkyveApp.Domain.Enums;
using SkyveApp.Domain.Systems;

using System.Collections.Generic;
using System.Linq;

namespace SkyveApp.Systems;
public class PackageUtil : IPackageUtil
{
	private readonly IModUtil _modUtil;
	private readonly IAssetUtil _assetUtil;
	private readonly IBulkUtil _bulkUtil;
	private readonly ILocale _locale;
	private readonly IPackageManager _contentManager;
	private readonly IPackageNameUtil _packageUtil;
	private readonly ISettings _settings;

	public PackageUtil(IModUtil modUtil, IAssetUtil assetUtil, IBulkUtil bulkUtil, ILocale locale, IPackageNameUtil packageUtil, IPackageManager contentManager, ISettings settings)
	{
		_modUtil = modUtil;
		_assetUtil = assetUtil;
		_bulkUtil = bulkUtil;
		_locale = locale;
		_packageUtil = packageUtil;
		_contentManager = contentManager;
		_settings = settings;
	}

	public bool IsIncluded(ILocalPackage localPackage)
	{
		if (localPackage is ILocalPackageWithContents packageWithContents)
		{
			if (packageWithContents.Mod is not null)
			{
				if (_modUtil.IsIncluded(packageWithContents.Mod))
				{
					return true;
				}
			}

			foreach (var item in packageWithContents.Assets)
			{
				if (_assetUtil.IsIncluded(item))
				{
					return true;
				}
			}

			return false;
		}

		return localPackage is IMod mod ? _modUtil.IsIncluded(mod) : localPackage is IAsset asset && _assetUtil.IsIncluded(asset);
	}

	public bool IsIncluded(ILocalPackage localPackage, out bool partiallyIncluded)
	{
		if (localPackage is ILocalPackageWithContents packageWithContents)
		{
			var included = false;
			var excluded = false;

			if (packageWithContents.Mod is not null)
			{
				if (IsIncluded(packageWithContents.Mod, out _))
				{
					included = true;
				}
				else
				{
					excluded = true;
				}
			}

			foreach (var item in packageWithContents.Assets)
			{
				if (IsIncluded(item, out _))
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

		partiallyIncluded = false;

		return localPackage is IMod mod ? _modUtil.IsIncluded(mod) : localPackage is IAsset asset && _assetUtil.IsIncluded(asset);
	}

	public bool IsEnabled(ILocalPackage package)
	{
		return package is IMod mod
			? _modUtil.IsEnabled(mod)
			: (package is not ILocalPackageWithContents packageWithContents 
				|| packageWithContents.Mod is null
				|| _modUtil.IsEnabled(packageWithContents.Mod));
	}

	public bool IsIncludedAndEnabled(ILocalPackage package)
	{
		return IsIncluded(package) && IsEnabled(package);
	}

	public void SetIncluded(ILocalPackage localPackage, bool value)
	{
		if (localPackage is ILocalPackageWithContents localPackageWithContents)
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

	public void SetEnabled(ILocalPackage package, bool value)
	{
		if (package is IMod mod)
		{
			_modUtil.SetEnabled(mod, value);
		}

		if (package is ILocalPackageWithContents packageWithContents && packageWithContents.Mod is not null)
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

	public IEnumerable<ILocalPackage> GetPackagesThatReference(IPackage package, bool withExcluded = false)
	{
		var compatibilityUtil = ServiceCenter.Get<ICompatibilityManager>();
		var packages = withExcluded || ServiceCenter.Get<ISettings>().UserSettings.ShowAllReferencedPackages
			? _contentManager.Packages.ToList()
			: _contentManager.Packages.AllWhere(IsIncluded);

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
