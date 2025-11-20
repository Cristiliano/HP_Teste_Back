using HP.Clima.Domain.Configuration;
using HP.Clima.Domain.Interfaces.Services;
using HP.Clima.Service.Handlers;
using HP.Clima.Service.Policies;
using HP.Clima.Service.Proxies.BrasilApi;
using HP.Clima.Service.Proxies.ViaCepApi;
using HP.Clima.Service.Proxies.OpenMeteo;
using HP.Clima.Service.Proxies.OpenWeatherMap;
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
        services.AddScoped<IWeatherService, WeatherService>();
        services.AddScoped<IValidationService, ValidationService>();

        return services;
    }

    public static IServiceCollection AddCepHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICepApiHandler, BrasilApiCepHandler>();
        services.AddScoped<ICepApiHandler, ViaCepHandler>();

        return services;
    }

    public static IServiceCollection AddWeatherHandlers(this IServiceCollection services)
    {
        services.AddScoped<IWeatherApiHandler, OpenMeteoWeatherHandler>();
        services.AddScoped<IWeatherApiHandler, OpenWeatherMapHandler>();

        return services;
    }

    public static IServiceCollection AddProxies(this IServiceCollection services)
    {
        services.AddBrasilApiProxy();
        services.AddViaCepProxy();
        services.AddOpenMeteoForecastProxy();
        services.AddOpenMeteoGeocodingProxy();
        services.AddOpenWeatherMapProxy();
        
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

    private static void AddOpenMeteoForecastProxy(this IServiceCollection services)
    {
        string openMeteo = "OpenMeteo";

        services.AddRefitClient<IOpenMeteoForecastProxy>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
                client.BaseAddress = new Uri(options.OpenMeteo.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<WeatherService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateRetryPolicy(
                    logger,
                    openMeteo,
                    resilienceOptions.RetryCount);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<WeatherService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateCircuitBreakerPolicy(
                    logger,
                    openMeteo,
                    resilienceOptions.CircuitBreaker);
            });
    }

    private static void AddOpenMeteoGeocodingProxy(this IServiceCollection services)
    {
        string openMeteoGeocoding = "OpenMeteoGeocoding";

        services.AddRefitClient<IOpenMeteoGeocodingProxy>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
                client.BaseAddress = new Uri(options.OpenMeteoGeocoding.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<WeatherService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateRetryPolicy(
                    logger,
                    openMeteoGeocoding,
                    resilienceOptions.RetryCount);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<WeatherService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateCircuitBreakerPolicy(
                    logger,
                    openMeteoGeocoding,
                    resilienceOptions.CircuitBreaker);
            });
    }

    private static void AddOpenWeatherMapProxy(this IServiceCollection services)
    {
        string openWeatherMap = "OpenWeatherMap";

        services.AddRefitClient<IOpenWeatherMapProxy>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
                client.BaseAddress = new Uri(options.OpenWeatherMap.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<WeatherService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateRetryPolicy(
                    logger,
                    openWeatherMap,
                    resilienceOptions.RetryCount);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<WeatherService>>();
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResiliencePolicyOptions>>().Value;
                
                return PollyPolicyFactory.CreateCircuitBreakerPolicy(
                    logger,
                    openWeatherMap,
                    resilienceOptions.CircuitBreaker);
            });
    }
}
