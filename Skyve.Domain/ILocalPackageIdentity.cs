namespace Skyve.Domain;
public interface ILocalPackageIdentity : IPackageIdentity
{
	string Folder { get; }
	string FilePath { get; }
}
