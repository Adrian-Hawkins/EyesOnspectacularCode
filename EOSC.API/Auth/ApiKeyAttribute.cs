using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Auth;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthFilter))
    {
    }
}