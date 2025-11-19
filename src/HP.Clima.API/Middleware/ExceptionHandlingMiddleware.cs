using HP.Clima.Domain.Exceptions;
using HP.Clima.API.Models;
using System.Diagnostics;

namespace HP.Clima.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        var problemDetails = new ProblemDetails
        {
            TraceId = traceId,
            Timestamp = DateTime.UtcNow
        };

        if (exception is ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Type = "https://api.example.com/errors/validation-error";
            problemDetails.Title = "Erro de Validação";
            problemDetails.Detail = validationEx.Detail;
            problemDetails.Instance = validationEx.Instance;
            problemDetails.Errors = validationEx.Errors;
        }
        else if (exception is NotFoundException notFoundEx)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Type = "https://api.example.com/errors/not-found";
            problemDetails.Title = "Recurso Não Encontrado";
            problemDetails.Detail = notFoundEx.Detail;
            problemDetails.Instance = notFoundEx.Instance;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Type = "https://api.example.com/errors/internal-server-error";
            problemDetails.Title = "Erro Interno do Servidor";
            problemDetails.Detail = "Um erro inesperado ocorreu. Por favor, tente novamente mais tarde.";
        }

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
