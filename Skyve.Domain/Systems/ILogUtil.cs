using System.Collections.Generic;
using System.IO;

namespace SkyveApp.Domain.Systems;
public interface ILogUtil
{
	string GameDataPath { get; }
	string GameLogFile { get; }

	string CreateZipFileAndSetToClipboard(string? folder = null);
	void CreateZipToStream(Stream fileStream);
	List<ILogTrace> SimplifyLog(string log, out string simpleLog);
}
