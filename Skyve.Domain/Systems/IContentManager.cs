using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IContentManager
{
	Task<List<IPackage>> LoadContents();
	IPackage? GetSaveFiles();
	IPackage? GetMapFiles();
	void ContentUpdated(string path, bool builtIn, bool workshop, bool self);
	void RefreshPackage(IPackage package, bool self);
	void StartListeners();
}
