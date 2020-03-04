using System.Net.Http;
using System.Threading.Tasks;
using Numaka.Common.Exceptions;

namespace Numaka.Common.Extensions
{
    /// <summary>
    /// HTTP Response Message Extensions
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Ensure success status code extension method
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            using (var content = response.Content)
            {
                var json = await content.ReadAsStringAsync();

                var message = $"StatusCode: {response.StatusCode}, StatusReason: {response.ReasonPhrase}";

                throw new ApiException(message, content: json);
            }
        }
    }
}