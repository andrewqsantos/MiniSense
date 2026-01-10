using MiniSense.Domain.Entities;

namespace MiniSense.Domain.Interfaces.Repositories;

public interface IMeasurementUnitRepository
{
    Task<IEnumerable<MeasurementUnit>> GetAllAsync(CancellationToken ct = default);
}