using FluentAssertions;
using MiniSense.Domain.Entities;
using Xunit;

namespace MiniSense.Domain.Tests.Entities;

public class SensorDataTests
{
    [Fact]
    public void Constructor_Should_Create_Instance_When_Data_Is_Valid()
    {
        int streamId = 10;
        double value = 42.5;
        DateTime timestamp = DateTime.UtcNow;

        var sensorData = new SensorData(streamId, value, timestamp);

        sensorData.DataStreamId.Should().Be(streamId);
        sensorData.Value.Should().Be(value);
        sensorData.Timestamp.Should().Be(timestamp);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_DataStreamId_Is_Invalid(int invalidStreamId)
    {
        double value = 10;
        DateTime timestamp = DateTime.UtcNow;

        Action action = () => _ = new SensorData(invalidStreamId, value, timestamp);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid Stream Id*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Timestamp_Is_Too_Far_In_Future()
    {
        int streamId = 1;
        DateTime futureDate = DateTime.UtcNow.AddMinutes(10); 

        Action action = () => _ = new SensorData(streamId, 20.0, futureDate);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Timestamp cannot be in the future*");
    }

    [Fact]
    public void Constructor_Should_Accept_Timestamp_Within_Tolerance()
    {
        int streamId = 1;
        DateTime nearFutureDate = DateTime.UtcNow.AddMinutes(4);

        var sensorData = new SensorData(streamId, 20.0, nearFutureDate);

        sensorData.Should().NotBeNull();
        sensorData.Timestamp.Should().Be(nearFutureDate);
    }
}