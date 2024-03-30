using EOSC.API.SharedResponse;

namespace EOSC.API.Response;

public class Base64Response(Base64ServiceResponseCode responseCode = default)
    : BaseResponse<Base64ServiceResponseCode>(responseCode)
{
    public Base64Response(string convertedData) : this()
    {
        ConvertedData = convertedData;
    }

    public string ConvertedData { get; init; } = null!;

    public static implicit operator Base64Response(Base64ServiceResponseCode responseCode) =>
        new(responseCode);
}