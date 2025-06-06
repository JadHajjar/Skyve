﻿using Skyve.Compatibility.Domain.Enums;

using System;

namespace Skyve.Compatibility.Domain.Interfaces;

public interface IPackageStatus<TType> : IGenericPackageStatus where TType : struct, Enum
{
	TType Type { get; set; }

	IPackageStatus<TType> Duplicate();
}
