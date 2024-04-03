namespace EOSC.API.Auth;

public class ApiKeyValidation : IApiKeyValidation
{
    public bool IsValidApiKey(string userApiKey)
    {
        if (string.IsNullOrWhiteSpace(userApiKey))
            return false;
        // TODO: make call to github
        string apiKey = "gho_102";
        return apiKey == userApiKey;
    }
}