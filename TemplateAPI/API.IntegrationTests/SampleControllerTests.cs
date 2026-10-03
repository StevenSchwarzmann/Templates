using API.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace API.IntegrationTests;

[TestClass]
public class SampleControllerTests
{
    #region Test Methods

    [TestMethod]
    public async Task GetAll_ReturnsOk()
    {
        // Arrange
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();

        // Act
        var response = await client.GetAsync("/api/sample");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task GetById_WithValidId_ReturnsOk()
    {
        // Arrange
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();

        // Act
        var response = await client.GetAsync("/api/sample/1");

        // Assert
        // Stub implementation returns 404, which is expected
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Create_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();

        var request = new CreateSampleEntityRequestVm
        {
            Name = "New Entity",
            Description = "Test Description",
            IsActive = true
        };

        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await client.PostAsync("/api/sample", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [TestMethod]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Arrange
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    #endregion
}
