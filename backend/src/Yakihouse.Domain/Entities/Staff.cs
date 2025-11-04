using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class Staff : AggregateRoot
{
    private Staff()
    {
        // Required by EF Core
    }

    public Staff(string fullName, Guid roleId, string phone, string? email)
    {
        FullName = fullName;
        RoleId = roleId;
        Phone = phone;
        Email = email;
        Status = StaffStatus.Active;
    }

    public string FullName { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string? Email { get; private set; }
    public StaffStatus Status { get; private set; }
    public void UpdateProfile(string fullName, string phone, string? email)
    {
        FullName = fullName;
        Phone = phone;
        Email = email;
        Touch();
    }

    public void ChangeRole(Guid roleId)
    {
        RoleId = roleId;
        Touch();
    }

    public void SetStatus(StaffStatus status)
    {
        Status = status;
        Touch();
    }
}

