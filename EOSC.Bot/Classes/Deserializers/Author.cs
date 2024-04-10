using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes.Deserializers;

public class Author
{
    [JsonPropertyName("username")] public required string Username { get; set; }
    [JsonPropertyName("public_flags")] public required int PublicFlags { get; set; }
    [JsonPropertyName("id")] public required string Id { get; set; }
    [JsonPropertyName("global_name")] public required string GlobalName { get; set; }
}