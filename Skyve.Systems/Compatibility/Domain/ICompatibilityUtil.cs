using Skyve.Domain;
using Skyve.Systems.Compatibility.Domain.Api;

using System;

namespace Skyve.Systems.Compatibility.Domain;
public interface ICompatibilityUtil
{
	DateTime MinimumModDate { get; }

	void PopulateAutomaticPackageInfo(CompatibilityPackageData info, IPackage package, IWorkshopInfo? workshopInfo);
	void PopulatePackageReport(IndexedPackage packageData, CompatibilityInfo info, CompatibilityHelper compatibilityHelper);
}
