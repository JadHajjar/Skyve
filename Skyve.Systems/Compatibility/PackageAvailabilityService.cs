using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skyve.Systems.Compatibility;
public class PackageAvailabilityService
{
	private readonly IPackageManager _packageManager;
	private readonly IPackageUtil _contentUtil;
	private readonly CompatibilityManager _compatibilityManager;
	private readonly Dictionary<ulong, (bool enabled, bool enabledWithAlternatives)> _cache;

	public PackageAvailabilityService(IPackageManager packageManager, IPackageUtil contentUtil, ILogger logger, CompatibilityManager compatibilityManager)
	{
		_packageManager = packageManager;
		_contentUtil = contentUtil;
		_compatibilityManager = compatibilityManager;
		_cache = new();

		var ids = new List<ulong>();

		ids.AddRange(_packageManager.Packages.Select(x => x.Id));
		ids.AddRange(_compatibilityManager.CompatibilityData.Packages.Keys);

		logger.Info($"[Compatibility] Caching package availability for {ids.Distinct().Count(x => x > 0)} items");

		foreach (var package in ids.Distinct().Where(x => x > 0))
		{
			_cache[package] = (GetPackageEnabled(package, false), GetPackageEnabled(package, true));
		}

		logger.Info("[Compatibility] Package Availability Cached");
	}

	public bool IsPackageEnabled(ulong id, bool withAlternativesAndSuccessors)
	{
		if (_cache.TryGetValue(id, out var status))
		{
			return withAlternativesAndSuccessors ? status.enabledWithAlternatives : status.enabled;
		}

		return false;
	}

	internal void UpdateInclusionStatus(ulong id)
	{
		_cache[id] = (GetPackageEnabled(id, false), GetPackageEnabled(id, true));
	}

	private bool GetPackageEnabled(ulong id, bool withAlternativesAndSuccessors)
	{
		var indexedPackage = _compatibilityManager.CompatibilityData.Packages.TryGet(id);

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
						if (isEnabled(package.LocalData))
						{
							return true;
						}
					}
				}
			}
		}

		foreach (var package in _compatibilityManager.FindPackage(indexedPackage, withAlternativesAndSuccessors))
		{
			if (isEnabled(package.LocalData))
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
					if (isEnabled(package.LocalData))
					{
						return true;
					}
				}
			}
		}

		return false;

		bool isEnabled(ILocalPackageIdentity? package) => package is not null && _contentUtil.IsIncludedAndEnabled(package);
	}
}
