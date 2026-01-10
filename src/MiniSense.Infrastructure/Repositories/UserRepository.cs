using Microsoft.EntityFrameworkCore;
using MiniSense.Domain.Interfaces.Repositories;
using MiniSense.Infrastructure.Persistence;

namespace MiniSense.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await context.Users.AnyAsync(u => u.Id == id, ct);
    }
}