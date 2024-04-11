using EOSC.Common.Requests;
using Microsoft.AspNetCore.Mvc;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services.ChatGPT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EOSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GptController : ControllerBase
    {
        // POST api/<GPTController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GPTRequest requestString)
        {
            var generatedText = await GptQuery.GenerateText(requestString.requestString);
            return Ok(generatedText);
        }
    }
}