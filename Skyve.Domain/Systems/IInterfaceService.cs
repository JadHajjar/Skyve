using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IInterfaceService
{
	void OpenParadoxLogin();
	void OpenOptionsPage();
	void ViewSpecificPackages(List<IPackageIdentity> packages, string title);
}
