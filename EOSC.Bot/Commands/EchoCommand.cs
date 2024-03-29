using Discord.Commands;

namespace EOSC.Bot.Commands
{
    public class EchoCommand : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        [Summary("Echoes back what was said")]
        public async Task ExecuteAsync([Remainder] string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                await ReplyAsync($"Usage: !echo <phrase>");
                return;
            }

            await ReplyAsync(phrase);
        }
    }
}
