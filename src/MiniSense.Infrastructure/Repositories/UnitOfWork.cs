using MiniSense.Domain.Interfaces.Repositories;
using MiniSense.Infrastructure.Persistence;

namespace MiniSense.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}