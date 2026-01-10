using Microsoft.AspNetCore.Mvc;
using MiniSense.Application.DTOs;
using MiniSense.Application.Interfaces;

namespace MiniSense.API.Controllers;

[ApiController]
[Route("api/streams")]
[Produces("application/json")]
public class StreamsController(IMiniSenseService service) : ControllerBase
{
    /// <summary>
    /// Consulta uma stream específica e seu histórico de medições.
    /// </summary>
    /// <param name="key">Chave da stream.</param>
    [HttpGet("{key}")]
    [ProducesResponseType(typeof(DataStreamDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStreamByKey(Guid key)
    {
        var stream = await service.GetStreamByKeyAsync(key);
        if (stream is null) return NotFound(new { message = "Stream not found" });

        return Ok(stream);
    }

    /// <summary>
    /// Publica uma nova leitura (medição) para a stream.
    /// </summary>
    /// <param name="key">Chave da stream.</param>
    /// <param name="request">Valor e Timestamp.</param>
    [HttpPost("{key}/data")]
    [ProducesResponseType(typeof(SensorDataDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMeasurement(Guid key, [FromBody] CreateMeasurementRequest request)
    {
        try
        {
            var data = await service.AddMeasurementAsync(key, request);
            return Ok(data);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex) // Ex: Stream desabilitada
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}