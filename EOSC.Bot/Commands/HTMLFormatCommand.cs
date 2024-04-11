using EOSC.Common.Services;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Bot.Config;

namespace EOSC.Bot.Commands
{
	[Command("htmlformat")]
	public class HTMLFormatCommand : BaseCommand
	{
		public override async Task SendCommand(string botToken, List<string> args, Message message)
		{
			string content = $"Please format (tabs = 4 spaces) the following HTML, I am using it in an application so please don't return anything besides the formatted HTML: {message.Content}";
			if (content == null)
			{
				await SendMessageAsync("Usage: !htmlformat <html>", message, botToken);
				return;
			}

			GPTRequest request = new GPTRequest
			(
				content
			);
            _apiCallService.SetHeader(message.Author.GlobalName);
            _apiCallService.SetCustomHeader("bot", _botAuth.GetBotToken());
            var response =
			await _apiCallService.MakeApiCall<GPTRequest, GPTResponse>(
				"/api/GPT",
				request
			);

			try
			{

				await SendMessageAsync(response.responseString, message, botToken);
			}
			catch (Exception ex)
			{
				await SendMessageAsync($"ErrorHFC: {ex.Message}", message, botToken);
			}
		}
	}
}
