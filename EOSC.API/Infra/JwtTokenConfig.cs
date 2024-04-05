using System.Text.Json.Serialization;

namespace EOSC.API.Infra;

public class JwtTokenConfig
{
    [JsonPropertyName("secret")] public string Secret { get; set; } = string.Empty;

    [JsonPropertyName("issuer")] public string Issuer { get; set; } = string.Empty;

    [JsonPropertyName("audience")] public string Audience { get; set; } = string.Empty;

    [JsonPropertyName("accessTokenExpiration")]
    public int AccessTokenExpiration { get; set; }
}