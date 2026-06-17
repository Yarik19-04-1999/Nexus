using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Options;

public sealed class NexusOptions
{
    public bool UseOpenApi { get; init; } = true;
    public bool UseResponseCompression { get; init; } = true;
    public TimeSpan RequestTimeout { get; init; } = CommonConstants.DefaultRequestTimeout;
}
