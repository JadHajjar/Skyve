using Extensions;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility;
internal class LoadOrderHelper : ILoadOrderHelper
{
	private readonly CompatibilityManager _compatibilityManager;
	private readonly IPackageManager _packageManager;
	private readonly ISkyveDataManager _skyveDataManager;

	public LoadOrderHelper(IPackageManager packageManager, ICompatibilityManager compatibilityManager, ISkyveDataManager skyveDataManager)
	{
		_packageManager = packageManager;
		_skyveDataManager = skyveDataManager;
		_compatibilityManager = (CompatibilityManager)compatibilityManager;
	}

	private List<ModInfo> GetEntities()
	{
		var entities = new List<ModInfo>();

		foreach (var mod in _packageManager.Packages)
		{
			var info = _skyveDataManager.GetPackageCompatibilityInfo(mod);
			var interaction = info?.Interactions?.FirstOrDefault(x => x.Type is InteractionType.RequiredPackages);
			var loadOrder = info?.Interactions?.FirstOrDefault(x => x.Type is InteractionType.LoadAfter);

			entities.Add(new ModInfo(mod
				, (interaction?.Packages.SelectWhereNotNull(x => (IPackageIdentity)new GenericPackageIdentity(x)).ToArray() ?? new IPackageIdentity[0])!
				, (loadOrder?.Packages.SelectWhereNotNull(x => (IPackageIdentity)new GenericPackageIdentity(x)).ToArray() ?? new IPackageIdentity[0])!));
		}

		return entities;
	}

	public IEnumerable<IPackage> GetOrderedMods()
	{
		var entities = GetEntities();

		foreach (var entity in entities)
		{
			foreach (var item in entity.RequiredMods)
			{
				Increment(entities, item, entity.Mod, 0);
			}
		}

		bool changed;
		var runs = 10;

		do
		{
			changed = false;

			foreach (var entity in entities)
			{
				if (entity.LoadAfterMods.Length > 0)
				{
					var loadAfter = entity.LoadAfterMods.SelectMany(x => GetEntity(x.Id, entities, entity.Mod)).AllWhere(x => x is not null);

					if (loadAfter.Count > 0)
					{
						var order = loadAfter.Min(x => x.Order) - 1;

						if (order != entity.Order)
						{
							changed = true;
							entity.Order = order;
						}
					}
				}
			}
		}
		while (changed && --runs > 0);

		return entities.OrderByDescending(x => x.Order).Select(x => x.Mod);
	}

	private void Increment(List<ModInfo> modEntityMap, IPackageIdentity identity, IPackageIdentity original, int nesting)
	{
		if (nesting++ > 100)
		{
			return;
		}

		foreach (var entity in GetEntity(identity.Id, modEntityMap, original))
		{
			entity.Order += 100;

			foreach (var item in entity.RequiredMods)
			{
				Increment(modEntityMap, item, identity, nesting);
			}
		}
	}

	private IEnumerable<ModInfo> GetEntity(ulong id, List<ModInfo> modEntityMap, IPackageIdentity original)
	{
		var indexedPackage = _skyveDataManager.TryGetPackageInfo(id);

		foreach (var item in modEntityMap.Where(x => x.Mod.Id == id))
		{
			yield return item;
		}

		if (indexedPackage is null)
		{
			yield break;
		}

		foreach (var item in indexedPackage.Group)
		{
			if (item.Key != id)
			{
				foreach (var package in _compatibilityManager.FindPackage(item.Value, true))
				{
					if (original.Id != package.Id)
					{
						foreach (var entity in modEntityMap.Where(x => x.Mod.Id == package.Id))
						{
							yield return entity;
						}
					}
				}
			}
		}

		foreach (var package in _compatibilityManager.FindPackage(indexedPackage, true))
		{
			if (original.Id != package.Id)
			{
				foreach (var entity in modEntityMap.Where(x => x.Mod.Id == package.Id))
				{
					yield return entity;
				}
			}
		}
	}

	private class ModInfo
	{
		public int Order = 1000;
		public IPackage Mod;
		public IPackageIdentity[] RequiredMods;
		public IPackageIdentity[] LoadAfterMods;

		public ModInfo(IPackage mod, IPackageIdentity[] requiredMods, IPackageIdentity[] afterLoadMods)
		{
			Mod = mod;
			RequiredMods = requiredMods;
			LoadAfterMods = afterLoadMods;
		}
	}
}
