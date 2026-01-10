using Microsoft.AspNetCore.Mvc;
using MiniSense.Application.DTOs;
using MiniSense.Application.Interfaces;

namespace MiniSense.API.Controllers;

[ApiController]
[Route("api/devices")]
[Produces("application/json")]
public class DevicesController(IMiniSenseService service) : ControllerBase
{
    /// <summary>
    /// Obtém detalhes de um dispositivo e suas últimas medições.
    /// </summary>
    /// <param name="key">Chave de identificação (Guid string).</param>
    [HttpGet("{key}")]
    [ProducesResponseType(typeof(SensorDeviceDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviceByKey(Guid key)
    {
        var device = await service.GetDeviceByKeyAsync(key);
        if (device is null) return NotFound(new { message = "Device not found" });

        return Ok(device);
    }

    /// <summary>
    /// Adiciona uma nova Stream de dados (ex: temperatura) a um dispositivo existente.
    /// </summary>
    /// <param name="key">Chave do dispositivo.</param>
    /// <param name="request">Dados da nova stream.</param>
    [HttpPost("{key}/streams")]
    [ProducesResponseType(typeof(DataStreamDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterStream(Guid key, [FromBody] CreateStreamRequest request)
    {
        try
        {
            var stream = await service.RegisterStreamAsync(key, request);
            
            return CreatedAtAction(
                nameof(StreamsController.GetStreamByKey), 
                "Streams", 
                new { key = stream.Key }, 
                stream);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}