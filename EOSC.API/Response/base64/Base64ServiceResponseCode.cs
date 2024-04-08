using EOSC.Common.Constant;

namespace EOSC.API.Response.base64;

public enum Base64ServiceResponseCode
{
    Success = GlobalResponseCode.Success,

    InvalidBase64 = GlobalResponseCode.InvalidRequest
    // Size limit? 
}