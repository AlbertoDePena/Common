using System;

namespace Numaka.Common.Extensions
{
    /// <summary>
    /// ExceptionExtensions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
		///     Extract exception's message (including inner exception).
		///     Useful for gathering all nested messages for a given exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>System.String.</returns>
        public static string ToDetails(this Exception exception)
        {
            var message = exception.Message;

            if (exception.InnerException == null)
            {
                return message;
            }

            if (exception.InnerException.Message == message)
            {
                return message;
            }

            return $"{message}\n{ToDetails(exception.InnerException)}";
        }
    }
}