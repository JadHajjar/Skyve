using Skyve.Domain.Enums;

using System;

namespace Skyve.Domain;

public interface IBackupMetaData
{
	bool IsArchived { get; set; }
	string? Name { get; set; }
	DateTime BackupTime { get; set; }
	DateTime ContentTime { get; }
	int FileCount { get; set; }
	string? Root { get; }
	string? Type { get; }
	RestoreAction RestoreType { get; }
	string? ItemMetaDataType { get; set; }
}