using System.Net.Http.Json;
using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Models.OpenMeteo;
using Nexus.Application.Core.Exceptions;

namespace Information.Infrastructure.Providers.OpenMeteo;

internal class OpenMeteoWeatherProvider : IWeatherProvider
{
    private const string SourceName = "OpenMeteo";
    private const string Timezone = "Europe%2FKyiv";
    private const int HourlyCount = 24;
    private const int HourlyFetchDays = 2;
    private const int ForecastDays = 5;

    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private static string FormHourlyUrl(double lat, double lon)
        => $"/v1/forecast?latitude={lat}&longitude={lon}" +
           "&hourly=temperature_2m,apparent_temperature,precipitation_probability,weathercode,windspeed_10m" +
           $"&timezone={Timezone}&forecast_days={HourlyFetchDays}";

    private static string FormDailyUrl(double lat, double lon)
        => $"/v1/forecast?latitude={lat}&longitude={lon}" +
           "&daily=temperature_2m_max,temperature_2m_min,precipitation_sum,precipitation_probability_max,weathercode,windspeed_10m_max" +
           $"&timezone={Timezone}&forecast_days={ForecastDays}";

    public async Task<IReadOnlyList<HourlyWeather>> GetHourlyForecast(WeatherCity city, CancellationToken cancellationToken = default)
    {
        try
        {
            var (lat, lon) = CityCoordinates.All[city];
            var url = FormHourlyUrl(lat, lon);

            var response = await _httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url, cancellationToken);

            if (response is null)
            {
                throw InformationExceptions.ProviderUnavailable(SourceName);
            }

            var now = DateTime.Now;
            var startIndex = response.Hourly.Time
                .Select((t, i) => (Time: DateTime.Parse(t), Index: i))
                .First(x => x.Time >= now)
                .Index;

            return Enumerable.Range(startIndex, HourlyCount)
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
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception)
        {
            throw InformationExceptions.ProviderUnavailable(SourceName);
        }
    }

    public async Task<IReadOnlyList<DailyWeather>> GetDailyForecast(WeatherCity city, CancellationToken cancellationToken = default)
    {
        try
        {
            var (lat, lon) = CityCoordinates.All[city];
            var url = FormDailyUrl(lat, lon);

            var response = await _httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url, cancellationToken);

            if (response is null)
            {
                throw InformationExceptions.ProviderUnavailable(SourceName);
            }

            return Enumerable.Range(0, response.Daily.Time.Count)
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
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception)
        {
            throw InformationExceptions.ProviderUnavailable(SourceName);
        }
    }
}
