namespace Numaka.Common.Contracts
{
    /// <summary>
    /// Kubernetes base address resolver interface
    /// </summary>
    public interface IBaseAddressResolver
    {
        /// <summary>
        /// Resolve private microservice base address.
        /// <para>K8s provides the host and port using the following convention:
        /// MY_SERVICE_SERVICE_HOST and MY_SERVICE_SERVICE_PORT</para>
        /// <para>MY_SERVICE is derived from the yml file in the Service object (kind: Service) => metadata.name</para>
        /// <para>So metadata.name: my-cool-private-service translates to MY_COOL_PRIVATE_SERVICE</para>
        /// </summary>
        /// <param name="serviceName">The kubernetes service name</param>
        /// <returns>The base address</returns>
        string ResolveBaseAddress(string serviceName);
    }
}
