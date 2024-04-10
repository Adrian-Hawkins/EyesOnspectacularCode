using System.Net.NetworkInformation;
using System.Text.Json;
using EOSC.Common.Requests;
using Microsoft.AspNetCore.Mvc;
using EOSC.Common.Responses;
using EOSC.API.Attributes;
using Microsoft.AspNetCore.Components.Forms;
using System.Xml;

namespace EOSC.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JsonFormatController : ControllerBase
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    [Tool("jsonpretty")]
    [HttpPost]
    public IActionResult Post([FromBody] JsonPrettyRequest request)
    {
        try
        {

            var jsonDocument = JsonDocument.Parse(request.MinifiedJson);
            var prettyJson = JsonSerializer.Serialize(jsonDocument.RootElement, _options);
            var response = new JsonPrettyResponse(prettyJson);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    [Tool("xmlpretty")]
    [HttpPost]
    [Route("xmlpretty")]
    public IActionResult PostXml([FromBody] XmlPrettyRequest request)
    {
        try
        {
            Console.WriteLine("Received Object : " + request);
            string prettyXml = formatXMl(request.MinifiedXml);
            var response = new XmlPrettyResponse(prettyXml);
            //logic to save to the database
            return Ok(response); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing the request. "+ex.Message);
        }
    }

    private string formatXMl(string InputText)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(InputText);

        string formattedXml;

        using (StringWriter stringWriter = new StringWriter())
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    xmlDoc.WriteTo(xmlWriter);
                }

                formattedXml = stringWriter.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        return formattedXml;
    }
}