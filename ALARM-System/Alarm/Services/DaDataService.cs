using Alarm.Models;
using Newtonsoft.Json;
using System.Text;

namespace Alarm.Services
{
    public class DaDataService
    {
        private readonly HttpClient _httpClient;

        public DaDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DaDataResponse> GetCompanyInfo(string inn)
        {
            var url = "http://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party";
            var requestBody = JsonConvert.SerializeObject(new { query = inn });

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Accept", "application/json");

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string apiKey = config["ApiKeys:DaData"];

            request.Headers.Add("Authorization", $"Token {apiKey}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DaDataResponse>(jsonResponse);
        }
    }
}
