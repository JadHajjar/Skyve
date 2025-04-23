using Skyve.Domain;

using System.Collections.Generic;

namespace Skyve.Compatibility.Domain.Interfaces;
public interface ICompatibilityInfo : IPackageIdentity
{
	bool IsDlc { get; }
	IPackageCompatibilityInfo? Info { get; }
	IEnumerable<ICompatibilityItem> ReportItems { get; }
	ILocalPackageData? LocalData { get; }
}
