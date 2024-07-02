using System.Diagnostics;

namespace Skyve.Domain.Systems;
public interface IIOUtil
{
	Process? Execute(string exeFile, string args, bool useShellExecute = true, bool createNoWindow = false, bool administrator = false);
	void RunBatch(string command);
	string? ToRealPath(string? path);
	void WaitForUpdate();
}
