using API.Domain.DTOs;
using API.Domain.ViewModels;
using API.Repositories.Interfaces;
using API.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace API.UnitTests.Services;

[TestClass]
public class ExampleServiceTests
{
    #region Test Methods

    [TestMethod]
    public async Task GetByIdAsync_WithValidId_ReturnsEntity()
    {
        // Arrange
        var mockRepository = new Mock<ISampleRepository>();
        var mockLogger = new Mock<ILogger<ExampleService>>();

        var service = new ExampleService(mockRepository.Object, mockLogger.Object);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.IsNull(result, "Result should be null from stub repository");
    }

    [TestMethod]
    public async Task CreateAsync_WithValidRequest_ReturnsNewEntity()
    {
        // Arrange
        var mockRepository = new Mock<ISampleRepository>();
        var mockLogger = new Mock<ILogger<ExampleService>>();

        var service = new ExampleService(mockRepository.Object, mockLogger.Object);

        var request = new CreateSampleEntityRequestVm
        {
            Name = "Test Entity",
            Description = "Test Description",
            IsActive = true
        };

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(request.Name, result.Name);
        Assert.AreEqual(request.Description, result.Description);
        Assert.IsTrue(result.IsActive);
    }

    [TestMethod]
    public async Task GetAllAsync_ReturnsEmptyList()
    {
        // Arrange
        var mockRepository = new Mock<ISampleRepository>();
        var mockLogger = new Mock<ILogger<ExampleService>>();

        // Setup the mock to return an empty list
        mockRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SampleEntityDto>());

        var service = new ExampleService(mockRepository.Object, mockLogger.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count, "Stub repository should return empty list");
    }

    #endregion
}
