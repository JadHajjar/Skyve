using Skyve.Systems.Compatibility.Domain.Api;

using System.Collections.Generic;

namespace Skyve.Systems.Compatibility.Domain;

public class IndexedPackageStatus
{
	public PackageStatus Status { get; }
	public Dictionary<ulong, IndexedPackage> Packages { get; }

	public IndexedPackageStatus(PackageStatus status, Dictionary<ulong, IndexedPackage> packages)
	{
		Status = status;
		Packages = new();

		if (status.Packages is not null)
		{
			foreach (var item in status.Packages)
			{
				if (packages.ContainsKey(item))
				{
					Packages[item] = packages[item];
				}
			}
		}
	}

	public static implicit operator IndexedPackageStatus(PackageStatus status)
	{
		return new(status, new());
	}
}
