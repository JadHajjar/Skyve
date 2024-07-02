using Skyve.Domain.Enums;

namespace Skyve.Domain;
public interface IAsset : ILocalPackageIdentity
{
	IPackage? Package { get; set; }
	AssetType AssetType { get; }
}
