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
	List<ILogTrace> SimplifyLog(string log, out string simpleLog);
}
