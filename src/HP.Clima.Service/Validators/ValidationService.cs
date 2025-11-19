using FluentValidation;
using HP.Clima.Domain.Interfaces.Services;

namespace HP.Clima.Service.Validators;

public class ValidationService : IValidationService
{
    public async Task ValidateAsync<T>(T obj, IValidator<T> validator, string? instance = null)
    {
        var validationResult = await validator.ValidateAsync(obj);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            throw new Domain.Exceptions.ValidationException(
                detail: "Um ou mais erros de validação ocorreram.",
                instance: instance ?? string.Empty,
                errors: errors
            );
        }
    }
}

