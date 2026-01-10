using Microsoft.AspNetCore.Mvc;
using MiniSense.Application.DTOs;
using MiniSense.Application.Interfaces;

namespace MiniSense.API.Controllers;

[ApiController]
[Route("api/measurement-units")]
[Produces("application/json")]
public class UnitsController(IMiniSenseService service) : ControllerBase
{
    /// <summary>
    /// Lista todas as unidades de medida disponíveis.
    /// </summary>
    /// <returns>Lista de unidades (ex: Celsius, Lux, etc).</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MeasurementUnitDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var units = await service.GetAllUnitsAsync();
        return Ok(units);
    }
}