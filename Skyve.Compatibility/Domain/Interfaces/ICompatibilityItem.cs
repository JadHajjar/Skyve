using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Collections.Generic;

namespace Skyve.Compatibility.Domain.Interfaces;
public interface ICompatibilityItem : IPackageIdentity
{
	ulong PackageId { get; }
	IGenericPackageStatus Status { get; }
	ReportType Type { get; }
	IEnumerable<IPackageIdentity> Packages { get; }

	string GetMessage(IWorkshopService workshopService, IPackageNameUtil packageNameUtil);
}
