using API.Domain.DTOs;
using API.Domain.ViewModels;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services;

/// <summary>
/// Generic sample service implementation for template API
/// Replace with your actual service implementation
/// </summary>
public class ExampleService : IExampleService
{
    #region Data Members

    private readonly ISampleRepository _repository;
    private readonly ILogger<ExampleService> _logger;

    #endregion

    #region Constructors

    public ExampleService(ISampleRepository repository, ILogger<ExampleService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    #region IExampleService Implementation

    public async Task<SampleEntityResponseVm?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ExampleService => GetByIdAsync Started with ID: {Id}", id);

        try
        {
            SampleEntityDto? entity = await _repository.GetByIdAsync(id, cancellationToken);

            if (entity is null)
            {
                _logger.LogWarning("Entity with ID {Id} not found", id);
                return null;
            }

            var response = new SampleEntityResponseVm
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };

            _logger.LogInformation("ExampleService => GetByIdAsync Completed");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetByIdAsync for ID: {Id}", id);
            throw;
        }
    }

    public async Task<List<SampleEntityResponseVm>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ExampleService => GetAllAsync Started");

        try
        {
            List<SampleEntityDto> entities = await _repository.GetAllAsync(cancellationToken);

            var response = entities.Select(e => new SampleEntityResponseVm
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                IsActive = e.IsActive,
                CreatedDate = e.CreatedDate,
                ModifiedDate = e.ModifiedDate
            }).ToList();

            _logger.LogInformation("ExampleService => GetAllAsync Completed. Found {Count} entities", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync");
            throw;
        }
    }

    public async Task<SampleEntityResponseVm> CreateAsync(CreateSampleEntityRequestVm request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ExampleService => CreateAsync Started with Name: {Name}", request.Name);

        try
        {
            var dto = new SampleEntityDto
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            int newId = await _repository.CreateAsync(dto, cancellationToken);

            var response = new SampleEntityResponseVm
            {
                Id = newId,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };

            _logger.LogInformation("ExampleService => CreateAsync Completed with new ID: {Id}", newId);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, CreateSampleEntityRequestVm request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ExampleService => UpdateAsync Started for ID: {Id}", id);

        try
        {
            var dto = new SampleEntityDto
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                ModifiedDate = DateTime.UtcNow
            };

            bool result = await _repository.UpdateAsync(id, dto, cancellationToken);

            _logger.LogInformation("ExampleService => UpdateAsync Completed for ID: {Id}. Result: {Result}", id, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAsync for ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ExampleService => DeleteAsync Started for ID: {Id}", id);

        try
        {
            bool result = await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("ExampleService => DeleteAsync Completed for ID: {Id}. Result: {Result}", id, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteAsync for ID: {Id}", id);
            throw;
        }
    }

    #endregion
}
