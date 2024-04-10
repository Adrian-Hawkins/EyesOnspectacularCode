using EOSC.API.Response.base64;
using EOSC.API.ServiceObject;
using EOSC.API.SharedResponse;
using EOSC.Common.Requests;
using EOSC.Common.Responses;

namespace EOSC.API.Service.base64;

public interface IBase64Service
{
    /*ValueResponse<string, Base64ServiceResponseCode>*/
    Base64EncodeResponse ConvertToBase64(Base64EncodeRequest request);
    Base64DecodeResponse ConvertFromBase64(Base64DecodeRequest request);
    Base64Response ConvertImageToBase64(ConvertBase64FileRequest request);
    Base64ByteResponse ConvertImageFromBase64(Base64EncodeRequest request);
}