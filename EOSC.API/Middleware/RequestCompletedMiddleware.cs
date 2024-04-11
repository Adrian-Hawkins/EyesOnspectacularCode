using EOSC.API.Attributes;
using EOSC.API.Repo;
using System.Text;

namespace EOSC.API.Middleware
{
    public class RequestCompletedMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCompletedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// This middleware function will be called after a request has been made and will
        /// update our history without us having to add code to controllers, the only required code
        /// is adding the "tool" attribute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using (var requestBodyStream = new MemoryStream())
            using (var responseBodyStream = new MemoryStream())
            {
                await context.Request.Body.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;

                context.Response.Body = responseBodyStream;
                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                var endpoint = context.GetEndpoint();
                if (endpoint != null)
                {
                    var toolAttribute = endpoint.Metadata.GetMetadata<ToolAttribute>();
                    if (toolAttribute != null)
                    {
                        var toolName = toolAttribute.ToolName;
                        var claimUsername = context.User.Claims.ToList().Find(c => c.Type == "username");
                        if (claimUsername != null)
                        {
                            await HistoryRepo.UpdateHistory(claimUsername.Value, toolName, requestBodyText, responseBodyText);
                        }
                        else if (context.Request.Headers.TryGetValue("username", out var username))
                        {
                            await HistoryRepo.UpdateHistory(username!, toolName, responseBodyText, responseBodyText);
                        }
                    }
                }
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}