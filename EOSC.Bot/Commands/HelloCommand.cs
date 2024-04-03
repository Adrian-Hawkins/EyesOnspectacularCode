using Discord.Commands;
using Discord.WebSocket;

namespace EOSC.Bot.Commands
{
    public class HelloCommand : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        public async Task ExecuteAsync()
        {
            SocketUser user = Context.User;
            await ReplyAsync($"Hello there {user.Mention}!");
        }
    }
}
