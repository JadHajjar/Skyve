namespace Skyve.Domain;

public interface IBackupInstructions
{
	bool DoSavesBackup { get; set; }
	bool DoLocalModsBackup { get; set; }
	bool DoMapsBackup { get; set; }
}

public interface IRestoreInstructions
{

}
