using System;

namespace Skyve.Domain;
public interface ILocalPackageIdentity : IPackageIdentity
{
	string Folder { get; }
	string FilePath { get; }
	long FileSize { get; }
	DateTime LocalTime { get; }
}
