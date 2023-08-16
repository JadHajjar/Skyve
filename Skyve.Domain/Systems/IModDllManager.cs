using System;

namespace Skyve.Domain.Systems;
public interface IModDllManager
{
	void SaveDllCache();
	void ClearDllCache();
	void SetDllModCache(string path, bool isMod, Version? version);
	bool? GetDllModCache(string path, out Version? version);
}
