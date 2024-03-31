namespace EOSC.API.SharedResponse;

public class ValueResponse<TResponseValue, TResponseCode>(TResponseCode responseCode = default)
    : BaseResponse<TResponseCode>(responseCode)
    where TResponseCode : struct, Enum
{
    public ValueResponse(TResponseValue value, TResponseCode responseCode = default)
        : this(responseCode)
    {
        Value = value;
    }

    public TResponseValue Value { get; set; }

    public static implicit operator ValueResponse<TResponseValue, TResponseCode>(TResponseCode responseCode) =>
        new(responseCode);
}