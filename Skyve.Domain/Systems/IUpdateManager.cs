using System;
using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IUpdateManager
{
	bool IsFirstTime();
	bool IsPackageKnown(ILocalPackage package);
	DateTime GetLastUpdateTime(ILocalPackage package);
	void SendUpdateNotifications();
	IEnumerable<ILocalPackageWithContents>? GetNewOrUpdatedPackages();
}
