using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands
{
    [Command("YamlToJson")]
    public class YamlTojsonCommand : BaseCommand
    {
        private readonly ApiCallService _apiCallService = new();
        public override async Task SendCommand(string discordToken, List<string> args, Message message)
        {
            string yaml = string.Join(" ", args).Replace("\"", "'");

            var request = new YamlToJsonRequest
            (
                yaml
            );
            
            var response =
            await _apiCallService.MakeApiCall<YamlToJsonRequest, YamlToJsonResponse>(
                "/api/YamlToJson",
                request
            );

            await SendMessageAsync($"{response.JsonResult}", message, discordToken);
        }
    }
}
