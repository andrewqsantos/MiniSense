using MiniSense.Domain.Entities;

namespace MiniSense.Domain.Interfaces.Repositories;

public interface ISensorDeviceRepository
{
    Task<SensorDevice?> GetByKeyWithStreamsAsync(Guid key, CancellationToken ct = default);
    Task<IEnumerable<SensorDevice>> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid key, CancellationToken ct = default);
    
    void Add(SensorDevice device);
}