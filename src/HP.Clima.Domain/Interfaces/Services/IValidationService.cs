using FluentValidation;

namespace HP.Clima.Domain.Interfaces.Services;

public interface IValidationService
{
    Task ValidateAsync<T>(T obj, IValidator<T> validator, string? instance = null);
}
