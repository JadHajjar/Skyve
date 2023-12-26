using System.Collections.Generic;

namespace Skyve.Domain;
public interface ICompatibilityInfo
{
	ILocalPackageData? Package { get; }
	IPackageCompatibilityInfo? Info { get; }
	IEnumerable<ICompatibilityItem> ReportItems { get; }
}
