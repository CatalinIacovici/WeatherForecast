# WeatherForecast

A minimal ASP.NET Core 7 Web API that exposes weather forecast endpoints, complete with Swagger/OpenAPI documentation and a unit-test project.

---

## Table of Contents

- [Overview](#overview)
- [Solution Structure](#solution-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Data Models](#data-models)
- [Running Tests](#running-tests)
- [Dependencies](#dependencies)

---

## Overview
##MYComment
WeatherForecast is a lightweight ASP.NET Core Minimal API that generates random 5-day weather forecasts. It demonstrates:

- Minimal API style with `app.MapGet`
- OpenAPI / Swagger integration
- Record types and computed properties
- Filtering logic (bad-weather endpoint)
- Class inheritance in C#

---

## Solution Structure

```
WeatherForecast/
├── Program.cs                        # Application entry point and all API endpoints
├── WeatherForecast.csproj            # Main project file (.NET 7, Nullable enabled)
├── appsettings.json                  # Default application configuration
├── appsettings.Development.json      # Development-specific overrides
├── Properties/
│   └── launchSettings.json           # Launch profiles (HTTP, HTTPS, IIS Express)
└── WeatherForecast.Tests/
    └── UnitTest1.cs                  # xUnit test project placeholder
```

---

## Prerequisites

| Tool | Minimum Version |
|------|----------------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 7.0 |
| Any IDE or editor (Visual Studio 2022 / VS Code / Rider) | — |

---

## Getting Started

### Clone the repository

```powershell
git clone https://github.com/CatalinIacovici/WeatherForecast.git
cd WeatherForecast
```

### Run the API

```powershell
dotnet run --project WeatherForecast.csproj
```

The application will start and open the Swagger UI automatically (in Development mode).

| Profile | URL |
|---------|-----|
| HTTP    | http://localhost:5008/swagger |
| HTTPS   | https://localhost:7219/swagger |

---

## Configuration

### `appsettings.json`

| Key | Default | Description |
|-----|---------|-------------|
| `Logging:LogLevel:Default` | `Information` | Minimum log level for all categories |
| `Logging:LogLevel:Microsoft.AspNetCore` | `Warning` | Minimum log level for ASP.NET Core internals |
| `AllowedHosts` | `*` | Hosts the application will respond to |

### `Properties/launchSettings.json`

Defines three launch profiles:

- **http** — runs on `http://localhost:5008`, opens Swagger UI on launch.
- **https** — runs on `https://localhost:7219` and `http://localhost:5008`, opens Swagger UI on launch.
- **IIS Express** — runs on `http://localhost:46218` with SSL on port `44372`.

All profiles set `ASPNETCORE_ENVIRONMENT=Development`, which enables the Swagger middleware.

---

## API Endpoints

### `GET /weatherforecast`

Returns a randomly generated 5-day weather forecast.

**Response — `200 OK`**

```json
[
  {
    "date": "2025-07-10",
    "temperatureC": 23,
    "summary": "Warm",
    "temperatureF": 73
  }
]
```

---

### `GET /badweather`

Returns only the days from the 5-day forecast that are considered "bad weather": temperature at or below 0 °C **or** a cold summary label (`Freezing`, `Bracing`, `Chilly`).

**Response — `200 OK`**

```json
[
  {
    "date": "2025-07-11",
    "temperatureC": -5,
    "summary": "Freezing",
    "temperatureF": 23
  }
]
```

Returns an empty array `[]` when no bad-weather days are present in the forecast.

---

## Data Models

### `WeatherForecast` *(record)*

| Property | Type | Description |
|----------|------|-------------|
| `Date` | `DateOnly` | Forecast date |
| `TemperatureC` | `int` | Temperature in Celsius (random, −20 to 55) |
| `Summary` | `string?` | Descriptive label (see table below) |
| `TemperatureF` | `int` *(computed)* | Temperature in Fahrenheit: `32 + (TemperatureC / 0.5556)` |

#### Summary Labels (coldest → hottest)

`Freezing` · `Bracing` · `Chilly` · `Cool` · `Mild` · `Warm` · `Balmy` · `Hot` · `Sweltering` · `Scorching`

---

### `A` and `B` *(classes)*

`Program.cs` also contains two demonstration classes that illustrate C# inheritance and safe casting:

| Class | Base | Key Member |
|-------|------|-----------|
| `A` | — | `Name` (`string`, init-only) |
| `B` | `A` | `LastName` (`string`, default `"BB"`) |

A cast of an `A` instance to `B` via `as` is performed at startup to demonstrate that the result is `null` when the object is not actually a `B`.

---

## Running Tests

The `WeatherForecast.Tests` project uses **xUnit**. Run all tests with:

```powershell
dotnet test
```

> **Note:** `UnitTest1.cs` currently contains a single empty test placeholder (`Test1`). Add your own test cases to expand coverage.

---

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.AspNetCore.OpenApi` | 7.0.12 | Generates OpenAPI metadata for Minimal API endpoints |
| `Swashbuckle.AspNetCore` | 6.5.0 | Serves the interactive Swagger UI |
