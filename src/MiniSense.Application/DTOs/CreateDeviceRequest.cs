using System.ComponentModel.DataAnnotations;
using MiniSense.Domain.Constants;

namespace MiniSense.Application.DTOs;

public sealed record CreateDeviceRequest(
    [Required] 
    [MaxLength(ValidationConstants.MaxLabelLength)] 
    string Label,

    [MaxLength(ValidationConstants.MaxDescriptionLength)] 
    string Description
);