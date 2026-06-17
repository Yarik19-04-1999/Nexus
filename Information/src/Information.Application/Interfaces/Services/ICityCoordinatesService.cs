using Information.Application.Enums;

namespace Information.Application.Interfaces.Services;

public interface ICityCoordinatesService
{
    (double Lat, double Lon) Get(WeatherCity city);
}
