using System;

namespace Skyve.Domain;

public interface IPackageStatus<TType> : IGenericPackageStatus where TType : struct, Enum
{
	TType Type { get; set; }
}
