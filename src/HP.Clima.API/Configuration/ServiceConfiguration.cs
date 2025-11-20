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
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "HP Clima API",
                Version = "v1",
                Description = "API para consulta de CEPs e informações meteorológicas",
                Contact = new()
                {
                    Name = "HP Clima",
                    Email = "contato@hpclima.com.br"
                }
            });

            // Habilita comentários XML para documentação
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }

            // Habilita anotações do Swashbuckle
            options.EnableAnnotations();
            
            // Configura exemplos de schemas
            options.SchemaFilter<SwaggerSchemaExampleFilter>();
        });
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
