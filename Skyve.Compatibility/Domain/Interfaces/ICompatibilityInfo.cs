using Skyve.Domain;

using System.Collections.Generic;

namespace Skyve.Compatibility.Domain.Interfaces;
public interface ICompatibilityInfo : IPackageIdentity
{
	IPackageCompatibilityInfo? Info { get; }
	IEnumerable<ICompatibilityItem> ReportItems { get; }
}
