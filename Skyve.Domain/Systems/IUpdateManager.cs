using System;
using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IUpdateManager
{
	bool IsFirstTime();
	bool IsPackageKnown(ILocalPackageData package);
	DateTime GetLastUpdateTime(ILocalPackageData package);
	void SendUpdateNotifications();
	IEnumerable<ILocalPackageData>? GetNewOrUpdatedPackages();
}
