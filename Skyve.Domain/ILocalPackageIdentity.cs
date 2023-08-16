namespace Skyve.Domain;
public interface ILocalPackageIdentity : IPackageIdentity
{
	string FilePath { get; }
}
