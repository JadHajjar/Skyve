using Skyve.Systems.Compatibility.Domain.Api;

using System.Collections.Generic;

namespace Skyve.Systems.Compatibility.Domain;

public class IndexedPackageInteraction
{
	public PackageInteraction Interaction { get; }
	public Dictionary<ulong, IndexedPackage> Packages { get; }

	public IndexedPackageInteraction(PackageInteraction interaction, Dictionary<ulong, IndexedPackage> packages)
	{
		Interaction = interaction;
		Packages = new();

		if (interaction.Packages is not null)
		{
			foreach (var item in interaction.Packages)
			{
				if (packages.ContainsKey(item))
				{
					Packages[item] = packages[item];
				}
			}
		}
	}

	public static implicit operator IndexedPackageInteraction(PackageInteraction status)
	{
		return new(status, new());
	}
}