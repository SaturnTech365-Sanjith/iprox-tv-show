using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Iprox.Presentation.TvShows.Minimal.Api.Configuration;

public static class SwaggerConfig
{
    public static OpenApiInfo CreateApiInfo(string version, string title, string description)
    {
        return new OpenApiInfo
        {
            Version = version,
            Title = title,
            Description = description,
            Contact = new OpenApiContact
            {
                Name = "Iprox TV Shows",
                Email = "info@iprox.nl",
                Url = new Uri("https://iprox.nl")
            }
        };
    }
}

public class HideEndpointsFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        // Hide specific endpoints
        document.Paths.Remove("/api/v1.0/shows/datasync");
    }
}