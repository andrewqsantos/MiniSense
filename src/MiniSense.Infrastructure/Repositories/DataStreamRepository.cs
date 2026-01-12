using Microsoft.EntityFrameworkCore;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Interfaces.Repositories;
using MiniSense.Infrastructure.Persistence;

namespace MiniSense.Infrastructure.Repositories;

public class DataStreamRepository(AppDbContext context) : IDataStreamRepository
{
    public void Add(DataStream stream)
    {
        context.DataStreams.Add(stream);
    }

    public async Task<DataStream?> GetByKeyAsync(Guid key, CancellationToken ct = default)
    {
        return await context.DataStreams
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Key == key, ct);
    }
}