using System.Collections.Generic;

namespace SkyveApp.Domain;

public class IPackageEqualityComparer : IEqualityComparer<IPackage>
{
	public bool Equals(IPackage x, IPackage y)
	{
		return x is null
			? y is null
			: y is null
			? x is null
			: x.Id == y.Id
|| (x is ILocalPackage localPackage1 && y is ILocalPackage localPackage2 && localPackage1.Folder == localPackage2.Folder);
	}

	public int GetHashCode(IPackage obj)
	{
		return -1586376059 + obj.Id.GetHashCode() + (obj is ILocalPackage localPackage ? localPackage.Folder : string.Empty).GetHashCode();
	}
}
