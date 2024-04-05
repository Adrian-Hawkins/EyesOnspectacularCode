using EOSC.Common.Constant;

namespace EOSC.API.SharedResponse;

public abstract class BaseResponse<TResponseCode>(TResponseCode responseCode)
    where TResponseCode : Enum
{
    /*
     * {
     *  "success": true
     *  "value": {}
     * }
     */

    public bool IsSuccessful => GlobalResponseCode == GlobalResponseCode.Success;

    public GlobalResponseCode GlobalResponseCode => (GlobalResponseCode)(object)responseCode;
}