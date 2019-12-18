using Serilog;
using System;
using System.IO;

namespace Numaka.Common.Logging
{
    /// <summary>
    /// Logger
    /// </summary>
    public class Logger : ILogger
    {
        private readonly Serilog.Core.Logger _logger;
        private bool _disposed;

        /// <summary>
        /// Log to console and file
        /// </summary>
        /// <param name="serviceName">The service name</param>
        /// <param name="isDebugMode">When false, minimum log level is set to warning</param>
        public Logger(string serviceName, bool isDebugMode = false)
        {
            if (string.IsNullOrWhiteSpace(serviceName)) throw new ArgumentNullException(nameof(serviceName));

            var filePath = $"{Directory.GetCurrentDirectory()}/Logs/{serviceName}.log";

            _logger =
                (isDebugMode ? new LoggerConfiguration().MinimumLevel.Debug() : new LoggerConfiguration().MinimumLevel.Warning())
                .WriteTo.Console()
                .WriteTo.File(filePath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _logger.Information("Disposing logger...");
                _logger.Dispose();
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public void LogException(string message, Exception exception) => _logger.Error(exception, message);

        /// <inheritdoc />
        public void LogMessage(string message) => _logger.Information(message);

        /// <inheritdoc />
        public void LogWarning(string message) => _logger.Warning(message);
    }
}