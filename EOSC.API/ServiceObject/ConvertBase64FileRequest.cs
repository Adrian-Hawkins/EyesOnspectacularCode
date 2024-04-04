using System.ComponentModel.DataAnnotations;

namespace EOSC.API.ServiceObject;

public class ConvertBase64FileRequest
{
    [Required] public required IFormFile File { get; set; }
}