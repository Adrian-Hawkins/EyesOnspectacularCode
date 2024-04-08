using System.Xml.Serialization;
using YamlDotNet.Serialization;

namespace EOSC.API.Service.format;

using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class FormatConvertService : IFormatConvertService
{
    public string Convert(string data, string from, string to)
    {
        var deserializer = new Deserializer();
        var yamlObject = deserializer.Deserialize(data);
        var serializer = new XmlSerializer(yamlObject!.GetType());
        serializer.Serialize(Console.Out, yamlObject);

        // var serializer = new Serializer(SerializationOptions.JsonCompatible);
        // serializer.Serialize(Console.Out, yamlObject);
        return "";
    }
}