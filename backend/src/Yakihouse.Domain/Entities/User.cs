namespace Yakihouse.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "Staff"; // Admin, Manager, Staff, Kitchen
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public User()
    {
    }

    public User(string username, string passwordHash, string fullName, string email, string role)
    {
        Id = Guid.NewGuid();
        Username = username;
        PasswordHash = passwordHash;
        FullName = fullName;
        Email = email;
        Role = role;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void UpdateRole(string role)
    {
        Role = role;
    }
}
