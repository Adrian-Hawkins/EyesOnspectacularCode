using EOSC.Common.Constant;

namespace EOSC.API.SharedResponse;

public class ErrorResponse(GlobalResponseCode errorCode)
{
    private GlobalResponseCode ErrorCode { get; } = errorCode;
    public string ErrorMessage => ErrorCode.ToString();
}