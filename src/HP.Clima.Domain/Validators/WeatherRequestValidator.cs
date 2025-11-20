using FluentValidation;
using HP.Clima.Domain.DTOs;

namespace HP.Clima.Domain.Validators;

public class WeatherRequestValidator : AbstractValidator<WeatherRequestDto>
{
    public WeatherRequestValidator()
    {
        RuleFor(x => x.Days)
            .InclusiveBetween(1, 7)
            .WithMessage("O parâmetro 'days' deve ser um número inteiro entre 1 e 7");
    }
}
