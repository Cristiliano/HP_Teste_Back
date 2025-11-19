using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Repositories;
using HP.Clima.Infra.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace HP.Clima.Test.Integration;

public class ZipCodeIntegrationTest
{
    [Fact]
    public async Task Should_Create_And_Retrieve_ZipCode()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddInfrastructure(null!); 
        
        var serviceProvider = services.BuildServiceProvider();
        var repository = serviceProvider.GetRequiredService<IZipCodeRepository>();

        var zipCode = new ZipCodeEntity
        {
            ZipCode = "01310-100",
            Street = "Avenida Paulista",
            District = "Bela Vista",
            City = "São Paulo",
            State = "SP",
            Ibge = "3550308",
            Lat = -23.561414,
            Lon = -46.655981,
            Provider = "ViaCEP"
        };

        // Act
        await repository.CreateAsync(zipCode);
        var result = await repository.GetByZipCodeAsync("01310-100");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("01310-100", result.ZipCode);
        Assert.Equal("Avenida Paulista", result.Street);
        Assert.Equal("São Paulo", result.City);
    }

    [Fact]
    public async Task Should_Return_All_ZipCodes()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddInfrastructure(null!);
        
        var serviceProvider = services.BuildServiceProvider();
        var repository = serviceProvider.GetRequiredService<IZipCodeRepository>();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ZipCodeEntity>>(result);
    }
}
