using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Options;

public sealed class NexusOptions
{
    public static readonly NexusOptions Default = new();

    public bool UseCorrelationId { get; init; } = true;
    public bool UseOpenApi { get; init; } = true;
    public bool UseResponseCompression { get; init; } = true;
    public bool UseRequestTimeouts { get; init; } = true;
    public bool UseHttpsRedirection { get; init; } = true;
    public bool UseSecurityHeaders { get; init; } = true;
    public bool UseExceptionHandling { get; init; } = true;
    public bool UseHealthChecks { get; init; } = true;
    public bool UseScalarUi { get; init; } = true;
    public TimeSpan RequestTimeout { get; init; } = RequestTimeoutConstants.Default;
}
