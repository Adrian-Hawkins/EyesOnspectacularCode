using System.Net.Http.Headers;
using EOSC.API.Infra;
using EOSC.API.Service.github_auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Controllers;

[ApiController]
public class AuthController(
    IGitHubAuth gitHubAuth,
    IJwtAuthManager jwtAuthManager,
    IConfiguration configuration
) : ControllerBase
{
    private readonly HttpClient _httpClient = new()
        { DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } } };

    [AllowAnonymous]
    [HttpPost("[controller]/login")]
    public ActionResult Login([FromBody] string request)
    {
        if (!gitHubAuth.IsValidUser())
        {
            return Unauthorized();
        }

        var jwtResult = jwtAuthManager.GenerateTokens(gitHubAuth.GetUserName(), DateTime.Now);

        return Ok(jwtResult);
    }

    [AllowAnonymous]
    [HttpPost("/login/oauth2/code/github")]
    public async Task<ActionResult> AuthTest([FromQuery] string code)
    {
        try
        {
            var githubAccessAuthorizeValues =
                configuration.GetSection("github:access_request").Get<GithubAccessTokenRequest>()!;
            githubAccessAuthorizeValues.Code = code;

            var response = await _httpClient.PostAsJsonAsync("https://github.com/login/oauth/access_token",
                githubAccessAuthorizeValues);
            var githubAccessToken = (await response.Content.ReadFromJsonAsync<GithubAccessToken>())!;
            if (githubAccessToken.Error != null)
            {
                return BadRequest(githubAccessToken.Error);
            }

            return Ok(githubAccessToken.AccessToken);
        }
        catch (Exception e)
        {
            // TODO: error making call
            Console.WriteLine(e);
        }

        return Ok();
    }
}