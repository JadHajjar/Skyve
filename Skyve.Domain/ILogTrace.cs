using System.Collections.Generic;

namespace Skyve.Domain;
public interface ILogTrace
{
	string Title { get; }
	string Timestamp { get; }
	bool Crash { get; }
	List<string> Trace { get; }
}
