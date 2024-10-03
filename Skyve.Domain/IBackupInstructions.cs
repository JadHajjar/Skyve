namespace Skyve.Domain;

public interface IBackupInstructions
{
	bool DoSavesBackup { get; set; }
	bool DoLocalModsBackup { get; set; }
}

public interface IRestoreInstructions
{

}
