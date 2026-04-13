using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMI.Infrastructure.Repository;
using Newtonsoft.Json;

namespace EMI.Infrastructure.HttpClientService
{
   public class HttpApiClientFactory : IHttpApiClientFactory
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMongoDbRepository<ApiLog> _mongoRepository;

        public HttpApiClientFactory(IHttpClientFactory clientFactory, IMongoDbRepository<ApiLog> mongoRepository)
        {
            _clientFactory = clientFactory;
            _mongoRepository = mongoRepository;
        }

        private HttpClient CreateClient(string baseUrl, Dictionary<string, string>? headers = null)
        {
            var client = _clientFactory.CreateClient("ExternalApi");
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();

            if (headers != null)
            {
                foreach (var kvp in headers)
                    client.DefaultRequestHeaders.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }

            return client;
        }

        private async Task LogAsync(ApiLog log)
        {
            try
            {
                _ = _mongoRepository.CreateAsync(log); // fire-and-forget
            }
            catch { /* swallow logging errors */ }
        }

        private static StringContent SerializeBody(object body, bool excludeNull = true)
        {
            var json = JsonConvert.SerializeObject(body, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = excludeNull ? NullValueHandling.Ignore : NullValueHandling.Include
            });
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> GetAsync(
            string baseUrl, string resource, string applicationName, long apiUserId, Dictionary<string, string>? headers = null)
        {
            var client = CreateClient(baseUrl, headers);
            var requestUri = new Uri(client.BaseAddress!, resource);

            var start = DateTime.UtcNow;
            var response = await client.GetAsync(requestUri);
            var end = DateTime.UtcNow;

            await LogAsync(new ApiLog
            {
                Application = applicationName,
                ApiUserId = apiUserId,
                CreatedBy = "SYSTEM",
                CreationDate = DateTime.Now,
                RequestUri = requestUri.ToString(),
                RequestMethod = "GET",
                ResponseContentBody = await response.Content.ReadAsStringAsync(),
                ResponseStatusCode = (int)response.StatusCode,
                RequestTimestamp = start,
                ResponseTimestamp = end
            });

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(
            string baseUrl, string resource, object body, string applicationName, long apiUserId,
            Dictionary<string, string>? headers = null, bool excludeNull = true)
        {
            var client = CreateClient(baseUrl, headers);
            var requestUri = new Uri(client.BaseAddress!, resource);
            var content = SerializeBody(body, excludeNull);

            var start = DateTime.UtcNow;
            var response = await client.PostAsync(requestUri, content);
            var end = DateTime.UtcNow;

            await LogAsync(new ApiLog
            {
                Application = applicationName,
                ApiUserId = apiUserId,
                CreatedBy = "SYSTEM",
                CreationDate = DateTime.Now,
                RequestUri = requestUri.ToString(),
                RequestMethod = "POST",
                RequestContentBody = await content.ReadAsStringAsync(),
                ResponseContentBody = await response.Content.ReadAsStringAsync(),
                ResponseStatusCode = (int)response.StatusCode,
                RequestTimestamp = start,
                ResponseTimestamp = end
            });

            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(
            string baseUrl, string resource, object body, string applicationName, long apiUserId,
            Dictionary<string, string>? headers = null)
        {
            var client = CreateClient(baseUrl, headers);
            var requestUri = new Uri(client.BaseAddress!, resource);
            var content = SerializeBody(body);

            var start = DateTime.UtcNow;
            var response = await client.PutAsync(requestUri, content);
            var end = DateTime.UtcNow;

            await LogAsync(new ApiLog
            {
                Application = applicationName,
                ApiUserId = apiUserId,
                CreatedBy = "SYSTEM",
                CreationDate = DateTime.Now,
                RequestUri = requestUri.ToString(),
                RequestMethod = "PUT",
                RequestContentBody = await content.ReadAsStringAsync(),
                ResponseContentBody = await response.Content.ReadAsStringAsync(),
                ResponseStatusCode = (int)response.StatusCode,
                RequestTimestamp = start,
                ResponseTimestamp = end
            });

            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            string baseUrl, string resource, string applicationName, long apiUserId, Dictionary<string, string>? headers = null)
        {
            var client = CreateClient(baseUrl, headers);
            var requestUri = new Uri(client.BaseAddress!, resource);

            var start = DateTime.UtcNow;
            var response = await client.DeleteAsync(requestUri);
            var end = DateTime.UtcNow;

            await LogAsync(new ApiLog
            {
                Application = applicationName,
                ApiUserId = apiUserId,
                CreatedBy = "SYSTEM",
                CreationDate = DateTime.Now,
                RequestUri = requestUri.ToString(),
                RequestMethod = "DELETE",
                ResponseContentBody = await response.Content.ReadAsStringAsync(),
                ResponseStatusCode = (int)response.StatusCode,
                RequestTimestamp = start,
                ResponseTimestamp = end
            });

            return response;
        }
    }
}
