using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EOSC.API.Dto;
using EOSC.API.Exts;
using EOSC.API.Response;
using EOSC.API.Response.base64;
using EOSC.API.Service;
using EOSC.API.Service.base64;
using EOSC.API.ServiceObject;
using EOSC.API.SharedResponse;
using EOSC.Common.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace EOSC.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
//[Authorize]

public class Base64ConverterController(ILogger<Base64ConverterController> logger, IBase64Service service)
    : ControllerBase
{
    // [ProducesResponseType(typeof(Base64Response), 200)]
    [ProducesResponseType(typeof(ValueResponse<string, Base64ServiceResponseCode>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [HttpPost("convertToBase64", Name = "convertToBase64")]
    public IActionResult ConvertToBase64([FromBody] ConvertBase64Request request) =>
        this.PrepareResponse(service.ConvertToBase64(request));

    [ProducesResponseType(typeof(Base64Response), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [HttpPost("convertFromBase64", Name = "convertFromBase64")]
    public IActionResult ConvertFromBase64([FromBody] ConvertBase64Request request)
    {
        return this.PrepareResponse(service.ConvertFromBase64(request));
    }


    [ProducesResponseType(typeof(Base64Response), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [Consumes("multipart/form-data")]
    [HttpPost("convertToBase64Image", Name = "convertToBase64Image")]
    public IActionResult ConvertImageToBase64([FromForm] ConvertBase64FileRequest request) =>
        this.PrepareResponse(service.ConvertImageToBase64(request));


    [ProducesResponseType(typeof(File), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [HttpPost("convertBase64ToImage", Name = "convertBase64ToImage")]
    public IActionResult ConvertBase64ToImage([FromBody] ConvertBase64Request request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new ErrorResponse(GlobalResponseCode.InvalidRequest));
        }

        var response = service.ConvertImageFromBase64(request);
        if (response.IsSuccessful) return File(response.ConvertedData, "image/jpeg");
        return BadRequest(new ErrorResponse(response.GlobalResponseCode));
    }

    /*[HttpPost("convertBase64ToImage", Name = "convertBase64ToImage")]
    public IActionResult GetImageFromBase64([FromBody] JsonElement jsonBody)
    {
        if (jsonBody.ValueKind != JsonValueKind.Object)
        {
            return BadRequest("JSON body must be an object.");
        }

        if (!jsonBody.TryGetProperty("data", out JsonElement base64ImageElement))
        {
            return BadRequest("JSON object must contain 'base64ImageString' property.");
        }

        string? base64ImageString = base64ImageElement.GetString();

        if (string.IsNullOrEmpty(base64ImageString))
        {
            return BadRequest("Input string cannot be null or empty.");
        }

        try
        {
            // Convert the Base64 image string to bytes
            // byte[] imageBytes = Convert.FromBase64String(base64ImageString);
            var convertedData = service.ConvertImageFromBase64(new ConvertBase64Request { Data = base64ImageString })
                .ConvertedData;


            // Return the image bytes
            return File(convertedData, "image/jpeg");
        }
        catch (FormatException)
        {
            return BadRequest("Input string is not in valid Base64 format.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while decoding image from Base64.");
            return StatusCode(500, "An error occurred while decoding image from Base64.");
        }
    }*/

    /*[HttpGet("toBase64", Name = "toBase64")]
    public IActionResult Get(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return BadRequest("Input string cannot be null or empty.");
        }

        try
        {
            // Convert the input string to Base64
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string base64String = Convert.ToBase64String(bytes);

            return Ok(base64String);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while converting to Base64.");
            return StatusCode(500, "An error occurred while converting to Base64.");
        }
    }

    [HttpGet("fromBase64", Name = "fromBase64")]
    public IActionResult GetFromBase64(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return BadRequest("Input string cannot be null or empty.");
        }

        try
        {
            // Convert the Base64 string to original form
            byte[] bytes = Convert.FromBase64String(input);
            string originalString = Encoding.UTF8.GetString(bytes);

            return Ok(originalString);
        }
        catch (FormatException)
        {
            return BadRequest("Input string is not in valid Base64 format.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while decoding from Base64.");
            return StatusCode(500, "An error occurred while decoding from Base64.");
        }
    }

    [HttpPost("imageFromBase64", Name = "imageFromBase64")]
    public IActionResult GetImageFromBase64([FromBody] JsonElement jsonBody)
    {
        if (jsonBody.ValueKind != JsonValueKind.Object)
        {
            return BadRequest("JSON body must be an object.");
        }

        if (!jsonBody.TryGetProperty("base64ImageString", out JsonElement base64ImageElement))
        {
            return BadRequest("JSON object must contain 'base64ImageString' property.");
        }

        string? base64ImageString = base64ImageElement.GetString();

        if (string.IsNullOrEmpty(base64ImageString))
        {
            return BadRequest("Input string cannot be null or empty.");
        }

        try
        {
            // Convert the Base64 image string to bytes
            byte[] imageBytes = Convert.FromBase64String(base64ImageString);

            // Return the image bytes
            return File(imageBytes, "image/jpeg");
        }
        catch (FormatException)
        {
            return BadRequest("Input string is not in valid Base64 format.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while decoding image from Base64.");
            return StatusCode(500, "An error occurred while decoding image from Base64.");
        }
    }*/
}