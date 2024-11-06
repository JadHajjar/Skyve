using Skyve.Domain.Enums;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IPackageUtil
{
	DownloadStatus GetStatus(IPackageIdentity? mod, out string reason);

	bool IsEnabled(IPackageIdentity package, int? playsetId = null, bool withVersion = true);
	bool IsIncluded(IPackageIdentity package, int? playsetId = null, bool withVersion = true);
	bool IsIncluded(IPackageIdentity package, out bool partiallyIncluded, int? playsetId = null, bool withVersion = true);
	bool IsIncludedAndEnabled(IPackageIdentity package, int? playsetId = null, bool withVersion = true);
	Task SetIncluded(IPackageIdentity package, bool value, int? playsetId = null, bool withVersion = true);
	Task SetEnabled(IPackageIdentity package, bool value, int? playsetId = null, bool withVersion = true);
	Task SetIncluded(IEnumerable<IPackageIdentity> packages, bool value, int? playsetId = null, bool withVersion = true);
	Task SetEnabled(IEnumerable<IPackageIdentity> packages, bool value, int? playsetId = null, bool withVersion = true);
	string? GetSelectedVersion(IPackageIdentity package, int? playsetId = null);
}
