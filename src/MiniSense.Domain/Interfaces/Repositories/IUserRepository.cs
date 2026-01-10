namespace MiniSense.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}