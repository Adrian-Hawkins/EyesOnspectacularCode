using System.ComponentModel.DataAnnotations;

namespace EOSC.API.ServiceObject;

public class ConvertBase64Request
{
    [Required] public required string Data { get; set; }
}