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
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace EOSC.API.Controllers;

[ApiController]
[Route("/api")]
public class Base64ConverterController(IBase64Service service)
    : ControllerBase
{
    // [ProducesResponseType(typeof(Base64Response), 200)]
    // [ProducesResponseType(typeof(ValueResponse<string, Base64ServiceResponseCode>), 200)]
    [ProducesResponseType(typeof(Base64EncodeResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [HttpPost("b64e", Name = "convertToBase64")]
    public IActionResult ConvertToBase64([FromBody] Base64EncodeRequest request) =>
        Ok(service.ConvertToBase64(request));

    [ProducesResponseType(typeof(Base64Response), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [HttpPost("b64d", Name = "convertFromBase64")]
    public IActionResult ConvertFromBase64([FromBody] Base64DecodeRequest request) =>
        Ok(service.ConvertFromBase64(request));

    /*[ProducesResponseType(typeof(Base64Response), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [Consumes("multipart/form-data")]
    [HttpPost("convertToBase64Image", Name = "convertToBase64Image")]
    public IActionResult ConvertImageToBase64([FromForm] ConvertBase64FileRequest request) =>
        this.PrepareResponse(service.ConvertImageToBase64(request));


    [ProducesResponseType(typeof(File), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [HttpPost("convertBase64ToImage", Name = "convertBase64ToImage")]
    public IActionResult ConvertBase64ToImage([FromBody] Base64EncodeRequest request)
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
    }*/
}