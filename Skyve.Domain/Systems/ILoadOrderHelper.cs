using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface ILoadOrderHelper
{
	IEnumerable<IPackage> GetOrderedMods();
}
