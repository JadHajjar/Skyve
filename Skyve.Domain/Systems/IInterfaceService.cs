using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IInterfaceService
{
	void ViewSpecificPackages(List<ILocalPackageWithContents> packages, string title);
}
