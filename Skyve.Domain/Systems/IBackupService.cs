using System;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IBackupService
{
	Func<Task>? PreBackupTask { get; set; }
	Func<Task>? PostBackupTask { get; set; }

	Task<bool> Run();
}
