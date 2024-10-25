using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IBackupService
{
	Task Run();
}
