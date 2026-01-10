using Microsoft.AspNetCore.Mvc;
using MiniSense.Application.DTOs;
using MiniSense.Application.Interfaces;

namespace MiniSense.API.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController(IMiniSenseService service) : ControllerBase
{
    /// <summary>
    /// Consulta todos os dispositivos pertencentes a um usuário específico.
    /// </summary>
    /// <param name="userId">ID interno do usuário.</param>
    [HttpGet("{userId:int}/devices")]
    [ProducesResponseType(typeof(IEnumerable<SensorDeviceDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserDevices(int userId)
    {
        var devices = await service.GetDevicesByUserAsync(userId);
        return Ok(devices);
    }

    /// <summary>
    /// Registra um novo dispositivo sensor para o usuário.
    /// </summary>
    /// <param name="userId">ID do usuário dono do dispositivo.</param>
    /// <param name="request">Dados do novo dispositivo.</param>
    [HttpPost("{userId:int}/devices")]
    [ProducesResponseType(typeof(SensorDeviceSummaryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Usuário não existe
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Validação falhou
    public async Task<IActionResult> RegisterDevice(int userId, [FromBody] CreateDeviceRequest request)
    {
        try
        {
            var device = await service.RegisterDeviceAsync(userId, request);
            
            // Retorna 201 Created e o Header 'Location' apontando para a rota de consulta do device
            return CreatedAtAction(
                nameof(DevicesController.GetDeviceByKey), 
                "Devices", 
                new { key = device.Key }, 
                device);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}