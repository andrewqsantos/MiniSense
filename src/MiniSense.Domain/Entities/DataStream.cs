using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities.Base;
using MiniSense.Domain.Enums;

namespace MiniSense.Domain.Entities;

public class DataStream : Entity
{
    public Guid Key { get; private set; }
    public string Label { get; private set; }
    public bool Enabled { get; private set; }

    public int SensorDeviceId { get; private set; }
    public virtual SensorDevice SensorDevice { get; private set; } = null!;

    public int MeasurementUnitId { get; private set; }
    public virtual MeasurementUnit MeasurementUnit { get; private set; } = null!;

    public UnitType UnitType => (UnitType)MeasurementUnitId;

    private readonly List<SensorData> _measurements = new();
    public IReadOnlyCollection<SensorData> Measurements => _measurements.AsReadOnly();

    protected DataStream() { }

    public DataStream(int sensorDeviceId, string label, UnitType unitType)
    {
        if (sensorDeviceId <= 0)
            throw new ArgumentException("Invalid Device Id", nameof(sensorDeviceId));

        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label cannot be empty", nameof(label));

        if (label.Length > ValidationConstants.MaxStreamLabelLength)
            throw new ArgumentException($"Label max length is {ValidationConstants.MaxStreamLabelLength}", nameof(label));

        if (!Enum.IsDefined(typeof(UnitType), unitType))
            throw new ArgumentException("Invalid Measurement Unit", nameof(unitType));

        Key = Guid.NewGuid();
        SensorDeviceId = sensorDeviceId;
        Label = label;
        MeasurementUnitId = (int)unitType;
        Enabled = true;
    }

    public void Disable() => Enabled = false;
    public void Enable() => Enabled = true;

    public SensorData AddMeasurement(double value, DateTime timestamp)
    {
        if (!Enabled)
            throw new InvalidOperationException($"Cannot add measurement to disabled stream '{Label}'.");

        var data = new SensorData(this.Id, value, timestamp);
        _measurements.Add(data);

        return data;
    }
}