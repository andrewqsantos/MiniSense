namespace MiniSense.Application.DTOs;

public sealed record MeasurementUnitDto(
    int Id, 
    string Symbol, 
    string Description
);