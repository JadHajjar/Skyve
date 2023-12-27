using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IBulkUtil
{
	void SetBulkEnabled(IEnumerable<ILocalPackageIdentity> packages, bool value);
	void SetBulkIncluded(IEnumerable<ILocalPackageIdentity> packages, bool value);
}
