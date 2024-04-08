using System.Text;
using EOSC.API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace EOSC.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CurlConverterController : ControllerBase
{
    private readonly ILogger<CurlConverterController> _logger;
    private readonly HttpClient _client;


    public CurlConverterController(ILogger<CurlConverterController> logger)
    {
        _logger = logger;
        _client = new HttpClient();
    }

    [HttpGet(Name = "toCurl")]
    public async Task<ConvertedCurlDto?> Get()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://curlconv.netlify.app/convert")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var curlDto = await response.Content.ReadFromJsonAsync<ConvertedCurlDto>();
            return curlDto;
        }
        catch (Exception ex)
        {
            //TODO: Handle exceptions here
            throw;
        }
    }
}