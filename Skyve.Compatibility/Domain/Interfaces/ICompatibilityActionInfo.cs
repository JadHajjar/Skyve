using Skyve.Domain;

using System.Drawing;
using System.Threading.Tasks;

namespace Skyve.Compatibility.Domain.Interfaces;

public interface ICompatibilityActionInfo
{
	Color? Color { get; }
	string Icon { get; }
	string Text { get; }

	Task Invoke(ICompatibilityItem message, IPackageIdentity? package = null);
}
