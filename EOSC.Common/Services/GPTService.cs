using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Config;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace EOSC.Common.Services
{
	public class GPTService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiBaseUrl;

		public GPTService()
		{
			//IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<APIEndpoint>().Build();
			IConfiguration config = new ConfigurationBuilder()
				 .AddUserSecrets<APIEndpoint>()
				 .AddEnvironmentVariables()
				 .Build();
			_apiBaseUrl = config["api:endpoint"] ?? throw new Exception("Please provide api endpoint");
			_httpClient = new HttpClient();
		}

		public async Task<DateTimeConversionResponse> queryGPT(GPTRequest request)
		{
			try
			{
				string jsonRequest = JsonSerializer.Serialize(request);
				StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _httpClient.PostAsync(_apiBaseUrl + "/api/GPT", content);

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
