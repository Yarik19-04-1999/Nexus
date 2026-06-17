using Information.Application.Enums;
using Information.Application.Interfaces.Services;

namespace Information.Application.Services;

public class CityCoordinatesService : ICityCoordinatesService
{
    private static readonly IReadOnlyDictionary<WeatherCity, (double Lat, double Lon)> Coordinates =
        new Dictionary<WeatherCity, (double, double)>
        {
            [WeatherCity.Kyiv] = (50.4547, 30.5238),
            [WeatherCity.Kharkiv] = (49.9935, 36.2304),
            [WeatherCity.Odesa] = (46.4825, 30.7233),
            [WeatherCity.Lviv] = (49.8397, 24.0297),
        };

    public (double Lat, double Lon) Get(WeatherCity city) => Coordinates[city];
}
