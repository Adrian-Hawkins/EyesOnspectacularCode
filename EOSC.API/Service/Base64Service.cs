using System.Text;
using EOSC.API.Response;
using EOSC.API.ServiceObject;

namespace EOSC.API.Service;

public class Base64Service : IBase64Service
{
    /// <summary>
    /// Converts the given data to a Base64 encoded string.
    /// </summary>
    /// <param name="request">The request object containing the data to be converted.</param>
    /// <returns>A Base64Response object containing the converted data.</returns>
    public Base64Response ConvertToBase64(ConvertBase64Request request) =>
        new(base64String: Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Data)));

    /// <summary>
    /// Converts a base64 string to its corresponding value.
    /// </summary>
    /// <param name="request">The request object containing the base64 string to convert.</param>
    /// <returns>A Base64Response object containing the converted data.</returns>
    public Base64Response ConvertFromBase64(ConvertBase64Request request)
    {
        var b64String = request.Data;
        // Very 'cool' so we need to create a buffer that is big enough to hold the result check out https://en.wikipedia.org/wiki/Base64
        var buffer = new byte[(b64String.Length * 3 + 3) / 4 - (b64String.Length > 0 && b64String[^1] == '='
            ? b64String.Length > 1 && b64String[^2] == '=' ? 2 : 1
            : 0)];

        var tryFromBase64String = Convert.TryFromBase64String(request.Data, buffer, out _);
        if (!tryFromBase64String)
        {
            // TODO:error
            return Base64ServiceResponseCode.InvalidBase64;
        }

        return new Base64Response
        {
            Base64String = Encoding.UTF8.GetString(buffer)
        };
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

    public Base64ByteResponse ConvertImageFromBase64(ConvertBase64Request request)
    {
        var b64String = request.Data;
        // Very 'cool' so we need to create a buffer that is big enough to hold the result check out https://en.wikipedia.org/wiki/Base64
        var buffer = new byte[(b64String.Length * 3 + 3) / 4 - (b64String.Length > 0 && b64String[^1] == '='
            ? b64String.Length > 1 && b64String[^2] == '=' ? 2 : 1
            : 0)];

        var tryFromBase64String = Convert.TryFromBase64String(request.Data, buffer, out _);
        return !tryFromBase64String ? Base64ServiceResponseCode.InvalidBase64 : new Base64ByteResponse(buffer);
    }
}