using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EOSC.Common.Config
{
    internal class WebUrl
    {
        [JsonPropertyName("url")] 
        public string url { get; init; } = string.Empty;
    }
}
