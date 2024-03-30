using System.Text;
using System.Text.Json.Serialization;
using EOSC.API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace EOSC.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FormatConverterController : ControllerBase
{
    private readonly ILogger<FormatConverterController> _logger;


    public FormatConverterController(ILogger<FormatConverterController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "jsonToXml")]
    public async Task<ConvertedCurlDto?> Get()
    {
       throw new NotImplementedException();
    }
}