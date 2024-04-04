namespace EOSC.API.SharedResponse;

public class EmptyResponse<TResponseCode>(TResponseCode responseCode) : BaseResponse<TResponseCode>(responseCode)
    where TResponseCode : Enum
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="responseCode"></param>
    /// <returns></returns>
    public static implicit operator EmptyResponse<TResponseCode>(TResponseCode responseCode) => new(responseCode);
}