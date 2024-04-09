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
		private readonly ApiCallService _apiCallService = new();
		public override async Task SendCommand(string botToken, List<string> args, Message message)
		{
			string content = $"Please format (tabs = 4 spaces) the following HTML, I am using it in an application so please don't return anything besides the formatted HTML: {message.Content}";
			if (content == null)
			{
				await SendMessageAsync("Usage: !htmlformat <html>", message.ChannelId, botToken);
				return;
			}

			GPTRequest request = new GPTRequest
			(
				content
			);
			var response =
			await _apiCallService.MakeApiCall<GPTRequest, GPTResponse>(
				"/api/GPT",
				request
			);

			try
			{

				await SendMessageAsync(response.responseString, message.ChannelId, botToken);
			}
			catch (Exception ex)
			{
				await SendMessageAsync($"ErrorHFC: {ex.Message}", message.ChannelId, botToken);
			}
		}
	}
}
