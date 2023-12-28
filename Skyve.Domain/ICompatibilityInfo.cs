using System.Collections.Generic;

namespace Skyve.Domain;
public interface ICompatibilityInfo : IPackageIdentity
{
	IPackageCompatibilityInfo? Info { get; }
	IEnumerable<ICompatibilityItem> ReportItems { get; }
}
