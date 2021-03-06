#nullable enable
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using Windows.Foundation.Diagnostics;

namespace MatrixUWP.Utils
{
    [EventData]
    internal class LogMessage<T>
    {
        public LogMessage(T message)
        {
            Message = message;
        }
        public T Message;
    }

    internal static class LogHelper
    {
        public static void Log(this Exception ex, LoggingLevel severity = LoggingLevel.Information) => Debugger.Break();

        public static void Log(string message, LoggingLevel severity = LoggingLevel.Information) => Debugger.Break();
    }
}
