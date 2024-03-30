using EOSC.API.Response;
using EOSC.API.ServiceObject;

namespace EOSC.API.Service;

public interface IBase64Service
{
    Base64Response ConvertToBase64(ConvertBase64Request request);
    Base64Response ConvertFromBase64(ConvertBase64Request request);
    Base64Response ConvertImageToBase64(ConvertBase64FileRequest request);
    Base64ByteResponse ConvertImageFromBase64(ConvertBase64Request request);
}