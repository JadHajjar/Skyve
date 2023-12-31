﻿using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IBulkUtil
{
	void SetBulkEnabled(IEnumerable<ILocalPackage> packages, bool value);
	void SetBulkIncluded(IEnumerable<ILocalPackage> packages, bool value);
}
