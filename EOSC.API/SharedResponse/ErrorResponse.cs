using EOSC.Common.Constant;

namespace EOSC.API.SharedResponse;

public class ErrorResponse(GlobalResponseCode errorCode)
{
    public bool IsSuccessful => false;
    private GlobalResponseCode ErrorCode { get; } = errorCode;
    public string ErrorMessage => ErrorCode.ToString();
}