using System.Text;
using EOSC.API.Attributes;
using EOSC.API.Dto;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;


namespace EOSC.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurlConverterController : ControllerBase
{
    private readonly HttpClient _client = new();

    public CurlConverterController()
    {
        _client = new HttpClient();
    }
    [Tool("curlconvert")]
    [HttpPost]
    public async Task<IActionResult> ConvertCurl([FromBody] CurlRequest request)
    {
        try
        {
            Console.WriteLine("Received Object : " + request);
            string prettyXml = await MakeHttpRequest(request.command,request.language);
            var response = new CurlResponse(prettyXml);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing the request. " + ex.Message);
        }
    }


    private async Task<string> MakeHttpRequest(string Command,string SwapMode)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://curlconv.netlify.app/convert");
            string requestBody = $@"{{
                                        ""language"": ""{SwapMode}"",
                                        ""command"": ""{Command}""
                                    }}";

            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                string htmlFormatted = data
                                        .Split('[')
                                        .Skip(1)
                                        .First()
                                        .Replace("\",", "")
                                        .Trim('[', ']', '"')
                                        .Trim()
                                        .Replace("\\n", "<br>");
                return htmlFormatted;

            }
            else
            {
                return "Error: " + response.ReasonPhrase;
            }
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }
}