using Nexus.Application.Core.Interfaces;

namespace Nexus.Application.Core.Extensions;

public static class HasExpiresAtExtension
{
    public static bool IsExpired(this IHasExpiresAt entity)
        => entity.ExpiresAt.HasValue && entity.ExpiresAt.Value < DateTime.UtcNow;
}
