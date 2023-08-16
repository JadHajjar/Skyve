using System.Collections.Generic;

namespace Skyve.Domain;
public interface ICompatibilityInfo
{
	ILocalPackage? Package { get; }
	IPackageCompatibilityInfo? Info { get; }
	IEnumerable<ICompatibilityItem> ReportItems { get; }
}
