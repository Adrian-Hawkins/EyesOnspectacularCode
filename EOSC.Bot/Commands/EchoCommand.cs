using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;

namespace EOSC.Bot.Commands;

[Command("echo")]
public class EchoCommand : BaseCommand
{
    public override async Task SendCommand(string botToken, List<string> args, Message message)
    {
        await SendMessageAsync($"{string.Join(" ", args)}", message.ChannelId, botToken);
    }
}