using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Systems;
public interface ISkyveDataManager
{
	ReviewRequest[]? ReviewRequests { get; }

	Task DownloadData();
	void Start(List<IPackage> packages);
	void ResetCache();
	bool IsBlacklisted(string packageId);
	bool IsBlacklisted(IPackageIdentity package);
	string GetIdFromModName(string fileName);
	IIndexedPackageCompatibilityInfo? GetPackageCompatibilityInfo(IPackageIdentity identity);
	IIndexedPackageCompatibilityInfo TryGetPackageInfo(string id);
	IEnumerable<string> GetPackagesKeys();
	Task<ReviewReply?> GetReviewStatus(IPackageIdentity package);
}
