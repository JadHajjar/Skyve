using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IUpdateManager
{
	bool IsFirstTime();
	bool IsPackageKnown(ILocalPackage package);
	IEnumerable<ILocalPackage> GetNewPackages();
}
