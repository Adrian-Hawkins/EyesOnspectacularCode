using System.Text.Json;
using System.Text.Json.Serialization;

namespace EOSC.Bot.Classes.Deserializers;

/// <summary>
/// op integer Gateway opcode, which indicates the payload type
/// d? mixed(any JSON value) Event data
/// s? integer * Sequence number of event used for resuming sessions and heartbeating
/// t?string* Event name
/// * s and t are null when op is not 0 (Gateway Dispatch opcode).
/// </summary>
public class BaseMessage
{
    [JsonPropertyName("op")] public GatewayOpCode OpCode { get; init; }
    [JsonPropertyName("d")] public JsonElement? Data { get; init; }
    [JsonPropertyName("s")] public int? SequenceNumber { get; init; }
    [JsonPropertyName("t")] public string? EventName { get; init; }

    public override string ToString()
    {
        return $"{OpCode} {SequenceNumber} {EventName} {Data}";
    }
}