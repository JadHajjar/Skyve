using System;
using System.Runtime.CompilerServices;

namespace Skyve.Domain.Systems;
public interface ILogger
{
	string LogFilePath { get; }
	string PreviousLogFilePath { get; }

	void Info(object message, [CallerLineNumber] int? lineNumber = default, [CallerMemberName] string? memberName = default);
	void Warning(object message, [CallerLineNumber] int? lineNumber = default, [CallerMemberName] string? memberName = default);
	void Error(object message, [CallerLineNumber] int? lineNumber = default, [CallerMemberName] string? memberName = default);
	void Exception(object message, [CallerLineNumber] int? lineNumber = default, [CallerMemberName] string? memberName = default);
	void Exception(Exception exception, object message, [CallerLineNumber] int? lineNumber = default, [CallerMemberName] string? memberName = default);

#if DEBUG
	void Debug(object message, [CallerLineNumber] int? lineNumber = default, [CallerMemberName] string? memberName = default);
#endif
}
