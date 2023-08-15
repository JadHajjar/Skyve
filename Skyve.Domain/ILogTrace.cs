using System.Collections.Generic;

namespace SkyveApp.Domain;
public interface ILogTrace
{
	string Title { get; }
	string Timestamp { get; }
	bool Crash { get; }
	List<string> Trace { get; }
}
