using EOSC.API.Attributes;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services.ChatGPT;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EOSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonToYamlController : ControllerBase
    {
        [Tool("JsonToYaml")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] YamlToJsonRequest request)
        {
            string query = $@"
                    Please convert this json to yaml, in your response only provide the converted yaml, only say the word ""invalid"" if the json found is not valid

                    {request.YamlData}
            ";

            GPTResponse result = await GptQuery.GenerateText(query);
            return Ok(new YamlToJsonResponse(result.responseString));

        }
    }
}
