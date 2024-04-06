namespace EOSC.Bot.Commands
{
    public class HelloCommand : BaseCommand
    {
        public async void SendCommand(string content, string channelId, string discordToken)
        {
            await SendMessageAsync("hello @everyone", channelId, discordToken);
        }
    }
}
