using System.Collections.Generic;

namespace Skyve.Domain;

public class IPackageEqualityComparer : IEqualityComparer<IPackage>
{
	public bool Equals(IPackage x, IPackage y)
	{
		return x is null
			? y is null
			: y is null
			? x is null
			: x.Id == y.Id
|| x is ILocalPackageData localPackage1 && y is ILocalPackageData localPackage2 && localPackage1.Folder == localPackage2.Folder;
	}

	public int GetHashCode(IPackage obj)
	{
		return -1586376059 + obj.Id.GetHashCode() + (obj is ILocalPackageData localPackage ? localPackage.Folder : string.Empty).GetHashCode();
	}
}
