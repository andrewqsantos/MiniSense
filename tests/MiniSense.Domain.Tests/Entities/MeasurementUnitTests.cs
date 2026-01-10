using FluentAssertions;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;
using Xunit;

namespace MiniSense.Domain.Tests.Entities;

public class MeasurementUnitTests
{
    private string GenerateString(int length) => new string('X', length);

    [Fact]
    public void Constructor_Should_Create_Valid_Unit()
    {
        string symbol = "°C";
        string description = "Celsius";

        var unit = new MeasurementUnit(symbol, description);

        unit.Symbol.Should().Be(symbol);
        unit.Description.Should().Be(description);
        unit.Streams.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_Should_Throw_When_Symbol_Is_Empty(string invalidSymbol)
    {
        Action action = () => new MeasurementUnit(invalidSymbol, "Description");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Symbol cannot be empty*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Symbol_Is_Too_Long()
    {
        var longSymbol = GenerateString(ValidationConstants.MaxUnitSymbolLength + 1);
        
        Action action = () => new MeasurementUnit(longSymbol, "Description");
        
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Symbol max length is*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_Should_Throw_When_Description_Is_Empty(string invalidDesc)
    {
        Action action = () => new MeasurementUnit("Kg", invalidDesc);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Description cannot be empty*");
    }
}