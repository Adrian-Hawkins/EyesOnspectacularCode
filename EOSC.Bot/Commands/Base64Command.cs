using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands;

[Command("b64")]
public class Base64Command : BaseCommand
{
    private readonly ApiCallService _apiCallService = new();

    public override async Task SendCommand(string botToken, List<string> args, Message message)
    {
        // Show usage if we get less than 2 params.
        if (args.Count >= 1) await SendMessageAsync("Usage: b64 [-e|-d] <message>", message.ChannelId, botToken);

        // Get the type and remove it from the message.
        var type = args.ElementAtOrDefault(0);
        args.RemoveAt(0);
        if (type != null)
        {
            // Should have known that args would be a pain we want to concatenate again
            var messageJoined = string.Join(" ", args);
            switch (type)
            {
                case "-e":
                {
                    var base64DecodeResponse =
                        await _apiCallService.MakeApiCall<Base64EncodeRequest, Base64EncodeResponse>(
                            "/b64e",
                            new Base64EncodeRequest(messageJoined));
                    await SendMessageAsync(base64DecodeResponse.EncodedMessage, message.ChannelId,
                        botToken);
                    break;
                }
                case "-d":
                    var base64EncodeResponse =
                        await _apiCallService.MakeApiCall<Base64DecodeRequest, Base64DecodeResponse>(
                            "/b64d",
                            new Base64DecodeRequest(messageJoined));
                    await SendMessageAsync(base64EncodeResponse.DecodedMessage, message.ChannelId,
                        botToken);
                    break;
            }
        }
    }
}