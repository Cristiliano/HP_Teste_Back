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

public class CepServicePersistenceTests
{
    private readonly Faker _faker;
    private readonly Mock<IZipCodeRepository> _mockRepository;
    private readonly Mock<ILogger<CepService>> _mockLogger;
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly Mock<IValidator<CepRequestDto>> _mockValidator;
    private readonly List<ICepApiHandler> _mockHandlers;

    public CepServicePersistenceTests()
    {
        _faker = new Faker("pt_BR");
        _mockRepository = new Mock<IZipCodeRepository>();
        _mockLogger = new Mock<ILogger<CepService>>();
        _mockValidationService = new Mock<IValidationService>();
        _mockValidator = new Mock<IValidator<CepRequestDto>>();
        _mockHandlers = new List<ICepApiHandler>();
    }

    [Fact]
    public async Task SaveCepInfoAsync_ShouldThrowConflictWhenCepAlreadyExists()
    {
        var zipCode = "01311000";
        var existingEntity = new ZipCodeEntity
        {
            Id = Guid.NewGuid(),
            ZipCode = zipCode,
            City = _faker.Address.City()
        };

        _mockRepository.Setup(x => x.GetByZipCodeAsync(zipCode))
            .ReturnsAsync(existingEntity);

        var service = new CepService(
            _mockHandlers,
            _mockRepository.Object,
            _mockValidationService.Object,
            _mockValidator.Object,
            _mockLogger.Object
        );

        var request = new CepRequestDto(zipCode);

        var act = async () => await service.SaveCepInfoAsync(request);

        await act.Should().ThrowAsync<ConflictException>()
            .Where(e => e.Detail.Contains("já está cadastrado"))
            .Where(e => e.Instance == "/api/cep");
    }

    [Fact]
    public async Task GetCepInfoAsync_ShouldReturnCachedValueWhenExists()
    {
        var zipCode = "01311000";
        var cachedEntity = new ZipCodeEntity
        {
            Id = Guid.NewGuid(),
            ZipCode = zipCode,
            Street = _faker.Address.StreetName(),
            City = _faker.Address.City(),
            State = _faker.Address.StateAbbr(),
            Provider = "brasilapi"
        };

        _mockRepository.Setup(x => x.GetByZipCodeAsync(zipCode))
            .ReturnsAsync(cachedEntity);

        var service = new CepService(
            _mockHandlers,
            _mockRepository.Object,
            _mockValidationService.Object,
            _mockValidator.Object,
            _mockLogger.Object
        );

        var result = await service.GetCepInfoAsync(zipCode);

        result.Should().NotBeNull();
        result!.ZipCode.Should().Be(zipCode);
        result.City.Should().Be(cachedEntity.City);
        _mockRepository.Verify(x => x.GetByZipCodeAsync(zipCode), Times.Once);
    }

    [Fact]
    public async Task GetCepInfoAsync_ShouldSaveToRepositoryAfterExternalApiFetch()
    {
        var normalizedZipCode = "01311000";
        var zipCodeDto = new ZipCodeDto
        {
            ZipCode = normalizedZipCode,
            City = _faker.Address.City(),
            Street = _faker.Address.StreetName(),
            Provider = "brasilapi"
        };

        var mockHandler = new Mock<ICepApiHandler>();
        mockHandler.Setup(x => x.ApiName).Returns("BrasilAPI");
        mockHandler.Setup(x => x.TryGetCepAsync(normalizedZipCode))
            .ReturnsAsync((true, zipCodeDto));

        var handlers = new List<ICepApiHandler> { mockHandler.Object };

        _mockRepository.Setup(x => x.GetByZipCodeAsync(normalizedZipCode))
            .ReturnsAsync((ZipCodeEntity?)null);

        _mockRepository.Setup(x => x.CreateAsync(It.IsAny<ZipCodeEntity>()))
            .Returns(Task.CompletedTask);

        var service = new CepService(
            handlers,
            _mockRepository.Object,
            _mockValidationService.Object,
            _mockValidator.Object,
            _mockLogger.Object
        );

        var result = await service.GetCepInfoAsync(normalizedZipCode);

        result.Should().NotBeNull();
        _mockRepository.Verify(x => x.CreateAsync(It.Is<ZipCodeEntity>(
            e => e.ZipCode == normalizedZipCode && e.Provider == "brasilapi"
        )), Times.Once);
    }
}
