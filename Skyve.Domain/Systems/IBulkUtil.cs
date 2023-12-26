using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IBulkUtil
{
	void SetBulkEnabled(IEnumerable<ILocalPackageData> packages, bool value);
	void SetBulkIncluded(IEnumerable<ILocalPackageData> packages, bool value);
}
