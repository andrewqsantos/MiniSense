using Microsoft.EntityFrameworkCore;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Interfaces.Repositories;
using MiniSense.Infrastructure.Persistence;

namespace MiniSense.Infrastructure.Repositories;

public class SensorDeviceRepository(AppDbContext context) : ISensorDeviceRepository
{
    public void Add(SensorDevice device) => context.SensorDevices.Add(device);

    public async Task<bool> ExistsAsync(Guid key, CancellationToken ct = default)
        => await context.SensorDevices.AnyAsync(d => d.Key == key, ct);

    public async Task<IEnumerable<SensorDevice>> GetByUserIdAsync(int userId, CancellationToken ct = default)
    {
        return await context.SensorDevices
            .Include(d => d.Streams)
                .ThenInclude(s => s.Measurements)
            .Where(d => d.UserId == userId)
            .AsNoTracking()
            .ToListAsync(ct);
    }
    
    public async Task<SensorDevice?> GetByKeyWithStreamsAsync(Guid key, CancellationToken ct = default)
    {
        return await context.SensorDevices
            .Include(d => d.Streams)
            .ThenInclude(s => s.Measurements.OrderByDescending(m => m.Timestamp))
            .AsSplitQuery() 
            .FirstOrDefaultAsync(d => d.Key == key, ct);
    }
}