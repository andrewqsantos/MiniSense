using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities.Base;

namespace MiniSense.Domain.Entities;

public class User : Entity
{
    public string Username { get; private set; }
    public string Email { get; private set; }

    private readonly List<SensorDevice> _devices = new();
    public IReadOnlyCollection<SensorDevice> Devices => _devices.AsReadOnly();

    protected User()
    {
        Username = string.Empty;
        Email = string.Empty;
    }

    public User(string username, string email)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (username.Length > ValidationConstants.MaxUsernameLength)
            throw new ArgumentException($"Username too long (max {ValidationConstants.MaxUsernameLength})", nameof(username));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
            
        if (!email.Contains("@")) 
            throw new ArgumentException("Invalid email format", nameof(email));

        Username = username;
        Email = email;
    }
    
    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
            throw new ArgumentException("Invalid email", nameof(newEmail));
        Email = newEmail;
    }
    
    public void AddDevice(SensorDevice device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        _devices.Add(device);
    }
}