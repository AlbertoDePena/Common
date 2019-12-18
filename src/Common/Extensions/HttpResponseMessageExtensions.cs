using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Numaka.Common.Extensions
{
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

                throw new ApplicationException($"{message}\nContent: {json}");
            }
        }
    }
}