namespace MiniSense.Application.DTOs;

public sealed record SensorDeviceSummaryDto(
    int Id,
    Guid Key,
    string Label,
    string Description
);