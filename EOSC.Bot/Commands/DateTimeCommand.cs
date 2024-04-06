using EOSC.Common.Services;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;


namespace EOSC.Bot.Commands
{
    [Command("datetime")]
    public class DateTimeCommand : BaseCommand
    {
        private readonly DateTimeService _conversionService;

        public DateTimeCommand()
        {
            _conversionService = new DateTimeService();
        }

        public override async Task SendCommand(string botToken, List<string> args, Message message)
        {


            if (args.Count() != 3)
            {
                await SendMessageAsync("Usage: !convertdatetime <(dateTimeString)> <(originalFormat)> <(desiredFormat)>", message.ChannelId, botToken);
                return;
            }

            string dateTimeString = args[0].Replace("(", "").Replace(")", "");
            string originalFormat = args[1].Replace("(", "").Replace(")", ""); ;
            string desiredFormat = args[2].Replace("(", "").Replace(")", ""); ;

            DatetimeRequest request = new DatetimeRequest
            (
                dateTimeString,
                originalFormat,
                desiredFormat
            );

            try
            {
                DateTimeConversionResponse response = await _conversionService.ConvertDateTime(request);
                await SendMessageAsync(response.ConvertedTime, message.ChannelId, botToken);
            }
            catch (Exception ex)
            {
                await SendMessageAsync($"Error: {ex.Message}", message.ChannelId, botToken);
            }
        }
    }
}