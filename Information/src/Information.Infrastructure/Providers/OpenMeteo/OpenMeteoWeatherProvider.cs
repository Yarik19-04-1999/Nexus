using System.Net.Http.Json;
using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Models.OpenMeteo;
using Nexus.Application.Core.Models;

namespace Information.Infrastructure.Providers.OpenMeteo;

internal class OpenMeteoWeatherProvider : IWeatherProvider
{
    private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";
    private const string SourceName = "OpenMeteo";
    private const int HourlyCount = 24;

    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<CityWeather>> GetWeather(WeatherCity city, CancellationToken cancellationToken = default)
    {
        try
        {
            var (lat, lon) = CityCoordinates.All[city];
            var url = $"{BaseUrl}?latitude={lat}&longitude={lon}" +
                      "&hourly=temperature_2m,apparent_temperature,precipitation_probability,weathercode,windspeed_10m" +
                      "&daily=temperature_2m_max,temperature_2m_min,precipitation_sum,precipitation_probability_max,weathercode,windspeed_10m_max" +
                      "&timezone=Europe%2FKiev&forecast_days=5";

            var response = await _httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url, cancellationToken);

            if (response is null)
            {
                return InformationResultConstants.ProviderUnavailable<CityWeather>(SourceName);
            }

            var hourly = Enumerable.Range(0, HourlyCount)
                .Select(i => new HourlyWeather
                {
                    Time = DateTime.Parse(response.Hourly.Time[i]),
                    Temperature = response.Hourly.Temperature[i],
                    ApparentTemperature = response.Hourly.ApparentTemperature[i],
                    PrecipitationProbability = response.Hourly.PrecipitationProbability[i],
                    WeatherCode = (WeatherCode)response.Hourly.WeatherCode[i],
                    WindSpeed = response.Hourly.WindSpeed[i],
                })
                .ToList();

            var daily = Enumerable.Range(0, response.Daily.Time.Count)
                .Select(i => new DailyWeather
                {
                    Date = DateOnly.Parse(response.Daily.Time[i]),
                    MaxTemperature = response.Daily.MaxTemperature[i],
                    MinTemperature = response.Daily.MinTemperature[i],
                    PrecipitationSum = response.Daily.PrecipitationSum[i],
                    PrecipitationProbability = response.Daily.PrecipitationProbability[i],
                    WeatherCode = (WeatherCode)response.Daily.WeatherCode[i],
                    MaxWindSpeed = response.Daily.MaxWindSpeed[i],
                })
                .ToList();

            return Result<CityWeather>.Success(new CityWeather
            {
                City = city,
                Hourly = hourly,
                Daily = daily,
            });
        }
        catch (Exception)
        {
            return InformationResultConstants.ProviderUnavailable<CityWeather>(SourceName, canRetry: true);
        }
    }
}
