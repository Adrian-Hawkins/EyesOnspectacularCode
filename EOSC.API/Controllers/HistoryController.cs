using EOSC.API.Attributes;
using EOSC.API.Repo;
using EOSC.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username, HttpContext context)
        {
            string un;
            var claimUsername = context.User.Claims.ToList().Find(c => c.Type == "username");
            if (claimUsername != null)
            {
                un = claimUsername.Value;
            }
            else if (context.Request.Headers.TryGetValue("username", out var uname))
            {
                un = uname;
            }
            else
            {
                return BadRequest("No username found");
            }

            List<string> history = HistoryRepo.GetHistory(un);
            if(history.Count == 0 || history == null)
            {
                return BadRequest($"Invalid username: {un}");
            }
            return Ok(new HistoryResponse(history));
        }
    }
}
