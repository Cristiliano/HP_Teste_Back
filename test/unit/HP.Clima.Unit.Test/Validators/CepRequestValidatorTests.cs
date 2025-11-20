using FluentAssertions;
using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Validators;
using HP.Clima.Test.Unit.Validators.Mocks;

namespace HP.Clima.Test.Unit.Validators;

public class CepRequestValidatorTests
{
    private readonly CepRequestValidator _validator;

    public CepRequestValidatorTests()
    {
        _validator = new CepRequestValidator();
    }

    [Theory]
    [InlineData("01311000")]
    [InlineData("01311-000")]
    [InlineData("1234567")]
    public async Task Validate_ShouldBeValidForCorrectFormats(string zipCode)
    {
        var request = new CepRequestDto { ZipCode = zipCode };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Validate_ShouldBeInvalidForEmptyZipCode(string zipCode)
    {
        var request = new CepRequestDto { ZipCode = zipCode };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("CEP é obrigatório"));
    }

    [Theory]
    [ClassData(typeof(InvalidCepFormatsTestData))]
    public async Task Validate_ShouldBeInvalidForIncorrectFormats(string zipCode, string expectedErrorFragment)
    {
        var request = new CepRequestDto { ZipCode = zipCode };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedErrorFragment));
    }
}
