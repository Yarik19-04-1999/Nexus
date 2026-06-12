using Information.Infrastructure.Enums;

namespace Information.Infrastructure.Options;

public class ExchangeRateOptions
{
    public ExchangeRateProviderType ProviderType { get; init; }
    public int CacheDurationMinutes { get; init; } = 60;
}
