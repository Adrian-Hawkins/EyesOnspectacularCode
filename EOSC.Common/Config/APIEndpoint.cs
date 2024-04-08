using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EOSC.Common.Config
{
    public class APIEndpoint
    {
        [JsonPropertyName("endpoint")] public string Token { get; init; } = string.Empty;
    }
}
