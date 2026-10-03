# Template API

A generic, reusable ASP.NET Core template API built on .NET 10 with best practices for enterprise API development.

## Overview

This template provides a solid foundation for building RESTful APIs with:

- **Dependency Injection** - Built-in service container configuration
- **Repository Pattern** - Data access abstraction layer
- **Logging** - Serilog integration with structured logging
- **Health Checks** - Built-in health check endpoints
- **Correlation IDs** - Request tracing across services
- **Swagger/OpenAPI** - Interactive API documentation
- **Exception Handling** - Centralized error handling middleware
- **Test Projects** - Unit and integration test templates

## Project Structure

```
TemplateAPI/
├── API/                          # Main API project
│   ├── Configuration/            # Service configuration and middleware
│   ├── Controllers/              # API endpoints
│   ├── Domain/                   # Business entities and models
│   │   ├── DTOs/                # Data transfer objects
│   │   ├── Entities/            # Domain entities
│   │   └── ViewModels/          # API request/response models
│   ├── Repositories/             # Data access layer
│   │   └── Interfaces/          # Repository contracts
│   ├── Services/                 # Business logic layer
│   │   └── Interfaces/          # Service contracts
│   ├── appsettings.json         # Configuration
│   ├── appsettings.Development.json
│   └── Program.cs               # Entry point and DI setup
├── API.UnitTests/                # Unit tests
└── API.IntegrationTests/         # Integration tests
```

## Getting Started

### Prerequisites

- .NET 10 SDK or later
- Visual Studio 2022 or later (recommended)
- SQL Server (optional, for database features)

### Running the Application

1. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

2. **Build the solution:**
   ```bash
   dotnet build
   ```

3. **Run the API:**
   ```bash
   cd TemplateAPI\API
   dotnet run
   ```

4. **Access the API:**
   - Swagger UI: https://localhost:7013/swagger
   - Health Check: https://localhost:7013/health
   - API: https://localhost:7013/api

### Ports

- HTTPS: 7013
- HTTP: 5013

## Configuration

### appsettings.json

Key configuration sections:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=YourDatabase;..."
  },
  "Serilog": {
	// Logging configuration
  },
  "AzureKeyVaultSettings": {
	"SecretsKeyVaultUri": ""
  }
}
```

**Important:** Replace placeholder values with your actual configuration before deploying.

## Customization Guide

### 1. Rename the API

Replace all instances of:
- `TemplateAPI` → Your API name
- `SampleEntity`, `SampleController` → Your domain entities
- Connection strings and database names

### 2. Update Domain Models

- Modify `Domain/Entities/SampleEntity.cs` with your business entity
- Update `Domain/DTOs/SampleEntityDto.cs` with your data contracts
- Create `Domain/ViewModels/` for request/response models

### 3. Implement Data Access

- Replace `Repositories/SampleRepository.cs` with actual database implementation
- Update `Repositories/Interfaces/ISampleRepository.cs` with your data contracts
- Add Dapper queries or Entity Framework DbContext as needed

### 4. Implement Business Logic

- Replace `Services/ExampleService.cs` with your business logic
- Update `Services/Interfaces/IExampleService.cs` with your service contracts
- Add validation, business rules, and domain logic

### 5. Create API Endpoints

- Replace `Controllers/SampleController.cs` with your API controllers
- Extend `Controllers/ApiControllerBase.cs` with common controller functionality
- Add authentication/authorization as needed

### 6. Configure Health Checks

In `Configuration/HealthCheckExtensions.cs`, add database and dependency checks:

```csharp
// Example: Add SQL Server health check
var connectionString = configuration.GetConnectionString("DefaultConnection");
healthChecks.AddSqlServer(connectionString, name: "Database");
```

### 7. Add Logging

The template uses Serilog with structured logging. Use in your code:

```csharp
_logger.LogInformation("Processing item {ItemId} for user {UserId}", itemId, userId);
_logger.LogError(ex, "Error processing item {ItemId}", itemId);
```

## API Endpoints

### Sample Endpoints (Reference Implementation)

The template includes example CRUD endpoints in `SampleController`:

- `GET /api/sample` - Get all items
- `GET /api/sample/{id}` - Get item by ID
- `POST /api/sample` - Create new item
- `PUT /api/sample/{id}` - Update item
- `DELETE /api/sample/{id}` - Delete item

### Health Checks

- `GET /health` - Full health check with all components
- `GET /health/ready` - Readiness probe
- `GET /health/live` - Liveness probe

## Testing

### Run Unit Tests

```bash
dotnet test API.UnitTests
```

### Run Integration Tests

```bash
dotnet test API.IntegrationTests
```

### Write Tests

1. **Unit Tests** - Test individual services/repositories with mocked dependencies
2. **Integration Tests** - Test full request/response cycle using WebApplicationFactory

Example Unit Test:
```csharp
[TestMethod]
public async Task GetByIdAsync_WithValidId_ReturnsEntity()
{
	var mockRepository = new Mock<ISampleRepository>();
	var mockLogger = new Mock<ILogger<ExampleService>>();
	var service = new ExampleService(mockRepository.Object, mockLogger.Object);

	var result = await service.GetByIdAsync(1);

	Assert.IsNotNull(result);
}
```

## Middleware & Middleware Pipeline

The template includes several key middleware components:

1. **CorrelationIdMiddleware** - Generates/captures correlation IDs for request tracing
2. **Exception Handler** - Centralized error handling in `/error` endpoint
3. **HTTPS Redirect** - Enforces HTTPS
4. **Authorization** - Ready for authentication implementation

## Logging

Serilog is configured with:
- Console output
- File output (rolling daily)
- Optional Azure Blob Storage (configure in appsettings.json)

Structured logging properties captured:
- `SequenceNumber` - Daily incrementing sequence
- `InstanceId` - Application instance identifier
- `CorrelationId` - Request trace identifier

## Error Handling

The template includes centralized error handling through the `ErrorController`:

- Returns consistent ProblemDetails responses
- Includes error codes for client handling
- Logs detailed error information server-side
- Returns correlation IDs for support troubleshooting

## Security Considerations

1. **Update User Secrets** - Configure sensitive settings in user secrets during development
2. **Azure Key Vault** - Configure in `AzureKeyVaultSettings:SecretsKeyVaultUri` for production
3. **Add Authentication** - Implement JWT, OAuth 2.0, or other authentication
4. **Add Authorization** - Implement role-based or policy-based authorization
5. **Enable CORS** - Configure as needed for your clients
6. **Request Validation** - Add input validation to all endpoints

## Deployment

### Environment Configurations

Create environment-specific files:
- `appsettings.Production.json`
- `appsettings.Staging.json`

### Docker

Example Dockerfile:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["API/API.csproj", "."]
RUN dotnet restore "API.csproj"

COPY . .
RUN dotnet build "API/API.csproj" -c Release -o /app/build
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

FROM runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "API.dll"]
```

