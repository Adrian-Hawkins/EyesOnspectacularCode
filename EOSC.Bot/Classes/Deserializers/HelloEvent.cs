using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes.Deserializers;

public class HelloEvent
{
    [JsonPropertyName("heartbeat_interval")]
    public int HeartbeatInterval { get; init; }
}