using Information.Application.Enums;

namespace Information.Application.Constants;

public static class CityCoordinates
{
    public static readonly IReadOnlyDictionary<WeatherCity, (double Lat, double Lon)> All =
        new Dictionary<WeatherCity, (double, double)>
        {
            [WeatherCity.Kyiv] = (50.4547, 30.5238),
            [WeatherCity.Kharkiv] = (49.9935, 36.2304),
            [WeatherCity.Odesa] = (46.4825, 30.7233),
            [WeatherCity.Lviv] = (49.8397, 24.0297),
        };
}
