using System.Reflection;
using FluentAssertions;
using MiniSense.Application.DTOs;
using MiniSense.Application.Interfaces;
using MiniSense.Application.Services;
using MiniSense.Domain.Entities;
using MiniSense.Domain.Entities.Base;
using MiniSense.Domain.Enums;
using MiniSense.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace MiniSense.Application.Tests.Services;

public class MiniSenseServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISensorDeviceRepository> _deviceRepoMock;
    private readonly Mock<IDataStreamRepository> _streamRepoMock;
    private readonly Mock<IMeasurementUnitRepository> _unitRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IDeviceQueryService> _queryServiceMock;
    private readonly Mock<ISensorDataRepository> _dataRepoMock;

    private readonly MiniSenseService _service;

    public MiniSenseServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _deviceRepoMock = new Mock<ISensorDeviceRepository>();
        _streamRepoMock = new Mock<IDataStreamRepository>();
        _unitRepoMock = new Mock<IMeasurementUnitRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _queryServiceMock = new Mock<IDeviceQueryService>();
        _dataRepoMock = new Mock<ISensorDataRepository>();

        _service = new MiniSenseService(
            _unitOfWorkMock.Object,
            _deviceRepoMock.Object,
            _streamRepoMock.Object,
            _unitRepoMock.Object,
            _userRepoMock.Object,
            _queryServiceMock.Object,
            _dataRepoMock.Object
        );
    }

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
    public async Task RegisterDeviceAsync_Should_Success_When_User_Exists()
    {
        int userId = 1;
        var request = new CreateDeviceRequest("Device 1", "Description");

        _userRepoMock.Setup(r => r.ExistsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _service.RegisterDeviceAsync(userId, request);

        result.Should().NotBeNull();
        result.Label.Should().Be("Device 1");

        _deviceRepoMock.Verify(r => r.Add(It.Is<SensorDevice>(d => d.Label == "Device 1")), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterDeviceAsync_Should_Throw_When_User_Not_Found()
    {
        _userRepoMock.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var request = new CreateDeviceRequest("Dev", "Desc");

        Func<Task> act = async () => await _service.RegisterDeviceAsync(99, request);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User with Id 99 not found.");
            
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task RegisterStreamAsync_Should_Success_When_Valid()
    {
        var deviceKey = Guid.NewGuid();
        var deviceId = 10;
        
        var device = new SensorDevice(1, "Device Test", "Desc");
        SetId(device, deviceId);

        var request = new CreateStreamRequest("Temp Sensor", (int)UnitType.Celsius);

        _deviceRepoMock.Setup(r => r.GetByKeyWithStreamsAsync(deviceKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(device);

        var result = await _service.RegisterStreamAsync(deviceKey, request);

        result.Should().NotBeNull();
        result.Label.Should().Be("Temp Sensor");
        result.UnitId.Should().Be((int)UnitType.Celsius);

        _streamRepoMock.Verify(r => r.Add(It.IsAny<DataStream>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterStreamAsync_Should_Throw_When_UnitType_Invalid()
    {
        var deviceKey = Guid.NewGuid();
        var device = new SensorDevice(1, "Dev", "Desc");
        SetId(device, 1);

        _deviceRepoMock.Setup(r => r.GetByKeyWithStreamsAsync(deviceKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(device);

        var request = new CreateStreamRequest("Temp", 999); 

        Func<Task> act = async () => await _service.RegisterStreamAsync(deviceKey, request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid Unit Type ID.");
    }

    [Fact]
    public async Task RegisterStreamAsync_Should_Throw_When_Device_Not_Found()
    {
        _deviceRepoMock.Setup(r => r.GetByKeyWithStreamsAsync(It.IsAny<Guid>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync((SensorDevice?)null);

        var request = new CreateStreamRequest("Temp", (int)UnitType.Celsius);

        Func<Task> act = async () => await _service.RegisterStreamAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Device not found.");
    }

    [Fact]
    public async Task AddMeasurementAsync_Should_Add_And_Commit_Success()
    {
        var streamKey = Guid.NewGuid();
        var streamId = 50;

        var stream = new DataStream(1, "Stream A", UnitType.Celsius);
        SetId(stream, streamId);

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var request = new CreateMeasurementRequest(timestamp, 25.5);

        _streamRepoMock.Setup(r => r.GetByKeyAsync(streamKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stream);

        var result = await _service.AddMeasurementAsync(streamKey, request);

        result.Should().NotBeNull();
        result.Value.Should().Be(25.5);
        result.Timestamp.Should().Be(timestamp);
        
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddMeasurementAsync_Should_Throw_When_Stream_Not_Found()
    {
        _streamRepoMock.Setup(r => r.GetByKeyAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DataStream?)null);

        var request = new CreateMeasurementRequest(123456, 10);

        Func<Task> act = async () => await _service.AddMeasurementAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Stream not found.");

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetDeviceByKeyAsync_Should_Call_QueryService()
    {
        var key = Guid.NewGuid();
        var expectedDto = new SensorDeviceDetailDto(1, key,"Dev", "Desc", new List<DataStreamDto>());

        _queryServiceMock.Setup(q => q.GetDeviceSummaryAsync(key))
            .ReturnsAsync(expectedDto);

        var result = await _service.GetDeviceByKeyAsync(key);

        result.Should().Be(expectedDto);
        _queryServiceMock.Verify(q => q.GetDeviceSummaryAsync(key), Times.Once);
    }
}