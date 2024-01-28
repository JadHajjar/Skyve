using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Systems.Compatibility.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Systems;
public interface ISkyveDataManager
{
	Task DownloadData();
	void Start(List<IPackage> packages);
	void ResetCache();
	bool IsBlacklisted(IPackageIdentity package);
	ulong GetIdFromModName(string fileName);
	IIndexedPackageCompatibilityInfo? GetPackageCompatibilityInfo(IPackageIdentity identity);
	IKnownUser TryGetAuthor(string? id);
	IIndexedPackageCompatibilityInfo TryGetPackageInfo(ulong id);
	IEnumerable<ulong> GetPackagesKeys();
}
