using Bogus;
using FluentAssertions;
using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Mappers;
using HP.Clima.Domain.Models;

namespace HP.Clima.Test.Unit.Mappers;

public class ZipCodeMapperTests
{
    private readonly Faker _faker;

    public ZipCodeMapperTests()
    {
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public void ToDto_ShouldMapEntityToDto()
    {
        var entity = new ZipCodeEntity
        {
            Id = Guid.NewGuid(),
            ZipCode = "01311000",
            Street = _faker.Address.StreetName(),
            District = _faker.Address.County(),
            City = _faker.Address.City(),
            State = _faker.Address.StateAbbr(),
            Ibge = _faker.Random.Number(1000000, 9999999).ToString(),
            Lat = _faker.Address.Latitude(),
            Lon = _faker.Address.Longitude(),
            Provider = "brasilapi",
            CreatedAtUtc = DateTime.UtcNow
        };

        var result = entity.ToDto();

        var expectedLocation = new LocationZipCodeDto
        {
            Lat = entity.Lat!.Value,
            Lon = entity.Lon!.Value
        };

        var expectedDto = new ZipCodeDto
        {
            ZipCode = entity.ZipCode,
            Street = entity.Street,
            District = entity.District,
            City = entity.City,
            State = entity.State,
            Ibge = entity.Ibge,
            Location = expectedLocation,
            Provider = entity.Provider
        };

        result.Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public void ToDto_ShouldHandleNullCoordinates()
    {
        var entity = new ZipCodeEntity
        {
            ZipCode = "01311000",
            Lat = null,
            Lon = null
        };

        var result = entity.ToDto();

        result.Location.Lat.Should().Be(0);
        result.Location.Lon.Should().Be(0);
    }

    [Fact]
    public void ToEntity_ShouldMapDtoToEntity()
    {
        var dto = new ZipCodeDto
        {
            ZipCode = "01311000",
            Street = _faker.Address.StreetName(),
            District = _faker.Address.County(),
            City = _faker.Address.City(),
            State = _faker.Address.StateAbbr(),
            Ibge = _faker.Random.Number(1000000, 9999999).ToString(),
            Location = new LocationZipCodeDto
            {
                Lat = _faker.Address.Latitude(),
                Lon = _faker.Address.Longitude()
            },
            Provider = "viacep"
        };

        var result = dto.ToEntity();

        var expectedEntity = new ZipCodeEntity
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

        result.Should().BeEquivalentTo(expectedEntity, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.CreatedAtUtc));
    }

    [Fact]
    public void BrasilApiCepResponseToDto_ShouldMapWithCoordinates()
    {
        var response = new BrasilApiCepResponse
        {
            Cep = "01311000",
            Street = _faker.Address.StreetName(),
            Neighborhood = _faker.Address.County(),
            City = _faker.Address.City(),
            State = _faker.Address.StateAbbr(),
            Location = new LocationData
            {
                Coordinates = new CoordinatesData
                {
                    Latitude = "-23561414",
                    Longitude = "-46656178"
                }
            }
        };

        var result = response.BrasilApiCepResponseToDto();

        var expectedLocation = new LocationZipCodeDto
        {
            Lat = -23561414,
            Lon = -46656178
        };

        var expectedDto = new ZipCodeDto
        {
            ZipCode = "01311000",
            Street = response.Street,
            District = response.Neighborhood,
            City = response.City,
            State = response.State,
            Location = expectedLocation,
            Provider = "brasilapi"
        };

        result.Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public void BrasilApiCepResponseToDto_ShouldHandleInvalidCoordinates()
    {
        var response = new BrasilApiCepResponse
        {
            Cep = "01311000",
            Location = new LocationData
            {
                Coordinates = new CoordinatesData
                {
                    Latitude = "invalid",
                    Longitude = "invalid"
                }
            }
        };

        var result = response.BrasilApiCepResponseToDto();

        result.Location.Lat.Should().Be(0);
        result.Location.Lon.Should().Be(0);
    }

    [Fact]
    public void ViaCepResponseToDto_ShouldMapAndNormalizeZipCode()
    {
        var response = new ViaCepApiResponse
        {
            Cep = "01311-000",
            Logradouro = _faker.Address.StreetName(),
            Bairro = _faker.Address.County(),
            Localidade = _faker.Address.City(),
            Uf = _faker.Address.StateAbbr(),
            Ibge = _faker.Random.Number(1000000, 9999999).ToString()
        };

        var result = response.ViaCepResponseToDto();

        var expectedLocation = new LocationZipCodeDto
        {
            Lat = 0,
            Lon = 0
        };

        var expectedDto = new ZipCodeDto
        {
            ZipCode = "01311000",
            Street = response.Logradouro,
            District = response.Bairro,
            City = response.Localidade,
            State = response.Uf,
            Ibge = response.Ibge,
            Location = expectedLocation,
            Provider = "viacep"
        };

        result.Should().BeEquivalentTo(expectedDto);
    }
}
