using SkyveApp.Domain.Enums;

using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;

public interface IPackageUtil
{
	IEnumerable<ILocalPackage> GetPackagesThatReference(IPackage package, bool withExcluded = false);
	DownloadStatus GetStatus(IPackage mod, out string reason);
	bool IsEnabled(ILocalPackage package);
	bool IsIncluded(ILocalPackage localPackage);
	bool IsIncluded(ILocalPackage localPackage, out bool partiallyIncluded);
	bool IsIncludedAndEnabled(ILocalPackage package);
	void SetIncluded(ILocalPackage localPackage, bool value);
	void SetEnabled(ILocalPackage localPackage, bool value);
}
