using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities.Base;

namespace MiniSense.Domain.Entities;

public class MeasurementUnit : Entity
{
    public string Symbol { get; private set; }
    public string Description { get; private set; }

    private readonly List<DataStream> _streams = new();
    public IReadOnlyCollection<DataStream> Streams => _streams.AsReadOnly();

    protected MeasurementUnit()
    {
        Symbol = string.Empty;
        Description = string.Empty;
    }

    public MeasurementUnit(string symbol, string description)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol cannot be empty", nameof(symbol));
            
        if (symbol.Length > ValidationConstants.MaxUnitSymbolLength)
            throw new ArgumentException($"Symbol max length is {ValidationConstants.MaxUnitSymbolLength}", nameof(symbol));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        Symbol = symbol;
        Description = description;
    }
    
    public void AddDataStream(DataStream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream), "O stream não pode ser nulo.");

        if (!_streams.Contains(stream))
        {
            _streams.Add(stream);
        }
    }
    
    
    public void RemoveDataStream(DataStream stream)
    {
        if (_streams.Contains(stream))
        {
            _streams.Remove(stream);
        }
    }
    
}