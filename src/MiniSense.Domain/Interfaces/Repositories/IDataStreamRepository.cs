using MiniSense.Domain.Entities;

namespace MiniSense.Domain.Interfaces.Repositories;

public interface IDataStreamRepository
{
    Task<DataStream?> GetByKeyAsync(Guid key, CancellationToken ct = default);
    void Add(DataStream stream);
}