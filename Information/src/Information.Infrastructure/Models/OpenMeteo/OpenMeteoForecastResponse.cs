using System.Text.Json.Serialization;

namespace Information.Infrastructure.Models.OpenMeteo;

internal class OpenMeteoForecastResponse
{
    [JsonPropertyName("hourly")]
    public OpenMeteoHourly? Hourly { get; init; }

    [JsonPropertyName("daily")]
    public OpenMeteoDaily? Daily { get; init; }
}

internal class OpenMeteoHourly
{
    [JsonPropertyName("time")]
    public IReadOnlyList<string> Time { get; init; } = default!;

    [JsonPropertyName("temperature_2m")]
    public IReadOnlyList<double> Temperature { get; init; } = default!;

    [JsonPropertyName("apparent_temperature")]
    public IReadOnlyList<double> ApparentTemperature { get; init; } = default!;

    [JsonPropertyName("precipitation_probability")]
    public IReadOnlyList<int> PrecipitationProbability { get; init; } = default!;

    [JsonPropertyName("weathercode")]
    public IReadOnlyList<int> WeatherCode { get; init; } = default!;

    [JsonPropertyName("windspeed_10m")]
    public IReadOnlyList<double> WindSpeed { get; init; } = default!;
}

internal class OpenMeteoDaily
{
    [JsonPropertyName("time")]
    public IReadOnlyList<string> Time { get; init; } = default!;

    [JsonPropertyName("temperature_2m_max")]
    public IReadOnlyList<double> MaxTemperature { get; init; } = default!;

    [JsonPropertyName("temperature_2m_min")]
    public IReadOnlyList<double> MinTemperature { get; init; } = default!;

    [JsonPropertyName("precipitation_sum")]
    public IReadOnlyList<double> PrecipitationSum { get; init; } = default!;

    [JsonPropertyName("precipitation_probability_max")]
    public IReadOnlyList<int> PrecipitationProbability { get; init; } = default!;

    [JsonPropertyName("weathercode")]
    public IReadOnlyList<int> WeatherCode { get; init; } = default!;

    [JsonPropertyName("windspeed_10m_max")]
    public IReadOnlyList<double> MaxWindSpeed { get; init; } = default!;
}
