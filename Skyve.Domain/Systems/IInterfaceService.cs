using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IInterfaceService
{
	void OpenParadoxLogin();
	void OpenOptionsPage();
	void ViewSpecificPackages(List<ILocalPackageData> packages, string title);
}
