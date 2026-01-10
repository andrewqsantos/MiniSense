using MiniSense.Domain.Entities;

namespace MiniSense.Domain.Interfaces.Repositories;

public interface ISensorDataRepository
{
    void Add(SensorData device);
}