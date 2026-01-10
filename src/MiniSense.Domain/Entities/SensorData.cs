using MiniSense.Domain.Entities.Base;

namespace MiniSense.Domain.Entities;

public class SensorData : Entity
{
    public DateTime Timestamp { get; private set; }
    public double Value { get; private set; }

    public int DataStreamId { get; private set; }
    public virtual DataStream DataStream { get; private set; } = null!;

    protected SensorData() { }

    internal SensorData(int dataStreamId, double value, DateTime timestamp)
    {
        if (dataStreamId <= 0)
            throw new ArgumentException("Invalid Stream Id", nameof(dataStreamId));
            
        if (timestamp > DateTime.UtcNow.AddMinutes(5)) 
            throw new ArgumentException("Timestamp cannot be in the future", nameof(timestamp));

        DataStreamId = dataStreamId;
        Value = value;
        Timestamp = timestamp;
    }
}