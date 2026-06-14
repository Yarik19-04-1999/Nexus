namespace Nexus.Infrastructure.Core.Options;

public class ExternalServiceOptions
{
    public string BaseUrl { get; set; } = default!;
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMinutes(1);
}
