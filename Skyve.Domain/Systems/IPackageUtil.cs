using Skyve.Domain.Enums;

using System.Collections.Generic;

namespace Skyve.Domain.Systems;

public interface IPackageUtil
{
	IEnumerable<IPackage> GetPackagesThatReference(IPackageIdentity package, bool withExcluded = false);
	DownloadStatus GetStatus(IPackageIdentity? mod, out string reason);
	bool IsEnabled(IPackageIdentity package);
	bool IsIncluded(IPackageIdentity localPackage);
	bool IsIncluded(IPackageIdentity localPackage, out bool partiallyIncluded);
	bool IsIncludedAndEnabled(IPackageIdentity package);
	void SetIncluded(IPackageIdentity localPackage, bool value);
	void SetEnabled(IPackageIdentity localPackage, bool value);
}
