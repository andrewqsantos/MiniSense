namespace MiniSense.Application.DTOs;

public sealed record SensorDataDetailDto(
    int Id,
    long Timestamp,
    double Value,
    int UnitId
);