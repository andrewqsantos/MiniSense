using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities.Base;

namespace MiniSense.Domain.Entities;

public class MeasurementUnit : Entity
{
    public string Symbol { get; private set; }
    public string Description { get; private set; }

    private readonly List<DataStream> _streams = new();
    public IReadOnlyCollection<DataStream> Streams => _streams.AsReadOnly();

    protected MeasurementUnit() { }

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
}