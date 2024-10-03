using Skyve.Domain.Systems;

namespace Skyve.Domain;
public interface IBackupItem
{
	IBackupMetaData MetaData { get; }

	bool CanSave();
	void Save(IBackupSystem backupManager);
}
