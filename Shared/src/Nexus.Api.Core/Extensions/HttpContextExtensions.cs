using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Api.Core.Extensions;

public static class HttpContextExtensions
{
    public static ICorrelationContextAccessor ResolveCorrelationContextAccessor(this HttpContext context)
        => context.ResolveService<ICorrelationContextAccessor>();

    public static T ResolveService<T>(this HttpContext context) where T : class
        => context.RequestServices.GetRequiredService<T>();

    public static string GetRequestIdentifier(this HttpContext context)
        => context.ResolveCorrelationContextAccessor()?.GetCorrelationId() ?? context.TraceIdentifier;
}
