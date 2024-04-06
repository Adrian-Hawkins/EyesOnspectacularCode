namespace EOSC.Bot.Commands
{
    public class HelloCommand : BaseCommand
    {
        public async override Task SendCommand(string channelId, string discordToken)
        {
            await SendMessageAsync("hello @everyone", channelId, discordToken);
        }

        public override string GetCommandName()
        {
            return "hello";
        }
    }
}