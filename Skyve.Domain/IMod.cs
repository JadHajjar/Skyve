using System;

namespace Skyve.Domain;

public interface IMod : ILocalPackage
{
	Version Version { get; }
}
