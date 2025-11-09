using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class MenuItem : AggregateRoot
{
    private readonly List<MenuOptionGroup> _optionGroups = new();

    private MenuItem()
    {
    }

    internal MenuItem(Guid categoryId, string name, string? description, decimal basePrice, Guid? kitchenStationId = null)
    {
        CategoryId = categoryId;
        Name = name;
        Description = description;
        BasePrice = basePrice;
        KitchenStationId = kitchenStationId;
        Status = MenuItemStatus.Available;
    }

    public Guid CategoryId { get; private set; }
    public MenuCategory Category { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal BasePrice { get; private set; }
    public Guid? KitchenStationId { get; private set; }
    public KitchenStation? KitchenStation { get; private set; }
    public MenuItemStatus Status { get; private set; }
    public IReadOnlyCollection<MenuOptionGroup> OptionGroups => _optionGroups.AsReadOnly();

    public void UpdateDetails(string name, string? description, decimal basePrice)
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;
        Touch();
    }

    public void SetStatus(MenuItemStatus status)
    {
        Status = status;
        Touch();
    }

    public MenuOptionGroup AddOptionGroup(string name, bool isRequired)
    {
        var group = new MenuOptionGroup(Id, name, isRequired);
        _optionGroups.Add(group);
        return group;
    }
}

