namespace Nexus.Application.Core.Constants;

public static class OptionsErrorMessages
{
    public static string Required(string sectionName, string propertyName)
        => Required($"{sectionName}.{propertyName}");

    public static string Required(string optionsPath)
        => $"{optionsPath} is required.";

    public static string MustBeNotEmpty(string? optionsName, string propertyName)
        => $"[{optionsName}] {propertyName} must not be empty.";

    public static string MustBeGreaterThanZero(string? optionsName, string propertyName)
        => $"[{optionsName}] {propertyName} must be greater than zero.";

    public static string MustBeValidHttpOrHttps(string? optionsName, string propertyName)
        => $"[{optionsName}] {propertyName} must be a valid HTTP or HTTPS URL.";
}
