var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Array of descriptive weather summary labels used to categorize temperature ranges,
// from the coldest ("Freezing") to the hottest ("Scorching").
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Returns a 5-day weather forecast with random temperatures and summary labels.
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

var badWeatherSummaries = new HashSet<string> { "Freezing", "Bracing", "Chilly" };

var sunnyWeatherSummaries = new HashSet<string> { "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

// Returns bad weather days from a 5-day forecast (temperature below 0°C or a cold summary).
app.MapGet("/badweather", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ));

    var badWeather = forecast
        .Where(f => f.TemperatureC <= 0 || badWeatherSummaries.Contains(f.Summary ?? string.Empty))
        .ToArray();

    return badWeather;
})
.WithName("GetBadWeather")
.WithOpenApi();

//My comment
// Returns sunny weather days from a 5-day forecast (temperature above 20°C or a warm/hot summary).
app.MapGet("/sunnyweather", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ));

    var sunnyWeather = forecast
        .Where(f => f.TemperatureC > 20 || sunnyWeatherSummaries.Contains(f.Summary ?? string.Empty))
        .ToArray();

    return sunnyWeather;
})
.WithName("GetSunnyWeather")
.WithOpenApi();

A a = new A { Name = "AA" };

B? b = a as B;

Console.WriteLine(b as A);

// Returns a 5-day weather forecast for Timisoara with city-specific temperature ranges (-5°C to 38°C).
app.MapGet("/weatherforecast/timisoara", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new TimisoaraWeatherForecast
        (
            City: "Timisoara",
            Date: DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC: Random.Shared.Next(-5, 38),
            Summary: summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetTimisoaraWeatherForecast")
.WithOpenApi();

app.Run();

internal record TimisoaraWeatherForecast(string City, DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

class A
{
    public string Name { get; init; } = default!;

}

class B : A
{
    public string LastName { get; set; }

    public B()
    {
        LastName = "BB";

    }
}