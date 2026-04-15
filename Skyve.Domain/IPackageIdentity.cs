namespace Skyve.Domain;

public interface IPackageIdentity
{
	string Source { get; }
	string Id { get; }
	string Name { get; }
	string? Url { get; }
	string? Version { get; set; }
}