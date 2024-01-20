namespace Skyve.Domain;

public interface IPackageRequirement : IPackageIdentity
{
	bool IsDlc { get; }
	bool IsOptional { get; }
}