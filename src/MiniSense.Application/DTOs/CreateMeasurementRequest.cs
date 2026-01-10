using System.ComponentModel.DataAnnotations;

namespace MiniSense.Application.DTOs;

public sealed record CreateMeasurementRequest(
    [Required] 
    long Timestamp,

    [Required] 
    double Value
 );