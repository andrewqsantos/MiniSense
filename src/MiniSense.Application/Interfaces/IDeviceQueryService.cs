using MiniSense.Application.DTOs;

namespace MiniSense.Application.Interfaces;

public interface IDeviceQueryService
{
    Task<SensorDeviceDetailDto?> GetDeviceSummaryAsync(Guid key);
    Task<DataStreamDto?> GetStreamHistoryAsync(Guid key);
}