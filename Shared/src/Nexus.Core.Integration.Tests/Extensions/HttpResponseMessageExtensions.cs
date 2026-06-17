using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.ViewModels;

namespace Nexus.Core.Integration.Tests.Extensions;

public static class HttpResponseMessageExtensions
{
    public static void ShouldBeOk(this HttpResponseMessage response) =>
        response.ShouldBeStatusCode(HttpStatusCode.OK);

    public static void ShouldBeCreated(this HttpResponseMessage response) =>
        response.ShouldBeStatusCode(HttpStatusCode.Created);

    public static void ShouldBeNoContent(this HttpResponseMessage response) =>
        response.ShouldBeStatusCode(HttpStatusCode.NoContent);

    public static void ShouldBeBadRequest(this HttpResponseMessage response) =>
        response.ShouldBeStatusCode(HttpStatusCode.BadRequest);

    public static void ShouldBeNotFound(this HttpResponseMessage response) =>
        response.ShouldBeStatusCode(HttpStatusCode.NotFound);

    public static void ShouldBeDomainError(this HttpResponseMessage response) =>
        response.ShouldBeStatusCode(StatusCodeConstants.DomainErrorStatusCode);

    public static void ShouldBeInternalServerError(this HttpResponseMessage response) =>
        response.ShouldBeStatusCode(StatusCodeConstants.InternalErrorStatusCode);

    public static void ShouldBeStatusCode(this HttpResponseMessage response, HttpStatusCode statusCode) =>
        response.StatusCode.Should().Be(statusCode);

    public static Task<T?> ReadResponse<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        response.Content.ReadFromJsonAsync<T>(cancellationToken);

    public static Task<DomainErrorResponse?> ReadDomainErrorResponse(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        response.ReadResponse<DomainErrorResponse>(cancellationToken);

    public static Task<UnexpectedErrorResponse?> ReadUnexpectedErrorResponse(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        response.ReadResponse<UnexpectedErrorResponse>(cancellationToken);
}
