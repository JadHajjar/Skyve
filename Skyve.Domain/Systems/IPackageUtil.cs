using Skyve.Domain.Enums;

using System.Collections.Generic;

namespace Skyve.Domain.Systems;

public interface IPackageUtil
{
	IEnumerable<IPackage> GetPackagesThatReference(IPackage package, bool withExcluded = false);
	DownloadStatus GetStatus(IPackage mod, out string reason);
	bool IsEnabled(IPackage package);
	bool IsIncluded(IPackage localPackage);
	bool IsIncluded(IPackage localPackage, out bool partiallyIncluded);
	bool IsIncludedAndEnabled(IPackage package);
	void SetIncluded(IPackage localPackage, bool value);
	void SetEnabled(IPackage localPackage, bool value);
}
