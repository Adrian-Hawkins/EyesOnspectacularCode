using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Responses;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands;

[Command("GetHistory")]
public class GetHistoryCommand : BaseCommand
{
    private readonly ApiCallService _apiCallService = new();

    // private readonly HistoryService _historyService = new HistoryService();

    public override async Task SendCommand(string discordToken, List<string> args, Message message)
    {
        var result =
            await _apiCallService.MakeGetApiCall<HistoryResponse>(
                $"/api/history/{message.Author.GlobalName}"
            );

        var response = $@"History found for <@{message.Author.Id}>: \n\n";
        if (result?.history == null)
            response = $"No history found for <@{message.Author.Id}>";
        else
            foreach (var item in result.history)
            {
                var clean = item.Replace("\"", "");
                response += $"{clean}\\n\\n";
            }

        await SendMessageAsync($"{response}", message.ChannelId, discordToken);
    }
}