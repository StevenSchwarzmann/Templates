namespace API.Domain.DTOs;

/// <summary>
/// Generic sample DTO for template API
/// Replace with your actual data transfer objects
/// </summary>
public class SampleEntityDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    #endregion
}
