using System;

namespace Numaka.Common.Exceptions
{
    /// <summary>
    /// Repository exception
    /// </summary>
    public class RepositoryException : Exception
    {
        /// <inheritdoc />
        public RepositoryException() { }

        /// <inheritdoc />
        public RepositoryException(string message) : base(message) { }

        /// <inheritdoc />
        public RepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }
}
