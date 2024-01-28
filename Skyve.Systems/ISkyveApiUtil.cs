using Skyve.Systems.Compatibility.Domain;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Systems;

public interface ISkyveApiUtil
{
	Task<Dictionary<string, string>?> Translations();
}