using System.Text.Json.Serialization;

namespace EOSC.API.Service.github_auth;

public class GithubAccessTokenRequest
{
    // C# is terrible at consistency when I add JsonPropertyName("client_id") I expect that the variable name can be anything but NO it has to be the same why do we even have things like this???? 
    [JsonPropertyName("client_id")] public required string client_id { get; init; }
    [JsonPropertyName("client_secret")] public required string client_secret { get; init; }
    public string? Code { get; set; }
    public string Accept { get; set; } = "json";
}