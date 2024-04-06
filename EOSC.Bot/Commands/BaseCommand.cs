using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSC.Bot.Commands
{
    public class BaseCommand
    {
        public async Task SendMessageAsync(string message, string channelId, string botToken)
        {
            try
            {
                string url = $"https://discordapp.com/api/v9/channels/{channelId}/messages";
                string jsonPayload = $"{{\"content\": \"{message}\"}}";
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botToken}");
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("POST request successful. Message sent.");
                    }
                    else
                    {
                        Console.WriteLine($"POST request failed with status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
    }
}
