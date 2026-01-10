using FluentAssertions;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Enums;
using MiniSense.Domain.Entities.Base;
using System.Reflection;
using Xunit;

namespace MiniSense.Domain.Tests.Entities;

public class DataStreamTests
{
    private string GenerateString(int length) => new string('a', length);

    private void SetId(Entity entity, int id)
    {
        var prop = typeof(Entity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(entity, id);
        }
        else
        {
            var field = typeof(Entity).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(entity, id);
        }
    }

    [Fact]
    public void Constructor_Should_Create_Valid_Stream()
    {
        int deviceId = 1;
        string label = "Temperature Sensor";
        UnitType unit = UnitType.Celsius;

        var stream = new DataStream(deviceId, label, unit);

        stream.Key.Should().NotBeEmpty();
        stream.Label.Should().Be(label);
        stream.SensorDeviceId.Should().Be(deviceId);
        stream.MeasurementUnitId.Should().Be((int)unit);
        stream.Enabled.Should().BeTrue();
        stream.Measurements.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_DeviceId_Is_Invalid(int invalidDeviceId)
    {
        Action action = () => new DataStream(invalidDeviceId, "Valid Label", UnitType.Celsius);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid Device Id*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_Throw_When_Label_Is_Empty(string invalidLabel)
    {
        Action action = () => new DataStream(1, invalidLabel, UnitType.Celsius);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Label cannot be empty*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Label_Is_Too_Long()
    {
        var longLabel = GenerateString(ValidationConstants.MaxStreamLabelLength + 1);

        Action action = () => new DataStream(1, longLabel, UnitType.Celsius);

        action.Should().Throw<ArgumentException>()
            .WithMessage($"Label max length is {ValidationConstants.MaxStreamLabelLength}*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_UnitType_Is_Invalid()
    {
        var invalidUnit = (UnitType)999; 
        Action action = () => new DataStream(1, "Label", invalidUnit);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid Measurement Unit*");
    }

    [Fact]
    public void Disable_Should_Set_Enabled_To_False()
    {
        var stream = new DataStream(1, "Label", UnitType.Celsius);

        stream.Disable();

        stream.Enabled.Should().BeFalse();
    }

    [Fact]
    public void AddMeasurement_Should_Add_To_Collection_When_Enabled()
    {
        var stream = new DataStream(1, "Label", UnitType.Celsius);
        
        SetId(stream, 10); 
        
        var value = 25.5;
        var timestamp = DateTime.UtcNow;

        var result = stream.AddMeasurement(value, timestamp);

        result.Should().NotBeNull();
        result.Value.Should().Be(value);
        result.Timestamp.Should().Be(timestamp);
        result.DataStreamId.Should().Be(10);

        stream.Measurements.Should().HaveCount(1);
        stream.Measurements.First().Should().Be(result);
    }

    [Fact]
    public void AddMeasurement_Should_Throw_When_Stream_Is_Disabled()
    {
        var stream = new DataStream(1, "Label", UnitType.Celsius);
        SetId(stream, 10);
        stream.Disable();

        Action action = () => stream.AddMeasurement(20, DateTime.UtcNow);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage($"Cannot add measurement to disabled stream '{stream.Label}'.");
    }
}