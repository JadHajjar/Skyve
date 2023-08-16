namespace Skyve.Domain;

public interface IPlaysetEntry : ILocalPackageIdentity
{
	bool IsMod { get; }
	string? RelativePath { get; set; }
}

public interface IPlaysetModEntry : IPlaysetEntry
{
	bool IsEnabled { get; }
}