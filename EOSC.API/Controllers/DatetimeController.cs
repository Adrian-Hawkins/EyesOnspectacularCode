using Microsoft.AspNetCore.Mvc;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.API.Attributes;

namespace EOSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatetimeController : ControllerBase
    {
        [Tool("datetime")]
        [HttpPost]
        public IActionResult Post([FromBody] DatetimeRequest request)
        {
            if (!DateTime.TryParseExact(request.dateTimeString, request.originalFormat,
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                    out var dateTime))
            {
                return BadRequest($"Invalid date-time string: {request.dateTimeString}");
            }

            var convertedDateTime =
                dateTime.ToString(request.desiredFormat, System.Globalization.CultureInfo.InvariantCulture);
            var response = new DateTimeConversionResponse(convertedDateTime);
            return Ok(response);
        }
    }
}