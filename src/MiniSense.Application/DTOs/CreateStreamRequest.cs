using System.ComponentModel.DataAnnotations;
using MiniSense.Domain.Constants;

namespace MiniSense.Application.DTOs;

public sealed record CreateStreamRequest(
    [Required] 
    [MaxLength(ValidationConstants.MaxStreamLabelLength)] 
    string Label,

    [Required] 
    [Range(1, 5)]
    int UnitId
);