using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface ILoadOrderHelper
{
	IEnumerable<IMod> GetOrderedMods();
}
