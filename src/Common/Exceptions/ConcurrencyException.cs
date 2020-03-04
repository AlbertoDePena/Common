using System;

namespace Numaka.Common.Exceptions
{
    /// <summary>
    /// Concurrency exception
    /// </summary>
    public class ConcurrencyException : Exception
    {
        /// <inheritdoc />
        public ConcurrencyException() { }

        /// <inheritdoc />
        public ConcurrencyException(string message) : base(message) { }

        /// <inheritdoc />
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}