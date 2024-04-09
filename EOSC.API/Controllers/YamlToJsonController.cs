using EOSC.API.Attributes;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services.ChatGPT;
using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YamlToJsonController : ControllerBase
    {
        [Tool("YamlToJson")]
        [HttpPost("YamlToJson")]
        public async Task<IActionResult> Post([FromBody] YamlToJsonRequest request)
        {
            string query = $@"
                    Please convert this yaml to json, in your response only provide the converted json, only say the word ""invalid"" if the yaml found is not valid

                    {request.data}
            ";

            GPTResponse result = await GPTquery.GenerateText(query);
            return Ok(new YamlToJsonResponse(result.responseString));

        }
    }
}
