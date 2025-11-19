using HP.Clima.Domain.Configuration;
using HP.Clima.Domain.Interfaces.Services;
using HP.Clima.Service.Handlers;
using HP.Clima.Service.Policies;
using HP.Clima.Service.Proxies.BrasilApi;
using HP.Clima.Service.Proxies.ViaCepApi;
using HP.Clima.Service.Services;
using HP.Clima.Service.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

namespace HP.Clima.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICepService, CepService>();
        services.AddScoped<IValidationService, ValidationService>();

        return services;
    }

    public static IServiceCollection AddCepHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICepApiHandler, BrasilApiCepHandler>();
        services.AddScoped<ICepApiHandler, ViaCepHandler>();

        return services;
    }

    public static IServiceCollection AddProxies(this IServiceCollection services)
    {
        services.AddBrasilApiProxy();
        services.AddViaCepProxy();
        
        return services;
    }

    private static void AddBrasilApiProxy(this IServiceCollection services)
    {
        string brasilApi = "BrasilAPI";

        services.AddRefitClient<IBrasilApiProxy>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
                client.BaseAddress = new Uri(options.BrasilApi.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<CepService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateRetryPolicy(
                    logger,
                    brasilApi,
                    resilienceOptions.RetryCount);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<CepService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateCircuitBreakerPolicy(
                    logger,
                    brasilApi,
                    resilienceOptions.CircuitBreaker);
            });
    }

    private static void AddViaCepProxy(this IServiceCollection services)
    {
        string viaCep = "ViaCEP";

        services.AddRefitClient<IViaCepApiProxy>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
                client.BaseAddress = new Uri(options.ViaCep.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<CepService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateRetryPolicy(
                    logger,
                    viaCep,
                    resilienceOptions.RetryCount);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<CepService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateCircuitBreakerPolicy(
                    logger,
                    viaCep,
                    resilienceOptions.CircuitBreaker);
            });
    }
}
