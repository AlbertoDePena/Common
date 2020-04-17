using Numaka.Common.Contracts;
using System;

namespace Numaka.Common
{
    /// <inheritdoc/>
    public class BaseAddressResolver : IBaseAddressResolver
    {
        private readonly Func<string, string> _environmentVariableFunc;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environmentVariableFunc">Optional function to get the environment variable. If not provided, Environment.GetEnvironmentVariable is used...</param>
        public BaseAddressResolver(Func<string, string> environmentVariableFunc = null)
        {
            _environmentVariableFunc = environmentVariableFunc ?? Environment.GetEnvironmentVariable;
        }

        /// <inheritdoc/>
        public string ResolveBaseAddress(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ArgumentNullException(nameof(serviceName));

            serviceName = serviceName.Replace("-", "_").ToUpper();

            var host = _environmentVariableFunc($"{serviceName}_SERVICE_HOST");
            var port = _environmentVariableFunc($"{serviceName}_SERVICE_PORT");

            return $"http://{host}:{port}";
        }
    }
}
