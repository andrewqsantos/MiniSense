using System.Text.Json.Serialization;

namespace MiniSense.Application.DTOs;

public sealed record DataStreamDto(
    int Id,
    Guid Key,             
    string Label,
    int UnitId,             
    int DeviceId,
    int MeasurementCount,
    
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IEnumerable<SensorDataSummaryDto> Measurements
);