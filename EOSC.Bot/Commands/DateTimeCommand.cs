using Discord.Commands;
using EOSC.Common.Services;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSC.Bot.Commands
{
    public class DateTimeCommand : ModuleBase<SocketCommandContext>
    {
        private readonly DateTimeService _conversionService;

        public DateTimeCommand()
        {
            _conversionService = new DateTimeService();
        }

        [Command("convertdatetime")]
        public async Task ConvertDateTimeAsync([Remainder] string messageContent)
        {
            string[] fields = messageContent.Split('(', ')', StringSplitOptions.RemoveEmptyEntries);

            if (fields.Length != 3)
            {
                await ReplyAsync("Usage: !convertdatetime <(dateTimeString)> <(originalFormat)> <(desiredFormat)>");
                return;
            }

            string dateTimeString = fields[0].Replace("(", "").Replace(")", "");
            string originalFormat = fields[1].Replace("(", "").Replace(")", ""); ;
            string desiredFormat = fields[2].Replace("(", "").Replace(")", ""); ;

            DatetimeRequest request = new DatetimeRequest
            (
                dateTimeString,
                originalFormat,
                desiredFormat
            );

            try
            {
                DateTimeConversionResponse response = await _conversionService.ConvertDateTime(request);
                await ReplyAsync($"Converted datetime: {response.ConvertedTime}");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"Error: {ex.Message}");
            }
        }
    }
}