using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using EOSC.Common.Config;
using Microsoft.Extensions.Configuration;

namespace EOSC.Common.Services;

public class BaseService
{
    private readonly HttpClient _httpClient;

    public BaseService()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<APIEndpoint>().Build();
        var apiBaseUrl = config["api:endpoint"] ?? throw new Exception("Please provide api endpoint");
        _httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
    }

    public async Task<T?> RunRequest<T, TValue>(string path, TValue requestValue)
    {
        try
        {
            // Convert object to json 
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestValue), Encoding.UTF8,
                "application/json");
            
            // Post json to endpoint
            var response = await _httpClient.PostAsync(path, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadFromJsonAsync<T>();
                return jsonResponse;
            }

            throw new Exception($"Failed to convert datetime. StatusCode: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}