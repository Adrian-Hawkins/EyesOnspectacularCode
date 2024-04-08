using System.Text.Json.Serialization;

namespace EOSC.API.Config
{
    public class DB
    {
        [JsonPropertyName("token")] 
        public string Token { get; init; } = string.Empty;
    }
}
