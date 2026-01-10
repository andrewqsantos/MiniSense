using Microsoft.EntityFrameworkCore;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Interfaces.Repositories;
using MiniSense.Infrastructure.Persistence;

namespace MiniSense.Infrastructure.Repositories;

public class SensorDataRepository(AppDbContext context) : ISensorDataRepository
{
    public void Add(SensorData data) => context.SensorData.Add(data);
}