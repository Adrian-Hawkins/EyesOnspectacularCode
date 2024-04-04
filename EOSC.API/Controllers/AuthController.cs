using EOSC.API.Infra;
using EOSC.API.Service.github_auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    IGitHubAuth gitHubAuth,
    IJwtAuthManager jwtAuthManager) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult Login([FromBody] string request)
    {
        if (!gitHubAuth.IsValidUser())
        {
            return Unauthorized();
        }
        
        var jwtResult = jwtAuthManager.GenerateTokens(gitHubAuth.GetUserName(), DateTime.Now);
        
        return Ok(jwtResult);
    }
}