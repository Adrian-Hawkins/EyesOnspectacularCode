using EOSC.Common.Services;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using System.Text.RegularExpressions;
using EOSC.Bot.Config;

namespace EOSC.Bot.Commands
{
    [Command("datetime")]
    public class DateTimeCommand : BaseCommand
    {
        private readonly ApiCallService _apiCallService = new();

        public override async Task SendCommand(string botToken, List<string> args, Message message)
        {
            string content = message.Content;
            string pattern = @"\((.*?)\)";
            MatchCollection matches = Regex.Matches(content, pattern);
            List<string> parsedList = matches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
            if (parsedList.Count() != 3)
            {
                await SendMessageAsync("Usage: !datetime <(dateTimeString)> <(originalFormat)> <(desiredFormat)>", message.ChannelId, botToken);
                return;
            }
            string dateTimeString = parsedList[0];
            string originalFormat = parsedList[1];
            string desiredFormat = parsedList[2];
            DatetimeRequest request = new DatetimeRequest
            (
                dateTimeString,
                originalFormat,
                desiredFormat
            );
            var response =
            await _apiCallService.MakeApiCall<DatetimeRequest, DateTimeConversionResponse>(
                "/api/Datetime",
                request
            );

            try
            {
                
                await SendMessageAsync(response.ConvertedTime, message.ChannelId, botToken);
            }
            catch (Exception ex)
            {
                await SendMessageAsync($"Error: {ex.Message}", message.ChannelId, botToken);
            }
        }
    }
}