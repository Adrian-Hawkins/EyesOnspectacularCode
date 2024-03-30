using System.Text;
using EOSC.API.Response;
using EOSC.API.ServiceObject;

namespace EOSC.API.Service;

public class Base64Service : IBase64Service
{
    public Base64Response ConvertToBase64(ConvertBase64Request request) =>
        new(convertedData: Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Data)));

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
            ConvertedData = Encoding.UTF8.GetString(buffer)
        };
    }

    public string ConvertFromBase64(string input)
    {
        throw new NotImplementedException();
    }

    public string ConvertImageToBase64(byte[] input)
    {
        throw new NotImplementedException();
    }

    public byte[] ConvertImageFromBase64(string input)
    {
        throw new NotImplementedException();
    }
}