using EOSC.API.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            return Ok(history);
        }

    }
}
