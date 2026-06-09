namespace WeatherForecast.Tests;

public class WeatherForecastTests
{
    [Fact]
    public void TemperatureF_IsCalculatedCorrectly()
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Today), 0, "Freezing");
        Assert.Equal(32, forecast.TemperatureF);
    }

    [Fact]
    public void TemperatureF_ForBoilingPoint_IsCorrect()
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Today), 100, "Scorching");
        int expected = 32 + (int)(100 / 0.5556);
        Assert.Equal(expected, forecast.TemperatureF);
    }

    [Fact]
    public void Date_IsStoredCorrectly()
    {
        var date = new DateOnly(2024, 6, 15);
        var forecast = new WeatherForecast(date, 20, "Warm");
        Assert.Equal(date, forecast.Date);
    }

    [Fact]
    public void TemperatureC_IsStoredCorrectly()
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Today), -10, "Chilly");
        Assert.Equal(-10, forecast.TemperatureC);
    }

    [Fact]
    public void Summary_CanBeNull()
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Today), 15, null);
        Assert.Null(forecast.Summary);
    }
}
