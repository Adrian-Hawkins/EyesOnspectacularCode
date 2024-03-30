namespace EOSC.API.SharedResponse;

public class ValueResponse<TResponseValue, TResponseCode>(TResponseCode responseCode = default)
    : BaseResponse<TResponseCode>(responseCode)
    where TResponseCode : struct, Enum
    where TResponseValue : struct

{
    public TResponseValue Value { get; set; } = default;

    public static implicit operator ValueResponse<TResponseValue, TResponseCode>(TResponseCode responseCode) =>
        new(responseCode);
}