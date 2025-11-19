namespace HP.Clima.Domain.Exceptions;

public class ValidationException(
    string detail, 
    string instance = "", 
    Dictionary<string, string[]>? errors = null) : Exception(detail)
{
    public string Detail { get; } = detail;
    public string Instance { get; } = instance;
    public Dictionary<string, string[]>? Errors { get; } = errors;
}

public class NotFoundException(
    string detail, 
    string instance = "") : Exception(detail)
{
    public string Detail { get; } = detail;
    public string Instance { get; } = instance;
}