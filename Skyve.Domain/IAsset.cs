using Skyve.Domain.Enums;

using System.Collections.Generic;

namespace Skyve.Domain;
public interface IAsset : ILocalPackageIdentity
{
	IPackage? Package { get; set; }
	AssetType AssetType { get; }
	string[] Tags { get; }
}
