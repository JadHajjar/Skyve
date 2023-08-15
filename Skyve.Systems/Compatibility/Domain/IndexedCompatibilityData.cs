using Extensions;

using SkyveApp.Domain.Enums;
using SkyveApp.Systems.Compatibility.Domain.Api;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyveApp.Systems.Compatibility.Domain;

public class IndexedCompatibilityData
{
	public IndexedCompatibilityData(CompatibilityData? data)
	{
		Packages = data?.Packages?.ToDictionary(x => x.SteamId, x => NewMethod(x, data.Packages)) ?? new();
		PackageNames = new(StringComparer.InvariantCultureIgnoreCase);
		Authors = data?.Authors?.ToDictionary(x => x.SteamId) ?? new();
		BlackListedIds = new(data?.BlackListedIds ?? new());
		BlackListedNames = new(data?.BlackListedNames ?? new());

		foreach (var item in Packages.Values)
		{
			if (item.Package.FileName is not null and not "")
			{
				PackageNames[item.Package.FileName] = item.Package.SteamId;
			}

			item.Load(Packages);
		}

		foreach (var item in Packages.Values)
		{
			item.SetUpInteractions(Packages);
		}
	}

	private static IndexedPackage NewMethod(CompatibilityPackageData package, List<CompatibilityPackageData> packages)
	{
		var nonTest = package.Statuses?.FirstOrDefault(x => x.Type == StatusType.TestVersion && (x.Packages?.Any() ?? false));

		if (nonTest is not null)
		{
			var id = nonTest.Packages![0];
			var stable = packages.FirstOrDefault(x => x.SteamId == id);

			if (stable is not null)
			{
				package.Links = stable.Links;
				package.RequiredDLCs = stable.RequiredDLCs;
				package.Tags = stable.Tags;
				package.Note = stable.Note;
				package.Usage = stable.Usage;
				package.Type = stable.Type;
				package.Statuses ??= new();
				package.Statuses.AddRange(stable.Statuses ?? new());
				package.Interactions ??= new();
				package.Interactions.AddRange(stable.Interactions?.Where(x => x.Type > InteractionType.Alternative) ?? Enumerable.Empty<PackageInteraction>());
			}
		}

		return new IndexedPackage(package);
	}

	public Dictionary<string, ulong> PackageNames { get; }
	public Dictionary<ulong, IndexedPackage> Packages { get; }
	public Dictionary<ulong, Author> Authors { get; }
	public HashSet<ulong> BlackListedIds { get; }
	public HashSet<string> BlackListedNames { get; }
}
