using System.Text.Json;
using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes.Deserializers;

public class BaseMessageTyped<T>
{
    [JsonPropertyName("op")] public GatewayOpCode OpCode { get; init; }
    [JsonPropertyName("d")] public T? Data { get; init; }
    [JsonPropertyName("s")] public int? SequenceNumber { get; init; }
    [JsonPropertyName("t")] public string? EventName { get; init; }

    public override string ToString()
    {
        return $"{OpCode} {SequenceNumber} {EventName} {Data}";
    }
}