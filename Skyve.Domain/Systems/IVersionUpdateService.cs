using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IVersionUpdateService
{
	void Run(List<ILocalPackageData> content);
}
