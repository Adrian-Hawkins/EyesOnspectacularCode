using Discord.WebSocket;

namespace EOSC.Bot.Interfaces.Commands
{
    internal interface IBotCommand
    {
        Task MessageHandler(SocketMessage message);
    }
}