﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IBackupSystem
{
	IBackupInstructions BackupInstructions { get; }
	IRestoreInstructions RestoreInstructions { get; }

	Task<bool> DoBackup();
	void DoCleanup();
	List<IRestoreItem> GetAllBackups();
	long GetBackupsSizeOnDisk();
	Task<bool> Restore(IBackupMetaData metaData, string file);
	void Save(IBackupMetaData metaData, string[] files);
}
