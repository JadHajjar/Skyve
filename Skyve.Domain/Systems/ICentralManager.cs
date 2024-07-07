using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface ICentralManager
{
	Task Initialize();
	Task<bool> RunCommands();
	void Start();
}
