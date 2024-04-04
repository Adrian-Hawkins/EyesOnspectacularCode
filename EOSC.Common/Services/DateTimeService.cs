using EOSC.Common.Requests;
using EOSC.Common.Responses;
using System.Text;
using System.Text.Json;


namespace EOSC.Common.Services
{
    public class DateTimeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public DateTimeService()
        {
            //grab from env
            _apiBaseUrl = "http://localhost:5168";
            _httpClient = new HttpClient();
        }

        public async Task<DateTimeConversionResponse> ConvertDateTime(DatetimeRequest request)
        {
            try
            {
                string jsonRequest = JsonSerializer.Serialize(request);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_apiBaseUrl + "/api/Datetime", content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    DateTimeConversionResponse? conversionResponse = JsonSerializer.Deserialize<DateTimeConversionResponse>(jsonResponse, options);
                    return conversionResponse ?? throw new Exception("Something went wrong :(");
                }
                else
                {
                    throw new Exception($"Failed to convert datetime. StatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
