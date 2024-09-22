namespace Skyve.Domain.Systems;
public interface ISettings
{
	IUserSettings UserSettings { get; }
	ISessionSettings SessionSettings { get; }
	IFolderSettings FolderSettings { get; }
	IBackupSettings BackupSettings { get; }

	void ResetFolderSettings();
	void ResetUserSettings();
}
