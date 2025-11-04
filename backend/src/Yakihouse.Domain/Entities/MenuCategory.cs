using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class MenuCategory : AggregateRoot
{
    private readonly List<MenuItem> _items = new();

    private MenuCategory()
    {
    }

    public MenuCategory(string name, int displayOrder)
    {
        Name = name;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public string Name { get; private set; } = null!;
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<MenuItem> Items => _items.AsReadOnly();

    public void UpdateDetails(string name, int displayOrder)
    {
        Name = name;
        DisplayOrder = displayOrder;
        Touch();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        Touch();
    }

    public MenuItem AddItem(string name, string? description, decimal basePrice)
    {
        var item = new MenuItem(Id, name, description, basePrice);
        _items.Add(item);
        return item;
    }
}

