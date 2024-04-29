using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Domain;
public interface IModCommentsInfo
{
	bool CanPost { get; set; }
	bool HasMore { get; set; }
	int Page { get; set; }
	List<IModComment>? Posts { get; set; }
}
