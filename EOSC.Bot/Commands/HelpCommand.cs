using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;

namespace EOSC.Bot.Commands;
[Command("help")]
public class HelpCommand : BaseCommand
{
	public override async Task SendCommand(string discordToken, List<string> args, Message message)
	{
		await SendMessageAsync(
  @"**Commands List:**

    1. `!b64`: Encodes into Base64 or decodes Base64-encoded data back to original form.
    2. `!datetime`: Converts date/times from one format to another.
    3. `!GetHistory`: Returns the history of queries and responses for the current user.
    4. `!curlconvert`: Converts curl commands to a language of your choice.
    5. `!htmlformat`: Formats unformatted HTML.
    6. `!jsonpretty`: Formates unformatted JSON.

    **Usage:**

    To use a command, type `!command_name` followed by any required flags or parameters.

    For example:
    - `!b64 [-e|-d] <data>`: use the `-e` or `-d` flags to encode or decode data respectively.
    - `!datetime <dateTimeString> <originalFormat> <desiredFormat>`: For example: 
       `(2022-04-01 12:34:56) (yyyy-MM-dd HH:mm:ss) (MMMM dd, yyyy)`.
    - `!GetHistory`
    - `!curlconvert <language> <curlCommand>`: For example: 
      `!curlconvert java curl -X POST http://example.com/api/endpoint -H ""Content-Type: application/json"" -d ""{key1:value1, key2:value2}""`
    - `!htmlformat <html>`
    - `!jsonpretty <json>`
    "
		, message, discordToken);
	}
}