## Documentation

For more information on dependencies:
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Serilog Documentation](https://serilog.net/)
- [Dapper Documentation](https://github.com/DapperLib/Dapper)
- [Swagger/OpenAPI](https://swagger.io/)

## Common Tasks

### Add a New Endpoint

1. Create entity in `Domain/Entities/`
2. Create DTO in `Domain/DTOs/`
3. Create ViewModels in `Domain/ViewModels/`
4. Create repository interface and implementation
5. Create service interface and implementation
6. Create controller extending `ApiControllerBase`
7. Add service registrations in `Configuration/ServiceCollectionExtensions.cs`

### Add Database Connection

1. Update connection strings in `appsettings.json`
2. Implement database access in repository
3. Add health check in `Configuration/HealthCheckExtensions.cs`
4. Test connections

### Add Authentication

1. Add authentication package (e.g., `Microsoft.AspNetCore.Authentication.JwtBearer`)
2. Configure in `Program.cs`
3. Add `[Authorize]` attributes to controllers/endpoints
4. Implement token validation

## Troubleshooting

### Build Failures

- Ensure .NET 10 SDK is installed: `dotnet --version`
- Clear NuGet cache: `dotnet nuget locals all --clear`
- Restore packages: `dotnet restore`

### Connection String Issues

- Verify server and database names in `appsettings.json`
- Check SQL Server is running
- Verify credentials and permissions
- Test connection string separately

### Health Check Failures

- Check database connectivity
- Verify configuration settings
- Review application logs

## Next Steps

1. **Customize Domain Models** - Replace sample entities with your business models
2. **Implement Data Access** - Add database queries and operations
3. **Add Business Logic** - Implement service methods
4. **Secure the API** - Add authentication and authorization
5. **Write Tests** - Add comprehensive unit and integration tests
6. **Configure Logging** - Set up appropriate log levels and sinks
7. **Deploy** - Configure for your target environment

## Support

For issues or questions:
1. Check the troubleshooting section above
2. Review the official documentation links
3. Examine sample implementations in the template
4. Check application logs for detailed error information

## License

[Add your license information here]

---

**Last Updated:** October 2026
**Template Version:** 1.0
**Target Framework:** .NET 10
