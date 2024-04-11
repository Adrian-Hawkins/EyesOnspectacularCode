using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands;

[Command("jsonpretty")]

public class JsonPrettyCommand : BaseCommand
{
    private readonly ApiCallService _apiCallService = new();

    public override async Task SendCommand(string discordToken, List<string> args, Message message)
    {

        if (message.Content.Split(" ").Length <=1)
        {
            await SendMessageAsync("Usage: !jsonpretty <json>", message.ChannelId, discordToken);
            return;
        }

        string jsonString = message.Content.Substring("!jsonpretty ".Length);
        var response = await JsonPrettier(jsonString);

        await SendMessageAsync($"```{response}```", message.ChannelId, discordToken);
    }

    private async Task<string> JsonPrettier(string InputText)
    {
        try
        {
            var requestObject = new JsonPrettyRequest(InputText);
            var JsonPretty =
                await _apiCallService.MakeApiCall<JsonPrettyRequest, JsonPrettyResponse>(
                "/api/JsonFormat",
                    requestObject);

            return JsonPretty.PrettifiedJson.ToString();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}

