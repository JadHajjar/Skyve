namespace Skyve.Domain;

public interface ILocalPackageWithContents : ILocalPackage
{
	IAsset[] Assets { get; }
	IMod? Mod { get; }
}
