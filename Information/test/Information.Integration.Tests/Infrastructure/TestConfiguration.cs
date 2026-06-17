namespace Information.Integration.Tests.Infrastructure;

internal static class TestConfiguration
{
    internal static readonly Dictionary<string, string?> Values = new()
    {
        // SQL Server — fake, DB is never hit in controller tests
        ["SqlServer:ConnectionString"] = "Server=(localdb)\\mssqllocaldb;Database=InformationTest;Trusted_Connection=True;",

        // Exchange rate
        ["ExchangeRate:ProviderType"] = "Nbu",
        ["ExchangeRate:CacheExpiration"] = "01:00:00",

        // Weather
        ["Weather:ProviderType"] = "OpenMeteo",
        ["Weather:HourlyCacheExpiration"] = "00:10:00",
        ["Weather:DailyCacheExpiration"] = "01:00:00",

        // External service options
        ["Nbu:BaseUrl"] = "https://bank.gov.ua",
        ["Nbu:Timeout"] = "00:00:30",
        ["OpenMeteo:BaseUrl"] = "https://api.open-meteo.com",
        ["OpenMeteo:Timeout"] = "00:00:30",
        ["EpicGames:BaseUrl"] = "https://store-site-backend-static.ak.epicgames.com",
        ["EpicGames:Timeout"] = "00:00:30",
        ["EpicGames:CacheExpiration"] = "00:10:00",

        // Telegram bot — fake token, bot hosted service is removed in tests
        ["IceAgeBrief:Token"] = "0000000000:AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
    };
}
