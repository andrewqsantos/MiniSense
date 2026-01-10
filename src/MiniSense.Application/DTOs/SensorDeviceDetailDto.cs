namespace MiniSense.Application.DTOs;

public sealed record SensorDeviceDetailDto(
    int Id, 
    Guid Key,
    string Label,
    string Description,
    IEnumerable<DataStreamDto> Streams
);