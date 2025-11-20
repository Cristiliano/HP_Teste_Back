using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Models;

namespace HP.Clima.Domain.Mappers;

public static class WeatherMapper
{
    public static WeatherResponseDto OpenWeatherMapToDto(
        this OpenWeatherMapCurrentResponse current,
        OpenWeatherMapForecastResponse forecast,
        ZipCodeEntity zipCodeEntity,
        double lat,
        double lon,
        int days)
    {
        string openweathermap = "openweathermap";

        var dailyForecasts = forecast.List
            .Where(f => f.DtTxt.Contains("12:00:00"))
            .Take(days)
            .Select(item => new DailyWeatherDto
            {
                Date = DateTimeOffset.FromUnixTimeSeconds(item.Dt).ToString("yyyy-MM-dd"),
                TempMinC = item.Main.TempMin,
                TempMaxC = item.Main.TempMax
            })
            .ToList();

        return new WeatherResponseDto
        {
            SourceZipCodeId = zipCodeEntity.Id.GetHashCode(),
            Location = new LocationDto
            {
                Lat = lat,
                Lon = lon,
                City = zipCodeEntity.City,
                State = zipCodeEntity.State
            },
            Current = new CurrentWeatherDto
            {
                TemperatureC = current.Main.Temp,
                Humidity = current.Main.Humidity / 100.0,
                ApparentTemperatureC = current.Main.FeelsLike,
                ObservedAt = DateTimeOffset.FromUnixTimeSeconds(current.Dt).UtcDateTime
            },
            Daily = dailyForecasts,
            Provider = openweathermap
        };
    }

    public static WeatherResponseDto OpenMeteoToDto(
        this OpenMeteoForecastResponse forecast,
        ZipCodeEntity zipCodeEntity,
        double lat,
        double lon)
    {
        string openmeteo = "open-meteo";

        var dailyList = new List<DailyWeatherDto>();
        var timeCount = forecast.Daily.Time.Count;
        var minCount = forecast.Daily.Temperature2mMin.Count;
        var maxCount = forecast.Daily.Temperature2mMax.Count;
        
        var count = Math.Min(Math.Min(timeCount, minCount), maxCount);
        
        for (int i = 0; i < count; i++)
        {
            dailyList.Add(new DailyWeatherDto
            {
                Date = forecast.Daily.Time[i],
                TempMinC = forecast.Daily.Temperature2mMin[i],
                TempMaxC = forecast.Daily.Temperature2mMax[i]
            });
        }
        
        return new WeatherResponseDto
        {
            SourceZipCodeId = zipCodeEntity.Id.GetHashCode(),
            Location = new LocationDto
            {
                Lat = lat,
                Lon = lon,
                City = zipCodeEntity.City,
                State = zipCodeEntity.State
            },
            Current = new CurrentWeatherDto
            {
                TemperatureC = forecast.Current.Temperature2m,
                Humidity = forecast.Current.RelativeHumidity2m / 100.0,
                ApparentTemperatureC = forecast.Current.ApparentTemperature,
                ObservedAt = DateTime.Parse(forecast.Current.Time)
            },
            Daily = dailyList,
            Provider = openmeteo
        };
    }
}
