using Nexus.Application.Core.Extensions;

namespace Nexus.Application.Core.Tests.Extensions;

public class DateOnlyExtensionsTests
{
    [Fact]
    public void Yesterday_CrossesMonthBoundaryWithLeapDay()
    {
        var date = new DateOnly(2024, 3, 1);

        var result = date.Yesterday();

        Assert.Equal(new DateOnly(2024, 2, 29), result);
    }

    [Fact]
    public void WeekAgo_CrossesYearBoundary()
    {
        var date = new DateOnly(2024, 1, 1);

        var result = date.WeekAgo();

        Assert.Equal(new DateOnly(2023, 12, 25), result);
    }

    [Fact]
    public void MonthAgo_ClampsToLastDayOfMonth()
    {
        var date = new DateOnly(2024, 3, 31);

        var result = date.MonthAgo();

        Assert.Equal(new DateOnly(2024, 2, 29), result);
    }

    [Fact]
    public void YearAgo_SubtractsOneYear()
    {
        var date = new DateOnly(2024, 1, 1);

        var result = date.YearAgo();

        Assert.Equal(new DateOnly(2023, 1, 1), result);
    }

    [Theory]
    [InlineData(2024, 6, 15)]
    [InlineData(2023, 12, 31)]
    [InlineData(2024, 3, 1)]
    [InlineData(2025, 1, 1)]
    public void Yesterday_AddDaysOneEqualsOriginal(int year, int month, int day)
    {
        var date = new DateOnly(year, month, day);

        var result = date.Yesterday().AddDays(1);

        Assert.Equal(date, result);
    }
}
