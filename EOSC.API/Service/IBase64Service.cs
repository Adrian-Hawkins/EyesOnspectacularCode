using EOSC.API.Response;
using EOSC.API.ServiceObject;

namespace EOSC.API.Service;

public interface IBase64Service
{
    Base64Response ConvertToBase64(ConvertBase64Request request);
    Base64Response ConvertFromBase64(ConvertBase64Request request);
    string ConvertImageToBase64(byte[] input);
    byte[] ConvertImageFromBase64(string input);
}