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
        var mongoSection = configuration.GetSection(MongoDbSettings.SectionName);
        
        var mongoOptions = new MongoDbSettings
        {
            UseInMemory = bool.TryParse(mongoSection[nameof(MongoDbSettings.UseInMemory)], out var useInMemory) && useInMemory,
            ConnectionString = mongoSection[nameof(MongoDbSettings.ConnectionString)] ?? "mongodb://localhost:27017",
            DatabaseName = mongoSection[nameof(MongoDbSettings.DatabaseName)] ?? "HP_Clima_DB",
            ZipCodeLookupCollectionName = mongoSection[nameof(MongoDbSettings.ZipCodeLookupCollectionName)] ?? "ZipCodeLookups"
        };
        
        if (mongoOptions.UseInMemory)
        {
            services.AddSingleton(sp =>
            {
                var mongoInMemoryService = new MongoInMemoryService();
                mongoInMemoryService.Start();
                return mongoInMemoryService;
            });
        }
        
        services.AddSingleton(sp =>
        {
            if (mongoOptions.UseInMemory)
            {
                var mongoInMemoryService = sp.GetRequiredService<MongoInMemoryService>();
                return new MongoDbSettings
                {
                    UseInMemory = true,
                    ConnectionString = mongoInMemoryService.ConnectionString,
                    DatabaseName = mongoOptions.DatabaseName,
                    ZipCodeLookupCollectionName = mongoOptions.ZipCodeLookupCollectionName
                };
            }
            
            return mongoOptions;
        });

        services.AddScoped<MongoDbContext>();
        
        return services;
    }
}