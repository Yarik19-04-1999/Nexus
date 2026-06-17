using Microsoft.Extensions.DependencyInjection;
using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Options;

public sealed class NexusOptions
{
    public bool UseOpenApi { get; init; } = true;
    public bool UseResponseCompression { get; init; } = true;
    public TimeSpan RequestTimeout { get; init; } = CommonConstants.DefaultRequestTimeout;

    public Action<IHealthChecksBuilder>? HealthCheckCustomAction { get; private set; }

    public NexusOptions WithHealthCheckCustomAction(Action<IHealthChecksBuilder> action)
    {
        if (HealthCheckCustomAction is not null)
        {
            throw new InvalidOperationException(ValidationErrorMessages.HealthCheckCustomActionAlreadySet);
        }

        HealthCheckCustomAction = action;
        return this;
    }
}
