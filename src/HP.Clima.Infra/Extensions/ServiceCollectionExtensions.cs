using HP.Clima.Domain.Repositories;
using HP.Clima.Infra.Configuration;
using HP.Clima.Infra.Data.Context;
using HP.Clima.Infra.Repositories;
using HP.Clima.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HP.Clima.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbSettings(configuration);
        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IZipCodeRepository, ZipCodeRepository>();
        return services;
    }

    private static IServiceCollection AddMongoDbSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoInMemoryService = new MongoInMemoryService();
        mongoInMemoryService.Start();
        services.AddSingleton(mongoInMemoryService);

        var mongoSettings = new MongoDbSettings
        {
            ConnectionString = mongoInMemoryService.ConnectionString,
            DatabaseName = "HP_Clima_DB",
            ZipCodeLookupCollectionName = "ZipCodeLookups"
        };
        services.AddSingleton(mongoSettings);

        services.AddScoped<MongoDbContext>();
        
        return services;
    }
}