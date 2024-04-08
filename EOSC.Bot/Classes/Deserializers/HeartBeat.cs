using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EOSC.Bot.Classes.Deserializers
{
    public class HeartBeat
    {
        [JsonPropertyName("heartbeat_interval")]
        public int HeartbeatInterval { get; init; }
    }
}
