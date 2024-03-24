using Skyve.Domain;

namespace Skyve.Compatibility.Domain.Interfaces;

public interface ICompatibilityActionsHelper
{
	bool CanSnooze(ICompatibilityItem message);
	ICompatibilityActionInfo? GetAction(ICompatibilityItem message, IPackageIdentity package);
	ICompatibilityActionInfo? GetBulkAction(ICompatibilityItem message);
	ICompatibilityActionInfo? GetRecommendedAction(ICompatibilityItem message);
	bool HasAction(ICompatibilityItem message, IPackageIdentity package);
	bool HasBulkAction(ICompatibilityItem message);
	bool HasRecommendedAction(ICompatibilityItem message);
}
