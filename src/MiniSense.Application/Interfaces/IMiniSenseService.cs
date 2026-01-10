using MiniSense.Application.DTOs;

namespace MiniSense.Application.Interfaces;

public interface IMiniSenseService
{
    // GET
    Task<IEnumerable<MeasurementUnitDto>> GetAllUnitsAsync();
    Task<IEnumerable<SensorDeviceDetailDto>> GetDevicesByUserAsync(int userId);
    Task<SensorDeviceDetailDto?> GetDeviceByKeyAsync(Guid key);
    Task<DataStreamDto?> GetStreamByKeyAsync(Guid key);
    
    // POST
    Task<SensorDeviceSummaryDto> RegisterDeviceAsync(int userId, CreateDeviceRequest request);
    Task<DataStreamDto> RegisterStreamAsync(Guid deviceKey, CreateStreamRequest request);
    Task<SensorDataDetailDto> AddMeasurementAsync(Guid streamKey, CreateMeasurementRequest request);
}