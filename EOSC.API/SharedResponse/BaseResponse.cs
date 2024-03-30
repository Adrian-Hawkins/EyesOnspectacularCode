using EOSC.Common.Constant;

namespace EOSC.API.SharedResponse;

public abstract class BaseResponse<TResponseCode> where TResponseCode : Enum
{
    public bool IsSuccessful => GlobalResponseCode == GlobalResponseCode.Success;

    public GlobalResponseCode GlobalResponseCode => (GlobalResponseCode)(object)_responseCode;
    private readonly TResponseCode _responseCode;

    public BaseResponse(TResponseCode responseCode) => _responseCode = responseCode;
}