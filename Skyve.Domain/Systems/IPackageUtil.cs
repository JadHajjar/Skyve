﻿using Skyve.Domain.Enums;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IPackageUtil
{
	IEnumerable<IPackage> GetPackagesThatReference(IPackageIdentity package, bool withExcluded = false);
	DownloadStatus GetStatus(IPackageIdentity? mod, out string reason);

	bool IsEnabled(IPackageIdentity package, int? playsetId = null);
	bool IsIncluded(IPackageIdentity package, int? playsetId = null);
	bool IsIncluded(IPackageIdentity package, out bool partiallyIncluded, int? playsetId = null);
	bool IsIncludedAndEnabled(IPackageIdentity package, int? playsetId = null);
	Task SetIncluded(IPackageIdentity package, bool value, int? playsetId = null);
	Task SetEnabled(IPackageIdentity package, bool value, int? playsetId = null);
	Task SetIncluded(IEnumerable<IPackageIdentity> packages, bool value, int? playsetId = null);
	Task SetEnabled(IEnumerable<IPackageIdentity> packages, bool value, int? playsetId = null);
}
