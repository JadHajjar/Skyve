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

	public bool IsPackageEnabled(ulong steamId, bool withAlternativesAndSuccessors)
	{
		if (_cache.TryGetValue(steamId, out var status))
		{
			return withAlternativesAndSuccessors ? status.enabledWithAlternatives : status.enabled;
		}

		return false;
	}

	private bool GetPackageEnabled(ulong steamId, bool withAlternativesAndSuccessors)
	{
		var indexedPackage = _compatibilityManager.CompatibilityData.Packages.TryGet(steamId);

		if (isEnabled(_packageManager.GetPackageById(new GenericPackageIdentity(steamId))))
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
				if (item.Key != steamId)
				{
					foreach (var package in FindPackage(item.Value, withAlternativesAndSuccessors))
					{
						if (isEnabled(package))
						{
							return true;
						}
					}
				}
			}
		}

		foreach (var package in FindPackage(indexedPackage, withAlternativesAndSuccessors))
		{
			if (isEnabled(package))
			{
				return true;
			}
		}

		foreach (var item in indexedPackage.Group)
		{
			if (item.Key != steamId)
			{
				foreach (var package in FindPackage(item.Value, withAlternativesAndSuccessors))
				{
					if (isEnabled(package))
					{
						return true;
					}
				}
			}
		}

		return false;

		bool isEnabled(ILocalPackage? package) => package is not null && _contentUtil.IsIncludedAndEnabled(package);
	}

	private IEnumerable<ILocalPackage> FindPackage(IndexedPackage package, bool withAlternativesAndSuccessors)
	{
		var localPackage = _packageManager.GetPackageById(new GenericPackageIdentity(package.Package.SteamId));

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		localPackage = _packageManager.Mods.FirstOrDefault(x => x.IsLocal && Path.GetFileName(x.FilePath) == package.Package.FileName)?.LocalParentPackage;

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		if (!withAlternativesAndSuccessors || !package.Interactions.ContainsKey(InteractionType.SucceededBy))
		{
			yield break;
		}

		var packages = package.Interactions[InteractionType.SucceededBy]
					.SelectMany(x => x.Packages.Values)
					.Where(x => x.Package != package.Package)
					.Select(x => FindPackage(x, true))
					.FirstOrDefault(x => x is not null);

		if (packages is not null)
		{
			foreach (var item in packages)
			{
				yield return item;
			}
		}
	}
}
