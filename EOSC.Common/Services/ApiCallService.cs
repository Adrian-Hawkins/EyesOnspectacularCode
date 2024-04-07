using System.Net.Http.Json;
using System.Text.Json;
using EOSC.Common.Config;
using Microsoft.Extensions.Configuration;

namespace EOSC.Common.Services;

public class ApiCallService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ApiCallService()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets<APIEndpoint>()
            .AddEnvironmentVariables()
            .Build();
        _apiBaseUrl = config["api:endpoint"] ?? throw new Exception("Please provide api endpoint");
        _httpClient = new HttpClient();
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<TO> MakeApiCall<TI, TO>(string path, TI request)
    {
        try
        {
            var postAsJsonAsync = await _httpClient.PostAsJsonAsync(_apiBaseUrl + path, request);
            var readFromJsonAsync = await postAsJsonAsync.Content.ReadFromJsonAsync<TO>(_jsonSerializerOptions);
            return readFromJsonAsync ?? throw new Exception("Unable to do conversion");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}