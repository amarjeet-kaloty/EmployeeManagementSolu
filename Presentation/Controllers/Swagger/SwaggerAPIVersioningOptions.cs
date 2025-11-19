using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Controllers.Swagger
{
    public class SwaggerAPIVersioningOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public SwaggerAPIVersioningOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo
                {
                    Title = $"Employee API v{description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                };

                options.SwaggerDoc(description.GroupName, info);
            }
        }
    }
}
