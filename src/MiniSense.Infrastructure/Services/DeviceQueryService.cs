using Microsoft.EntityFrameworkCore;
using MiniSense.Application.DTOs;
using MiniSense.Application.Interfaces;
using MiniSense.Infrastructure.Persistence;

namespace MiniSense.Infrastructure.Services;

public class DeviceQueryService(AppDbContext context) : IDeviceQueryService
{
    public async Task<SensorDeviceDetailDto?> GetDeviceSummaryAsync(Guid key)
    {
        return await context.SensorDevices
            .AsNoTracking()
            .Where(d => d.Key == key)
            .Select(d => new SensorDeviceDetailDto(
                d.Id,
                d.Key,
                d.Label,
                d.Description,
                d.Streams.Select(s => new DataStreamDto(
                    s.Id,
                    s.Key,
                    s.Label,
                    s.MeasurementUnitId,
                    s.SensorDeviceId,
                    s.Measurements.Count(),
                    
                    s.Measurements
                        .OrderByDescending(m => m.Timestamp)
                        .Take(5)
                        .Select(m => new SensorDataSummaryDto(
                            ((DateTimeOffset)m.Timestamp).ToUnixTimeSeconds(), 
                            m.Value))
                        .ToList()
                ))
            ))
            .FirstOrDefaultAsync();
    }
    
    public async Task<DataStreamDto?> GetStreamHistoryAsync(Guid key)
    {
        return await context.DataStreams
            .AsNoTracking()
            .Where(s => s.Key == key)
            .Select(s => new DataStreamDto(
                s.Id,
                s.Key,
                s.Label,
                s.MeasurementUnitId,
                s.SensorDeviceId,
                s.Measurements.Count(),
                
                s.Measurements
                    .OrderByDescending(m => m.Timestamp)
                    .Select(m => new SensorDataSummaryDto(
                        ((DateTimeOffset)m.Timestamp).ToUnixTimeSeconds(), 
                        m.Value))
                    .ToList()
            ))
            .FirstOrDefaultAsync();
    }
}