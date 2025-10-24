using Skyve.Domain.Enums;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IPackageUtil
{
	DownloadStatus GetStatus(IPackageIdentity? mod, out string reason);

	bool IsEnabled(IPackageIdentity package, string? playsetId = null, bool withVersion = true);
	bool IsIncluded(IPackageIdentity package, string? playsetId = null, bool withVersion = true);
	bool IsIncluded(IPackageIdentity package, out bool partiallyIncluded, string? playsetId = null, bool withVersion = true);
	bool IsIncludedAndEnabled(IPackageIdentity package, string? playsetId = null, bool withVersion = true);
	Task SetIncluded(IPackageIdentity package, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true);
	Task SetEnabled(IPackageIdentity package, bool value, string? playsetId = null);
	Task SetIncluded(IEnumerable<IPackageIdentity> packages, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true);
	Task SetEnabled(IEnumerable<IPackageIdentity> packages, bool value, string? playsetId = null);
	string? GetSelectedVersion(IPackageIdentity package, string? playsetId = null);
}
