using System;
using System.Collections.Generic;

namespace Skyve.Domain;
public interface ILogTrace
{
	string SourceFile { get; }
	string Type { get; }
	string Title { get; }
	DateTime Timestamp { get; }
	List<string> Trace { get; }
}
