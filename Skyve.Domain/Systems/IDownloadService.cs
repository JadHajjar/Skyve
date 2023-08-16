using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IDownloadService
{
	void Download(IEnumerable<IPackageIdentity> packageIds);
}
