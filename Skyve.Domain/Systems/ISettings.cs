﻿namespace SkyveApp.Domain.Systems;
public interface ISettings
{
	IUserSettings UserSettings { get; }
	ISessionSettings SessionSettings { get; }
	IFolderSettings FolderSettings { get; }

	void ResetFolderSettings();
	void ResetUserSettings();
}
