using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands;

[Command("xmlpretty")]

public class XmlPrettyCommand : BaseCommand
{
    private readonly ApiCallService _apiCallService = new();

    public override async Task SendCommand(string discordToken, List<string> args, Message message)
    {

        if (message.Content.Split(" ").Length <= 1)
        {
            await SendMessageAsync("Usage: !XmlPretty <xml>", message, discordToken);
            return;
        }

        string xmlString = message.Content.Substring("!xmlpretty ".Length);
        var response = await FormatXMl(xmlString);

        await SendMessageAsync($"```{response}```", message, discordToken);
    }

    private async Task<string> FormatXMl(string InputText)
    {

        string formattedXml;

        {
            try
            {
                var requestObject = new XmlPrettyRequest(InputText);
                var jsonPretty =
                    await _apiCallService.MakeApiCall<XmlPrettyRequest, XmlPrettyResponse>(
                    "/api/JsonFormat/xmlpretty",
                        requestObject);

                formattedXml = jsonPretty.PrettyXml.ToString();
            }
            catch (Exception ex)
            {
                formattedXml = "Error: " + ex.Message;
            }
        }
        return formattedXml;
    }
}
