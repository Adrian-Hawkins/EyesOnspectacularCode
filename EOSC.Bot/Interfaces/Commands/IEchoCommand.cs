using Discord.WebSocket;

namespace EOSC.Bot.Interfaces.Commands
{
    internal interface IEchoCommand
    {
        Task MessageHandler(SocketMessage message);
    }
}