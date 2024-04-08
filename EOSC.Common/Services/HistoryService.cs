using EOSC.Common.Config;
using EOSC.Common.Responses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EOSC.Common.Services
{
    public class HistoryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public HistoryService()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .AddUserSecrets<APIEndpoint>()
                 .AddEnvironmentVariables()
                 .Build();
            _apiBaseUrl = config["api:endpoint"] ?? throw new Exception("Please provide api endpoint");
            _httpClient = new HttpClient();
        }

        public async Task<HistoryResponse?> GetHistoryAsync(string username)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("username", username);
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/History/{username}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve history for user: {username}. Status code: {response.StatusCode}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                HistoryResponse? history = JsonSerializer.Deserialize<HistoryResponse>(jsonResponse, options);
                return history ?? throw new Exception("somethign wrong");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching history: {ex.Message}");
                return null;
                throw;
            }
        }
    }
}
