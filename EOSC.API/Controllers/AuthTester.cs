using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EOSC.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthTester(ILogger<AuthTester> logger) : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        var userName = User.Identity?.Name!;
        logger.LogInformation("User [{userName}] is viewing values.", userName);
        return new[] { "value1", "value2" };
    }
}