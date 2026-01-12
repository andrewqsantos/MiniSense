using MiniSense.Application.DTOs;
using MiniSense.Domain.Entities;

namespace MiniSense.Application.Extensions;

public static class DomainMappingExtensions
{
    public static SensorDeviceSummaryDto ToSummaryDto(this SensorDevice device)
    {
        return new SensorDeviceSummaryDto(
            device.Id,
            device.Key,
            device.Label,
            device.Description
        );
    }
    
    public static SensorDeviceDetailDto ToDetailDto(this SensorDevice device)
    {
        return new SensorDeviceDetailDto(
            device.Id,
            device.Key,
            device.Label,
            device.Description,
            device.Streams.Select(s => s.ToDto()).ToList()
        );
    }

    public static DataStreamDto ToDto(this DataStream stream)
    {
        return new DataStreamDto(
            stream.Id,
            stream.Key,
            stream.Label,
            stream.MeasurementUnitId,
            stream.SensorDeviceId,
            stream.Measurements.Count, 
            null
        );
    }
}