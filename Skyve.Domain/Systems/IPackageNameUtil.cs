using System.Collections.Generic;
using System.Drawing;

namespace SkyveApp.Domain.Systems;
public interface IPackageNameUtil
{
	string CleanName(IPackageIdentity? package, bool keepTags = false);
	string CleanName(IPackageIdentity? package, out List<(Color Color, string Text)> tags, bool keepTags = false);
	string GetVersionText(string name);
}
