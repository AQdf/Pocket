using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sho.BankIntegration.Monobank
{
    public class MonobankHttpClient
    {
        private const string HEADER_X_TOKEN = "X-Token";

        private readonly HttpClient _httpClient;

        public MonobankHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (string.IsNullOrWhiteSpace(_httpClient.BaseAddress.AbsoluteUri))
            {
                _httpClient.BaseAddress = new Uri(MonobankConfig.BANK_API_URL);
            }
        }

        internal async Task<HttpResponseMessage> GetPublicDataAsync(string relativeUri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(relativeUri);
            response.EnsureSuccessStatusCode();

            return response;
        }

        internal async Task<HttpResponseMessage> GetPersonalDataAsync(string relativeUri, string token)
        {
            Uri requestUri = new Uri(_httpClient.BaseAddress, relativeUri);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add(HEADER_X_TOKEN, token);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        internal async Task<HttpResponseMessage> PostAsync(string relativeUri, string body, string token)
        {
            Uri requestUri = new Uri(_httpClient.BaseAddress, relativeUri);
            StringContent content = new StringContent(body);
            content.Headers.Add(HEADER_X_TOKEN, token);
            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
