namespace EOSC.API.Service.format;

public interface IFormatConvertService
{
    string Convert(string data, string from, string to);
}