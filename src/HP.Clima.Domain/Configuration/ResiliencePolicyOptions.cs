namespace HP.Clima.Domain.Configuration;

public class ResiliencePolicyOptions
{
    public const string SectionName = "ResiliencePolicy";

    public int RetryCount { get; set; } = 3;
    public CircuitBreakerOptions CircuitBreaker { get; set; } = new();
}

public class CircuitBreakerOptions
{
    public double FailureThreshold { get; set; } = 5.0;
    public int SamplingDurationSeconds { get; set; } = 30;
    public int MinimumThroughput { get; set; } = 3;
    public int DurationOfBreakSeconds { get; set; } = 60;
}
