using Bogus;
using FluentAssertions;
using FluentValidation;
using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Exceptions;
using HP.Clima.Domain.Interfaces.Services;
using HP.Clima.Domain.Repositories;
using HP.Clima.Service.Handlers.Interfaces;
using HP.Clima.Service.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace HP.Clima.Test.Unit.Services;

public class CepServiceFallbackTests
{
    private readonly Faker _faker;
    private readonly Mock<IZipCodeRepository> _mockRepository;
    private readonly Mock<ILogger<CepService>> _mockLogger;
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly Mock<IValidator<CepRequestDto>> _mockValidator;

    public CepServiceFallbackTests()
    {
        _faker = new Faker("pt_BR");
        _mockRepository = new Mock<IZipCodeRepository>();
        _mockLogger = new Mock<ILogger<CepService>>();
        _mockValidationService = new Mock<IValidationService>();
        _mockValidator = new Mock<IValidator<CepRequestDto>>();
    }

    [Fact]
    public async Task GetCepInfoAsync_ShouldUseFallbackWhenFirstProviderFails()
    {
        var normalizedZipCode = "01311000";
        var zipCodeDto = new ZipCodeDto
        {
            ZipCode = normalizedZipCode,
            City = _faker.Address.City(),
            Provider = "viacep"
        };

        var mockFirstHandler = new Mock<ICepApiHandler>();
        mockFirstHandler.Setup(x => x.ApiName).Returns("BrasilAPI");
        mockFirstHandler.Setup(x => x.TryGetCepAsync(normalizedZipCode))
            .ReturnsAsync((false, null));

        var mockSecondHandler = new Mock<ICepApiHandler>();
        mockSecondHandler.Setup(x => x.ApiName).Returns("ViaCEP");
        mockSecondHandler.Setup(x => x.TryGetCepAsync(normalizedZipCode))
            .ReturnsAsync((true, zipCodeDto));

        var handlers = new List<ICepApiHandler> { mockFirstHandler.Object, mockSecondHandler.Object };

        _mockRepository.Setup(x => x.GetByZipCodeAsync(normalizedZipCode))
            .ReturnsAsync((ZipCodeEntity?)null);

        var service = new CepService(
            handlers,
            _mockRepository.Object,
            _mockValidationService.Object,
            _mockValidator.Object,
            _mockLogger.Object
        );

        var result = await service.GetCepInfoAsync(normalizedZipCode);

        result.Should().NotBeNull();
        result!.Provider.Should().Be("viacep");
        mockFirstHandler.Verify(x => x.TryGetCepAsync(normalizedZipCode), Times.Once);
        mockSecondHandler.Verify(x => x.TryGetCepAsync(normalizedZipCode), Times.Once);
    }

    [Fact]
    public async Task GetCepInfoAsync_ShouldThrowNotFoundWhenAllProvidersFail()
    {
        var normalizedZipCode = "99999999";

        var mockFirstHandler = new Mock<ICepApiHandler>();
        mockFirstHandler.Setup(x => x.ApiName).Returns("BrasilAPI");
        mockFirstHandler.Setup(x => x.TryGetCepAsync(normalizedZipCode))
            .ReturnsAsync((false, null));

        var mockSecondHandler = new Mock<ICepApiHandler>();
        mockSecondHandler.Setup(x => x.ApiName).Returns("ViaCEP");
        mockSecondHandler.Setup(x => x.TryGetCepAsync(normalizedZipCode))
            .ReturnsAsync((false, null));

        var handlers = new List<ICepApiHandler> { mockFirstHandler.Object, mockSecondHandler.Object };

        _mockRepository.Setup(x => x.GetByZipCodeAsync(normalizedZipCode))
            .ReturnsAsync((ZipCodeEntity?)null);

        var service = new CepService(
            handlers,
            _mockRepository.Object,
            _mockValidationService.Object,
            _mockValidator.Object,
            _mockLogger.Object
        );

        var act = async () => await service.GetCepInfoAsync(normalizedZipCode);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"CEP {normalizedZipCode} não encontrado nas bases de dados consultadas.");
        
        mockFirstHandler.Verify(x => x.TryGetCepAsync(normalizedZipCode), Times.Once);
        mockSecondHandler.Verify(x => x.TryGetCepAsync(normalizedZipCode), Times.Once);
    }

    [Fact]
    public async Task GetCepInfoAsync_ShouldUseFirstProviderWhenSuccessful()
    {
        var normalizedZipCode = "01311000";
        var zipCodeDto = new ZipCodeDto
        {
            ZipCode = normalizedZipCode,
            City = _faker.Address.City(),
            Provider = "brasilapi"
        };

        var mockFirstHandler = new Mock<ICepApiHandler>();
        mockFirstHandler.Setup(x => x.ApiName).Returns("BrasilAPI");
        mockFirstHandler.Setup(x => x.TryGetCepAsync(normalizedZipCode))
            .ReturnsAsync((true, zipCodeDto));

        var mockSecondHandler = new Mock<ICepApiHandler>();
        mockSecondHandler.Setup(x => x.ApiName).Returns("ViaCEP");

        var handlers = new List<ICepApiHandler> { mockFirstHandler.Object, mockSecondHandler.Object };

        _mockRepository.Setup(x => x.GetByZipCodeAsync(normalizedZipCode))
            .ReturnsAsync((ZipCodeEntity?)null);

        var service = new CepService(
            handlers,
            _mockRepository.Object,
            _mockValidationService.Object,
            _mockValidator.Object,
            _mockLogger.Object
        );

        var result = await service.GetCepInfoAsync(normalizedZipCode);

        result.Should().NotBeNull();
        result!.Provider.Should().Be("brasilapi");
        mockFirstHandler.Verify(x => x.TryGetCepAsync(normalizedZipCode), Times.Once);
        mockSecondHandler.Verify(x => x.TryGetCepAsync(normalizedZipCode), Times.Never);
    }
}
