using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes.Deserializers;


public class Message
{
    [JsonPropertyName("channel_id")] public required string ChannelId { get; init; }
    [JsonPropertyName("author")] public required Author Author { get; init; }
    [JsonPropertyName("content")] public required string Content { get; init; }
}