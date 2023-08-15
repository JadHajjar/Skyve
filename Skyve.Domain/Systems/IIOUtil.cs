using System.Diagnostics;

namespace SkyveApp.Domain.Systems;
public interface IIOUtil
{
	Process? Execute(string exeFile, string args, bool useShellExecute = true, bool createNoWindow = false);
	void RunBatch(string command);
	string? ToRealPath(string? path);
	void WaitForUpdate();
}
