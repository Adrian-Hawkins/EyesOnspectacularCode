using System.Text.Json.Serialization;

namespace EOSC.API.Service.github_auth;

public class GithubAccessToken
{
    [JsonPropertyName("access_token")] public string? AccessToken { get; set; }
    public string? Scope { get; set; }
    [JsonPropertyName("token_type")] public string? TokenType { get; set; }

    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
    public string? ErrorUri { get; set; }
}