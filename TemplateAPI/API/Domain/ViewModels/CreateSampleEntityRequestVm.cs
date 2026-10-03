namespace API.Domain.ViewModels;

/// <summary>
/// Generic sample request view model for template API
/// Replace with your actual view models
/// </summary>
public class CreateSampleEntityRequestVm
{
    #region Properties

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    #endregion
}
