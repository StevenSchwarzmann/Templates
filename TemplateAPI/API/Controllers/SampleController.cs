using API.Domain.ViewModels;
using API.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Generic sample controller demonstrating basic CRUD operations
/// Replace with your actual business logic controllers
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SampleController : ApiControllerBase
{
    #region Data Members

    private readonly IExampleService _service;
    private readonly ILogger<SampleController> _logger;

    #endregion

    #region Constructors

    public SampleController(IExampleService service, ILogger<SampleController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get a sample entity by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SampleEntityResponseVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("{ControllerName} => {ActionName} Started with ID: {Id}", 
            nameof(SampleController), nameof(GetById), id);

        try
        {
            SampleEntityResponseVm? entity = await _service.GetByIdAsync(id);

            if (entity is null)
            {
                _logger.LogWarning("Entity with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("{ControllerName} => {ActionName} Completed", 
                nameof(SampleController), nameof(GetById));

            return Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetById");
            throw;
        }
    }

    /// <summary>
    /// Get all sample entities
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<SampleEntityResponseVm>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("{ControllerName} => {ActionName} Started", 
            nameof(SampleController), nameof(GetAll));

        try
        {
            List<SampleEntityResponseVm> entities = await _service.GetAllAsync();

            _logger.LogInformation("{ControllerName} => {ActionName} Completed. Found {Count} entities", 
                nameof(SampleController), nameof(GetAll), entities.Count);

            return Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAll");
            throw;
        }
    }

    /// <summary>
    /// Create a new sample entity
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SampleEntityResponseVm), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSampleEntityRequestVm request)
    {
        _logger.LogInformation("{ControllerName} => {ActionName} Started with Name: {Name}", 
            nameof(SampleController), nameof(Create), request.Name);

        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogWarning("Invalid request: Name is required");
                return BadRequest("Name is required");
            }

            SampleEntityResponseVm newEntity = await _service.CreateAsync(request);

            _logger.LogInformation("{ControllerName} => {ActionName} Completed with new ID: {Id}", 
                nameof(SampleController), nameof(Create), newEntity.Id);

            return CreatedAtAction(nameof(GetById), new { id = newEntity.Id }, newEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create");
            throw;
        }
    }

    /// <summary>
    /// Update an existing sample entity
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] CreateSampleEntityRequestVm request)
    {
        _logger.LogInformation("{ControllerName} => {ActionName} Started with ID: {Id}", 
            nameof(SampleController), nameof(Update), id);

        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogWarning("Invalid request: Name is required");
                return BadRequest("Name is required");
            }

            bool result = await _service.UpdateAsync(id, request);

            if (!result)
            {
                _logger.LogWarning("Entity with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("{ControllerName} => {ActionName} Completed", 
                nameof(SampleController), nameof(Update));

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Update");
            throw;
        }
    }

    /// <summary>
    /// Delete a sample entity
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("{ControllerName} => {ActionName} Started with ID: {Id}", 
            nameof(SampleController), nameof(Delete), id);

        try
        {
            bool result = await _service.DeleteAsync(id);

            if (!result)
            {
                _logger.LogWarning("Entity with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("{ControllerName} => {ActionName} Completed", 
                nameof(SampleController), nameof(Delete));

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Delete");
            throw;
        }
    }

    #endregion
}
