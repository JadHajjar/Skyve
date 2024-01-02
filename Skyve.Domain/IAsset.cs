namespace Skyve.Domain;
public interface IAsset : ILocalPackageIdentity
{
	IPackage Package { get; }
}
