using Extensions;

using System;

namespace SkyveApp.Domain;
public interface ILocalPackage : IPackage, ILocalPackageIdentity
{
	[CloneIgnore]
	new ILocalPackageWithContents LocalParentPackage { get; }
	long LocalSize { get; }
	DateTime LocalTime { get; }
	string Folder { get; }
}
