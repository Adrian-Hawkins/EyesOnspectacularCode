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
        public async Task<IActionResult> Get(string username)
        {
            string un;
            var claimUsername = User.Claims.ToList().Find(c => c.Type == "username");
            if(claimUsername == null)
            {
                un = username;
            }
            else
            {
                un = claimUsername.Value;
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
