using Extensions;

using System;

namespace Skyve.Domain;
public interface ILocalPackageData : ILocalPackageIdentity
{
	long LocalSize { get; }
	DateTime LocalTime { get; }
	string Version { get; }
	bool IsCodeMod { get; }
	IAsset[] Assets { get; }
}
