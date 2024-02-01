using Skyve.Compatibility.Domain.Enums;

namespace Skyve.Compatibility.Domain.Interfaces;

public interface IGenericPackageStatus
{
	StatusAction Action { get; set; }
	ulong[]? Packages { get; set; }
	string? Note { get; set; }
	NotificationType Notification { get; }
	int IntType { get; set; }
	string LocaleKey { get; }
}
