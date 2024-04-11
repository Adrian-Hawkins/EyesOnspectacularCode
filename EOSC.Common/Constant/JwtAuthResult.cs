using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes;

public class JwtAuthResult
{
    [JsonPropertyName("accessToken")] public string AccessToken { get; set; } = string.Empty;
}