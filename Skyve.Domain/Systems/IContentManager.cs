using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IContentManager
{
	Task<List<ILocalPackageWithContents>> LoadContents();
	void ContentUpdated(string path, bool builtIn, bool workshop, bool self);
	IEnumerable<ILocalPackage> GetReferencingPackage(ulong steamId, bool includedOnly);
	void RefreshPackage(ILocalPackage package, bool self);
	void StartListeners();
}
