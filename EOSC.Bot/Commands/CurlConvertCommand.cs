using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Bot.Config;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;
using System.Text;

namespace EOSC.Bot.Commands;

[Command("curlconvert")]
public class CurlConvertCommand : BaseCommand
{
    private readonly ApiCallService _apiCallService = new();

    public override async Task SendCommand(string botToken, List<string> args, Message message)
    {
        List<string> supportedLanguages = new List<string>
        {
            "Java",
            "Rust",
            "Python"
        };

        if (message.Content.Split(" ").Length <= 1)
        {
            await SendMessageAsync("Usage: !curlconvert <language> <curl command>", message, botToken);
            return;
        }
        else if (message.Content.Split(" ").Length == 2)
        {
            await SendMessageAsync("Missing argument\n\nUsage: !curlconvert <language> <curl command>", message,
                botToken);
            return;
        }

        string language = message.Content.Split(" ")[1];
        string command = "curl " + message.Content.Split("curl ")[1];
        bool isSupported = supportedLanguages.Any(l => string.Equals(l, language, StringComparison.OrdinalIgnoreCase));
        if (supportedLanguages.Contains(language))
        {
            string response = await ConvertCurl(command.Replace("\"", "\'").Replace("\\", ""), language, message);
            await SendMessageAsync($"```\n{response}\n```", message, botToken);
        }
        else
        {
            string supportedLanguagesString = string.Join(", ", supportedLanguages);
            string response = $"Language not supported!\n\nSupported languages: {supportedLanguagesString}";
            await SendMessageAsync($"```\n{response}\n```", message, botToken);
        }
    }

    private async Task<string> ConvertCurl(string command, string swapMode, Message message)
    {
        try
        {
            _apiCallService.SetHeader(message.Author.GlobalName);
            _apiCallService.SetCustomHeader("bot", _botAuth.GetBotToken());
            var requestObject = new CurlRequest(command, swapMode);
            var curlCode =
                await _apiCallService.MakeApiCall<CurlRequest, CurlResponse>(
                    "/api/CurlConverter",
                    new CurlRequest(command, swapMode));

            return curlCode.code.ToString().Replace("<br>", "\n");
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}