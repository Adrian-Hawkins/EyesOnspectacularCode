using System.Net.Http.Json;
using System.Text.Json.Serialization;
using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Constant;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands;

public abstract class BaseCommand
{

    protected readonly BotAuth _botAuth = new();
    protected readonly ApiCallService _apiCallService = new();

    public abstract Task SendCommand(string botToken, List<string> args, Message message);


    public record Resp
    {
        [JsonPropertyName("content")] public required string Content { get; set; }
    }


    protected async Task SendMessageAsync(string sendData, Message messageObject, string botToken)
    {
        try
        {
            using var httpClient = new HttpClient();

            var headers = httpClient.DefaultRequestHeaders;
            headers.Add("Authorization", $"Bot {botToken}");
            headers.Add("username", messageObject.Author.Username);


            var url = $"https://discordapp.com/api/v9/channels/{messageObject.ChannelId}/messages";
            var req = new Resp
            {
                Content = sendData
            };

            var jsonContent = JsonContent.Create(req, typeof(Resp));
            Console.WriteLine(await jsonContent.ReadAsStringAsync());

            var responseMessage = await httpClient.PostAsJsonAsync(url, req);

            var readAsStringAsync = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(readAsStringAsync);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
        }
    }
}