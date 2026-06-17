using Microsoft.OpenApi;
using System.Net.Mime;

namespace Nexus.Api.Core.Utils;

public static class OpenApiResponseUtils
{
    public static OpenApiResponse CreateJson<T>(string description, OpenApiDocument? document)
    {
        return new OpenApiResponse
        {
            Description = description,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.Json] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchemaReference(typeof(T).Name, document)
                }
            }
        };
    }
}
