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
        if (args.Count <= 1)
        {
            await SendMessageAsync("Usage: b64 [-e|-d] <message>", message.ChannelId, botToken);
            return;
        }

        var type = args.FirstOrDefault()!;
        args.RemoveAt(0);
        var messageJoined = string.Join(" ", args);
        switch (type)
        {
            case "-e":
                var base64EncodeResponse =
                    await _apiCallService.MakeApiCall<Base64EncodeRequest, Base64EncodeResponse>(
                        "/api/b64e",
                        new Base64EncodeRequest(messageJoined));
                await SendMessageAsync(base64EncodeResponse.EncodedMessage, message.ChannelId, botToken);
                break;

            case "-d":
                var base64DecodeResponse =
                    await _apiCallService.MakeApiCall<Base64DecodeRequest, Base64DecodeResponse>(
                        "/api/b64d",
                        new Base64DecodeRequest(messageJoined));
                await SendMessageAsync(base64DecodeResponse.DecodedMessage, message.ChannelId, botToken);
                break;
        }
    }
}