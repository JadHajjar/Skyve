using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface IVersionUpdateService
{
	void Run(List<ILocalPackageWithContents> content);
}
