using System.Collections.Generic;

namespace Skyve.Domain.Systems;

public interface IContentManager
{
	List<ILocalPackageWithContents> LoadContents();
	void ContentUpdated(string path, bool builtIn, bool workshop, bool self);
	IEnumerable<ILocalPackage> GetReferencingPackage(ulong steamId, bool includedOnly);
	void RefreshPackage(ILocalPackage package, bool self);
	void StartListeners();
}
