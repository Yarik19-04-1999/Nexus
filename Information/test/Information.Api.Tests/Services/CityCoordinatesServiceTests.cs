using Information.Application.Enums;
using Information.Application.Services;

namespace Information.Api.Tests.Services;

public class CityCoordinatesServiceTests
{
    private readonly CityCoordinatesService _service = new();

    [Theory]
    [MemberData(nameof(AllCities))]
    public void Get_AllWeatherCities_HaveCoordinates(WeatherCity city)
    {
        var (lat, lon) = _service.Get(city);

        Assert.NotEqual(0, lat);
        Assert.NotEqual(0, lon);
    }

    public static IEnumerable<object[]> AllCities =>
        Enum.GetValues<WeatherCity>().Select(c => new object[] { c });
}
