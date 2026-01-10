using Microsoft.EntityFrameworkCore;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Interfaces.Repositories;
using MiniSense.Infrastructure.Persistence;

namespace MiniSense.Infrastructure.Repositories;

public class MeasurementUnitRepository(AppDbContext context) : IMeasurementUnitRepository
{
    public async Task<IEnumerable<MeasurementUnit>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.MeasurementUnits
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .ToListAsync(ct);
    }
}