using FluentValidation;
using HP.Clima.Domain.Configuration;
using HP.Clima.Domain.Validators;

namespace HP.Clima.API.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions(configuration);
        services.ConfigureEndpoints(configuration);
        services.ConfigureValidators();
    }

    private static void ConfigureEndpoints(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddSwaggerGen();
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HttpClientOptions>(
            configuration.GetSection(HttpClientOptions.SectionName)
        );
        
        services.Configure<ResiliencePolicyOptions>(
            configuration.GetSection(ResiliencePolicyOptions.SectionName)
        );
    }

    private static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CepRequestValidator>();
    }
}
