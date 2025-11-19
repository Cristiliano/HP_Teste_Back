using HP.Clima.API.Middleware;

namespace HP.Clima.API.Configuration;

public static class SetupConfiguration
{
    public static void ConfigureSetup(this WebApplication app)
    {
        app.CondigureSwagger();
        app.UseHttpsRedirection();
        app.UseExceptionHandling();
        app.MapControllers();
    }

    private static void CondigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
