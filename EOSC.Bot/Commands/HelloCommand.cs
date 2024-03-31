using Discord.Commands;

namespace EOSC.Bot.Commands
{
    public class HelloCommand : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        public async Task ExecuteAsync()
        {
            var user = Context.User;
            await ReplyAsync($"Hello {user.Mention}!");
        }
    }
}
