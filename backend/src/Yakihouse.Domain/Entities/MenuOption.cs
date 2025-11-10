using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class MenuOption : EntityBase
{
    private MenuOption()
    {
    }

    internal MenuOption(Guid groupId, string name, decimal extraPrice, bool isDefault)
    {
        GroupId = groupId;
        Name = name;
        ExtraPrice = extraPrice;
        IsDefault = isDefault;
    }

    public Guid GroupId { get; private set; }
    public MenuOptionGroup Group { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public decimal ExtraPrice { get; private set; }
    public bool IsDefault { get; private set; }

    public void UpdateDetails(string name, decimal extraPrice, bool isDefault)
    {
        Name = name;
        ExtraPrice = extraPrice;
        IsDefault = isDefault;
        Touch();
    }
}

