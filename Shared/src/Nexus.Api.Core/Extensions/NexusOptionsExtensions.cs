using Nexus.Api.Core.Options;

namespace Nexus.Api.Core.Extensions;

public static class NexusOptionsExtensions
{
    public static bool HasRequestTimeout(this NexusOptions options)
        => options.RequestTimeout > TimeSpan.Zero;
}
