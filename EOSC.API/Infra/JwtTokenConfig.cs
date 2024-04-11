using System.Text.Json.Serialization;

namespace EOSC.API.Infra;

public class JwtTokenConfig
{
    [JsonPropertyName("secret")] public string Secret { get; set; } = string.Empty;

    [JsonPropertyName("issuer")] public string Issuer { get; set; } = "ESOC";

    [JsonPropertyName("audience")] public string Audience { get; set; } = "ESOC";

    [JsonPropertyName("accessTokenExpiration")]
    public int AccessTokenExpiration { get; set; } = 86400;
}