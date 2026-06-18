using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.ViewModels;
using Nexus.Core.Integration.Tests.Constants;
using Nexus.Core.Integration.Tests.Models;

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

    public static async Task<DomainErrorResponse?> ReadDomainErrorResponse(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        await response.ReadResponse<DomainErrorResponse>(cancellationToken);

    public static async Task<UnexpectedErrorResponse?> ReadUnexpectedErrorResponse(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        await response.ReadResponse<UnexpectedErrorResponse>(cancellationToken);

    public static async Task<T?> ReadResponse<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        await response.ReadResponse<T>(null, cancellationToken);

    public static async Task<T?> ReadResponse<T>(this HttpResponseMessage response, JsonSerializerOptions? options, CancellationToken cancellationToken = default) =>
        await response.Content.ReadFromJsonAsync<T>(options, cancellationToken);

    public static async Task<string> ReadResponse(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        await response.Content.ReadAsStringAsync(cancellationToken);

    public static async Task<HealthReportResponse?> ReadHealthCheckReport(this HttpResponseMessage response, CancellationToken cancellationToken = default) =>
        await response.ReadResponse<HealthReportResponse>(JsonSerializerConstants.HealthCheck, cancellationToken);
}
