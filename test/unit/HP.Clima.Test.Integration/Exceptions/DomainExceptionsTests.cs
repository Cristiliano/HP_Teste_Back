using FluentAssertions;
using HP.Clima.Domain.Exceptions;

namespace HP.Clima.Test.Unit.Exceptions;

public class DomainExceptionsTests
{
    [Fact]
    public void NotFoundException_ShouldContainCorrectProperties()
    {
        var detail = "CEP não encontrado";
        var instance = "/api/cep/12345678";
        var errorCode = "CEP_NOT_FOUND";

        var exception = new NotFoundException(detail, instance, errorCode);

        exception.Detail.Should().Be(detail);
        exception.Instance.Should().Be(instance);
        exception.ErrorCode.Should().Be(errorCode);
        exception.Message.Should().Be(detail);
    }

    [Fact]
    public void ConflictException_ShouldContainCorrectProperties()
    {
        var detail = "CEP já cadastrado";
        var instance = "/api/cep";

        var exception = new ConflictException(detail, instance);

        exception.Detail.Should().Be(detail);
        exception.Instance.Should().Be(instance);
        exception.Message.Should().Be(detail);
    }

    [Fact]
    public void ValidationException_ShouldContainErrorsDictionary()
    {
        var detail = "Validation failed";
        var instance = "/api/cep";
        var errors = new Dictionary<string, string[]>
        {
            { "ZipCode", new[] { "CEP é obrigatório", "CEP deve ter 8 dígitos" } }
        };

        var exception = new ValidationException(detail, instance, errors);

        exception.Detail.Should().Be(detail);
        exception.Instance.Should().Be(instance);
        exception.Errors.Should().NotBeNull();
        exception.Errors.Should().ContainKey("ZipCode");
        exception.Errors!["ZipCode"].Should().HaveCount(2);
    }
}
