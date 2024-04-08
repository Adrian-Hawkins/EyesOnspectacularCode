using System.Text.Json;
using EOSC.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JsonFormatController : ControllerBase
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    [HttpPost]
    public IActionResult Post([FromBody] JsonPrettyRequest request)
    {
        var jsonDocument = JsonDocument.Parse(request.MinifiedJson);
        var prettyJson = JsonSerializer.Serialize(jsonDocument.RootElement, _options);
        return Ok(prettyJson);
    }
}