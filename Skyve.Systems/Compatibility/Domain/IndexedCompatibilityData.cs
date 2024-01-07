using Extensions;

using Skyve.Domain.Enums;
using Skyve.Systems.Compatibility.Domain.Api;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain;

public class IndexedCompatibilityData
{
	public IndexedCompatibilityData(CompatibilityData? data)
	{
		Packages = data?.Packages?.ToDictionary(x => x.Id, x => NewMethod(x, data.Packages)) ?? [];
		PackageNames = new(StringComparer.InvariantCultureIgnoreCase);
		Authors = data?.Authors?.ToDictionary(x => x.Id) ?? [];
		BlackListedIds = new(data?.BlackListedIds ?? []);
		BlackListedNames = new(data?.BlackListedNames ?? []);

		foreach (var item in Packages.Values)
		{
			if (item.Package.FileName is not null and not "")
			{
				PackageNames[item.Package.FileName] = item.Package.Id;
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
			var stable = packages.FirstOrDefault(x => x.Id == id);

			if (stable is not null)
			{
				package.Links = stable.Links;
				package.RequiredDLCs = stable.RequiredDLCs;
				package.Tags = stable.Tags;
				package.Note = stable.Note;
				package.Usage = stable.Usage;
				package.Type = stable.Type;
				package.Statuses ??= [];
				package.Statuses.AddRange(stable.Statuses ?? []);
				package.Interactions ??= [];
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
