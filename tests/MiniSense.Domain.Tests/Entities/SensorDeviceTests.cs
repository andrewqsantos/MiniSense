using FluentAssertions;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Entities.Base;
using MiniSense.Domain.Enums;
using System.Reflection;
using Xunit;

namespace MiniSense.Domain.Tests.Entities;

public class SensorDeviceTests
{
    private string GenerateString(int length) => new string('a', length);

    private void SetId(Entity entity, int id)
    {
        var prop = typeof(Entity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
            prop.SetValue(entity, id);
        else
        {
            var field = typeof(Entity).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(entity, id);
        }
    }

    [Fact]
    public void Constructor_Should_Create_Valid_Device()
    {
        int userId = 1;
        string label = "Kitchen Device";
        string description = "Device near the fridge";

        var device = new SensorDevice(userId, label, description);

        device.Key.Should().NotBeEmpty();
        device.UserId.Should().Be(userId);
        device.Label.Should().Be(label);
        device.Description.Should().Be(description);
        device.Streams.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_UserId_Is_Invalid(int invalidUserId)
    {
        Action action = () => new SensorDevice(invalidUserId, "Label", "Desc");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid User Id*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_Should_Throw_When_Label_Is_Empty(string invalidLabel)
    {
        Action action = () => new SensorDevice(1, invalidLabel, "Desc");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Label cannot be empty*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Label_Is_Too_Long()
    {
        var longLabel = GenerateString(ValidationConstants.MaxLabelLength + 1);
        Action action = () => new SensorDevice(1, longLabel, "Desc");
        
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Label max length is {ValidationConstants.MaxLabelLength}*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_Should_Throw_When_Description_Is_Empty(string invalidDesc)
    {
        Action action = () => new SensorDevice(1, "Label", invalidDesc);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Description cannot be empty*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Description_Is_Too_Long()
    {
        var longDesc = GenerateString(ValidationConstants.MaxDescriptionLength + 1);
        Action action = () => new SensorDevice(1, "Label", longDesc);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Description max length is {ValidationConstants.MaxDescriptionLength}*");
    }

    [Fact]
    public void AddStream_Should_Add_Stream_When_Valid()
    {
        var device = new SensorDevice(1, "Device 1", "Desc");
        SetId(device, 10);

        var stream = new DataStream(device.Id, "Temperature", UnitType.Celsius);

        device.AddStream(stream);

        device.Streams.Should().HaveCount(1);
        device.Streams.First().Label.Should().Be("Temperature");
    }

    [Fact]
    public void AddStream_Should_Throw_When_Stream_Is_Null()
    {
        var device = new SensorDevice(1, "Device 1", "Desc");
        Action action = () => device.AddStream(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddStream_Should_Throw_When_Duplicate_Label_Exists()
    {
        var device = new SensorDevice(1, "Device 1", "Desc");
        SetId(device, 10);

        var stream1 = new DataStream(device.Id, "Temperature", UnitType.Celsius);
        
        var stream2 = new DataStream(device.Id, "TEMPERATURE", UnitType.MegaGramPerCubic); 

        device.AddStream(stream1);

        Action action = () => device.AddStream(stream2);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage($"Stream '{stream2.Label}' already exists on this device.");
    }
}