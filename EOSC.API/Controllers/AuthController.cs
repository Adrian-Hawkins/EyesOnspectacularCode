using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    [HttpGet]
    public IActionResult GetToken()
    {
        
        return Ok();
    }
}