using System;

namespace Skyve.Domain.Systems;
public interface ILogger
{
	string LogFilePath { get; }
	string PreviousLogFilePath { get; }

	void Info(object message);
	void Warning(object message);
	void Error(object message);
	void Exception(Exception exception, object message);

#if DEBUG
	void Debug(object message);
#endif
}
