using HP.Clima.Domain.DTOs;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HP.Clima.API.Configuration;

/// <summary>
/// Filtro para adicionar exemplos aos schemas do Swagger
/// </summary>
public class SwaggerSchemaExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = context.Type switch
        {
            Type t when t == typeof(CepRequestDto) => new OpenApiObject
            {
                ["zipCode"] = new OpenApiString("01311-000")
            },
            
            Type t when t == typeof(ZipCodeDto) => new OpenApiObject
            {
                ["zipCode"] = new OpenApiString("01311000"),
                ["street"] = new OpenApiString("Avenida Paulista"),
                ["district"] = new OpenApiString("Bela Vista"),
                ["city"] = new OpenApiString("São Paulo"),
                ["state"] = new OpenApiString("SP"),
                ["ibge"] = new OpenApiString("3550308"),
                ["location"] = new OpenApiObject
                {
                    ["lat"] = new OpenApiDouble(-23.5613),
                    ["lon"] = new OpenApiDouble(-46.6565)
                },
                ["provider"] = new OpenApiString("brasilapi")
            },
            
            Type t when t == typeof(WeatherResponseDto) => new OpenApiObject
            {
                ["sourceZipCodeId"] = new OpenApiInteger(123456789),
                ["location"] = new OpenApiObject
                {
                    ["lat"] = new OpenApiDouble(-23.5475),
                    ["lon"] = new OpenApiDouble(-46.6361),
                    ["city"] = new OpenApiString("São Paulo"),
                    ["state"] = new OpenApiString("SP")
                },
                ["current"] = new OpenApiObject
                {
                    ["temperatureC"] = new OpenApiDouble(23.5),
                    ["humidity"] = new OpenApiDouble(0.65),
                    ["apparentTemperatureC"] = new OpenApiDouble(22.8),
                    ["observedAt"] = new OpenApiString("2025-11-20T14:00:00")
                },
                ["daily"] = new OpenApiArray
                {
                    new OpenApiObject
                    {
                        ["date"] = new OpenApiString("2025-11-20"),
                        ["tempMinC"] = new OpenApiDouble(15.2),
                        ["tempMaxC"] = new OpenApiDouble(28.7)
                    },
                    new OpenApiObject
                    {
                        ["date"] = new OpenApiString("2025-11-21"),
                        ["tempMinC"] = new OpenApiDouble(16.0),
                        ["tempMaxC"] = new OpenApiDouble(29.3)
                    },
                    new OpenApiObject
                    {
                        ["date"] = new OpenApiString("2025-11-22"),
                        ["tempMinC"] = new OpenApiDouble(17.1),
                        ["tempMaxC"] = new OpenApiDouble(30.2)
                    }
                },
                ["provider"] = new OpenApiString("open-meteo")
            },
            
            Type t when t == typeof(LocationDto) => new OpenApiObject
            {
                ["lat"] = new OpenApiDouble(-23.5475),
                ["lon"] = new OpenApiDouble(-46.6361),
                ["city"] = new OpenApiString("São Paulo"),
                ["state"] = new OpenApiString("SP")
            },
            
            Type t when t == typeof(CurrentWeatherDto) => new OpenApiObject
            {
                ["temperatureC"] = new OpenApiDouble(23.5),
                ["humidity"] = new OpenApiDouble(0.65),
                ["apparentTemperatureC"] = new OpenApiDouble(22.8),
                ["observedAt"] = new OpenApiString("2025-11-20T14:00:00")
            },
            
            Type t when t == typeof(DailyWeatherDto) => new OpenApiObject
            {
                ["date"] = new OpenApiString("2025-11-20"),
                ["tempMinC"] = new OpenApiDouble(15.2),
                ["tempMaxC"] = new OpenApiDouble(28.7)
            },
            
            _ => schema.Example
        };
    }
}
