namespace Skyve.Domain;

public interface IPackageRequirement : IPackageIdentity
{
	bool Optional { get; }
}