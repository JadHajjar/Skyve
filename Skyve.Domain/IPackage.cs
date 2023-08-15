using Extensions;

using System.Collections.Generic;

namespace SkyveApp.Domain;
public interface IPackage : IPackageIdentity
{
	bool IsMod { get; }
	bool IsLocal { get; }
	bool IsBuiltIn { get; }
	[CloneIgnore]
	ILocalPackageWithContents? LocalParentPackage { get; }
	[CloneIgnore]
	ILocalPackage? LocalPackage { get; }
	[CloneIgnore]
	IEnumerable<IPackageRequirement> Requirements { get; }
}
