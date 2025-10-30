using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Filters
{
    public class SimplifyErrorResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string statusCodes = "404";

            string problemDetailsSchemaId = typeof(ProblemDetails).Name;
            if (operation.Responses.TryGetValue(statusCodes, out var response))
            {
                if (response.Content.TryGetValue("text/plain", out var mediaType))
                {
                    bool isProblemDetails = mediaType.Schema.Reference.Id == problemDetailsSchemaId;

                    if (isProblemDetails)
                    {
                        mediaType.Schema = new OpenApiSchema { Type = "string" };
                        mediaType.Encoding = null;
                    }
                }
                response.Content.Remove("application/json"); 
                response.Content.Remove("text/json"); 
            }
        }
    }
}
