namespace EOSC.API.Auth;

public interface IApiKeyValidation
{
    bool IsValidApiKey(string userApiKey);
}