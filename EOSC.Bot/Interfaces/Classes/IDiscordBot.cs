namespace EOSC.Bot.Interfaces.Classes
{
    public interface IDiscordBot
    {
        Task ReceiveMessages(CancellationToken cancellationToken);
        Task StartAsync();
    }
}
