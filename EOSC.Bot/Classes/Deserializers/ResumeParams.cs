using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes.Deserializers;

public class ResumeParams
{
    [JsonPropertyName("server_id")] public ulong ServerId { get; set; }
    [JsonPropertyName("session_id")] public required string SessionId { get; set; }
    [JsonPropertyName("token")] public required string Token { get; set; }
}