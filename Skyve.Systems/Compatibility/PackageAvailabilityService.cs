using Extensions;

using Skyve.Domain;
using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility;
public class PackageAvailabilityService
{
	private readonly IPackageManager _packageManager;
	private readonly IPackageUtil _packageUtil;
	private readonly ISkyveDataManager _skyveDataManager;
	private readonly CompatibilityManager _compatibilityManager;
	private readonly Dictionary<ulong, (bool enabled, bool enabledWithAlternatives)> _cache;

	public PackageAvailabilityService(IPackageManager packageManager, IPackageUtil packageUtil, ISkyveDataManager skyveDataManager, CompatibilityManager compatibilityManager)
	{
		_packageManager = packageManager;
		_packageUtil = packageUtil;
		_skyveDataManager = skyveDataManager;
		_compatibilityManager = compatibilityManager;
		_cache = [];
	}

	public bool IsPackageEnabled(ulong id, bool withAlternativesAndSuccessors)
	{
		return _cache.TryGetValue(id, out var status) && (withAlternativesAndSuccessors ? status.enabledWithAlternatives : status.enabled);
	}

	internal void RefreshCache()
	{
		var ids = new List<ulong>();

		ids.AddRange(_packageManager.Packages.Select(x => x.Id));
		ids.AddRange(_skyveDataManager.GetPackagesKeys());

		_cache.Clear();
		foreach (var package in ids.Distinct().Where(x => x > 0))
		{
			_cache[package] = (GetPackageEnabled(package, false), GetPackageEnabled(package, true));
		}
	}

	internal void UpdateInclusionStatus(ulong id)
	{
		_cache[id] = (GetPackageEnabled(id, false), GetPackageEnabled(id, true));
	}

	private bool GetPackageEnabled(ulong id, bool withAlternativesAndSuccessors)
	{
		var indexedPackage = _skyveDataManager.TryGetPackageInfo(id);

		if (isEnabled(_packageManager.GetPackageById(new GenericPackageIdentity(id))?.LocalData))
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
