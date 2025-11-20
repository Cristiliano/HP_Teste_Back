using FluentAssertions;
using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Validators;
using HP.Clima.Test.Unit.Validators.Mocks;

namespace HP.Clima.Test.Unit.Validators;

public class WeatherRequestValidatorTests
{
    private readonly WeatherRequestValidator _validator;

    public WeatherRequestValidatorTests()
    {
        _validator = new WeatherRequestValidator();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(7)]
    public async Task Validate_ShouldBeValidForDaysInRange(int days)
    {
        var request = new WeatherRequestDto { Days = days };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(InvalidDaysTestData))]
    public async Task Validate_ShouldBeInvalidForDaysOutOfRange(int days)
    {
        var request = new WeatherRequestDto { Days = days };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("deve ser um número inteiro entre 1 e 7"));
    }
}
