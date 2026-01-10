namespace MiniSense.Application.DTOs;

public sealed record SensorDataSummaryDto(
    long Timestamp,
    double Value
);