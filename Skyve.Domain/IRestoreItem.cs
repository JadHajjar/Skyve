using Skyve.Domain.Systems;

using System.IO;
using System.Threading.Tasks;

namespace Skyve.Domain;

public interface IRestoreItem
{
	IBackupMetaData MetaData { get; }
	object? ItemMetaData { get; }
	FileInfo BackupFile { get; }

	Task<bool> Restore(IBackupSystem backupManager);
}