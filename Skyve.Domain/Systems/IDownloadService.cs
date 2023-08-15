using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface IDownloadService
{
	void Download(IEnumerable<IPackageIdentity> packageIds);
}
