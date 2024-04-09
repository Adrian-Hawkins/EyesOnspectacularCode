using System.Text.Json.Serialization;
using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;

namespace EOSC.Bot.Commands;

public abstract class BaseCommand
{
    public string? GetCommandName()
    {
        var attribute = Attribute.GetCustomAttribute(GetType(), typeof(CommandAttribute)) as CommandAttribute;
        return attribute?.CommandName;
    }

        public abstract Task SendCommand(string botToken, List<string> args, Message message);


        public record Resp
        {
            [JsonPropertyName("content")] public required string Content { get; set; }
        }


        protected async Task SendMessageAsync(string message, string channelId, string botToken)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botToken}");

                var url = $"https://discordapp.com/api/v9/channels/{channelId}/messages";
                var req = new Resp
                {
                    Content = message
                };

                var jsonContent = JsonContent.Create(req, typeof(Resp));
                Console.WriteLine(await jsonContent.ReadAsStringAsync());

                var responseMessage = await httpClient.PostAsJsonAsync(url, req);

                var readAsStringAsync = await responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine(readAsStringAsync);

                // responseMessage.EnsureSuccessStatusCode();


                /*string url = $"https://discordapp.com/api/v9/channels/{channelId}/messages";
                string jsonPayload = $"{{\"content\": \"{message}\"}}";
                var resp = new Resp
                {
                    Content = message
                };
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
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
    }
}