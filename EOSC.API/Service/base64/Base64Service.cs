using System.Text;
using EOSC.API.Response.base64;
using EOSC.API.ServiceObject;
using EOSC.API.SharedResponse;
using EOSC.Common.Requests;
using EOSC.Common.Responses;

namespace EOSC.API.Service.base64;

public class Base64Service : IBase64Service
{
    public Base64EncodeResponse
        ConvertToBase64(Base64EncodeRequest request) =>
        new(EncodedMessage: Convert.ToBase64String(Encoding.UTF8.GetBytes(request.EncodedMessage)));


    /// <summary>
    /// Converts a base64 string to its corresponding value.
    /// </summary>
    /// <param name="request">The request object containing the base64 string to convert.</param>
    /// <returns>A Base64Response object containing the converted data.</returns>
    public Base64DecodeResponse ConvertFromBase64(Base64DecodeRequest request)
    {
        var b64String = request.OriginalMessage;
        // Very 'cool' so we need to create a buffer that is big enough to hold the result check out https://en.wikipedia.org/wiki/Base64
        var buffer = new byte[(b64String.Length * 3 + 3) / 4 - (b64String.Length > 0 && b64String[^1] == '='
            ? b64String.Length > 1 && b64String[^2] == '=' ? 2 : 1
            : 0)];

        var tryFromBase64String = Convert.TryFromBase64String(request.OriginalMessage, buffer, out _);
        if (!tryFromBase64String)
        {
            // TODO:error
            // return Base64ServiceResponseCode.InvalidBase64;
        }

        return new Base64DecodeResponse(DecodedMessage: Encoding.UTF8.GetString(buffer));
    }

    public Base64Response ConvertImageToBase64(ConvertBase64FileRequest request)
    {
        var requestFile = request.File;

        if (requestFile.Length == 0)
        {
            //TODO: better error ;
        }

        using var memoryStream = new MemoryStream();

        // Copy the file content to memory stream
        request.File.CopyTo(memoryStream);
        var bytes = memoryStream.ToArray();
        var base64String = Convert.ToBase64String(bytes);
        var response = new Base64Response
        {
            Base64String = base64String
        };
        return response;
    }


    public Base64ByteResponse ConvertImageFromBase64(Base64EncodeRequest request)
    {
        var b64String = request.EncodedMessage;
        // Very 'cool' so we need to create a buffer that is big enough to hold the result check out https://en.wikipedia.org/wiki/Base64
        var buffer = new byte[(b64String.Length * 3 + 3) / 4 - (b64String.Length > 0 && b64String[^1] == '='
            ? b64String.Length > 1 && b64String[^2] == '=' ? 2 : 1
            : 0)];

        var tryFromBase64String = Convert.TryFromBase64String(request.EncodedMessage, buffer, out _);
        return !tryFromBase64String ? Base64ServiceResponseCode.InvalidBase64 : new Base64ByteResponse(buffer);
    }
}