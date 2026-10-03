using API.Domain.DTOs;

namespace API.Repositories.Interfaces;

/// <summary>
/// Generic sample repository interface for template API
/// Replace with your actual IRepository implementation
/// </summary>
public interface ISampleRepository
{
    #region Methods

    Task<SampleEntityDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<SampleEntityDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<int> CreateAsync(SampleEntityDto entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, SampleEntityDto entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    #endregion
}
