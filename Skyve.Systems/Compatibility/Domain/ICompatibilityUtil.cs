using SkyveApp.Domain;
using SkyveApp.Systems.Compatibility.Domain.Api;

using System;

namespace SkyveApp.Systems.Compatibility.Domain;
public interface ICompatibilityUtil
{
	DateTime MinimumModDate { get; }

	void PopulateAutomaticPackageInfo(CompatibilityPackageData info, IPackage package, IWorkshopInfo? workshopInfo);
	void PopulatePackageReport(IndexedPackage packageData, CompatibilityInfo info, CompatibilityHelper compatibilityHelper);
}
