using Skyve.Domain.Enums;

using System.Collections.Generic;

namespace Skyve.Domain.Systems;

public interface IPackageUtil
{
	IEnumerable<IPackage> GetPackagesThatReference(IPackageIdentity package, bool withExcluded = false);
	DownloadStatus GetStatus(ILocalPackageIdentity? mod, out string reason);
	bool IsEnabled(ILocalPackageIdentity package);
	bool IsIncluded(ILocalPackageIdentity localPackage);
	bool IsIncluded(ILocalPackageIdentity localPackage, out bool partiallyIncluded);
	bool IsIncludedAndEnabled(ILocalPackageIdentity package);
	void SetIncluded(ILocalPackageIdentity localPackage, bool value);
	void SetEnabled(ILocalPackageIdentity localPackage, bool value);
}
