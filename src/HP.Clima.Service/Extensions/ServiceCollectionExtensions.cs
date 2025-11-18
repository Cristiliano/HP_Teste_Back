using HP.Clima.Domain.Interfaces.Services;
using HP.Clima.Service.Proxies.BrasilApi;
using HP.Clima.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Refit;

namespace HP.Clima.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICepService, CepService>();

        return services;
    }

    public static IServiceCollection AddProxies(this IServiceCollection services, IConfiguration config)
    {
        services.AddRefitClient<IBrasilApiProxy>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(config.GetSection("BrasilApi:BaseUrl").Value!);
                c.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(config.GetSection("Timeout").Value!));
            });
        
        return services;
    }
}
