using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities.Base;

namespace MiniSense.Domain.Entities;

public class SensorDevice : Entity
{
    public Guid Key { get; private set; }
    public string Label { get; private set; }
    public string Description { get; private set; }
    
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;

    private readonly List<DataStream> _streams = new();
    public IReadOnlyCollection<DataStream> Streams => _streams.AsReadOnly();

    protected SensorDevice()
    {
        Label = string.Empty;
        Description = string.Empty;
    }

    public SensorDevice(int userId, string label, string description)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label cannot be empty.", nameof(label));

        if (label.Length > ValidationConstants.MaxLabelLength)
            throw new ArgumentException($"Label max length is {ValidationConstants.MaxLabelLength}.", nameof(label));

        if (userId <= 0)
            throw new ArgumentException("Invalid User Id.", nameof(userId));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        
        if (description.Length > ValidationConstants.MaxDescriptionLength)
            throw new ArgumentException($"Description max length is {ValidationConstants.MaxDescriptionLength}.", nameof(description));
        
        Key = Guid.NewGuid();
        UserId = userId;
        Label = label;
        Description = description;
    }

    public void AddStream(DataStream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        
        if (_streams.Any(x => x.Label.Equals(stream.Label, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Stream '{stream.Label}' already exists on this device.");

        _streams.Add(stream);
    }
}