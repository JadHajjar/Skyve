using System;

namespace SkyveApp.Domain;

public interface IMod : ILocalPackage
{
	Version Version { get; }
}
