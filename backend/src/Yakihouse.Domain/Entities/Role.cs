using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class Role : EntityBase
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    // EF Core constructor
    private Role()
    {
    }

    public Role(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        Touch();
    }
}

