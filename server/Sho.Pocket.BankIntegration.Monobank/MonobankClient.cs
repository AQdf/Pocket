using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sho.BankIntegration.Monobank
{
    public class MonobankClient
    {
        private readonly HttpClient _httpClient;

        public MonobankClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (string.IsNullOrWhiteSpace(_httpClient.BaseAddress.AbsoluteUri))
            {
                _httpClient.BaseAddress = new Uri(MonobankDefaultConfig.BANK_API_URL);
            }
        }

        public async Task<string> GetPublicDataAsync(string relativeUri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(relativeUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPersonalDataAsync(string relativeUri, string token)
        {
            Uri requestUri = new Uri(_httpClient.BaseAddress, relativeUri);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("X-Token", token);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
