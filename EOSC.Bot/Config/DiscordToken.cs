using System.Text.Json.Serialization;

namespace EOSC.Bot.Config;

public class DiscordToken
{
    [JsonPropertyName("token")] public string Token { get; init; } = string.Empty;
}