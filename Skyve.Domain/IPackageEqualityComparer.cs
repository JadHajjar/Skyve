using System.Collections.Generic;

namespace Skyve.Domain;

public class IPackageEqualityComparer : IEqualityComparer<IPackageIdentity>
{
	public bool Equals(IPackageIdentity x, IPackageIdentity y)
	{
		return x is null
			? y is null
			: y is null
			? x is null
			: x.Id == y.Id
			|| x is IPackage package1 && y is IPackage package2 && package1.LocalData?.Folder == package1.LocalData?.Folder
			|| x is ILocalPackageIdentity localPackage1 && y is ILocalPackageIdentity localPackage2 && localPackage1.Folder == localPackage2.Folder;
	}

	public int GetHashCode(IPackageIdentity obj)
	{
		return -1586376059 + obj.Id.GetHashCode()
			+ (obj is IPackage package ? package.LocalData?.Folder ?? string.Empty : obj is ILocalPackageIdentity localPackage ? localPackage.Folder : string.Empty).GetHashCode();
	}
}
