using EOSC.API.SharedResponse;
using Microsoft.AspNetCore.Mvc;

namespace EOSC.API.Exts;

public static class ControllerBaseExtensions
{
    public static IActionResult PrepareResponse<T>(this ControllerBase controller, BaseResponse<T> response)
        where T : Enum
    {
        return response.IsSuccessful
            ? controller.Ok(response)
            : controller.BadRequest(new ErrorResponse(response.GlobalResponseCode));
    }
}