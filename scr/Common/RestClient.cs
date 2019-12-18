using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Numaka.Common
{
    public class RestClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

        /// <summary>
        /// RestClient
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="token"></param>
        public RestClient(string baseAddress, string token = null)
        {
            if (string.IsNullOrWhiteSpace(baseAddress)) throw new ArgumentNullException(nameof(baseAddress));

            _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SetBearerToken(token);
        }

        /// <summary>
        /// Execute HTTP GET
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="token">The bearer token</param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(string requestUri, string token = null)
        {
            SetBearerToken(token);

            var response = await _httpClient.GetAsync(requestUri);

            await response.EnsureSuccessStatusCodeAsync();

            return await DeserializeAsync<TResult>(response);
        }

        /// <summary>
        /// Execute HTTP POST
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="model"></param>
        /// <param name="token">The bearer token</param>
        /// <returns></returns>
        public async Task PostAsync<TModel>(string requestUri, TModel model, string token = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            SetBearerToken(token);

            using (var content = GetStringContent(model))
            {
                var response = await _httpClient.PostAsync(requestUri, content);

                await response.EnsureSuccessStatusCodeAsync();
            }
        }

        /// <summary>
        /// Execute HTTP POST
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="model"></param>
        /// <param name="token">The bearer token</param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult, TModel>(string requestUri, TModel model, string token = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            SetBearerToken(token);

            using (var content = GetStringContent(model))
            {
                var response = await _httpClient.PostAsync(requestUri, content);

                await response.EnsureSuccessStatusCodeAsync();

                return await DeserializeAsync<TResult>(response);
            }
        }

        /// <summary>
        /// Execute HTTP DELETE
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="token">The bearer token</param>
        /// <returns></returns>
        public async Task DeleteAsync(string requestUri, string token = null)
        {
            SetBearerToken(token);

            var response = await _httpClient.DeleteAsync(requestUri);

            await response.EnsureSuccessStatusCodeAsync();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _httpClient.Dispose();
            }

            _disposed = true;
        }

        private async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            using (var content = response.Content)
            {
                var json = await content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private StringContent GetStringContent(object model)
        {
            var payload = JsonConvert.SerializeObject(model);

            return new StringContent(payload, Encoding.UTF8, "application/json");
        }

        private void SetBearerToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}