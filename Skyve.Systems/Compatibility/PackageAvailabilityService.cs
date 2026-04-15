using Extensions;

using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility;

public class PackageAvailabilityService
{
	private readonly IPackageManager _packageManager;
	private readonly IPackageUtil _packageUtil;
	private readonly ISkyveDataManager _skyveDataManager;
	private readonly IDlcManager _dlcManager;
	private readonly CompatibilityManager _compatibilityManager;
	private readonly Dictionary<string, (bool enabled, bool enabledWithAlternatives)> _cache;

	public PackageAvailabilityService(IPackageManager packageManager, IPackageUtil packageUtil, ISkyveDataManager skyveDataManager, CompatibilityManager compatibilityManager, IDlcManager dlcManager)
	{
		_packageManager = packageManager;
		_packageUtil = packageUtil;
		_skyveDataManager = skyveDataManager;
		_compatibilityManager = compatibilityManager;
		_dlcManager = dlcManager;
		_cache = [];
	}

	public bool IsPackageEnabled(ICompatibilityPackageIdentity id, bool withAlternativesAndSuccessors)
	{
		if (id.IsDlc)
		{
			return ulong.TryParse(id.Id, out var dlc) && _dlcManager.IsAvailable(dlc);
		}

		return IsPackageEnabled(id.Id, withAlternativesAndSuccessors);
	}

	public bool IsPackageEnabled(string id, bool withAlternativesAndSuccessors)
	{
		return _cache.TryGetValue(id, out var status) && (withAlternativesAndSuccessors ? status.enabledWithAlternatives : status.enabled);
	}

	internal void RefreshCache()
	{
		var ids = new List<string>();

		ids.AddRange(_packageManager.Packages.Select(x => x.Id));
		ids.AddRange(_skyveDataManager.GetPackagesKeys());

		_cache.Clear();
		foreach (var package in ids.Distinct().WhereNotEmpty())
		{
			_cache[package] = (GetPackageEnabled(package, false), GetPackageEnabled(package, true));
		}
	}

	internal void UpdateInclusionStatus(string id)
	{
		_cache[id] = (GetPackageEnabled(id, false), GetPackageEnabled(id, true));
	}

	private bool GetPackageEnabled(string id, bool withAlternativesAndSuccessors)
	{
		var indexedPackage = _skyveDataManager.TryGetPackageInfo(id);

		if (isEnabled(_packageManager.GetPackageById(new GenericPackageIdentity(Defaults.WORKSHOP_SOURCE, id))))
		{
			return true;
		}

		if (indexedPackage is null)
		{
			return false;
		}

		if (withAlternativesAndSuccessors)
		{
			foreach (var item in indexedPackage.RequirementAlternatives)
			{
				if (item.Key != id)
				{
					foreach (var package in _compatibilityManager.FindPackage(item.Value, withAlternativesAndSuccessors))
					{
						if (isEnabled(package))
						{
							return true;
						}
					}
				}
			}
		}

		foreach (var package in _compatibilityManager.FindPackage(indexedPackage, withAlternativesAndSuccessors))
		{
			if (isEnabled(package))
			{
				return true;
			}
		}

		foreach (var item in indexedPackage.Group)
		{
			if (item.Key != id)
			{
				foreach (var package in _compatibilityManager.FindPackage(item.Value, withAlternativesAndSuccessors))
				{
					if (isEnabled(package))
					{
						return true;
					}
				}
			}
		}

		return false;

		bool isEnabled(IPackageIdentity? package) => package is not null && _packageUtil.IsIncludedAndEnabled(package, withVersion: false);
	}
}
