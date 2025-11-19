using HP.Clima.Domain.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace HP.Clima.Service.Policies;

public static class PollyPolicyFactory
{
    public static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(
        ILogger logger,
        string apiName,
        int retryCount)
    {
        var delay = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: TimeSpan.FromSeconds(1),
            retryCount: retryCount);

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                delay,
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning(
                        "[{ApiName}] Tentativa {RetryAttempt} de {RetryCount}. Aguardando {Delay}ms antes de tentar novamente. Erro: {Error}",
                        apiName,
                        retryAttempt,
                        retryCount,
                        timespan.TotalMilliseconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString() ?? "Unknown"
                    );
                });
    }

    public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(
        ILogger logger,
        string apiName,
        CircuitBreakerOptions options)
    {
        var failureThreshold = options.FailureThreshold / 100.0;
        var samplingDuration = TimeSpan.FromSeconds(options.SamplingDurationSeconds);
        var durationOfBreak = TimeSpan.FromSeconds(options.DurationOfBreakSeconds);

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutException>()
            .AdvancedCircuitBreakerAsync(
                failureThreshold: failureThreshold,
                samplingDuration: samplingDuration,
                minimumThroughput: options.MinimumThroughput,
                durationOfBreak: durationOfBreak,
                onBreak: (result, breakDuration) =>
                {
                    logger.LogError(
                        "[{ApiName}] Circuit Breaker ABERTO por {BreakDuration}s. Erro: {Error}",
                        apiName,
                        breakDuration.TotalSeconds,
                        result.Exception?.Message ?? result.Result?.StatusCode.ToString() ?? "Unknown"
                    );
                },
                onReset: () =>
                {
                    logger.LogInformation(
                        "[{ApiName}] Circuit Breaker FECHADO. Operação normal restabelecida.",
                        apiName);
                },
                onHalfOpen: () =>
                {
                    logger.LogWarning(
                        "[{ApiName}] Circuit Breaker HALF-OPEN. Testando se o serviço voltou.",
                        apiName);
                }
            );
    }
}
