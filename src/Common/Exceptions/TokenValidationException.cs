using System;

namespace Numaka.Common.Exceptions
{
    /// <summary>
    /// Token validation exception
    /// </summary>
    public class TokenValidationException : Exception
    {
        /// <inheritdoc />
        public TokenValidationException() { }

        /// <inheritdoc />
        public TokenValidationException(string message) : base(message) { }

        /// <inheritdoc />
        public TokenValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
