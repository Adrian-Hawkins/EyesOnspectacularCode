using EOSC.API.Auth;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IApiKeyValidation _apiKeyValidation;

    public ApiKeyAuthFilter(IApiKeyValidation apiKeyValidation)
    {
        _apiKeyValidation = apiKeyValidation;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var ApiKeyHeaderName = "key";
        string userApiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName].ToString();

        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            context.Result = new BadRequestResult();
            return;
        }

        if (!_apiKeyValidation.IsValidApiKey(userApiKey))
            context.Result = new UnauthorizedResult();
    }
}

