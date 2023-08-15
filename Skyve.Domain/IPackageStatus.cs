using System;

namespace SkyveApp.Domain;

public interface IPackageStatus<TType> : IGenericPackageStatus where TType : struct, Enum
{
	TType Type { get; set; }
}
