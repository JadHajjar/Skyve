using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;

using System.Collections.Generic;

namespace Skyve.Compatibility.Domain.Interfaces;

public interface IGenericPackageStatus
{
	StatusAction Action { get; set; }
	IEnumerable<ICompatibilityPackageIdentity> Packages { get; set; }
	string? Header { get; set; }
	string? Note { get; set; }
	NotificationType Notification { get; }
	int IntType { get; set; }
	string LocaleKey { get; }
	string Class { get; }
}
