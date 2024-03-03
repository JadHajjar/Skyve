namespace Skyve.App.Interfaces;
public interface IRightClickService
{
	SlickStripItem[] GetRightClickMenuItems(IEnumerable<IPackageIdentity> packages);
	SlickStripItem[] GetRightClickMenuItems(IPackageIdentity package);
	SlickStripItem[] GetRightClickMenuItems(IPlayset playset, bool isLocal);
}
