using System;

namespace Numaka.Common.Logging
{
    /// <summary>
    /// ILogger
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        void LogException(string message, Exception exception);

        /// <summary>
        /// Log message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogMessage(string message);

        /// <summary>
        /// Log warning
        /// </summary>
        /// <param name="message">The message to log as a warning</param>
        void LogWarning(string message);
    }
}
