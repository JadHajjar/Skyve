using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IContentManager
{
	Task<List<ILocalPackageData>> LoadContents();
	void ContentUpdated(string path, bool builtIn, bool workshop, bool self);
	IEnumerable<ILocalPackageData> GetReferencingPackage(ulong steamId, bool includedOnly);
	void RefreshPackage(ILocalPackageData package, bool self);
	void StartListeners();
}
