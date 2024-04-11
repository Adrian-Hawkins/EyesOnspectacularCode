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
        public IActionResult Get(string username)
        {
            List<string> history = HistoryRepo.GetHistory(username);
            if(history.Count == 0 || history == null)
            {
                return BadRequest($"Invalid username: {username}");
            }
            return Ok(new HistoryResponse(history));
        }
    }
}
