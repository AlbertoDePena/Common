using System;

namespace Numaka.Common.Logging
{
    public interface ILogger : IDisposable
    {
        void LogException(string message, Exception exception);

        void LogMessage(string message);

        void LogWarning(string message);
    }
}
