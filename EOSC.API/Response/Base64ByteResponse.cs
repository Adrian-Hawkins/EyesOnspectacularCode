using EOSC.API.SharedResponse;

namespace EOSC.API.Response;

public class Base64ByteResponse(Base64ServiceResponseCode responseCode = default)
    : BaseResponse<Base64ServiceResponseCode>(responseCode)
{
    public Base64ByteResponse(byte[] convertedData) : this() => ConvertedData = convertedData;

    public byte[] ConvertedData { get; } = null!;

    public static implicit operator Base64ByteResponse(Base64ServiceResponseCode responseCode) =>
        new(responseCode);
}