using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands;

[Command("b64")]
public class Base64Command : BaseCommand
{
    private readonly BaseService _service = new();

    public override async Task SendCommand(string botToken, List<string> args, Message message)
    {
        if (args.Count >= 1)
        {
            // Show usage:
        }


        var type = args.ElementAtOrDefault(0);
        args.RemoveAt(0);
        if (type != null)
        {
            var messageJoined = string.Join(" ", args);
            switch (type)
            {
                case "e":
                {
                    // Should have known that args would be a pain we want to concatenate again
                    var base64DecodeResponse =
                        await _service.MakeApiCall<Base64DecodeRequest, Base64DecodeResponse>(
                            "/b64e",
                            new Base64DecodeRequest(messageJoined));
                    await SendMessageAsync(base64DecodeResponse.DecodedMessage, message.ChannelId,
                        botToken);
                    break;
                }
                case "d":
                    var base64EncodeResponse =
                        await _service.MakeApiCall<Base64EncodeRequest, Base64EncodeResponse>(
                            "/b64d",
                            new Base64EncodeRequest(messageJoined));
                    await SendMessageAsync(base64EncodeResponse.EncodedMessage, message.ChannelId,
                        botToken);
                    break;
            }
        }
    }
}