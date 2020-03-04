using System;
using System.Net.Http;

namespace Numaka.Common.Exceptions
{
    /// <summary>
    /// API Exception
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// The HTTP request content (details of the exception)
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public ApiException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ApiException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public ApiException(string message, string content) : base(message)
        {
            Content = content;
        }
    }
}