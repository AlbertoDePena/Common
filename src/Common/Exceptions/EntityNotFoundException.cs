using System;

namespace Numaka.Common.Exceptions
{
    /// <summary>
    /// Entity not found exception
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <inheritdoc />
        public EntityNotFoundException() { }

        /// <inheritdoc />
        public EntityNotFoundException(string message) : base(message) { }

        /// <inheritdoc />
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
