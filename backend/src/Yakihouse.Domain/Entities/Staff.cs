using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class Staff : AggregateRoot
{
    private Staff()
    {
        // Required by EF Core
    }

    public Staff(string fullName, string email, string phone, Guid roleId, DateTime hireDate)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        RoleId = roleId;
        HireDate = hireDate;
        IsActive = true;
    }

    public string FullName { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string? Email { get; private set; }
    public DateTime HireDate { get; private set; }
    public bool IsActive { get; private set; }
    public StaffStatus Status { get; private set; }
    
    public void UpdateDetails(string fullName, string email, string phone, Guid roleId)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        RoleId = roleId;
        Touch();
    }

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

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        Touch();
    }

    public void SetStatus(StaffStatus status)
    {
        Status = status;
        Touch();
    }
}

