namespace Skyve.Domain;

public interface IPackageIdentity : IThumbnailObject
{
	ulong Id { get; }
	string Name { get; }
	string? Url { get; }
}