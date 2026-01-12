using FluentAssertions;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;
using Xunit;

namespace MiniSense.Domain.Tests.Entities;

public class UserTests
{
    private string GenerateString(int length) => new string('a', length);

    [Fact]
    public void Constructor_Should_Create_Valid_User()
    {
        string username = "admin";
        string email = "admin@minisense.com";

        var user = new User(username, email);

        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.Devices.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_Should_Throw_When_Username_Is_Invalid(string? invalidUsername)
    {
        Action action = () => _ = new User(invalidUsername!, "test@test.com");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Username cannot be empty*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Username_Is_Too_Long()
    {
        var longName = GenerateString(ValidationConstants.MaxUsernameLength + 1);
        
        Action action = () => _ = new User(longName, "test@test.com");
        
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Username too long*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_Should_Throw_When_Email_Is_Empty(string? invalidEmail)
    {
        Action action = () => _ = new User("user", invalidEmail!);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Email cannot be empty*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Email_Format_Is_Invalid()
    {
        Action action = () => _ = new User("user", "email-sem-arroba.com");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format*");
    }

    [Fact]
    public void UpdateEmail_Should_Update_When_Valid()
    {
        var user = new User("user", "old@test.com");
        var newEmail = "new@test.com";

        user.UpdateEmail(newEmail);

        user.Email.Should().Be(newEmail);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    public void UpdateEmail_Should_Throw_When_Invalid(string invalidEmail)
    {
        var user = new User("user", "valid@test.com");

        Action action = () => user.UpdateEmail(invalidEmail);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email*");
    }
}