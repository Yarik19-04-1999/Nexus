using Microsoft.OpenApi;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.Utils;
using Nexus.Api.Core.ViewModels;

namespace Nexus.Api.Core.Extensions;

public static class OpenApiResponsesExtensions
{
    public static void TryAddDefaultResponses(this OpenApiResponses responses, OpenApiDocument? document)
    {
        TryAddDomainErrorResponse(responses, document);
        TryAddUnexpectedErrorResponse(responses, document);
    }

    public static void TryAddDomainErrorResponse(this OpenApiResponses responses, OpenApiDocument? document)
        => TryAddResponse<DomainErrorResponse>(responses, StatusCodeConstants.DomainError, "Domain error", document);

    public static void TryAddUnexpectedErrorResponse(this OpenApiResponses responses, OpenApiDocument? document)
        => TryAddResponse<UnexpectedErrorResponse>(responses, StatusCodeConstants.InternalError, "Unexpected server error", document);

    public static void TryAddResponse<T>(this OpenApiResponses responses, int statusCodes, string description, OpenApiDocument? document)
    {
        responses.TryAdd(
            statusCodes.ToString(), 
            OpenApiResponseUtils.CreateJson<T>(description, document));
    }
}
