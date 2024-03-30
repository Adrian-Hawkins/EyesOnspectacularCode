using EOSC.Common.Constant;

namespace EOSC.API.Response;

public enum Base64ServiceResponseCode
{
    Success = GlobalResponseCode.Success,

    InvalidBase64 = GlobalResponseCode.InvalidRequest
    // Size limit? 
}