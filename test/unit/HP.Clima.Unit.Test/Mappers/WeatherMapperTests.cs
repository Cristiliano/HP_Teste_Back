using Bogus;
using FluentAssertions;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Mappers;
using HP.Clima.Domain.Models;

namespace HP.Clima.Test.Unit.Mappers;

public class WeatherMapperTests
{
    private readonly Faker _faker;

    public WeatherMapperTests()
    {
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public void OpenMeteoToDto_ShouldMapCurrentAndDailyWeather()
    {
        var lat = _faker.Address.Latitude();
        var lon = _faker.Address.Longitude();
        var city = _faker.Address.City();
        var state = _faker.Address.StateAbbr();

        var zipCodeEntity = new ZipCodeEntity
        {
            Id = Guid.NewGuid(),
            ZipCode = "01311000",
            City = city,
            State = state
        };

        var forecast = new OpenMeteoForecastResponse
        {
            Current = new Current
            {
                Time = "2025-11-20T12:00",
                Temperature2m = 25.5,
                RelativeHumidity2m = 65,
                ApparentTemperature = 27.0
            },
            Daily = new Daily
            {
                Time = ["2025-11-20", "2025-11-21", "2025-11-22"],
                Temperature2mMin = [18.0, 19.0, 17.5],
                Temperature2mMax = [28.0, 29.0, 27.5]
            }
        };

        var result = forecast.OpenMeteoToDto(zipCodeEntity, lat, lon);

        result.Should().NotBeNull();
        result.Provider.Should().Be("open-meteo");
        result.Location.Lat.Should().Be(lat);
        result.Location.Lon.Should().Be(lon);
        result.Location.City.Should().Be(city);
        result.Location.State.Should().Be(state);
        result.Current.TemperatureC.Should().Be(25.5);
        result.Current.Humidity.Should().Be(0.65);
        result.Current.ApparentTemperatureC.Should().Be(27.0);
        result.Current.ObservedAt.Should().BeCloseTo(DateTime.Parse("2025-11-20T12:00"), TimeSpan.FromSeconds(1));
        result.Daily.Should().HaveCount(3);
        result.Daily[0].Date.Should().Be("2025-11-20");
        result.Daily[0].TempMinC.Should().Be(18.0);
        result.Daily[0].TempMaxC.Should().Be(28.0);
        result.Daily[1].Date.Should().Be("2025-11-21");
        result.Daily[2].Date.Should().Be("2025-11-22");
    }

    [Fact]
    public void OpenMeteoToDto_ShouldHandleMismatchedArraySizes()
    {
        var zipCodeEntity = new ZipCodeEntity
        {
            Id = Guid.NewGuid(),
            City = _faker.Address.City()
        };

        var forecast = new OpenMeteoForecastResponse
        {
            Current = new Current 
            { 
                Temperature2m = 20.0,
                Time = "2025-11-20T12:00"
            },
            Daily = new Daily
            {
                Time = ["2025-11-20", "2025-11-21"],
                Temperature2mMin = [15.0],
                Temperature2mMax = [25.0, 26.0, 27.0]
            }
        };

        var result = forecast.OpenMeteoToDto(zipCodeEntity, 0, 0);

        result.Daily.Should().HaveCount(1);
    }

    [Fact]
    public void OpenWeatherMapToDto_ShouldMapCurrentAndForecast()
    {
        var lat = _faker.Address.Latitude();
        var lon = _faker.Address.Longitude();
        var city = _faker.Address.City();
        var state = _faker.Address.StateAbbr();
        var days = 3;

        var zipCodeEntity = new ZipCodeEntity
        {
            Id = Guid.NewGuid(),
            ZipCode = "01311000",
            City = city,
            State = state
        };

        var current = new OpenWeatherMapCurrentResponse
        {
            Dt = 1700481600,
            Main = new Main
            {
                Temp = 24.5,
                Humidity = 70,
                FeelsLike = 26.0
            }
        };

        var forecast = new OpenWeatherMapForecastResponse
        {
            List =
            [
                new() { Dt = 1700481600, DtTxt = "2025-11-20 12:00:00", Main = new Main { TempMin = 18.0, TempMax = 28.0 } },
                new() { Dt = 1700568000, DtTxt = "2025-11-21 12:00:00", Main = new Main { TempMin = 19.0, TempMax = 29.0 } },
                new() { Dt = 1700654400, DtTxt = "2025-11-22 12:00:00", Main = new Main { TempMin = 17.5, TempMax = 27.5 } },
                new() { Dt = 1700740800, DtTxt = "2025-11-23 12:00:00", Main = new Main { TempMin = 16.0, TempMax = 26.0 } }
            ]
        };

        var result = current.OpenWeatherMapToDto(forecast, zipCodeEntity, lat, lon, days);

        result.Should().NotBeNull();
        result.Provider.Should().Be("openweathermap");
        result.Location.Lat.Should().Be(lat);
        result.Location.Lon.Should().Be(lon);
        result.Location.City.Should().Be(city);
        result.Location.State.Should().Be(state);
        result.Current.TemperatureC.Should().Be(24.5);
        result.Current.Humidity.Should().Be(0.7);
        result.Current.ApparentTemperatureC.Should().Be(26.0);
        result.Daily.Should().HaveCount(3);
        result.Daily[0].TempMinC.Should().Be(18.0);
        result.Daily[0].TempMaxC.Should().Be(28.0);
        result.Daily[1].TempMinC.Should().Be(19.0);
        result.Daily[1].TempMaxC.Should().Be(29.0);
        result.Daily[2].TempMinC.Should().Be(17.5);
        result.Daily[2].TempMaxC.Should().Be(27.5);
    }

    [Fact]
    public void OpenWeatherMapToDto_ShouldFilterNoonForecasts()
    {
        var zipCodeEntity = new ZipCodeEntity
        {
            Id = Guid.NewGuid(),
            City = _faker.Address.City()
        };

        var current = new OpenWeatherMapCurrentResponse
        {
            Dt = 1700481600,
            Main = new Main { Temp = 20.0, Humidity = 60, FeelsLike = 21.0 }
        };

        var forecast = new OpenWeatherMapForecastResponse
        {
            List =
            [
                new() { DtTxt = "2025-11-20 09:00:00", Main = new Main { TempMin = 18.0, TempMax = 28.0 } },
                new() { DtTxt = "2025-11-20 12:00:00", Main = new Main { TempMin = 19.0, TempMax = 29.0 } },
                new() { DtTxt = "2025-11-20 15:00:00", Main = new Main { TempMin = 17.5, TempMax = 27.5 } },
                new() { DtTxt = "2025-11-21 12:00:00", Main = new Main { TempMin = 16.0, TempMax = 26.0 } }
            ]
        };

        var result = current.OpenWeatherMapToDto(forecast, zipCodeEntity, 0, 0, 5);

        result.Daily.Should().HaveCount(2);
        result.Daily.Should().OnlyContain(d => !string.IsNullOrEmpty(d.Date));
    }
}
