using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using System.Text.Json;

namespace EOSC.Bot.Commands
{
	[Command("jsonformat")]
	public class JsonFormatCommand : BaseCommand
	{
		public async override Task SendCommand(string botToken, List<string> args, Message message)
		{
			string unformattedJson = message.Content;
			if (string.IsNullOrEmpty(unformattedJson))
			{
				await SendMessageAsync("Usage: !jsonformat <json>", message.ChannelId, botToken);
			}

			try
			{
				string formattedJson = FormatJson(unformattedJson);
				await SendMessageAsync($"Formatted JSON:\n```json\n{formattedJson}\n```", message.ChannelId, botToken);
			}
			catch (JsonException ex)
			{
				await SendMessageAsync($"Error formatting JSON:\n{ex.Message}", message.ChannelId, botToken);
			}
		}

		private string FormatJson(string jsonString)
		{
            dynamic jsonObject = JsonSerializer.Deserialize<object>(jsonString);
            return JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { WriteIndented = true });
        }
	}
}
