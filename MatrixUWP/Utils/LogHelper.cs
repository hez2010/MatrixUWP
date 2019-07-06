using System;
using System.Diagnostics.Tracing;
using Windows.Foundation.Diagnostics;

namespace MatrixUWP.Utils
{
    [EventData]
    class LogMessage<T>
    {
        public LogMessage(T message)
        {
            Message = message;
        }
        public T Message;
    }
    static class LogHelper
    {
        private const string splitString = "\n-----------------\n";
#if DEBUG
        private static readonly EventSource logger = new EventSource("MatrixUWP-Debug", EventSourceSettings.EtwSelfDescribingEventFormat);
#else
        private static readonly LoggingChannel logger = new LoggingChannel("MatrixUWP", null);
#endif
        public static void Log(this Exception ex, LoggingLevel severity = LoggingLevel.Information)
        {
            var name = Enum.GetName(typeof(LoggingLevel), severity);
            logger.Write(name, new LogMessage<Exception>(ex));
        }

        public static void Log(string message, LoggingLevel severity = LoggingLevel.Information)
        {
            var name = Enum.GetName(typeof(LoggingLevel), severity);
            logger.Write(name, new LogMessage<string>(message));
        }
    }
}
