using API.Domain.ViewModels;

namespace API.Services.Interfaces;

/// <summary>
/// Generic sample service interface for template API
/// Replace with your actual IService implementation
/// </summary>
public interface IExampleService
{
    #region Methods

    Task<SampleEntityResponseVm?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<SampleEntityResponseVm>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<SampleEntityResponseVm> CreateAsync(CreateSampleEntityRequestVm request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, CreateSampleEntityRequestVm request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    #endregion
}
