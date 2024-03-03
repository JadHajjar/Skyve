using Skyve.Compatibility.Domain.Interfaces;


using System;

namespace Skyve.Systems.Compatibility.Domain;
public interface ICompatibilityUtil
{
	DateTime MinimumModDate { get; }

	void PopulatePackageReport(IPackageCompatibilityInfo packageData, CompatibilityInfo info, CompatibilityHelper compatibilityHelper);
}
