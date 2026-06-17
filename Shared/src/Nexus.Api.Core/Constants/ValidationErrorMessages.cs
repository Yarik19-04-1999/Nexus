namespace Nexus.Api.Core.Constants;

public static class ValidationErrorMessages
{
    public const string RequestTimeoutShouldBeGreaterThenZero = "Request timeout must be greater than zero.";
    public const string HealthCheckCustomActionAlreadySet = "WithHealthCheckCustomAction has already been set. It can only be configured once.";
}
