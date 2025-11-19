using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Models;

namespace HP.Clima.Domain.Mappers;

public static class ZipCodeMapper
{
    public static ZipCodeDto ToDto(this ZipCodeEntity entity)
    {
        return new ZipCodeDto
        {
            ZipCode = entity.ZipCode,
            Street = entity.Street,
            District = entity.District,
            City = entity.City,
            State = entity.State,
            Ibge = entity.Ibge,
            Location = new LocationDto
            {
                Lat = entity.Lat ?? 0,
                Lon = entity.Lon ?? 0
            },
            Provider = entity.Provider
        };
    }

    public static ZipCodeEntity ToEntity(this ZipCodeDto dto)
    {
        return new ZipCodeEntity
        {
            ZipCode = dto.ZipCode,
            Street = dto.Street,
            District = dto.District,
            City = dto.City,
            State = dto.State,
            Ibge = dto.Ibge,
            Lat = dto.Location.Lat,
            Lon = dto.Location.Lon,
            Provider = dto.Provider
        };
    }

    public static ZipCodeDto BrasilApiCepResponseToDto(this BrasilApiCepResponse response)
    {
        _ = double.TryParse(response.Location?.Coordinates?.Latitude, out var lat);
        _ = double.TryParse(response.Location?.Coordinates?.Longitude, out var lon);

        return new ZipCodeDto
        {
            ZipCode = response.Cep,
            Street = response.Street,
            District = response.District,
            City = response.City,
            State = response.State,
            Ibge = "",
            Location = new LocationDto
            {
                Lat = lat,
                Lon = lon
            },
            Provider = "brasilapi"
        };
    }

    public static ZipCodeDto ViaCepResponseToDto(this ViaCepApiResponse response)
    {
        return new ZipCodeDto
        {
            ZipCode = response.Cep.Replace("-", ""),
            Street = response.Logradouro,
            District = response.Bairro,
            City = response.Localidade,
            State = response.Uf,
            Ibge = response.Ibge,
            Location = new LocationDto
            {
                Lat = 0, 
                Lon = 0  
            },
            Provider = "viacep"
        };
    }
}