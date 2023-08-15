using System.Collections.Generic;

namespace SkyveApp.Domain;
public interface ICompatibilityInfo
{
	ILocalPackage? Package { get; }
	IPackageCompatibilityInfo? Info { get; }
	IEnumerable<ICompatibilityItem> ReportItems { get; }
}
