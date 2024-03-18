using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface ICentralManager
{
	Task Initialize();
	void Start();
}
