using Discord.Commands;

namespace EOSC.Bot.Commands
{
    public class EchoCommand : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
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
