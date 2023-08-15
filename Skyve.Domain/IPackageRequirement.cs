namespace SkyveApp.Domain;

public interface IPackageRequirement : IPackageIdentity
{
	bool Optional { get; }
}