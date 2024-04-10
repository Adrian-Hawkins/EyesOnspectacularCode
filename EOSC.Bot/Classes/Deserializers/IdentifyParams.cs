using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes.Deserializers;

public class IdentifyParams
{
    [JsonPropertyName("token")] public required string Token { get; set; }
    [JsonPropertyName("properties")] public required IDictionary<string, string> Properties { get; set; }
    [JsonPropertyName("large_threshold")] public int LargeThreshold { get; set; }
    [JsonPropertyName("shard")] public required int[] ShardingParams { get; set; }
    [JsonPropertyName("presence")] public PresenceParams? Presence { get; set; }
    [JsonPropertyName("intents")] public int? Intents { get; set; }
}

public class PresenceParams
{
    [JsonPropertyName("activities")] public required ActivityParams[] Activities { get; set; }
    [JsonPropertyName("afk")] public bool Afk { get; set; } = false;
}

public class ActivityParams
{
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("type")] public int Type { get; set; }
}