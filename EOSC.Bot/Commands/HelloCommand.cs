using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
namespace EOSC.Bot.Commands
{
    [Command("hello")]
    public class HelloCommand : BaseCommand
    {
        public async override Task SendCommand(string discordToken, List<string> args, Message message)
        {
            await SendMessageAsync($"hello @{message.Author.GlobalName}", message.ChannelId, discordToken);
        }
    }
}