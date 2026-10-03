namespace API.Domain.Entities;

/// <summary>
/// Generic sample entity for template API
/// Replace with your actual entity model
/// </summary>
public class SampleEntity
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    #endregion
}
