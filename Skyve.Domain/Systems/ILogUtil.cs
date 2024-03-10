using System.Collections.Generic;
using System.IO;

namespace Skyve.Domain.Systems;
public interface ILogUtil
{
	string GameDataPath { get; }
	string GameLogFile { get; }
	string GameLogFolder { get; }

	string CreateZipFile(string? folder = null);
	void CreateZipToStream(Stream fileStream);
	List<ILogTrace> GetCurrentLogsTrace();
#if CS2
	List<ILogTrace> ExtractTrace(string originalFile, string log);
#else
	List<ILogTrace> SimplifyLog(string originalFile, string log, out string simpleLog);
#endif
}
