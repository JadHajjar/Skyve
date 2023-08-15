namespace SkyveApp.Domain;
public interface ILocalPackageIdentity : IPackageIdentity
{
	string FilePath { get; }
}
