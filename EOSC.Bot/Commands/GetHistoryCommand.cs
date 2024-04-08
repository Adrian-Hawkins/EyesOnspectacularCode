using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Responses;
using EOSC.Common.Services;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EOSC.Bot.Commands
{
    [Command("GetHistory")]
    public class GetHistoryCommand: BaseCommand
    {
        HistoryService historyService = new HistoryService();
        public async override Task SendCommand(string discordToken, List<string> args, Message message)
        {
            string response = $"History found for @{message.Author.Username}: \\n\\n";
            HistoryResponse result = await historyService.GetHistoryAsync("User2");
            if (result?.history == null)
            {
                response = $"No history found for <@{message.Author.Id}>";
            }
            else
            {
                foreach (var item in result.history)
                {
                    string clean = item.Replace("\"", "");
                    response += $"{clean}\\n\\n";
                }
            }
            await SendMessageAsync($"{response}", message.ChannelId, discordToken);

        }
    }
}
