using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EOSC.Common.Config;
using Microsoft.Extensions.Configuration;
using EOSC.Common.Responses;
namespace EOSC.Common.Services.ChatGPT
{
	public class GPTquery
	{
		public static async Task<GPTResponse> GenerateText(string prompt)
		{
			Console.WriteLine($"Prompt: {prompt}");
			HttpClient client = new HttpClient();
			var url = "https://api.openai.com/v1/chat/completions";
            IConfiguration config = new ConfigurationBuilder()
				.AddUserSecrets<GPT>()
				.AddEnvironmentVariables()
				.Build();
            string apiKey = config["gpt:key"] ?? throw new Exception("Please provide api key");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

			var request = new
			{
				model = "gpt-3.5-turbo",
				messages = new[] { new { role = "user", content = prompt } },
				max_tokens = 100, // Maximum number of tokens to generate
				temperature = 0.7, // Controls the randomness of the generated text
				top_p = 1 // Controls the diversity of the generated text
			};

			var jsonRequest = JsonSerializer.Serialize(request);
			var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

			var response = await client.PostAsync(url, content);
			var responseBody = await response.Content.ReadAsStringAsync();

			// Parse response JSON
			var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
			var choices = jsonResponse.GetProperty("choices")[0];
			var con = choices.GetProperty("message");
			var generatedText = con.GetProperty("content").GetString();
			Console.WriteLine(generatedText);

			return new GPTResponse(generatedText);
		}
	}
}
