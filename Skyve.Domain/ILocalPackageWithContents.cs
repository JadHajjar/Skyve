namespace SkyveApp.Domain;

public interface ILocalPackageWithContents : ILocalPackage
{
	IAsset[] Assets { get; }
	IMod? Mod { get; }
}
