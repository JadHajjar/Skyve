using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;

using System.Collections.Generic;
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
	IIndexedPackageCompatibilityInfo TryGetPackageInfo(ulong id);
	IEnumerable<ulong> GetPackagesKeys();
	Task<ReviewReply?> GetReviewStatus(IPackageIdentity package);
}
