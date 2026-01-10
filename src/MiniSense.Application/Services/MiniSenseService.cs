using MiniSense.Application.DTOs;
using MiniSense.Application.Extensions;
using MiniSense.Application.Interfaces;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Enums;
using MiniSense.Domain.Interfaces.Repositories;

namespace MiniSense.Application.Services;

public class MiniSenseService(
    IUnitOfWork unitOfWork,
    ISensorDeviceRepository deviceRepository,
    IDataStreamRepository streamRepository,
    IMeasurementUnitRepository unitRepository,
    IUserRepository userRepository,
    IDeviceQueryService queryService,
    ISensorDataRepository dataRepository
    ) : IMiniSenseService
{
    public async Task<IEnumerable<MeasurementUnitDto>> GetAllUnitsAsync()
    {
        var units = await unitRepository.GetAllAsync();
        return units.Select(u => new MeasurementUnitDto(u.Id, u.Symbol, u.Description));
    }

    public async Task<IEnumerable<SensorDeviceDetailDto>> GetDevicesByUserAsync(int userId)
    {
        var devices = await deviceRepository.GetByUserIdAsync(userId);
        return devices.Select(d => d.ToDetailDto());
    }

    public async Task<SensorDeviceDetailDto?> GetDeviceByKeyAsync(Guid key)
    {
        return await queryService.GetDeviceSummaryAsync(key);
    }

    public async Task<SensorDeviceSummaryDto> RegisterDeviceAsync(int userId, CreateDeviceRequest request)
    {
        if (!await userRepository.ExistsAsync(userId))
            throw new KeyNotFoundException($"User with Id {userId} not found.");

        var device = new SensorDevice(userId, request.Label, request.Description);

        deviceRepository.Add(device);
        await unitOfWork.CommitAsync();

        return device.ToSummaryDto();
    }

    public async Task<DataStreamDto> RegisterStreamAsync(Guid deviceKey, CreateStreamRequest request)
    {
        var device = await deviceRepository.GetByKeyWithStreamsAsync(deviceKey);
        
        if (device == null) 
            throw new KeyNotFoundException("Device not found.");

        if (!Enum.IsDefined(typeof(UnitType), request.UnitId))
             throw new ArgumentException("Invalid Unit Type ID.");

        var stream = new DataStream(device.Id, request.Label, (UnitType)request.UnitId);

        device.AddStream(stream);

        streamRepository.Add(stream); 
        
        await unitOfWork.CommitAsync();

        return stream.ToDto();
    }

    public async Task<SensorDataDetailDto> AddMeasurementAsync(Guid streamKey, CreateMeasurementRequest request)
    {
        var stream = await streamRepository.GetByKeyAsync(streamKey);
        if (stream == null) 
            throw new KeyNotFoundException("Stream not found.");

        var timestamp = request.Timestamp.FromUnixTimestamp();
        
        var measurementEntity= stream.AddMeasurement(request.Value, timestamp);

        await unitOfWork.CommitAsync();
        
        return new SensorDataDetailDto(
            measurementEntity.Id,
            request.Timestamp,
            request.Value,
            stream.MeasurementUnitId
        );
    }
    
    public async Task<DataStreamDto?> GetStreamByKeyAsync(Guid key)
    {
        return await queryService.GetStreamHistoryAsync(key);
    }
}