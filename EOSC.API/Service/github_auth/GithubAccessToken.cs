using System.Text.Json.Serialization;

namespace EOSC.API.Service.github_auth;

public class GithubAccessTokenRequest
{
    [JsonPropertyName("client_id")] public required string ClientId { get; init; }
    [JsonPropertyName("client_secret")] public required string ClientSecret { get; init; }
    public string? Code { get; set; }
    public string Accept { get; set; } = "json";
}